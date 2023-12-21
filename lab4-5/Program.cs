namespace lab4;

class Program
{
    static void Main(string[] args)
    {
        int maxNumberOfDescriptors = 10;
        var fileSystem = new FileSystem(maxNumberOfDescriptors);

        fileSystem.MakeDirectory("dir1");
        fileSystem.MakeDirectory("/dir2");
        fileSystem.MakeDirectory("/dir1/dir3");
        fileSystem.Create("/dir1/file1");
        fileSystem.Create("dir1/file2");
        fileSystem.Ls();
        fileSystem.Ls("/");
        fileSystem.Stat();
        fileSystem.Stat("/");
        fileSystem.Stat("dir1/");
        fileSystem.Stat("dir1");
        // fileSystem.ChangeDirectory("dir1");
        // fileSystem.Create("file3");

        // symlink with regular file
        fileSystem.Ls();
        fileSystem.CreateSymlink("dir1/file1", "sym_file1");
        fileSystem.Ls();
        fileSystem.Stat("sym_file1");
        fileSystem.Stat("dir1/file1");

        var fd1 = fileSystem.Open("sym_file1");
        fileSystem.Truncate("dir1/file1", 128);
        byte[] byteArray = new byte[20];

        for (int i = 0; i < byteArray.Length; i++)
        {
            byteArray[i] = (byte)i;
        }

        fileSystem.Write(fd1, byteArray);
        fileSystem.Stat("sym_file1");
        fileSystem.Stat("dir1/file1");

        // fileSystem.Close(fd1);

        // symlink with directories
        fileSystem.Ls();
        fileSystem.CreateSymlink("dir1/dir3", "dir2/sym_dir3");
        fileSystem.Ls("dir2");

        var fd2 = fileSystem.Open("dir2/sym_dir3/./file1"); // doesn't exist
        var fd3 = fileSystem.Open("dir2/sym_dir3/../file1"); // exists

        // more complex example
        fileSystem.CreateSymlink("/dir2", "dir1/sym_dir2");
        fileSystem.Stat("dir2/sym_dir3/../sym_dir2");
        fileSystem.Stat("dir2");

        // fileSystem.Ls("dir3/");
        // fileSystem.Stat();
        // fileSystem.Stat("/");
        // fileSystem.Stat("/dir1");
        // fileSystem.Stat("/dir1/");
        // fileSystem.Stat("dir3");
        // fileSystem.Stat("file3");
        // fileSystem.Stat("./file3");
        // fileSystem.Stat("../dir1");
        // fileSystem.Stat("../file1");
        // fileSystem.Stat("/dir1/file3");


        // fileSystem.ChangeDirectory("/");
        // fileSystem.Ls("dir1");
        // fileSystem.RemoveDirectory("dir1");
        // fileSystem.Unlink("dir1/file1");
        // fileSystem.Unlink("dir1/file2");
        // fileSystem.Unlink("dir1/file3");
        // fileSystem.RemoveDirectory("dir1/dir3");
        // fileSystem.Ls("dir1");
        // fileSystem.RemoveDirectory("dir1");

        // fileSystem.Create("file1.txt");
        // fileSystem.Create("file2.txt");
        // fileSystem.Truncate("file2.txt", 128);

        // fileSystem.Create("file3.txt");
        // fileSystem.Create("file4.txt");
        // fileSystem.Create("file5.txt");
        // fileSystem.Ls();
        // fileSystem.Link("file2.txt", "file3.txt");
        // fileSystem.Stat("file2.txt");
        // var fd1 = fileSystem.Open("file2.txt");
        // fileSystem.Unlink("file2.txt");
        // fileSystem.Unlink("file3.txt");

        // byte[] byteArray = new byte[20];

        // for (int i = 0; i < byteArray.Length; i++)
        // {
        //     byteArray[i] = (byte)i;
        // }
        // fileSystem.Write(fd1, byteArray);

        // fileSystem.Close(fd1);

        // var fd2 = fileSystem.Open("nonExistedFile.txt");
        // var fd3 = fileSystem.Open("file1.txt");
        // var fd4 = fileSystem.Open("file1.txt");
        // var fd5 = fileSystem.Open("file1.txt");
        // fileSystem.Close(fd4);
        // fileSystem.Close(fd1);
        // var fd6 = fileSystem.Open("file1.txt");
        // var fd7 = fileSystem.Open("file2.txt");
        // fileSystem.Truncate("file2.txt", 128);
        // fileSystem.Seek(fd1, 64);

        // byte[] byteArray = new byte[20];

        // for (int i = 0; i < byteArray.Length; i++)
        // {
        //     byteArray[i] = (byte)i;
        // }
        // fileSystem.Write(fd1, byteArray);
        // fileSystem.Stat("file2.txt");
        // fileSystem.Truncate("file2.txt", 60);
        // fileSystem.Stat("file2.txt");

        // fileSystem.Seek(fd1, 20);

        // var data = fileSystem.Read(fd1, 300);
        // Console.WriteLine("Byte Array:");
        // foreach (var item in data)
        // {
        //     if (item == null)
        //         Console.Write("\\0 ");
        //     else
        //         Console.Write(item + " ");
        // }
        // Console.WriteLine();
    }
}
