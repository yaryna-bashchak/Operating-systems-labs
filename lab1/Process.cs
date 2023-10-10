namespace lab1;

public class Process
{
    public int Id { get; private set; }
    public VirtualPage[] PageTable { get; private set; }

    public int RequiredNumberOfRequests { get; private set; }
    public int CurrentNumberOfRequests { get; private set; } = 0;

    public Process(int id, int numberOfVirtualPages, int requiredNumberOfMemoryAccesses)
    {
        Id = id;
        RequiredNumberOfRequests = requiredNumberOfMemoryAccesses;
        PageTable = new VirtualPage[numberOfVirtualPages];

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
    }

    public void IncreaseCurrentRequestsCount(int numberOfRequests) => CurrentNumberOfRequests += numberOfRequests;
    
    public bool IsCompleted() => CurrentNumberOfRequests >= RequiredNumberOfRequests;
}
