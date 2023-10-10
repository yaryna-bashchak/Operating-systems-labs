namespace lab1;

class Program
{
    static void Main(string[] args)
    {
        int numberOfPhysicalPages = 15;
        int maxProcessCount = 5;
        uint startPageNumber = 0x00010000;

        var kernel = new Kernel(maxProcessCount, numberOfPhysicalPages, startPageNumber);

        var p1 = new Process(1, 20, 100);
        var p2 = new Process(2, 25, 100);

        kernel.AddProcess(p1);
        kernel.AddProcess(p2);

        kernel.Run();
        kernel.Run();
    }
}
