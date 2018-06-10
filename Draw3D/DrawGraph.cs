using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Collections;
using ClassLibrary;

namespace Draw3D
{
    public class DrawGraph
    {
        MyGraph ListN = new MyGraph();
        public Node Node;

        public static Node SelectNode;
        public static Node SelectNodeBeg;
        public Bitmap bitmap { get; set; }

        const int hx = 20, hy = 20;
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
        Font MyFont { get; set; }

        public DrawGraph(int VW, int VH)
        {
            this.bitmap = new Bitmap(VW, VH);
            this.MyFont = new Font("Segoe UI Semibold", 15, FontStyle.Bold);
        }

        public void DenyNode(int x, int y)
        {
            Node node = FindNode(x, y);
            if (node != null)
            {
                node.Visit = !node.Visit;
            }
        }

        public void ClearBlock()
        {
            int N = ListN.Nodes.Count;
            for (int i = 0; i <= N - 1; i++)
            {
                ListN.Nodes[i].Visit = false;
            }
        }

        public void ChangeBitmap(int VW, int VH)
        {
            bitmap = new Bitmap(VW, VH);
            Draw(false);
        }
        
        public void Clear()
        {
            int N = ListN.Nodes.Count;
            for (int i = 0; i < N; i++)
                ListN.Nodes[i].Edge = new List<Edge>(0);
            ListN.Nodes = new List<Node>(0);
        }

        public void AddNode(int x, int y) 
        {
            int N = ListN.Nodes.Count;
            ListN.Nodes.Add(new Node());
            ListN.Nodes[N].X = x;
            ListN.Nodes[N].Y = y;
        }

        public void AddEdge()
        {
            int n = -1; bool ok = false;
            int Ln = ListN.Nodes.Count;
            while ((n < Ln - 1) && !ok)
                ok = ListN.Nodes[++n] == SelectNode;
            int L = 0;
            if (SelectNodeBeg.Edge != null)
                L = SelectNodeBeg.Edge.Count;
            else
            {
                SelectNodeBeg.Edge = new List<Edge>();
            }
            SelectNodeBeg.Edge.Add(new Edge { numNode = n });
            double a1 = SelectNodeBeg.X;
            double b1 = SelectNodeBeg.Y;
            double a2 = SelectNode.X;
            double b2 = SelectNode.Y;

            SelectNodeBeg.Edge[L].A = (int)Math.Sqrt((a2 - a1) * (a2 - a1) + (b2 - b1) * (b2 - b1));
            SelectNodeBeg.Edge[L].x1c = X1 - SelectNodeBeg.X;
            SelectNodeBeg.Edge[L].x2c = X2 - SelectNode.X;
            SelectNodeBeg.Edge[L].yc = (SelectNode.Y + SelectNodeBeg.Y) / 2;
        }

        public Node FindNode(int x, int y) // найти узел
        {
            int N = ListN.Nodes.Count;
            int i = -1;
            bool Ok = false;
            while ((i < N - 1) && !Ok)
            {
                i++;
                Ok = (ListN.Nodes[i].X - hx <= x) && (x <= ListN.Nodes[i].X + hx) &&
                     (ListN.Nodes[i].Y - hy <= y) && (y <= ListN.Nodes[i].Y + hy);
            }
            if (Ok) return ListN.Nodes[i]; else return null;
        }

        public void DeSelectEdge()
        {
            int N = ListN.Nodes.Count;
            for (int i = 0; i < N; i++)
            {
                if (ListN.Nodes[i].Edge != null)
                {
                    int L = ListN.Nodes[i].Edge.Count;
                    for (int j = 0; j < L; j++)
                        ListN.Nodes[i].Edge[j].select = false;
                }
            }
        }

        public void Draw(bool fl) // нарисовать
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Color cl = Color.FromArgb(255, 255, 255);
                g.Clear(cl);
                Pen MyPen = Pens.Black;

                SolidBrush MyBrush = (SolidBrush)Brushes.White;
                string s;
                int N = ListN.Nodes.Count;

