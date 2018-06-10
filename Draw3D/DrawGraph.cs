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
        public List<Node> Nodes = new List<Node>();

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
            int N = Nodes.Count;
            for (int i = 0; i <= N - 1; i++)
            {
                Nodes[i].Visit = false;
            }
        }

        public void ChangeBitmap(int VW, int VH)
        {
            bitmap = new Bitmap(VW, VH);
            Draw(false);
        }
        
        public void Clear()
        {
            int N = Nodes.Count;
            for (int i = 0; i < N; i++)
                Nodes[i].Edge = new List<Edge>(0);//Array.Resize(ref Nodes[i].Edge, 0);
            Nodes = new List<Node>(0);//Array.Resize(ref Nodes, 0);
        }

        public void AddNode(int x, int y) 
        {
            int N = Nodes.Count;
            Nodes.Add(new Node());
            Nodes[N].X = x;
            Nodes[N].Y = y;
        }

        public void AddEdge()
        {
            int n = -1; bool ok = false;
            int Ln = Nodes.Count;
            while ((n < Ln - 1) && !ok)
                ok = Nodes[++n] == SelectNode;
            int L = 0; //SelectNodeBeg.Edge.Count; 
            if (SelectNodeBeg.Edge != null)
                L = SelectNodeBeg.Edge.Count;
            else
            {
                SelectNodeBeg.Edge = new List<Edge>();
            }

            L++;
            //SelectNodeBeg.Edge = new List<Edge>(++L);// Array.Resize(ref SelectNodeBeg.Edge, ++L);
            SelectNodeBeg.Edge.Add(new Edge{numNode = n});
            double a1 = SelectNodeBeg.X;
            double b1 = SelectNodeBeg.Y;
            double a2 = SelectNode.X;
            double b2 = SelectNode.Y;

            SelectNodeBeg.Edge[L - 1].A = (int)Math.Sqrt((a2 - a1) * (a2 - a1) + (b2 - b1) * (b2 - b1));
            SelectNodeBeg.Edge[L - 1].x1c = X1 - SelectNodeBeg.X;
            SelectNodeBeg.Edge[L - 1].x2c = X2 - SelectNode.X;
            SelectNodeBeg.Edge[L - 1].yc = (SelectNode.Y + SelectNodeBeg.Y) / 2;
        }

        public Node FindNode(int x, int y) // найти узел
        {
            int N = Nodes.Count;
            int i = -1;
            bool Ok = false;
            while ((i < N - 1) && !Ok)
            {
                i++;
                Ok = (Nodes[i].X - hx <= x) && (x <= Nodes[i].X + hx) &&
                     (Nodes[i].Y - hy <= y) && (y <= Nodes[i].Y + hy);
            }
            if (Ok) return Nodes[i]; else return null;
        }

        public void DeSelectEdge()
        {
            int N = Nodes.Count;
            for (int i = 0; i < N; i++)
            {
                if (Nodes[i].Edge != null)
                {
                    int L = Nodes[i].Edge.Count;
                    for (int j = 0; j < L; j++)
                        Nodes[i].Edge[j].select = false;
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
                int N = Nodes.Count;

                //Line
                for (int i = 0; i < N; i++)
                {
                    // Edge
                    if (Nodes[i].Edge != null)
                    {
                        int L = Nodes[i].Edge.Count;
                        MyBrush.Color = Color.White;
                        for (int j = 0; j < L; j++)
                        {
                            if (Nodes[i].Edge[j].select)
                                MyPen = Pens.Red;
                            else
                                MyPen = new Pen(Color.Black);
                            int a1 = Nodes[i].X;
                            int b1 = Nodes[i].Y;
                            int a2 = Nodes[Nodes[i].Edge[j].numNode].X;
                            int b2 = Nodes[Nodes[i].Edge[j].numNode].Y;
                            g.DrawLine(MyPen, new Point(a1, b1), new Point(a2, b2));

                            double a = Math.Atan2(b2 - b1, a2 - a1);
                            double d = Math.Sqrt((b2 - b1) * (b2 - b1) + (a2 - a1) * (a2 - a1)) - 3;
                            int x = (int)(a1 + (d - hy) * Math.Cos(a));
                            int y = (int)(b1 + (d - hy) * Math.Sin(a));
                            g.FillEllipse(Brushes.Black, x - 3, y - 3, 6, 6);
                        }
                    }
                }

                // Nodes
                for (int i = 0; i < N; i++)
                {
                    if (Nodes[i] == SelectNode)
                        MyPen = Pens.Black;
                    else
                        MyPen = Pens.Black;
                    if (Nodes[i].Visit)
                        MyBrush.Color = Color.Silver;
                    else
                        if (Nodes[i] == SelectNode)
                        MyBrush.Color = Color.CadetBlue;
                    else
                        MyBrush.Color = Color.AliceBlue;
                    g.FillEllipse(MyBrush, Nodes[i].X - hy, Nodes[i].Y - hy, 2 * hy, 2 * hy);
                    g.DrawEllipse(MyPen, Nodes[i].X - hy, Nodes[i].Y - hy, 2 * hy, 2 * hy);
                    s = Convert.ToString(i);
                    SizeF size = g.MeasureString(s, MyFont);
                    g.DrawString(s, MyFont, Brushes.Black,
                    Nodes[i].X - size.Width / 2,
                    Nodes[i].Y - size.Height / 2);

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
            int L = Nodes.Count;
            bool ok = false; int i = -1; NumLine = -1; int j = -1;
            while ((i < L - 1) && !ok)
            {
                i++;
                if (Nodes[i].Edge != null)
                {
                    int L1 = Nodes[i].Edge.Count; j = -1;
                    while ((j < L1 - 1) && !ok)
                    {
                        j++;
                        int a1 = Nodes[i].X;
                        int b1 = Nodes[i].Y;
                        int a2 = Nodes[Nodes[i].Edge[j].numNode].X;
                        int b2 = Nodes[Nodes[i].Edge[j].numNode].Y;
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
            int L = Nodes[NumNode].Edge.Count;
            for (int i = NumEdge; i < L - 2; i++)
                Nodes[NumNode].Edge[i] = Nodes[NumNode].Edge[i + 1];
            Nodes[NumNode].Edge = new List<Edge>(L - 1);// Array.Resize(ref Nodes[NumNode].Edge, L - 1);
        }
    }
}
