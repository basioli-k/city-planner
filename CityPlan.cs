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
        private long id;

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
            int halfR = (int)vertex_radius / 2;

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
                        id = listNodes[i].Id;
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
                        
                        drawLine(firstX , firstY, (int)listNodes[i].X, (int)listNodes[i].Y);
                        firstX = -1;
                        firstY = -1;
                        id = -1;
                    }
                }
            }
        }


        void drawNode(Node insertingNode)
        {
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawEllipse(Pens.Black, insertingNode.X - (int)vertex_radius / 2, insertingNode.Y - (int)vertex_radius / 2, (float)vertex_radius, (float)vertex_radius);
        }

        void drawLine(int firstX, int firstY, int secondX, int secondY)
        {
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawLine(Pens.Black, firstX, firstY, secondX, secondY);
        }

    }
}
