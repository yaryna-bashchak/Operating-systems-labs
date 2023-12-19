namespace lab4;

public class DirectoryEntry
{
    public string FileName { get; set; }
    public int FileDescriptorIndex { get; set; }

    public DirectoryEntry(string fileName, int fileDescriptorIndex)
    {
        FileName = fileName;
        FileDescriptorIndex = fileDescriptorIndex;
    }
}
