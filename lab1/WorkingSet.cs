namespace lab1;
public class WorkingSet
{
    public int SizeOfTable { get; set; }
    public int WorkingSetPercentage { get; set; }
    public int CurrentNumberOfRequests { get; set; } = 0;
    public List<int> IndexesSet { get; set; }

    public WorkingSet(int sizeOfTable, int workingSetPercentage)
    {
        SizeOfTable = sizeOfTable;
        WorkingSetPercentage = workingSetPercentage;
        IndexesSet = new List<int>();
        GenerateNewSet();
    }

    public void GenerateNewSet()
    {
        var rand = new Random();
        var allIndexes = Enumerable.Range(0, SizeOfTable).ToList();
        var shuffledNumbers = allIndexes.OrderBy(x => rand.Next()).ToList();

        var sizeOfSet = (int)Math.Floor((double)(SizeOfTable * WorkingSetPercentage / 100));
        IndexesSet = shuffledNumbers.Take(sizeOfSet).ToList();

        CurrentNumberOfRequests = 0;
    }
}
