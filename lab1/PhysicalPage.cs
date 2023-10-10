namespace lab1;

public class PhysicalPage
{
    public uint PPN { get; set; }
    public VirtualPage[] PageTable { get; set; }
    public int Idx { get; set; }
    public int Statistics { get; set; } // поки нічого не робить
}
