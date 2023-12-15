namespace lab4;

class Program
{
    static void Main(string[] args)
    {
        int numberOfBlocks = 30;
        int maxNumberOfDescriptors = 5;
        var fileSystem = new FileSystem(numberOfBlocks, maxNumberOfDescriptors);

        fileSystem.Create("file1.txt");
        fileSystem.Create("file2.txt");
        fileSystem.Stat("file1.txt");
        fileSystem.Stat("file2.txt");
    }
}
