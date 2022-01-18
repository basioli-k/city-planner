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

        private long start = -1;
        private long end = -1;

        private List<Node> listNodes = new List<Node>();
        private List<Road> listRoads = new List<Road>();

        private int tabIndex = 0;
        public int IndexTab { get { return tabIndex; } set { tabIndex = value; } }
        public long Start { get { return start; } set { start = value; } }
        public long End { get { return end; } set { end = value; } }
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
            if (tabIndex == 0)
                handleControls(sender, e);
            else
                handleRoute(sender, e);
        }

        void handleControls(object sender, MouseEventArgs e)
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
                bool overlap = false;
                foreach (var node in listNodes)
                {
                    if (Math.Sqrt((x - node.X) * (x - node.X) + (y - node.Y) * (y - node.Y)) < 2 * vertex_radius) { 
                        overlap = true;
                        break;
                    }
                }
                if (!overlap) {
                    Node insertingNode = new Node(x, y);
                    listNodes.Add(insertingNode);
                    drawNode(insertingNode, Brushes.Black);
                }
            }

            if(addLine != null)
            {
                if (firstX == -1)
                {
                    int i = 0; 
                    //check if the first end of the road is in the list of all nodes
                    for(i = 0; i < listNodes.Count; i++)
                    {
                        if (Math.Sqrt((x - listNodes[i].X) * (x - listNodes[i].X) + (y - listNodes[i].Y) * (y - listNodes[i].Y)) <  vertex_radius / 2)
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
                        if (Math.Sqrt((x - listNodes[i].X) * (x - listNodes[i].X) + (y - listNodes[i].Y) * (y - listNodes[i].Y)) <  vertex_radius / 2
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
                        Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), (float)road_width);
                        drawLine(Node1, Node2, blackPen);
                        firstX = -1;
                        firstY = -1;
                    }
                }
            }
        }

        void handleRoute(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var y = e.Y;

            foreach (var node in listNodes)
            {
                if (Math.Sqrt((x - node.X) * (x - node.X) + (y - node.Y) * (y - node.Y)) <  vertex_radius / 2)
                {
                    if (start == -1)
                        start = node.Id;
                    else
                        end = node.Id;

                    drawNode(node, Brushes.Red);
                    break;
                }
            }

            if (end == -1) return;

            repaintNodes(Brushes.Black);
            repaintRoads(Brushes.Black);
            Dijkstra dijkstra = new Dijkstra(listRoads);

            (double dist, List<long> path) = dijkstra.calculateRoute(start, end);

            MessageBox.Show(dist.ToString());

            drawPath(path);

            start = -1;
            end = -1;
        }

        public void repaintNodes(Brush brush)
        {
            foreach(var node in listNodes)
            {
                drawNode(node, brush);
            }
        }

        public void repaintRoads(Brush brush)
        {
            foreach(var road in listRoads)
            {
                drawRoad(road, new Pen(brush));
            }
        }

        void drawPath(List<long> path)
        {
            foreach(var intersection in path)
            {
                foreach(var node in listNodes)
                {
                    if (node.Id == intersection)
                    {
                        drawNode(node, Brushes.Blue);
                        break;
                    }
                }
            }

            for(int i = 0; i < path.Count() - 1; i++)
            {
                foreach(var road in listRoads)
                {
                    if (road.Src == path[i] && road.Dest == path[i + 1])
                    {
                        drawRoad(road, new Pen(Brushes.White));
                        drawRoad(road, new Pen(Brushes.Blue));
                        break;
                    }
                }
            }
        }

        void drawNode(Node insertingNode, Brush brush)
        {
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.FillEllipse(brush, insertingNode.X - (int)vertex_radius / 2, insertingNode.Y - (int)vertex_radius / 2, (float)vertex_radius, (float)vertex_radius);
        }

        void drawRoad(Road road, Pen pen)
        {
            Node src = new Node(0,0);
            Node dest = new Node(0,0);
            foreach(var node in listNodes)
            {
                if (node.Id == road.Src)
                {
                    src = node;
                }
                else if (node.Id == road.Dest)
                {
                    dest = node;
                }
            }
            drawLine(src, dest, pen);
        }

        void drawLine(Node Node1,Node Node2, Pen pen)
        {
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawLine(pen, Node1.X, Node1.Y, Node2.X, Node2.Y);
        }

    }
}
