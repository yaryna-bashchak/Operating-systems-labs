namespace lab1;
public class WorkingSet
{
    public int SizeOfTable { get; set; }
    public int SizeOfSet { get; set; }
    public List<int> IndexesSet { get; set; }

    public WorkingSet(int sizeOfTable, int sizeOfSet)
    {
        SizeOfTable = sizeOfTable;
        SizeOfSet = sizeOfSet;
        IndexesSet = GenerateNewSet();
    }

    public List<int> GenerateNewSet()
    {
        var rand = new Random();
        var allIndexes = Enumerable.Range(0, SizeOfTable).ToList();
        var shuffledNumbers = allIndexes.OrderBy(x => rand.Next()).ToList();

        return shuffledNumbers.Take(SizeOfSet).ToList();
    }
}
