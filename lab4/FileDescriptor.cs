namespace lab4;

public class FileDescriptor
{
    public bool IsRegularFile { get; set; }
    public int HardLinkCount { get; set; } = 1;
    public long FileSize { get; set; }
    public List<int> BlockMap { get; set; }

    public FileDescriptor(bool[] bitmap, bool isRegularFile = true)
    {
        IsRegularFile = isRegularFile;
        BlockMap = new List<int>();

        if (isRegularFile)
        {
            Random random = new Random();
            int numberOfBlocks = random.Next(1, 7);

            for (int i = 0; i < numberOfBlocks; i++)
            {
                int blockIndex = FindFreeBlockIndex(bitmap);
                if (blockIndex != -1)
                {
                    BlockMap.Add(blockIndex);
                }
            }

            FileSize = 64 * numberOfBlocks;
        }

        Console.WriteLine($"File descriptor for {(IsRegularFile ? "file" : "directory")} was created with file size {FileSize} and {BlockMap.Count} blocks.");
    }

    private int FindFreeBlockIndex(bool[] bitmap)
    {
        for (int i = 0; i < bitmap.Length; i++)
        {
            if (!bitmap[i])
            {
                bitmap[i] = true;
                return i;
            }
        }
        return -1;
    }
}
