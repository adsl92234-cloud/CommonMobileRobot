using System;
using System.Collections.Generic;
using System.Linq;

namespace AGVRoutePlanner
{
    /// <summary>
    /// 路徑規劃結果
    /// </summary>
    public class RouteResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<int> FullPath { get; set; } = new List<int>();           // 完整點位 ID 序列
        public List<int> PathToWaypoint { get; set; } = new List<int>();     // 起點 → 必經點
        public List<int> PathFromWaypoint { get; set; } = new List<int>();   // 必經點 → 終點
        public double TotalDistance { get; set; }
        public double DistanceToWaypoint { get; set; }
        public double DistanceFromWaypoint { get; set; }
    }

    /// <summary>
    /// Dijkstra 最短路徑搜尋器
    /// </summary>
    public class PathFinder
    {
        private readonly MapDataManager _mapData;

        public PathFinder(MapDataManager mapData)
        {
            _mapData = mapData;
        }

        /// <summary>
        /// 規劃路線：起點 → 必經點 → 終點
        /// 若無必經點，waypointId 傳入 -1
        /// </summary>
        public RouteResult FindRoute(int startId, int waypointId, int endId)
        {
            var result = new RouteResult();

            // 驗證點位存在
            if (!_mapData.PointDict.ContainsKey(startId))
                return Fail(result, $"起始點 {startId} 不存在於地圖中");
            if (!_mapData.PointDict.ContainsKey(endId))
                return Fail(result, $"終點 {endId} 不存在於地圖中");

            if (waypointId >= 0)
            {
                if (!_mapData.PointDict.ContainsKey(waypointId))
                    return Fail(result, $"必經點 {waypointId} 不存在於地圖中");

                // 起點 → 必經點
                var seg1 = Dijkstra(startId, waypointId);
                if (seg1 == null)
                    return Fail(result, $"無法從 {startId} 到達必經點 {waypointId}");

                // 必經點 → 終點
                var seg2 = Dijkstra(waypointId, endId);
                if (seg2 == null)
                    return Fail(result, $"無法從必經點 {waypointId} 到達終點 {endId}");

                result.PathToWaypoint = seg1.Path;
                result.DistanceToWaypoint = seg1.Distance;
                result.PathFromWaypoint = seg2.Path;
                result.DistanceFromWaypoint = seg2.Distance;

                // 合併完整路徑（去除重複的 waypoint）
                result.FullPath = new List<int>(seg1.Path);
                result.FullPath.AddRange(seg2.Path.Skip(1));
                result.TotalDistance = seg1.Distance + seg2.Distance;
            }
            else
            {
                // 無必經點：直接 起點 → 終點
                var seg = Dijkstra(startId, endId);
                if (seg == null)
                    return Fail(result, $"無法從 {startId} 到達終點 {endId}");

                result.FullPath = seg.Path;
                result.TotalDistance = seg.Distance;
            }

            result.Success = true;
            return result;
        }

        private class DijkstraResult
        {
            public List<int> Path { get; set; }
            public double Distance { get; set; }
        }

        private DijkstraResult Dijkstra(int startId, int endId)
        {
            var dist = new Dictionary<int, double>();
            var prev = new Dictionary<int, int>();
            var visited = new HashSet<int>();

            // 優先佇列（用 SortedSet 模擬）
            var pq = new SortedSet<(double dist, int id)>(
                Comparer<(double dist, int id)>.Create((a, b) =>
                {
                    int cmp = a.dist.CompareTo(b.dist);
                    return cmp != 0 ? cmp : a.id.CompareTo(b.id);
                }));

            foreach (var kvp in _mapData.AdjacencyList)
                dist[kvp.Key] = double.MaxValue;

            dist[startId] = 0;
            pq.Add((0, startId));

            while (pq.Count > 0)
            {
                var (d, u) = pq.Min;
                pq.Remove(pq.Min);

                if (visited.Contains(u)) continue;
                visited.Add(u);

                if (u == endId) break;

                if (!_mapData.AdjacencyList.ContainsKey(u)) continue;

                foreach (var edge in _mapData.AdjacencyList[u])
                {
                    if (visited.Contains(edge.TargetPointID)) continue;

                    double newDist = d + edge.Weight;
                    if (newDist < dist[edge.TargetPointID])
                    {
                        pq.Remove((dist[edge.TargetPointID], edge.TargetPointID));
                        dist[edge.TargetPointID] = newDist;
                        prev[edge.TargetPointID] = u;
                        pq.Add((newDist, edge.TargetPointID));
                    }
                }
            }

            // 回溯路徑
            if (!visited.Contains(endId) || dist[endId] == double.MaxValue)
                return null;

            var path = new List<int>();
            int current = endId;
            while (current != startId)
            {
                path.Add(current);
                if (!prev.ContainsKey(current)) return null;
                current = prev[current];
            }
            path.Add(startId);
            path.Reverse();

            return new DijkstraResult { Path = path, Distance = dist[endId] };
        }

        private RouteResult Fail(RouteResult r, string msg)
        {
            r.Success = false;
            r.ErrorMessage = msg;
            return r;
        }
    }
}