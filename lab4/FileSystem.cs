namespace lab4;

public class FileSystem
{
    // protected BlockStorage Storage { get; set; }
    public bool[] Bitmap { get; set; }
    public List<FileDescriptor?> Descriptors { get; set; }
    private List<int> FreeFileDescriptorNumbers { get; set; } = new List<int>();
    private List<OpenedFile> OpenedFiles { get; set; } = new List<OpenedFile>();
    private int BlockSize { get; set; } = 64;
    public Directory Directory { get; set; }

    public FileSystem(int numberOfBlocks, int maxNumberOfDescriptors, int maxNumberOfNumberDescriptors)
    {
        Bitmap = new bool[numberOfBlocks];
        Descriptors = new List<FileDescriptor?>(maxNumberOfDescriptors);
        Directory = new();

        for (int i = 0; i < maxNumberOfDescriptors; i++)
        {
            Descriptors.Add(null);
        }

        for (int i = 0; i < maxNumberOfNumberDescriptors; i++)
        {
            FreeFileDescriptorNumbers.Add(i);
        }

        int rootDescriptorIndex = FindFreeDescriptorIndex();
        if (rootDescriptorIndex != -1)
        {
            Descriptors[rootDescriptorIndex] = new FileDescriptor(false);
        }
    }

    public bool Create(string fileName)
    {
        if (Directory.Contains(fileName))
        {
            Console.WriteLine($"File '{fileName}' already exists in the directory.");
            return false;
        }

        int descriptorIndex = FindFreeDescriptorIndex();
        if (descriptorIndex == -1)
        {
            Console.WriteLine("No free descriptors available.");
            return false;
        }

        var fileDescriptor = new FileDescriptor();
        Descriptors[descriptorIndex] = fileDescriptor;

        Directory.AddEntry(fileName, descriptorIndex);

        return true;
    }

    public void Stat(string filename)
    {
        int descriptorIndex = Directory.FindDescriptorIndex(filename);
        if (descriptorIndex != -1)
        {
            FileDescriptor fileDescriptor = Descriptors[descriptorIndex]!;
            Console.WriteLine($"File Information for '{filename}':");
            Console.WriteLine($"Is Regular File: {fileDescriptor.IsRegularFile}");
            Console.WriteLine($"Hard Link Count: {fileDescriptor.HardLinkCount}");
            Console.WriteLine($"File Size: {fileDescriptor.FileSize} bytes");
            Console.WriteLine($"Block Map: [{string.Join(", ", fileDescriptor.BlockMap)}]");
        }
        else
        {
            Console.WriteLine($"File '{filename}' not found.");
        }
    }

    public void Ls()
    {
        Directory.Ls();
    }

    public int Open(string filename)
    {
        int descriptorIndex = Directory.FindDescriptorIndex(filename);
        if (descriptorIndex != -1)
        {
            FileDescriptor fileDescriptor = Descriptors[descriptorIndex]!;

            int fileDescriptorNumber;
            if (FreeFileDescriptorNumbers.Count != 0)
            {
                fileDescriptorNumber = FreeFileDescriptorNumbers.Min();
                FreeFileDescriptorNumbers.Remove(fileDescriptorNumber);
            }
            else
            {
                Console.WriteLine($"You can not open new file, all number descriptor is using now.");
                return -1;
            }

            OpenedFiles.Add(new OpenedFile
            {
                FileDescriptorNumber = fileDescriptorNumber,
                DescriptorIndex = descriptorIndex,
                Position = 0,
            });

            Console.WriteLine($"File '{filename}' was opened, File Descriptor: {fileDescriptorNumber}.");

            return fileDescriptorNumber;
        }
        else
        {
            Console.WriteLine($"File '{filename}' not found.");
            return -1;
        }
    }


    public void Close(int fileDescriptorNumber)
    {
        var openedFile = OpenedFiles.FirstOrDefault(f => f.FileDescriptorNumber == fileDescriptorNumber);

        if (openedFile != null)
        {
            FreeFileDescriptorNumbers.Add(fileDescriptorNumber);
            OpenedFiles.Remove(openedFile);
            Console.WriteLine($"File was closed, now File Descriptor {fileDescriptorNumber} is free.");
        }
        else
        {
            Console.WriteLine($"File with descriptor number '{fileDescriptorNumber}' not found or already closed.");
        }
    }

