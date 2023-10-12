namespace lab1;

public class Process
{
    public int Id { get; private set; }
    public VirtualPage[] PageTable { get; private set; }
    public WorkingSet WorkingSet { get; private set; }
    public int RequiredNumberOfRequests { get; private set; }
    public int CurrentNumberOfRequests { get; private set; } = 0;

    public Process(int id, int numberOfVirtualPages, int requiredNumberOfRequests, int workingSetPercentage)
    {
        Id = id;
        RequiredNumberOfRequests = requiredNumberOfRequests;
        PageTable = new VirtualPage[numberOfVirtualPages];
        WorkingSet = new WorkingSet(numberOfVirtualPages, (int)Math.Floor((double)(numberOfVirtualPages * workingSetPercentage / 100)));

        for (int i = 0; i < numberOfVirtualPages; i++)
        {
            PageTable[i] = new VirtualPage
            {
                P = false,
                M = false,
                R = false,
                PPN = 0,
            };
        }

        Console.WriteLine($"Process with id {id}, {numberOfVirtualPages} virtual pages, {WorkingSet.SizeOfSet} size of working set, {requiredNumberOfRequests} required number of requests was created.");
    }

    public void IncreaseCurrentRequestsCount(int numberOfRequests) => CurrentNumberOfRequests += numberOfRequests;
    
    public bool IsCompleted() => CurrentNumberOfRequests >= RequiredNumberOfRequests;
}
