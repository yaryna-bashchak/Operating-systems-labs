namespace lab4;

public class Directory
{
    public List<DirectoryEntry> Entries { get; } = new List<DirectoryEntry>();

    public bool Contains(string fileName)
    {
        return Entries.Any(entry => entry.FileName == fileName);
    }

    public void AddEntry(string fileName, int fileDescriptorIndex)
    {
        Entries.Add(new DirectoryEntry(fileName, fileDescriptorIndex));
        Console.WriteLine($"File with name '{fileName}' was created, descriptor index: {fileDescriptorIndex}");
    }

    public int FindDescriptorIndex(string filename)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (Entries[i].FileName == filename)
            {
                return Entries[i].FileDescriptorIndex;
            }
        }
        return -1;
    }

    public void Ls()
    {
        Console.WriteLine("Directory Listing:");
        foreach (var entry in Entries)
        {
            Console.WriteLine($"{entry.FileName}\t=> reg, {entry.FileDescriptorIndex}");
        }
    }
}