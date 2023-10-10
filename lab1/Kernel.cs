namespace lab1;

public class Kernel
{
    public MMU MemoryManager { get; private set; }
    public List<Process> Processes;
    public int NumberOfPagesFault;
    public int MaxProcessCount;
    private readonly Random Rand = new();
    private readonly List<Process> ProcessesToRemove = new();

    public Kernel(int maxProcessCount, int numberOfPhysicalPages, uint startPageNumber)
    {
        MemoryManager = new MMU(numberOfPhysicalPages, startPageNumber);
        Processes = new List<Process>();
        MaxProcessCount = maxProcessCount;
        NumberOfPagesFault = default;
    }

    public void AddProcess(Process process)
    {
        Processes.Add(process);
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
            Console.WriteLine($"Process with id {process.Id} has made {numberOfRequests} requests.");

            if (process.IsCompleted())
            {
                ProcessesToRemove.Add(process);
                Console.WriteLine($"Process with id {process.Id} was completed and removed from queue.");
            }

            // create new process if time
        }

        foreach (var process in ProcessesToRemove)
        {
            Processes.Remove(process);
        }
        ProcessesToRemove.Clear();
    }
}
