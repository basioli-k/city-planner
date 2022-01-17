using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace city_planner
{
    class Dijkstra
    {
        List<List<(long ind, long dist)>> adjacent = new List<List<(long ind, long dist)>> ();

        Dictionary<long, bool> visited = new Dictionary<long, bool> ();

        SortedList<long, long> priorityQueue = new SortedList<long, long> ();

        public Dijkstra(List<Node> nodes, List<Road> roads)
        {
            foreach (var road in roads)
            {
                adjacent[(int)road.Src].Add((road.Dest, road.Distance()));
            }
        }

        public long Run(int start, int end)
        {
            visited.Clear();

            priorityQueue.Clear();
            priorityQueue.Add(0, start);

            while (priorityQueue.Any())
            {
                // izbaci vec posjecene cvorove s vrha.
                while (priorityQueue.Any() && visited[ priorityQueue.First().Value ])
                {
                    priorityQueue.RemoveAt(0);
                }
                // Nemozemo doci do cilja.
                if (!priorityQueue.Any()) return -1;

                // uzmi prvi iz priority priorityQueuea i oznaci da si ga posjetio.
                var tmpdist = priorityQueue.First().Key;
                var tmpind = priorityQueue.First().Value;

                visited[tmpind] = true;

                // ako si dosao do cilja vrati udaljenost.
                if (tmpind == end) return tmpdist;

                // prosiri se na djecu.
                foreach (var v in adjacent[(int)tmpind])
                {
                    if (!visited[v.ind])
                    {
                        priorityQueue.Add(tmpdist + v.dist, v.ind);
                    }
                }

            }

            // nisi uspio doci do cilja.
            return -1;

        }
    }
}
