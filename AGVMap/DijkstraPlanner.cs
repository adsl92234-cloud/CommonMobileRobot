using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing; // 提供 PointF
using AML;

namespace AGVMap
{
    public class DijkstraPlanner : IPathPlanner
    {
        private readonly MapDataManager _mapData;

        public DijkstraPlanner(MapDataManager mapData)
        {
            _mapData = mapData;
        }

        public MapRouteData PlanRoute(string startId, string endId, string waypointId = null)
        {
            var route = new MapRouteData { Success = false };

            if (!_mapData.PointDict.ContainsKey(startId) || !_mapData.PointDict.ContainsKey(endId))
                return route;

            List<string> fullStringPath = new List<string>();
            double totalDist = 0;

            if (!string.IsNullOrEmpty(waypointId) && _mapData.PointDict.ContainsKey(waypointId))
            {
                var seg1 = RunDijkstra(startId, waypointId);
                var seg2 = RunDijkstra(waypointId, endId);

                if (seg1 == null || seg2 == null) return route;

                fullStringPath.AddRange(seg1.Path);
                fullStringPath.AddRange(seg2.Path.Skip(1));
                totalDist = seg1.Distance + seg2.Distance;
            }
            else
            {
                var seg = RunDijkstra(startId, endId);
                if (seg == null) return route;

                fullStringPath = seg.Path;
                totalDist = seg.Distance;
            }

            route.Success = true;
            route.StartID = _mapData.PointDict[startId].TagNumber;
            route.EndID = _mapData.PointDict[endId].TagNumber;
            route.WaypointID = string.IsNullOrEmpty(waypointId) ? -1 : _mapData.PointDict[waypointId].TagNumber;
            route.StartName = _mapData.PointDict[startId].Name;
            route.EndName = _mapData.PointDict[endId].Name;
            route.TotalDistance = totalDist;
            route.PlanTime = DateTime.Now;

            foreach (var id in fullStringPath)
            {
                var pt = _mapData.PointDict[id];
                route.FullPath.Add(pt.TagNumber);
                route.FullPathNames.Add(pt.Name ?? pt.ID);
                route.PathCoordinates.Add(new PointF((float)pt.X, (float)pt.Y));
            }
            route.RouteArray = route.FullPathNames.ToArray();

            return route;
        }

        private class DijkstraResult
        {
            public List<string> Path { get; set; }
            public double Distance { get; set; }
        }

        private DijkstraResult RunDijkstra(string startId, string endId)
        {
            if (!_mapData.AdjacencyList.ContainsKey(startId) || !_mapData.AdjacencyList.ContainsKey(endId))
                return null;

            var dist = new Dictionary<string, double>();
            var prev = new Dictionary<string, string>();
            var pq = new SortedSet<Tuple<double, string>>(Comparer<Tuple<double, string>>.Create((a, b) =>
            {
                int cmp = a.Item1.CompareTo(b.Item1);
                return cmp == 0 ? a.Item2.CompareTo(b.Item2) : cmp;
            }));

            foreach (var kvp in _mapData.AdjacencyList)
            {
                dist[kvp.Key] = double.MaxValue;
                prev[kvp.Key] = null;
            }

            dist[startId] = 0;
            pq.Add(new Tuple<double, string>(0, startId));

            while (pq.Count > 0)
            {
                var current = pq.Min;
                pq.Remove(current);
                string u = current.Item2;

                if (u == endId) break;
                if (current.Item1 > dist[u]) continue;
                if (!_mapData.AdjacencyList.ContainsKey(u)) continue;

                foreach (var edge in _mapData.AdjacencyList[u])
                {
                    double newDist = dist[u] + edge.Weight;
                    if (newDist < dist[edge.TargetPointID])
                    {
                        pq.Remove(new Tuple<double, string>(dist[edge.TargetPointID], edge.TargetPointID));
                        dist[edge.TargetPointID] = newDist;
                        prev[edge.TargetPointID] = u;
                        pq.Add(new Tuple<double, string>(newDist, edge.TargetPointID));
                    }
                }
            }

            if (dist[endId] == double.MaxValue) return null;

            var path = new List<string>();
            for (string at = endId; at != null; at = prev[at])
            {
                path.Add(at);
            }
            path.Reverse();

            return new DijkstraResult { Path = path, Distance = dist[endId] };
        }
    }
}