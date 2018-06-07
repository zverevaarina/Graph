using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;

namespace Draw3D
{
    public partial class FormGraph : Form
    {
        DrawGraph dgr;
        public static byte flTools = 0;
        bool drawing = false;
        Graphics g;
        public List<Node> Nodes;

        public FormGraph()
        {
            InitializeComponent();
            dgr = new DrawGraph(ClientRectangle.Width, ClientRectangle.Height);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
        }

        public void MyDraw(bool fl)
        {
            dgr.Draw(fl);
            g.DrawImage(dgr.bitmap, ClientRectangle);
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            MyDraw(false);
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            switch (flTools)
            {
                case 0:
                    DrawGraph.SelectNode = dgr.FindNode(e.X, e.Y);
                    drawing = DrawGraph.SelectNode != null;
                    break;
                case 1:
                    dgr.AddNode(e.X, e.Y);
                    MyDraw(false);
                    break;
                case 2:
                    DrawGraph.SelectNodeBeg = dgr.FindNode(e.X, e.Y);
                    drawing = DrawGraph.SelectNodeBeg != null;
                    dgr.X1 = e.X; dgr.Y1 = e.Y;
                    dgr.X2 = e.X; dgr.Y2 = e.Y;
                    break;
                case 3:
                    dgr.DeSelectEdge();
                    int NumLine = -1;
                    int NumNode = dgr.FindLine(e.X, e.Y, out NumLine);
                    if (NumNode != -1)
                    {
                        dgr.DelEdge(NumNode, NumLine);
                        MyDraw(false);
                    }
                    break;
                case 4:
                    dgr.DenyNode(e.X, e.Y);
                    MyDraw(false);
                    break;
            }
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                switch (flTools)
                {
                    case 0:
                        DrawGraph.SelectNode.X = e.X;
                        DrawGraph.SelectNode.Y = e.Y;
                        MyDraw(false);
                        break;
                    case 2:
                        dgr.X2 = e.X; dgr.Y2 = e.Y;
                        MyDraw(true);
                        break;
                }
            }
            else
            {
                switch (flTools)
                {
                    case 0:
                    case 2:
                    case 5:
                        DrawGraph.SelectNode = dgr.FindNode(e.X, e.Y);
                        MyDraw(false);
                        break;
                    case 3:
                        dgr.DeSelectEdge();
                        int NumLine = -1;
                        int NumNode = dgr.FindLine(e.X, e.Y, out NumLine);
                        if (NumNode != -1)
                        {
                            Nodes[NumNode].Edge[NumLine].select = true;
                            MyDraw(false);
                        }
                        break;
                }
            }
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
            switch (flTools)
            {
                case 2:
                    DrawGraph.SelectNode = dgr.FindNode(e.X, e.Y);
                    if ((DrawGraph.SelectNode != null) && (DrawGraph.SelectNode != DrawGraph.SelectNodeBeg))
                    {
                        dgr.AddEdge();
                        MyDraw(false);
                    }
                    break;
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (dgr != null)
                dgr.ChangeBitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            flTools = Convert.ToByte((sender as RadioButton).Tag);
        }

        private void buttonRoute_Click(object sender, EventArgs e)
        {
            MyGraph gr = new MyGraph();
            string s = gr.RouteSearch(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            if (s == "") s = "Маршрута не существует";
            labelRoute.Text = s;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dgr.ClearBlock();
        }
    }
}
