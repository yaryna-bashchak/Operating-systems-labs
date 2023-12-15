namespace lab4;

public class FileSystem
{
    // protected BlockStorage Storage { get; set; }
    public bool[] Bitmap { get; set; }
    public List<FileDescriptor?> Descriptors { get; set; }
    private List<int> FreeFileDescriptorNumbers { get; set; } = new List<int>();
    private List<OpenedFile> OpenedFiles { get; set; } = new List<OpenedFile>();
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
            Descriptors[rootDescriptorIndex] = new FileDescriptor(Bitmap, false);
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

        var fileDescriptor = new FileDescriptor(Bitmap);
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