namespace lab1;

public class Kernel
{
    public MMU MemoryManager { get; private set; }
    public List<Process> Processes;
    public int NumberOfPagesFault = 0;
    public int TotalNumberOfRequests = 0;
    public int NumberOfRequestsWithThisQuantumOfTime = 0;
    public int NumberOfRequestsFromLastUpdate = 0;
    private int ProcessCount = 0;
    private readonly int StartProcessCount;
    private readonly int MaxProcessCount;
    private readonly int QuantumOfTime;
    private readonly int WorkingSetPercentage;
    private readonly int IntervalToGenerateNewWorkingSet;
    private readonly int IntervalToUpdateSomePages;
    private readonly int NumberOfPagesToUpdateEachInterval;
    private readonly Random Rand = new();

    public Kernel(
        int maxProcessCount,
        int numberOfPhysicalPages,
        uint startPageNumber,
        int startProcessCount,
        int quantumOfTime,
        int workingSetPercentage,
        int intervalToGenerateNewWorkingSet,
        int intervalToUpdateSomePages,
        int numberOfPagesToUpdateEachInterval
    )
    {
        MemoryManager = new MMU(numberOfPhysicalPages, startPageNumber);
        Processes = new List<Process>();
        StartProcessCount = startProcessCount;
        MaxProcessCount = maxProcessCount;
        QuantumOfTime = quantumOfTime;
        WorkingSetPercentage = workingSetPercentage;
        IntervalToGenerateNewWorkingSet = intervalToGenerateNewWorkingSet;
        IntervalToUpdateSomePages = intervalToUpdateSomePages;
        NumberOfPagesToUpdateEachInterval = numberOfPagesToUpdateEachInterval;

        for (int i = 0; i < startProcessCount; i++)
        {
            AddNewProcess();
        }

        Console.WriteLine($"Kernel with {numberOfPhysicalPages} number of physical pages, {startProcessCount} start processes, {maxProcessCount} max process count, {quantumOfTime} quantum of time was created.");
    }

    public void AddNewProcess()
    {
        var id = ProcessCount + 1;
        var numberOfVirtualPages = Rand.Next(30, 60);
        var requiredNumberOfRequests = Rand.Next(300, 600);

        var process = new Process(id, numberOfVirtualPages, requiredNumberOfRequests, WorkingSetPercentage);
        Processes.Add(process);
        ProcessCount++;
    }

    public PhysicalPage PageFault(VirtualPage[] pageTable, int idx)
    {
        PhysicalPage physicalPage;

        if (MemoryManager.FreePages.Count > 0)
        {
            physicalPage = MemoryManager.FreePages[0];
            MemoryManager.FreePages.RemoveAt(0);
            // MemoryManager.BusyPages.Add(physicalPage); // only for Random Replacement Algolithm
            // Console.WriteLine("Page Fault.");
        }
        else
        {
            // physicalPage = RandomReplacementAlgorithm(); // only for Random Replacement Algolithm
            physicalPage = MemoryManager.NRUAlgorithm.NRUReplacementAlgorithm(); // only for NRU Algolithm
            physicalPage.PageTable![physicalPage.Idx].P = false;
        }

        physicalPage.PageTable = pageTable;
        physicalPage.Idx = idx;
        pageTable[idx].P = true;
        pageTable[idx].M = false;
        pageTable[idx].PPN = physicalPage.PPN;

        NumberOfPagesFault++;
        return physicalPage;
    }

    // only for Random Replacement Algolithm
    // public PhysicalPage RandomReplacementAlgorithm()
    // {
    //     int index = Rand.Next(MemoryManager.BusyPages.Count);
    //     PhysicalPage pageToReplace = MemoryManager.BusyPages[index];
    //     // Console.WriteLine($"Page Fault. Number of page to replace: {pageToReplace.PPN:X8}");

    //     return pageToReplace;
    // }

