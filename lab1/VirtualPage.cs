using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab1
{
    public class VirtualPage
    {
        public bool P { get; set; } // IsPresent
        public bool M { get; set; } // IsModified
        public bool R { get; set; } // IsReferenced
        public uint PPN { get; set; } // PhysicalPageNumber
    }
}