                //Line
                for (int i = 0; i < N; i++)
                {
                    // Edge
                    if (ListN.Nodes[i].Edge != null)
                    {
                        int L = ListN.Nodes[i].Edge.Count;
                        MyBrush.Color = Color.White;
                        for (int j = 0; j < L; j++)
                        {
                            if (ListN.Nodes[i].Edge[j].select)
                                MyPen = Pens.Red;
                            else
                                MyPen = new Pen(Color.Black);
                            int a1 = ListN.Nodes[i].X;
                            int b1 = ListN.Nodes[i].Y;
                            int a2 = ListN.Nodes[ListN.Nodes[i].Edge[j].numNode].X;
                            int b2 = ListN.Nodes[ListN.Nodes[i].Edge[j].numNode].Y;
                            g.DrawLine(MyPen, new Point(a1, b1), new Point(a2, b2));

                            double a = Math.Atan2(b2 - b1, a2 - a1);
                            double d = Math.Sqrt((b2 - b1) * (b2 - b1) + (a2 - a1) * (a2 - a1)) - 3;
                            int x = (int)(a1 + (d - hy) * Math.Cos(a));
                            int y = (int)(b1 + (d - hy) * Math.Sin(a));
                            g.FillEllipse(Brushes.Black, x - 3, y - 3, 6, 6);
                        }
                    }
                }

                // ListN.Nodes
                for (int i = 0; i < N; i++)
                {
                    if (ListN.Nodes[i] == SelectNode)
                        MyPen = Pens.Black;
                    else
                        MyPen = Pens.Black;
                    if (ListN.Nodes[i].Visit)
                        MyBrush.Color = Color.Silver;
                    else
                        if (ListN.Nodes[i] == SelectNode)
                        MyBrush.Color = Color.CadetBlue;
                    else
                        MyBrush.Color = Color.AliceBlue;
                    g.FillEllipse(MyBrush, ListN.Nodes[i].X - hy, ListN.Nodes[i].Y - hy, 2 * hy, 2 * hy);
                    g.DrawEllipse(MyPen, ListN.Nodes[i].X - hy, ListN.Nodes[i].Y - hy, 2 * hy, 2 * hy);
                    s = Convert.ToString(i);
                    SizeF size = g.MeasureString(s, MyFont);
                    g.DrawString(s, MyFont, Brushes.Black,
                    ListN.Nodes[i].X - size.Width / 2,
                    ListN.Nodes[i].Y - size.Height / 2);

                }
                if (fl)
                    g.DrawLine(Pens.Silver, new Point(X1, Y1), new Point(X2, Y2));
            }
        }

        int DistLine(int u, int v, int x1, int y1, int x2, int y2)  // расстояние до линии
        {

            int A = y2 - y1;
            int B = -x2 + x1;
            int C = -x1 * A - y1 * B;
            int D = A * A + B * B;
            if (D != 0)
                return (int)(Math.Abs(A * u + B * v + C) / Math.Sqrt(D));
            else
                return 0;
        }

        public int FindLine(int x, int y, out int NumLine)  // найти ребро
        {
            int L = ListN.Nodes.Count;
            bool ok = false; int i = -1; NumLine = -1; int j = -1;
            while ((i < L - 1) && !ok)
            {
                i++;
                if (ListN.Nodes[i].Edge != null)
                {
                    int L1 = ListN.Nodes[i].Edge.Count; j = -1;
                    while ((j < L1 - 1) && !ok)
                    {
                        j++;
                        int a1 = ListN.Nodes[i].X;
                        int b1 = ListN.Nodes[i].Y;
                        int a2 = ListN.Nodes[ListN.Nodes[i].Edge[j].numNode].X;
                        int b2 = ListN.Nodes[ListN.Nodes[i].Edge[j].numNode].Y;
                        int u1 = Math.Min(a1, a2);
                        int u2 = Math.Max(a1, a2);
                        int v1 = Math.Min(b1, b2);
                        int v2 = Math.Max(b1, b2);
                        int Eps = 4;
                        ok = (u1 - Eps <= x) && (x <= u2 + Eps) && (v1 - Eps <= y) && (y <= v2 + Eps);
                        ok = (DistLine(x, y, a1, b1, a2, b2) <= Eps) && ok;
                    }
                }
            }
            if (ok)
            {
                NumLine = j;
                return i;
            }
            else
                return -1;
        }

        public void DelEdge(int NumNode, int NumEdge)  // удалить ребро
        {
            int L = ListN.Nodes[NumNode].Edge.Count;
            for (int i = NumEdge; i < L - 2; i++)
                ListN.Nodes[NumNode].Edge[i] = ListN.Nodes[NumNode].Edge[i + 1];
            ListN.Nodes[NumNode].Edge = new List<Edge>(L - 1);
        }
    }
}