    public void Run()
    {
        for (int i = 0; i < Processes.Count; i++)
        {
            var process = Processes[i];

            if (process.WorkingSet.CurrentNumberOfRequests >= IntervalToGenerateNewWorkingSet)
            {
                process.WorkingSet.GenerateNewSet();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("New working set");
                Console.ResetColor();
                Console.Write($" for process ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{process.Id}");
                Console.ResetColor();
                Console.WriteLine($" was generated: {string.Join(", ", process.WorkingSet.IndexesSet)}");
            }

            int numberOfRequests = Rand.Next(40, 60);
            int numberOfPagesFaultBefore = NumberOfPagesFault;

            for (int j = 0; j < numberOfRequests; j++)
            {
                int idx;

                if (Rand.Next(100) < 90)
                {
                    idx = process.WorkingSet.IndexesSet[Rand.Next(process.WorkingSet.IndexesSet.Count)];
                }
                else
                {
                    idx = Rand.Next(process.PageTable.Length);
                }

                bool isModified = Rand.Next(10) <= 2; // modify/not-modify = 30/70
                MemoryManager.AccessPage(process.PageTable, idx, isModified, this);
            }

            process.IncreaseCurrentRequestsCount(numberOfRequests);
            TotalNumberOfRequests += numberOfRequests;
            NumberOfRequestsWithThisQuantumOfTime += numberOfRequests;
            NumberOfRequestsFromLastUpdate += numberOfRequests;

            int pageFaults = NumberOfPagesFault - numberOfPagesFaultBefore;
            PrintInfo(process, numberOfRequests, pageFaults);

            if (process.IsCompleted())
            {
                Processes.Remove(process);
                i--;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Finished");
                Console.ResetColor();
                Console.Write($": process with id ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{process.Id}");
                Console.ResetColor();
                Console.WriteLine(" was completed and removed from queue.");
            }

            if (IsTimeToCreateNewProcess())
            {
                AddNewProcess();
            }

            // only for NRU Algolithm
            if (IsItNewQuantumOfTime())
            {
                MemoryManager.NRUAlgorithm.ClearAllReferenceBits();
                IsTimeToUpdateSomePages();
            }
            else if (IsTimeToUpdateSomePages())
            {
                MemoryManager.NRUAlgorithm.UpdateNPages(NumberOfPagesToUpdateEachInterval);
            }
        }
    }

    private bool IsTimeToUpdateSomePages()
    {
        if (NumberOfRequestsFromLastUpdate >= IntervalToUpdateSomePages)
        {
            NumberOfRequestsFromLastUpdate -= IntervalToUpdateSomePages;
            return true;
        }
        return false;
    }

    private bool IsItNewQuantumOfTime()
    {
        if (NumberOfRequestsWithThisQuantumOfTime >= QuantumOfTime)
        {
            NumberOfRequestsWithThisQuantumOfTime -= QuantumOfTime;
            return true;
        }
        return false;
    }

    private bool IsTimeToCreateNewProcess()
    {
        bool isNewProcessNeeded = ProcessCount < MaxProcessCount;
        bool isNewProcessNeededJustNow = TotalNumberOfRequests - QuantumOfTime * (ProcessCount - StartProcessCount) >= QuantumOfTime;
        return isNewProcessNeeded && isNewProcessNeededJustNow;
    }

    private void PrintInfo(Process process, int numberOfRequests, int pageFaults)
    {
        Console.WriteLine($"Process with id {process.Id} has made {numberOfRequests} requests, {pageFaults} of which were page faults.");
        // Console.WriteLine("{0,6} {1,6} {2,6} {3,6}", "P", "M", "R", "PPN");
        // for (int i = 0; i < process.PageTable.Length; i++)
        // {
        //     Console.WriteLine("{0,6} {1,6} {2,6} {3,6}", process.PageTable[i].P, process.PageTable[i].M, process.PageTable[i].R, process.PageTable[i].PPN);
        // }
        if (MemoryManager.FreePages.Count != 0)
        {
            //Console.WriteLine($"Busy pages: {MemoryManager.BusyPages.Count}."); // only for Random Replacement Algolithm
            Console.WriteLine($"Busy pages: {MemoryManager.NRUAlgorithm.ClassifiedPages.Select(list => list.Count).Sum()}."); // only for NRU Algolithm
            Console.WriteLine($"Free pages: {MemoryManager.FreePages.Count}.");
        }
    }
}
