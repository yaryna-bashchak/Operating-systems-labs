namespace lab1;

public class Kernel
{
    public MMU MemoryManager { get; private set; }
    public List<Process> Processes;
    public int NumberOfPagesFault;

    public Kernel(int numberOfPhysicalPages, uint startPageNumber)
    {
        MemoryManager = new MMU(numberOfPhysicalPages, startPageNumber);
        Processes = new List<Process>();
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
            physicalPage.PageTable[physicalPage.Idx].P = false;
        }

        physicalPage.PageTable = pageTable;
        physicalPage.Idx = idx;
        pageTable[idx].P = true;
        pageTable[idx].PPN = physicalPage.PPN;

        NumberOfPagesFault++;
    }

    public PhysicalPage RandomReplacementAlgorithm()
    {
        Random rand = new();

        int index = rand.Next(MemoryManager.BusyPages.Count);
        PhysicalPage pageToReplace = MemoryManager.BusyPages[index];
        Console.WriteLine($"Page Fault. Number of page to replace: {pageToReplace.PPN:X8}");

        return pageToReplace;
    }

    public void Run()
    {
        
    }
}