    public void Seek(int fileDescriptorNumber, int offset)
    {
        var openedFile = OpenedFiles.FirstOrDefault(f => f.FileDescriptorNumber == fileDescriptorNumber);

        if (openedFile != null)
        {
            FileDescriptor fileDescriptor = Descriptors[openedFile.DescriptorIndex]!;
            if (offset >= 0 && offset < fileDescriptor.FileSize)
            {
                openedFile.Position = offset;
            }
            else
            {
                Console.WriteLine($"Invalid offset for file with descriptor number '{fileDescriptorNumber}'. Offset must be between 0 and file size.");
            }
        }
        else
        {
            Console.WriteLine($"File with descriptor number '{fileDescriptorNumber}' not found or not opened.");
        }
    }

    public byte[] Read(int fileDescriptorNumber, int size)
    {
        var openedFile = OpenedFiles.FirstOrDefault(f => f.FileDescriptorNumber == fileDescriptorNumber);

        if (openedFile != null)
        {
            FileDescriptor fileDescriptor = Descriptors[openedFile.DescriptorIndex]!;

            int remainingBytes = (int)(fileDescriptor.FileSize - openedFile.Position);

            if (size > remainingBytes)
            {
                Console.WriteLine($"Requested size ({size} bytes) exceeds the remaining bytes available for reading ({remainingBytes} bytes).");
                size = remainingBytes;
            }

            byte[] data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                int blockIndex = (int)(openedFile.Position / BlockSize);
                int blockOffset = (int)(openedFile.Position % BlockSize);

                data[i] = (byte)openedFile.Position;

                openedFile.Position++;
            }

            return data;
        }
        else
        {
            Console.WriteLine($"File with descriptor number '{fileDescriptorNumber}' not found or not opened.");
            return Array.Empty<byte>();
        }
    }

    public void Write(int fileDescriptorNumber, byte[] data)
    {
        var openedFile = OpenedFiles.FirstOrDefault(f => f.FileDescriptorNumber == fileDescriptorNumber);

        if (openedFile != null)
        {
            FileDescriptor fileDescriptor = Descriptors[openedFile.DescriptorIndex]!;

            int remainingBytes = Bitmap.Count(b => !b) * BlockSize + BlockSize - 1 - ((((fileDescriptor.FileSize - 1) % BlockSize)+ BlockSize) % BlockSize);
            Console.WriteLine($"remainingBytes {remainingBytes}");

            if (data.Length > remainingBytes)
            {
                Console.WriteLine($"Not enough blocks available for writing {data.Length} bytes.");
                return;
            }

            for (int i = 0; i < data.Length; i++)
            {
                int blockIndex = openedFile.Position / BlockSize;
                int blockOffset = openedFile.Position % BlockSize;
                // writing data to block at index blockIndex and offset blockOffset

                int numberOfBytesInIncompleteBlock = (fileDescriptor.FileSize + i + 1) % BlockSize;
                if (numberOfBytesInIncompleteBlock == 1)
                {
                    fileDescriptor.BlockMap.Add(FindFreeBlockIndex());
                }

                openedFile.Position++;
            }

            fileDescriptor.FileSize += data.Length;

            Console.WriteLine($"Successfully wrote {data.Length} bytes to the file with descriptor number '{fileDescriptorNumber}'.");
        }
        else
        {
            Console.WriteLine($"File with descriptor number '{fileDescriptorNumber}' not found or not opened.");
        }
    }

    private int FindFreeBlockIndex()
    {
        for (int i = 0; i < Bitmap.Length; i++)
        {
            if (!Bitmap[i])
            {
                Bitmap[i] = true;
                return i;
            }
        }
        return -1;
    }

    private int FindFreeDescriptorIndex()
    {
        for (int i = 0; i < Descriptors.Count; i++)
        {
            if (Descriptors[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}