namespace lab1;

public class Kernel
{
    //public MMU MemoryManager { get; private set; }
    public List<Process> Processes;
    public List<PhysicalPage> FreePages;
    public List<PhysicalPage> BusyPages;
    public int NumberOfPagesFault;

    public Kernel(int numberOfPhysicalPages, uint startPageNumber)
    {
        //MemoryManager = new MMU(physicalPages);
        Processes = new List<Process>();
        FreePages = new List<PhysicalPage>();
        BusyPages = new List<PhysicalPage>();
        NumberOfPagesFault = default;

        for (int i = 0; i < numberOfPhysicalPages; i++)
        {
            FreePages.Add(new PhysicalPage { PPN = startPageNumber + (uint)i });
        }
    }

    public void AddProcess(Process process)
    {
        Processes.Add(process);
    }

    public void PageFault(VirtualPage[] pageTable, int idx)
    {
        PhysicalPage physicalPage;

        if (FreePages.Count > 0)
        {
            physicalPage = FreePages[0];
            FreePages.RemoveAt(0);
            BusyPages.Add(physicalPage);
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

        int index = rand.Next(BusyPages.Count);
        PhysicalPage pageToReplace = BusyPages[index];
        Console.WriteLine($"Page Fault. Number of page to replace: {pageToReplace.PPN:X8}");

        return pageToReplace;
    }

    public void Run()
    {

    }
}
