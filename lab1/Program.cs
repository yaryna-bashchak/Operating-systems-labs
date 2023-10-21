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
        int workingSetPercentage = 70; // specifies size of working set as percentage of total number of virtual pages of process
        int intervalToGenerateNewWorkingSet = 200;
        int intervalToUpdateSomePages = 150;
        int numberOfPagesToUpdateEachInterval = 20;

        var kernel = new Kernel(
            maxProcessCount,
            numberOfPhysicalPages,
            startPageNumber,
            startProcessCount,
            quantumOfTime,
            workingSetPercentage,
            intervalToGenerateNewWorkingSet,
            intervalToUpdateSomePages,
            numberOfPagesToUpdateEachInterval
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
