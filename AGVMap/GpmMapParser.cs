using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AGVMap
{
    public class GpmMapParser : IMapParser
    {
        public bool CanParse(string json)
        {
            try
            {
                var root = JObject.Parse(json);
                return root["Map"] != null;
            }
            catch
            {
                return false;
            }
        }

        public bool Parse(string json, MapDataManager data)
        {
            var root = JObject.Parse(json);
            var mapNode = root["Map"] as JObject;

            if (mapNode == null) return false;

            data.Format = "GPM Map Format";
            data.MapName = mapNode["Name"]?.ToString() ?? "Unknown";
            data.MapNote = mapNode["Note"]?.ToString() ?? "";

            // 1. 解析 Points (字典結構)
            var pointsObj = mapNode["Points"] as JObject;
            if (pointsObj != null)
            {
                foreach (var prop in pointsObj.Properties())
                {
                    var item = prop.Value as JObject;
                    if (item == null) continue;

                    var point = new MapPoint
                    {
                        ID = prop.Name,
                        TagNumber = item["TagNumber"]?.Value<int>() ?? 0,
                        Name = item["Name"]?.ToString() ?? "",
                        X = item["X"]?.Value<double>() ?? 0,
                        Y = item["Y"]?.Value<double>() ?? 0,
                        StationType = item["StationType"]?.Value<int>() ?? 0,
                        Enable = item["Enable"]?.Value<bool>() ?? true,
                        IsVirtualPoint = item["IsVirtualPoint"]?.Value<bool>() ?? false,
                        IsAutoDoor = item["IsAutoDoor"]?.Value<bool>() ?? false,
                    };

                    var targets = item["Target"] as JObject;
                    if (targets != null)
                    {
                        foreach (var tp in targets.Properties())
                        {
                            double weight = tp.Value?.Value<double>() ?? 1.0;
                            point.Targets[tp.Name] = weight;
                        }
                    }
                    data.Points.Add(point);
                }
            }

            // 2. 解析 Segments
            var segmentsArray = mapNode["Segments"] as JArray;
            if (segmentsArray != null)
            {
                foreach (var item in segmentsArray)
                {
                    data.Paths.Add(new MapPath
                    {
                        FromID = item["StartPtIndex"]?.ToString(),
                        ToID = item["EndPtIndex"]?.ToString(),
                    });
                }
            }

            // 3. 解析 Regions
            var regionsArray = mapNode["Regions"] as JArray;
            if (regionsArray != null)
            {
                foreach (var item in regionsArray)
                {
                    var polygon = new MapPolygon { Name = item["Name"]?.ToString() ?? "" };
                    var coords = item["PolygonCoordinations"] as JArray;
                    if (coords != null)
                    {
                        foreach (var coord in coords)
                        {
                            var arr = coord as JArray;
                            if (arr != null && arr.Count >= 2)
                                polygon.Points.Add(new PointF(arr[0].Value<float>(), arr[1].Value<float>()));
                        }
                    }
                    data.Polygons.Add(polygon);
                }
            }

            data.BuildPointDict();
            BuildAdjacencyList(data);
            return true;
        }

        private void BuildAdjacencyList(MapDataManager data)
        {
            data.AdjacencyList.Clear();
            foreach (var point in data.Points)
            {
                data.AdjacencyList[point.ID] = new List<Edge>();
            }

            // 從 Targets 建立
            foreach (var point in data.Points.Where(p => p.Enable))
            {
                foreach (var target in point.Targets)
                {
                    var targetPoint = data.PointDict.ContainsKey(target.Key) ? data.PointDict[target.Key] : null;
                    if (targetPoint == null || !targetPoint.Enable) continue;

                    double weight = target.Value;
                    if (weight <= 0 || Math.Abs(weight - 1.0) < 0.001)
                    {
                        weight = Math.Sqrt(Math.Pow(point.X - targetPoint.X, 2) + Math.Pow(point.Y - targetPoint.Y, 2));
                    }

                    if (!data.AdjacencyList[point.ID].Any(e => e.TargetPointID == target.Key))
                    {
                        data.AdjacencyList[point.ID].Add(new Edge { TargetPointID = target.Key, Weight = weight });
                    }
                }
            }

            // 從 Paths 補充
            foreach (var path in data.Paths)
            {
                if (!data.PointDict.ContainsKey(path.FromID) || !data.PointDict.ContainsKey(path.ToID)) continue;
                var from = data.PointDict[path.FromID];
                var to = data.PointDict[path.ToID];
                if (!from.Enable || !to.Enable) continue;

                double dist = Math.Sqrt(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2));

                if (!data.AdjacencyList[from.ID].Any(e => e.TargetPointID == to.ID))
                    data.AdjacencyList[from.ID].Add(new Edge { TargetPointID = to.ID, Weight = dist });
                if (!data.AdjacencyList[to.ID].Any(e => e.TargetPointID == from.ID))
                    data.AdjacencyList[to.ID].Add(new Edge { TargetPointID = from.ID, Weight = dist });
            }
        }
    }
}