using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    class Dijkstra
    {
        List<List<(long ind, double dist)>> adjacent = new List<List<(long ind, double dist)>> ();

        Dictionary<long, long> parent = new Dictionary<long, long> ();

        Dictionary<long, bool> visited = new Dictionary<long, bool> ();

        SortedList<double, (long ind, long parent)> priorityQueue = new SortedList<double, (long ind, long parent)> ();

        public Dijkstra(List<Node> nodes, List<Road> roads)
        {
            foreach (var road in roads)
            {
                adjacent[(int)road.Src].Add((road.Dest, road.Distance()));
            }
        }

        public (double, List<long>) Run(int start, int end)
        {
            parent.Clear();
            visited.Clear();

            priorityQueue.Clear();
            priorityQueue.Add(0, (start, - 1));

            while (priorityQueue.Any())
            {
                // izbaci vec posjecene cvorove s vrha.
                while (priorityQueue.Any() && visited[ priorityQueue.First().Value.ind ])
                {
                    priorityQueue.RemoveAt(0);
                }
                // Nemozemo doci do cilja.
                if (!priorityQueue.Any()) return (-1, new List<long>());

                // uzmi prvi iz priority priorityQueuea i oznaci da si ga posjetio.
                var tmpdist = priorityQueue.First().Key;
                var tmpind = priorityQueue.First().Value.ind;
                var tmppar = priorityQueue.First().Value.parent;

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
                    if (!visited[v.ind])
                    {
                        priorityQueue.Add(tmpdist + v.dist, (v.ind, tmpind));
                    }
                }

            }

            // nisi uspio doci do cilja.
            return (-1, new List<long> ());

        }
    }
}
