namespace lab1;

public class MMU
{
    public List<PhysicalPage> FreePages;
    //public List<PhysicalPage> BusyPages; // only for Random Replacement Algolithm
    public NRUAlgorithm NRUAlgorithm; // only for NRU Algolithm

    public MMU(int numberOfPhysicalPages, uint startPageNumber)
    {
        FreePages = new List<PhysicalPage>();
        //BusyPages = new List<PhysicalPage>(); // only for Random Replacement Algolithm
        NRUAlgorithm = new NRUAlgorithm(); // only for NRU Algolithm

        for (int i = 0; i < numberOfPhysicalPages; i++)
        {
            FreePages.Add(new PhysicalPage { PPN = startPageNumber + (uint)i });
        }
    }

    public void AccessPage(VirtualPage[] pageTable, int idx, bool isModified, Kernel kernel)
    {
        PhysicalPage? physicalPage = default;

        if (!pageTable[idx].P)
        {
            physicalPage = kernel.PageFault(pageTable, idx);
        }

        pageTable[idx].R = true;

        if (isModified) pageTable[idx].M = true;

        // only for NRU Algolithm
        if (physicalPage != null)
        {
            NRUAlgorithm.AddPageToAppropriateClass(physicalPage);
        }
    }
}
