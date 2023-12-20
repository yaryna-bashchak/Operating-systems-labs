namespace lab4;

public enum FileType { Reg, Dir, Sym }

public class FileDescriptor
{
    public FileType Type { get; set; }
    public int HardLinkCount { get; set; } = 1;
    public int FileSize { get; set; } = 0;
    public List<byte?> FileData { get; set; } = new List<byte?>();
    public List<int> BlockMap { get; set; } = new List<int>();
    public Directory Directory { get; set; } // only for directories

    public FileDescriptor(FileType type = FileType.Reg)
    {
        Type = type;

        if (Type == FileType.Dir)
        {
            Directory = new Directory();
        }

        // Console.WriteLine($"File descriptor for {Type.ToString().ToLower()} was created.");
    }
}
