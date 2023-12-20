namespace lab4;

public enum FileType { Reg, Dir, Sym }

public class FileDescriptor
{
    public FileType Type { get; set; }
    public int HardLinkCount { get; set; } = 1;
    public int FileSize { get; set; } = 0; // only for regular files
    public List<byte?> FileData { get; set; } = new List<byte?>(); // only for regular files
    public List<int> BlockMap { get; set; } = new List<int>(); // only for regular files
    public Directory Directory { get; set; } // only for directories
    public string SymLinkTarget { get; set; } = ""; // only for symbolic links

    public FileDescriptor(FileType type = FileType.Reg)
    {
        Type = type;

        if (Type == FileType.Dir)
        {
            Directory = new Directory();
        }
    }
}
