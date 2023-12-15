namespace lab4;

class Program
{
    static void Main(string[] args)
    {
        int numberOfBlocks = 20;
        int maxNumberOfDescriptors = 5;
        int maxNumberOfNumberDescriptors = 3;
        var fileSystem = new FileSystem(numberOfBlocks, maxNumberOfDescriptors, maxNumberOfNumberDescriptors);

        fileSystem.Create("file1.txt");
        fileSystem.Create("file2.txt");
        fileSystem.Ls();
        var fd1 = fileSystem.Open("file2.txt");
        // var fd2 = fileSystem.Open("file3.txt");
        // var fd3 = fileSystem.Open("file1.txt");
        // var fd4 = fileSystem.Open("file1.txt");
        // var fd5 = fileSystem.Open("file1.txt");
        // fileSystem.Close(fd4);
        // fileSystem.Close(fd1);
        // var fd6 = fileSystem.Open("file1.txt");
        // var fd7 = fileSystem.Open("file2.txt");
        fileSystem.Write(fd1, new byte[1270]);

        // foreach (var bit in fileSystem.Bitmap)
        // {
        //     Console.Write(bit + " ");
        // }
        fileSystem.Seek(fd1, 20);

        var data = fileSystem.Read(fd1, 300);
        string hexString = BitConverter.ToString(data);
        Console.WriteLine("Byte Array:");
        Console.WriteLine(hexString);


    }
}
