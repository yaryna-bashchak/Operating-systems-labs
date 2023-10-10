namespace lab1;

public class MMU
{
    public List<PhysicalPage> FreePages;
    public List<PhysicalPage> BusyPages;

    public MMU(int numberOfPhysicalPages, uint startPageNumber)
    {
        FreePages = new List<PhysicalPage>();
        BusyPages = new List<PhysicalPage>();

        for (int i = 0; i < numberOfPhysicalPages; i++)
        {
            FreePages.Add(new PhysicalPage { PPN = startPageNumber + (uint)i });
        }
    }

    public static void AccessPage(VirtualPage[] pageTable, int idx, bool isModified, Kernel kernel)
    {
        if (!pageTable[idx].P)
        {
            kernel.PageFault(pageTable, idx);
        }

        pageTable[idx].R = true;

        if (isModified) pageTable[idx].M = true;
    }
}
