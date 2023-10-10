namespace lab1;

public class Kernel
{
    public MMU MemoryManager { get; private set; }
    public List<Process> Processes;
    public int NumberOfPagesFault = 0;
    public int TotalNumberOfRequests = 0;
    private int ProcessCount = 0;
    private readonly int StartProcessCount;
    private readonly int MaxProcessCount;
    private readonly int QuantumOfTime;
    private readonly Random Rand = new();
    private readonly List<Process> ProcessesToAdd = new();
    private readonly List<Process> ProcessesToRemove = new();

    public Kernel(
        int maxProcessCount,
        int numberOfPhysicalPages,
        uint startPageNumber,
        int startProcessCount,
        int quantumOfTime
    )
    {
        MemoryManager = new MMU(numberOfPhysicalPages, startPageNumber);
        Processes = new List<Process>();
        StartProcessCount = startProcessCount;
        MaxProcessCount = maxProcessCount;
        QuantumOfTime = quantumOfTime;

        for (int i = 0; i < startProcessCount; i++)
        {
            var process = GenerateNewProcess();
            Processes.Add(process);
        }

        Console.WriteLine($"Kernel with {numberOfPhysicalPages} number of physical pages, {startProcessCount} start processes, {maxProcessCount} max process count, {quantumOfTime} quantum of time was created.");
    }

    public Process GenerateNewProcess()
    {
        var id = ProcessCount + 1;
        var numberOfVirtualPages = Rand.Next(30, 60);
        var requiredNumberOfRequests = Rand.Next(50, 130);
        ProcessCount++;

        return new Process(id, numberOfVirtualPages, requiredNumberOfRequests);
    }

    public void PageFault(VirtualPage[] pageTable, int idx)
    {
        PhysicalPage physicalPage;

        if (MemoryManager.FreePages.Count > 0)
        {
            physicalPage = MemoryManager.FreePages[0];
            MemoryManager.FreePages.RemoveAt(0);
            MemoryManager.BusyPages.Add(physicalPage);
        }
        else
        {
            physicalPage = RandomReplacementAlgorithm();
            physicalPage.PageTable![physicalPage.Idx].P = false;
        }

        physicalPage.PageTable = pageTable;
        physicalPage.Idx = idx;
        pageTable[idx].P = true;
        pageTable[idx].PPN = physicalPage.PPN;

        NumberOfPagesFault++;
    }

    public PhysicalPage RandomReplacementAlgorithm()
    {
        int index = Rand.Next(MemoryManager.BusyPages.Count);
        PhysicalPage pageToReplace = MemoryManager.BusyPages[index];
        Console.WriteLine($"Page Fault. Number of page to replace: {pageToReplace.PPN:X8}");

        return pageToReplace;
    }

    public void Run()
    {
        foreach (var process in Processes)
        {
            // change working set if needed

            int numberOfRequests = Rand.Next(40, 60);
            for (int i = 0; i < numberOfRequests; i++)
            {
                // 90/10 from working set and from all
                int idx = Rand.Next(process.PageTable.Length);

                bool isModified = Rand.Next(10) <= 2;
                MMU.AccessPage(process.PageTable, idx, isModified, this);
            }

            process.IncreaseCurrentRequestsCount(numberOfRequests);
            TotalNumberOfRequests += numberOfRequests;
            Console.WriteLine($"Process with id {process.Id} has made {numberOfRequests} requests.");

            if (process.IsCompleted())
            {
                ProcessesToRemove.Add(process);
                Console.WriteLine($"Process with id {process.Id} was completed and removed from queue.");
            }

            if (IsTimeToCreateNewProcess())
            {
                ProcessesToAdd.Add(GenerateNewProcess());
            }
        }

        foreach (var process in ProcessesToRemove)
        {
            Processes.Remove(process);
        }
        ProcessesToRemove.Clear();

        foreach (var process in ProcessesToAdd)
        {
            Processes.Add(process);
        }
        ProcessesToAdd.Clear();
    }

    private bool IsTimeToCreateNewProcess()
    {
        bool isNewProcessNeeded = ProcessCount < MaxProcessCount;
        bool isNewProcessNeededJustNow = TotalNumberOfRequests - QuantumOfTime * (ProcessCount - StartProcessCount) >= QuantumOfTime;
        return isNewProcessNeeded && isNewProcessNeededJustNow;
    }
}
