using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class Node
    {
        public List<Edge> Edge { get; set; }
        public bool Visit { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
