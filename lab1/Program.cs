namespace lab1;

class Program
{
    static void Main(string[] args)
    {
        int numberOfPhysicalPages = 100;
        int maxProcessCount = 10;
        uint startPageNumber = 0x00010000;
        int startProcessCount = 5;
        int quantumOfTime = 200;
        int workingSetPercentage = 50;

        var kernel = new Kernel(
            maxProcessCount,
            numberOfPhysicalPages,
            startPageNumber,
            startProcessCount,
            quantumOfTime,
            workingSetPercentage
        );

        while (kernel.Processes.Count > 0)
        {
            kernel.Run();
        }

        Console.WriteLine($"Total requests: {kernel.TotalNumberOfRequests}.");
        Console.WriteLine($"Total page faults: {kernel.NumberOfPagesFault}.");
        Console.WriteLine($"{100 * kernel.NumberOfPagesFault / kernel.TotalNumberOfRequests}% of requests were page faults.");
    }
}
