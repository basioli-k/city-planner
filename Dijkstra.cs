using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    class Dijkstra
    {
        public List<(long ind, double dist)>[] adjacent = new List<(long ind, double dist)>[10000];

        Dictionary<long, long> parent = new Dictionary<long, long> ();

        Dictionary<long, bool> visited = new Dictionary<long, bool> ();

        List<(double dist, long ind, long parent)> priorityQueue = new List<(double dist, long ind, long parent)> ();

        public Dijkstra()
        {
            /* TEST graf
            adjacent = new List<(long ind, long dist)>[1000];
            adjacent[1] = new List<(long ind, long dist)>();
            adjacent[2] = new List<(long ind, long dist)>();
            adjacent[3] = new List<(long ind, long dist)>();
            adjacent[4] = new List<(long ind, long dist)>();
            adjacent[5] = new List<(long ind, long dist)>();

            adjacent[1].Add((2, 10));
            adjacent[2].Add((1, 10));

            adjacent[1].Add((3, 1));
            adjacent[3].Add((1, 1));
            
            adjacent[3].Add((2, 8));
            adjacent[2].Add((3, 8));
            
            adjacent[3].Add((4, 2));
            adjacent[4].Add((3, 2));

            adjacent[2].Add((4, 5));
            adjacent[4].Add((2, 5));

            adjacent[2].Add((5, 1));
            adjacent[5].Add((2, 1));

            adjacent[5].Add((4, 7));
            adjacent[4].Add((5, 7));
            */
        }
        public Dijkstra(List<Road> roads)
        {
            for (int i = 0; i < adjacent.Length; i++)
                adjacent[i] = new List<(long ind, double dist)>();
            foreach (var road in roads)
            {
                adjacent[(int)road.Src].Add((road.Dest, road.Distance()));
            }

        }

        public (double, List<long>) calculateRouteSmart(long start, long end, List<Node> listNodes)
        {
            Dictionary<long, long> previous = new Dictionary<long, long>();
            Dictionary<long, double> distance = new Dictionary<long, double>();
            HashSet<long> q = new HashSet<long>();

            foreach(var node in listNodes)
            {
                distance[node.Id] = long.MaxValue;
                previous[node.Id] = -1;
                q.Add(node.Id);
            }
            distance[start] = 0;

            while (q.Count() != 0)
            {
                double tmpdist = double.MaxValue;
                long tmpnode = -1;
                foreach (var node in q)
                    if (distance[node] < tmpdist)
                    {
                        tmpdist = distance[node];
                        tmpnode = node;
                    }

                q.Remove(tmpnode);

                if (tmpnode == end)
                {
                    List<long> path = new List<long>();
                    path.Add(tmpnode);
                    while (previous[tmpnode] != -1)
                    {
                        path.Add(previous[tmpnode]);
                        tmpnode = previous[tmpnode];

                    }
                    path.Reverse();

                    return (tmpdist, path);
                }

                foreach(var v in adjacent[(int)tmpnode])
                    if (tmpdist + v.dist < distance[v.ind])
                    {
                        distance[v.ind] = tmpdist + v.dist;
                        previous[v.ind] = tmpnode;
                    }
            }

            return (double.PositiveInfinity, new List<long>());
        }

        public (double, List<long>) calculateRoute(long start, long end)
        {
            parent.Clear();
            visited.Clear();

            priorityQueue.Clear();
            priorityQueue.Add((0, start, - 1));

            while (priorityQueue.Any())
            {
                priorityQueue.Sort((x, y) => x.dist.CompareTo(y.dist));
                // izbaci vec posjecene cvorove s vrha.
                while (priorityQueue.Any() && visited.ContainsKey(priorityQueue.First().ind) && visited[ priorityQueue.First().ind ])
                {
                    priorityQueue.RemoveAt(0);
                }
                // Nemozemo doci do cilja.
                if (!priorityQueue.Any()) return (double.PositiveInfinity, new List<long>());

                // uzmi prvi iz priority priorityQueuea i oznaci da si ga posjetio.
                var tmpdist = priorityQueue.First().dist;
                var tmpind = priorityQueue.First().ind;
                var tmppar = priorityQueue.First().parent;
                priorityQueue.RemoveAt(0);

                visited[tmpind] = true;
                parent[tmpind] = tmppar;

                // ako si dosao do cilja vrati udaljenost i izracunaj put.
                if (tmpind == end)
                {
                    List<long> path = new List<long>();
                    path.Add(tmpind);
                    while (parent[tmpind] != -1)
                    {
                        path.Add(parent[tmpind]);
                        tmpind = parent[tmpind];

                    }
                    path.Reverse();

                    return (tmpdist, path);
                }

                // prosiri se na djecu.
                foreach (var v in adjacent[(int)tmpind])
                {
                    if (!visited.ContainsKey(v.ind))
                    {
                        priorityQueue.Add((tmpdist + v.dist, v.ind, tmpind));
                    }
                }

            }

            // nisi uspio doci do cilja.
            return (double.PositiveInfinity, new List<long> ());

        }
    }
}
