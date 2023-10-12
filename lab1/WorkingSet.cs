namespace lab1;
public class WorkingSet
{
    public int SizeOfTable { get; set; }
    public int WorkingSetPercentage { get; set; }
    public List<int> IndexesSet { get; set; }

    public WorkingSet(int sizeOfTable, int workingSetPercentage)
    {
        SizeOfTable = sizeOfTable;
        WorkingSetPercentage = workingSetPercentage;
        IndexesSet = GenerateNewSet();
    }

    public List<int> GenerateNewSet()
    {
        var rand = new Random();
        var allIndexes = Enumerable.Range(0, SizeOfTable).ToList();
        var shuffledNumbers = allIndexes.OrderBy(x => rand.Next()).ToList();

        var sizeOfSet = (int)Math.Floor((double)(SizeOfTable * WorkingSetPercentage / 100));
        return shuffledNumbers.Take(sizeOfSet).ToList();
    }
}
