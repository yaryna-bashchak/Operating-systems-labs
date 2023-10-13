namespace lab1;

public class NRUAlgorithm
{
    private readonly List<PhysicalPage>[] ClassifiedPages = new List<PhysicalPage>[4];

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

    private void ClearAllReferenceBits()
    {
        for (int i = 0; i < ClassifiedPages.Length; i++)
        {
            for (int j = 0; j < ClassifiedPages[i].Count; j++)
            {
                var physicalPage = ClassifiedPages[i][j];
                physicalPage.PageTable![physicalPage.Idx].R = false;
            }
        }
    }
}
