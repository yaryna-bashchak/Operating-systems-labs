namespace lab1;

public class NRUAlgorithm
{
    public readonly List<PhysicalPage>[] ClassifiedPages = new List<PhysicalPage>[4];
    public NRUAlgorithm()
    {
        for (int i = 0; i < 4; i++)
        {
            ClassifiedPages[i] = new List<PhysicalPage>();
        }
    }

    public void AddPageToAppropriateClass(PhysicalPage page)
    {
        int index = CalculatePageClass(page);
        ClassifiedPages[index].Add(page);
    }

    public PhysicalPage NRUReplacementAlgorithm()
    {
        for (int i = 0; i < ClassifiedPages.Length; i++)
        {
            for (int j = 0; j < ClassifiedPages[i].Count; j++)
            {
                var physicalPage = ClassifiedPages[i][j];

                ClassifiedPages[i].RemoveAt(j);
                j--;

                if (CalculatePageClass(physicalPage) == i)
                {
                    //Console.WriteLine($"Page Fault. Number of page to replace: {physicalPage.PPN:X8}");
                    return physicalPage;
                }
                else
                {
                    AddPageToAppropriateClass(physicalPage);
                }
            }
        }

        throw new Exception("No pages to replace!");
    }

    public int CalculatePageClass(PhysicalPage page)
    {
        var virtualPage = page.PageTable![page.Idx];

        int index = 0;
        if (virtualPage.R) index += 2;
        if (virtualPage.M) index += 1;

        return index;
    }

    public void ClearAllReferenceBits()
    {
        for (int i = 0; i < ClassifiedPages.Length; i++)
        {
            for (int j = 0; j < ClassifiedPages[i].Count; j++)
            {
                var physicalPage = ClassifiedPages[i][j];
                if (physicalPage.PageTable![physicalPage.Idx].R)
                {
                    physicalPage.PageTable![physicalPage.Idx].R = false;
                    ClassifiedPages[i].RemoveAt(j);
                    j--;
                    AddPageToAppropriateClass(physicalPage);
                }
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"All reference bits were cleared.");
        Console.ResetColor();
    }
    }
}
