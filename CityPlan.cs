using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace city_planner
{
    public partial class CityPlan : UserControl
    {
        readonly double vertex_radius = 10;
        readonly double road_width = 3;

        private int firstX = -1;
        private int firstY = -1;

        private List<Node> listNodes = new List<Node>();
        private List<Road> listRoads = new List<Road>();

        public int FirstX { get { return firstX; } set { firstX = value; } }
        public int FirstY { get { return firstY; } set { firstY = value; } }
        public CityPlan()
        {
            InitializeComponent();
        }

        public event EventHandler addPoint;
        public event EventHandler addLine;

        private void CityPlan_MouseClick(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var y = e.Y;
            var database = Database.GetInstance();
            bool roadValid = false;

            //listNodes = Node.select_star();
            //listRoads = Road.select_star();
            
            if(addPoint != null)
            {
                addPoint(this, EventArgs.Empty);
                Node insertingNode = new Node(x, y);
                listNodes.Add(insertingNode);
                drawNode(insertingNode);
            }

            if(addLine != null)
            {
                if (firstX == -1)
                {
                    int i = 0; 
                    //check if the second end of the road is in the list of all nodes
                    for(i = 0; i < listNodes.Count; i++)
                    {
                        if (Math.Abs(x - listNodes[i].X) < vertex_radius/2 && Math.Abs(y - listNodes[i].Y) < vertex_radius/2)
                        {
                            roadValid = true;
                            break;
                        }
                    }
                    if (roadValid)
                    {
                        firstX = (int)listNodes[i].X;
                        firstY = (int)listNodes[i].Y;
                    }
                }
                else
                {
                    int i = 0;
                    //check if the second end of the road is in the list of all nodes
                    for (i = 0; i < listNodes.Count; i++)
                    {
                        if (Math.Abs(x - listNodes[i].X) < vertex_radius/2 && Math.Abs(y - listNodes[i].Y) < vertex_radius/2
                                && (Math.Abs(x - firstX) > vertex_radius/2 || Math.Abs(y - firstY) > vertex_radius/2))
                        {
                            roadValid = true;
                            break;
                        }
                    }
                    if (roadValid)
                    {
                        addLine(this, EventArgs.Empty);

                        Node Node1 = new Node(firstX, firstY);
                        Node Node2 = new Node(listNodes[i].X, listNodes[i].Y);
                        Road insertingRoad = new Road(Node1.Id, Node2.Id);
                        listRoads.Add(insertingRoad);
                        drawLine(Node1, Node2);
                        firstX = -1;
                        firstY = -1;
                    }
                }
            }
        }


        void drawNode(Node insertingNode)
        {
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.FillEllipse(Brushes.Black, insertingNode.X - (int)vertex_radius / 2, insertingNode.Y - (int)vertex_radius / 2, (float)vertex_radius, (float)vertex_radius);
        }

        void drawLine(Node Node1,Node Node2)
        {
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), (float)road_width);
            g.DrawLine(blackPen, Node1.X, Node1.Y, Node2.X, Node2.Y);
        }

    }
}
