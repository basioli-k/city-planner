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
        private string characteristic = "";

        private List<Node> listNodes = new List<Node>();
        private List<Road> listRoads = new List<Road>();

        private int tabIndex = 0;
        public int IndexTab { get { return tabIndex; } set { tabIndex = value; } }
        public long Start { get { return start; } set { start = value; } }
        public long End { get { return end; } set { end = value; } }
        public string Characteristic { get { return characteristic; } set { characteristic = value; } }
        public int FirstX { get { return firstX; } set { firstX = value; } }
        public int FirstY { get { return firstY; } set { firstY = value; } }
        public CityPlan()
        {
            InitializeComponent();
        }

        public event EventHandler addPoint;
        public event EventHandler addLine;
        public event EventHandler deleteObject;

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
            bool roadValid = false;

            listNodes = Node.select_star();
            listRoads = Road.select_star();
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

            // delete the node or road
            // if deleting node delete all adjacent roads
            if (deleteObject != null)
            {
                var temp = listNodes.Find(node => 
                                        Math.Sqrt((double)((node.X - x)* (node.X - x) + (node.Y - y)* (node.Y - y))) <= vertex_radius);

                var roadsToDelete = listRoads.FindAll(road => IsPointOnRoad(road, x, y));

                foreach(var road in roadsToDelete)
                {
                    listRoads.Remove(road);
                    road.delete();
                }
                if (temp != null)
                {
                    listNodes.Remove(temp);
                    temp.delete();
                }

                drawAllPointsAndRoads();

            }
        }
        private bool IsPointOnRoad(Road road, long x, long y)
        {
            var srcDest = road.GetSourceAndDest();
            
            var src = srcDest.Item1;
            var dest = srcDest.Item2;
            
            if (Math.Sqrt((double)((src.X - x) * (src.X - x) + (src.Y - y) * (src.Y - y))) <= vertex_radius ||
                Math.Sqrt((double)((dest.X - x) * (dest.X - x) + (dest.Y - y) * (dest.Y - y))) <= vertex_radius)
            {
                return true;
            }

            var tempPoint = Node.GetNonDbNode(x, y);
            var road_len = Node.Distance(src, dest);
            return Node.Distance(src, tempPoint) + Node.Distance(tempPoint, dest)
                <= Math.Sqrt(road_len*road_len + (double) road_width * road_width / 4); //TODO double check s nekim
        }

        public event EventHandler<double> print_dist;

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
            double dist = double.PositiveInfinity;
            List<long> path = new List<long>();

            // Ako moramo stati negdje
            if (characteristic != "")
            {
                // pronadi sva krizanja sa zadanom karakteristikom
                foreach (var stop in listNodes.FindAll(node => node.Characteristics.Contains(characteristic)))
                {
                    (double dist1, List<long> path1) = dijkstra.calculateRoute(start, stop.Id);
                    (double dist2, List<long> path2) = dijkstra.calculateRoute(stop.Id, end);

                    if (dist1 + dist2 < dist)
                    {
                        dist = dist1 + dist2;
                        path = path1.Concat(path2).ToList();
                    }
                }

                // pronadi sve ceste sa zadanom karakteriskom
                foreach (var road in listRoads.FindAll(road => road.Characteristics.Contains(characteristic)))
                {
                    (double dist1, List<long> path1) = dijkstra.calculateRoute(start, road.Src);
                    (double dist2, List<long> path2) = dijkstra.calculateRoute(road.Dest, end);

                    if (dist1 + dist2 + road.Distance() < dist)
                    {
                        dist = dist1 + dist2 + road.Distance();
                        path = path1.Concat(path2).ToList();
                    }
                }
            }
            // Ako samo trazimo najkraci put od start do end
            else
            {
                (dist, path) = dijkstra.calculateRouteSmart(start, end, listNodes);
            }

            print_dist?.Invoke(this, dist);

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
            Node src = new Node();
            Node dest = new Node();
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

        private void CityPlan_Load(object sender, EventArgs e)
        {
            //drawAllPointsAndRoads();
        }

        private void CityPlan_Paint(object sender, PaintEventArgs e)
        {
            //drawAllPointsAndRoads();
        }

        void drawAllPointsAndRoads()
        {
            SuspendLayout();
            var g = CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);
            listNodes = Node.select_star();
            listRoads = Road.select_star();
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), (float)road_width);
            for (int i = 0; i < listNodes.Count; i++)
            {
                drawNode(listNodes[i], Brushes.Black);
            }

            for (int i = 0; i < listRoads.Count; i++)
            {
                drawRoad(listRoads[i], blackPen);
            }
            ResumeLayout();
        }

    }
}
