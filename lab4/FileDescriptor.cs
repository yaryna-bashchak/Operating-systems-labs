namespace lab4;

public class FileDescriptor
{
    public bool IsRegularFile { get; set; }
    public int HardLinkCount { get; set; } = 1;
    public int FileSize { get; set; } = 0;
    public List<byte?> FileData { get; set; } = new List<byte?>();
    public List<int> BlockMap { get; set; } = new List<int>();

    public FileDescriptor(bool isRegularFile = true)
    {
        IsRegularFile = isRegularFile;

        Console.WriteLine($"File descriptor for {(IsRegularFile ? "file" : "directory")} was created.");
    }
}
