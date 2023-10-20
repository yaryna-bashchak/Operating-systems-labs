namespace lab1;

class Program
{
    static void Main(string[] args)
    {
        int numberOfPhysicalPages = 100;
        int maxProcessCount = 10;
        uint startPageNumber = 0x00010000;
        int startProcessCount = 4;
        int quantumOfTime = 500;
        int intervalToGenerateNewWorkingSet = 200;
        int intervalToUpdateSomePages = 150;
        int workingSetPercentage = 50;

        var kernel = new Kernel(
            maxProcessCount,
            numberOfPhysicalPages,
            startPageNumber,
            startProcessCount,
            quantumOfTime,
            workingSetPercentage,
            intervalToGenerateNewWorkingSet,
            intervalToUpdateSomePages
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
