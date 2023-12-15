namespace lab4;

public class FileSystem
{
    // protected BlockStorage Storage { get; set; }
    public bool[] Bitmap { get; set; }
    public List<FileDescriptor?> Descriptors { get; set; }
    public Directory Directory { get; set; }

    public FileSystem(int numberOfBlocks, int maxNumberOfDescriptors)
    {
        Bitmap = new bool[numberOfBlocks];
        Descriptors = new List<FileDescriptor?>(maxNumberOfDescriptors);
        Directory = new();

        for (int i = 0; i < maxNumberOfDescriptors; i++)
        {
            Descriptors.Add(null);
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