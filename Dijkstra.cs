using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    class Dijkstra
    {
        List<(long ind, long dist)>[] adjacent = new List<(long ind, long dist)>[10000];

        Dictionary<long, long> parent = new Dictionary<long, long> ();

        Dictionary<long, bool> visited = new Dictionary<long, bool> ();

        List<(long dist, long ind, long parent)> priorityQueue = new List<(long dist, long ind, long parent)> ();

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
            foreach (var road in roads)
            {
                if (!adjacent[(int)road.Src].Any())
                    adjacent[(int)road.Src] = new List<(long ind, long dist)>();
                adjacent[(int)road.Src].Add((road.Dest, road.Distance()));
            }
        }

        public (long, List<long>) Run(int start, int end)
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
                if (!priorityQueue.Any()) return (-1, new List<long>());

                // uzmi prvi iz priority priorityQueuea i oznaci da si ga posjetio.
                var tmpdist = priorityQueue.First().dist;
                var tmpind = priorityQueue.First().ind;
                var tmppar = priorityQueue.First().parent;

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
            return (-1, new List<long> ());

        }
    }
}
