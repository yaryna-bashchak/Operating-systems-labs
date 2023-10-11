namespace lab1;
public class WorkingSet
{
    public int Size { get; set; }
    public List<int> IndexesSet { get; set; }

    public WorkingSet(int size)
    {
        Size = size;
        IndexesSet = GenerateNewSet();
    }

    public List<int> GenerateNewSet()
    {
        var rand = new Random();
        var allIndexes = Enumerable.Range(0, Size).ToList();
        var shuffledNumbers = allIndexes.OrderBy(x => rand.Next()).ToList();

        return shuffledNumbers.Take(Size).ToList();
    }
}
