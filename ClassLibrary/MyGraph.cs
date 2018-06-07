using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class MyGraph
    {
        public List<Node> Nodes { get; set; }

        public MyGraph ()
        {
            this.Nodes = new List<Node>();
        }

        public string RouteSearch(int start, int end)
        {
            return FindRoute(start, end);
        }

        string FindRoute(int n, int dest)
        {
            string result = "";
            string r = "";
            Nodes[n].Visit = true;
            if (n == dest)
                result = Convert.ToString(n);
            if (Nodes[n].Edge != null)
            {
                int L = Nodes[n].Edge.Count;
                int i = -1; result = "";
                while ((i < L - 1) && (r == ""))
                {
                    int m = Nodes[n].Edge[++i].numNode;
                    if (!Nodes[m].Visit)
                    {
                        r = FindRoute(m, dest);
                    }
                }
                if (r != "") result = Convert.ToString(n) + " " + r;
                else Nodes[n].Visit = false;
            }
            return result;
        }
    }
}
