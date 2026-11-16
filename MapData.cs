using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace AGVRoutePlanner
{
    #region 資料模型

    public class MapFile
    {
        public MapOptions Options { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string ImageName { get; set; }
        public double[] MapImageSize { get; set; }
        public double[] MapImageBoundary { get; set; }
        public List<MapPoint> Points { get; set; }
        public List<MapPath> Paths { get; set; }
        public List<MapPolygon> Polygons { get; set; }
    }

    public class MapOptions
    {
        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }
        public double PhysicalWidth { get; set; }
        public double PhysicalHeight { get; set; }
        public double Rotation { get; set; }
        public double GridSize { get; set; }
    }

    public class MapPoint
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int TagNumber { get; set; }
        public double Direction { get; set; }
        public int StationType { get; set; }
        public bool IsVirtualPoint { get; set; }
        public bool IsAutoDoor { get; set; }
        public bool IsStandbyPoint { get; set; }
        public bool IsParking { get; set; }
        public bool IsAvoid { get; set; }
        public bool IsNarrowPath { get; set; }
        public bool IsMaintainPoint { get; set; }
        public bool IsSegment { get; set; }
        public bool Enable { get; set; }
        public string ImageName { get; set; }
        public string Bay { get; set; }
        public double Speed { get; set; }
        public int LsrMode { get; set; }
        public Dictionary<int, double> Targets { get; set; } = new Dictionary<int, double>();

        public override string ToString()
        {
            string display = !string.IsNullOrEmpty(DisplayName) ? DisplayName : Name;
            string label = $"{display} ({ID})";
            if (IsVirtualPoint) label += " [V]";
            if (StationType == 1) label += " [料架]";
            if (StationType == 3) label += " [換電]";
            if (StationType == 100) label += " [電梯]";
            if (IsAutoDoor) label += " [門]";
            return label;
        }
    }

    public class MapPath
    {
        public int ID { get; set; }
        public int StartPointID { get; set; }
        public int EndPointID { get; set; }
        public double Speed { get; set; }
        public double Weight { get; set; }
    }

    public class MapPolygon
    {
        public string Name { get; set; }
        public List<PolygonVertex> Vertices { get; set; }
        public bool IsNarrowPath { get; set; }
        public int MaxVehicleCapacity { get; set; }
    }

    public class PolygonVertex
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    #endregion

    /// <summary>
    /// 地圖資料管理器
    /// </summary>
    public class MapDataManager
    {
        public MapFile Map { get; private set; }
        public Dictionary<int, MapPoint> PointDict { get; private set; }
        public Dictionary<int, List<Edge>> AdjacencyList { get; private set; }

        public double MinX { get; private set; }
        public double MaxX { get; private set; }
        public double MinY { get; private set; }
        public double MaxY { get; private set; }

        public class Edge
        {
            public int TargetPointID { get; set; }
            public double Weight { get; set; }
            public double Speed { get; set; }
        }

        /// <summary>
        /// 從 JSON 載入地圖
        /// </summary>
        public void LoadFromJson(string jsonContent)
        {
            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var root = serializer.Deserialize<Dictionary<string, object>>(jsonContent);

            // 取得 "Map" 外層
            Dictionary<string, object> mapObj;
            if (root.ContainsKey("Map"))
                mapObj = root["Map"] as Dictionary<string, object>;
            else
                mapObj = root; // 若沒有 Map 外層，直接用 root

            Map = new MapFile
            {
                Name = GetString(mapObj, "Name", ""),
                Note = GetString(mapObj, "Note", ""),
                ImageName = GetString(mapObj, "ImageName", ""),
                Points = new List<MapPoint>(),
                Paths = new List<MapPath>(),
                Polygons = new List<MapPolygon>()
            };

            // 解析 Options
            Map.Options = ParseOptions(mapObj);

            // 解析 Map_Image_Size
            if (mapObj.ContainsKey("Map_Image_Size"))
            {
                var sizeArr = mapObj["Map_Image_Size"] as System.Collections.ArrayList;
                if (sizeArr != null && sizeArr.Count >= 2)
                    Map.MapImageSize = new double[] { Convert.ToDouble(sizeArr[0]), Convert.ToDouble(sizeArr[1]) };
            }

            // 解析 Map_Image_Boundary
            if (mapObj.ContainsKey("Map_Image_Boundary"))
            {
                var bndArr = mapObj["Map_Image_Boundary"] as System.Collections.ArrayList;
                if (bndArr != null && bndArr.Count >= 4)
                {
                    Map.MapImageBoundary = new double[]
                    {
                        Convert.ToDouble(bndArr[0]), Convert.ToDouble(bndArr[1]),
                        Convert.ToDouble(bndArr[2]), Convert.ToDouble(bndArr[3])
                    };
                }
            }

            // ★ 解析 Points（字典格式 "118": {...}, "119": {...}）
            ParsePointsDictionary(mapObj);

            // ★ 從每個 Point 的 Target 建立 Paths
            BuildPathsFromTargets();

            // 建立快速查詢
            PointDict = new Dictionary<int, MapPoint>();
            foreach (var pt in Map.Points)
                PointDict[pt.ID] = pt;

            // 座標邊界
            if (Map.Points.Count > 0)
            {
                MinX = Map.Points.Min(p => p.X);
                MaxX = Map.Points.Max(p => p.X);
                MinY = Map.Points.Min(p => p.Y);
                MaxY = Map.Points.Max(p => p.Y);
            }

            // 建立鄰接表
            BuildAdjacencyList();

            // 解析 Polygons（如果存在）
            ParsePolygons(mapObj);
        }

        #region 解析方法

        private MapOptions ParseOptions(Dictionary<string, object> mapObj)
        {
            var opt = new MapOptions();

            if (mapObj.ContainsKey("Options") && mapObj["Options"] is Dictionary<string, object> o)
            {
                opt.Rotation = GetDouble(o, "Rotation", 0);
                opt.GridSize = GetDouble(o, "gridSize", 20);
            }

            // 從 Map_Image_Boundary 取得物理尺寸
            if (mapObj.ContainsKey("Map_Image_Boundary"))
            {
                var bnd = mapObj["Map_Image_Boundary"] as System.Collections.ArrayList;
                if (bnd != null && bnd.Count >= 4)
                {
                    opt.PhysicalWidth = Convert.ToDouble(bnd[2]) - Convert.ToDouble(bnd[0]);
                    opt.PhysicalHeight = Convert.ToDouble(bnd[3]) - Convert.ToDouble(bnd[1]);
                }
            }

            if (mapObj.ContainsKey("Map_Image_Size"))
            {
                var sz = mapObj["Map_Image_Size"] as System.Collections.ArrayList;
                if (sz != null && sz.Count >= 2)
                {
                    opt.ImageWidth = Convert.ToDouble(sz[0]);
                    opt.ImageHeight = Convert.ToDouble(sz[1]);
                }
            }

            return opt;
        }

        private void ParsePointsDictionary(Dictionary<string, object> mapObj)
        {
            if (!mapObj.ContainsKey("Points")) return;

            var pointsObj = mapObj["Points"];

            // 字典格式: { "118": {...}, "119": {...} }
            if (pointsObj is Dictionary<string, object> pointsDict)
            {
                foreach (var kvp in pointsDict)
                {
                    int pointId;
                    if (!int.TryParse(kvp.Key, out pointId)) continue;

                    var data = kvp.Value as Dictionary<string, object>;
                    if (data == null) continue;

                    var pt = ParseSinglePoint(pointId, data);
                    Map.Points.Add(pt);
                }
            }
            // 陣列格式: [{...}, {...}]（備援相容）
            else if (pointsObj is System.Collections.ArrayList pointsArr)
            {
                foreach (Dictionary<string, object> data in pointsArr)
                {
                    int pointId = Convert.ToInt32(data.ContainsKey("ID") ? data["ID"] : data.ContainsKey("Name") ? data["Name"] : 0);
                    var pt = ParseSinglePoint(pointId, data);
                    Map.Points.Add(pt);
                }
            }
        }

        private MapPoint ParseSinglePoint(int id, Dictionary<string, object> data)
        {
            var pt = new MapPoint
            {
                ID = id,
                Name = GetString(data, "Name", id.ToString()),
                X = GetDouble(data, "X", 0),
                Y = GetDouble(data, "Y", 0),
                TagNumber = GetInt(data, "TagNumber", 0),
                Direction = GetDouble(data, "Direction", 0),
                StationType = GetInt(data, "StationType", 0),
                IsVirtualPoint = GetBool(data, "IsVirtualPoint"),
                IsAutoDoor = GetBool(data, "IsAutoDoor"),
                IsStandbyPoint = GetBool(data, "IsStandbyPoint"),
                IsParking = GetBool(data, "IsParking"),
                IsAvoid = GetBool(data, "IsAvoid"),
                IsNarrowPath = GetBool(data, "IsNarrowPath"),
                IsMaintainPoint = GetBool(data, "IsMaintainPoint"),
                IsSegment = GetBool(data, "IsSegment"),
                Enable = GetBool(data, "Enable", true),
                Bay = GetString(data, "Bay", ""),
                Speed = GetDouble(data, "Speed", 1.0),
                LsrMode = GetInt(data, "LsrMode", 1)
            };

            // 解析 Graph 取得 Display 名稱和圖示
            if (data.ContainsKey("Graph") && data["Graph"] is Dictionary<string, object> graph)
            {
                pt.DisplayName = GetString(graph, "Display", "");
                pt.ImageName = GetString(graph, "ImageName", "");
            }

            // ★ 解析 Target 字典（鄰接關係）
            if (data.ContainsKey("Target") && data["Target"] is Dictionary<string, object> targets)
            {
                foreach (var t in targets)
                {
                    int targetId;
                    if (int.TryParse(t.Key, out targetId))
                    {
                        double speed = Convert.ToDouble(t.Value);
                        pt.Targets[targetId] = speed;
                    }
                }
            }

            return pt;
        }

        /// <summary>
        /// 從每個 Point 的 Target 建立全域 Path 列表
        /// </summary>
        private void BuildPathsFromTargets()
        {
            int pathId = 0;
            var pathSet = new HashSet<string>(); // 避免重複

            foreach (var pt in Map.Points)
            {
                foreach (var target in pt.Targets)
                {
                    string key = $"{pt.ID}->{target.Key}";
                    if (pathSet.Contains(key)) continue;
                    pathSet.Add(key);

                    Map.Paths.Add(new MapPath
                    {
                        ID = pathId++,
                        StartPointID = pt.ID,
                        EndPointID = target.Key,
                        Speed = target.Value,
                        Weight = 0 // 稍後由距離計算
                    });
                }
            }
        }

        private void BuildAdjacencyList()
        {
            AdjacencyList = new Dictionary<int, List<Edge>>();

            foreach (var pt in Map.Points)
                AdjacencyList[pt.ID] = new List<Edge>();

            foreach (var path in Map.Paths)
            {
                if (!PointDict.ContainsKey(path.StartPointID) ||
                    !PointDict.ContainsKey(path.EndPointID))
                    continue;

                var p1 = PointDict[path.StartPointID];
                var p2 = PointDict[path.EndPointID];
                double dist = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));

                // Target 是單向的（A 的 Target 有 B，不代表 B 的 Target 有 A）
                AdjacencyList[path.StartPointID].Add(new Edge
                {
                    TargetPointID = path.EndPointID,
                    Weight = dist,
                    Speed = path.Speed
                });
            }
        }

        private void ParsePolygons(Dictionary<string, object> mapObj)
        {
            // 嘗試多種 key 名稱
            string[] possibleKeys = { "Polygons", "polygons", "Regions", "regions", "Polygon" };
            System.Collections.ArrayList arr = null;

            foreach (var key in possibleKeys)
            {
                if (mapObj.ContainsKey(key))
                {
                    arr = mapObj[key] as System.Collections.ArrayList;
                    if (arr != null) break;
                }
            }

            if (arr == null) return;

            foreach (Dictionary<string, object> item in arr)
            {
                var poly = new MapPolygon
                {
                    Name = GetString(item, "Name", ""),
                    IsNarrowPath = GetBool(item, "IsNarrowPath"),
                    MaxVehicleCapacity = GetInt(item, "MaxVehicleCapacity", 0),
                    Vertices = new List<PolygonVertex>()
                };

                // 嘗試找頂點資料
                System.Collections.ArrayList verts = null;
                string[] vertKeys = { "Vertices", "vertices", "Points", "points", "Coords" };
                foreach (var vk in vertKeys)
                {
                    if (item.ContainsKey(vk))
                    {
                        verts = item[vk] as System.Collections.ArrayList;
                        if (verts != null) break;
                    }
                }

                if (verts != null)
                {
                    foreach (var v in verts)
                    {
                        if (v is Dictionary<string, object> vd)
                        {
                            poly.Vertices.Add(new PolygonVertex
                            {
                                X = GetDouble(vd, "X", 0),
                                Y = GetDouble(vd, "Y", 0)
                            });
                        }
                    }
                }

                Map.Polygons.Add(poly);
            }
        }

        #endregion

        #region 查詢方法

        /// <summary>
        /// 取得所有非虛擬的站點（供 UI 下拉選單）
        /// </summary>
        public List<MapPoint> GetStationPoints()
        {
            return Map.Points
                .Where(p => !p.IsVirtualPoint && p.Enable)
                .OrderBy(p => p.ID)
                .ToList();
        }

        /// <summary>
        /// 取得所有點位（含虛擬點）
        /// </summary>
        public List<MapPoint> GetAllPoints()
        {
            return Map.Points
                .Where(p => p.Enable)
                .OrderBy(p => p.ID)
                .ToList();
        }

        /// <summary>
        /// 取得地圖統計資訊
        /// </summary>
        public string GetMapInfo()
        {
            int totalPoints = Map.Points.Count;
            int enabledPoints = Map.Points.Count(p => p.Enable);
            int virtualPoints = Map.Points.Count(p => p.IsVirtualPoint);
            int stationPoints = Map.Points.Count(p => !p.IsVirtualPoint && p.Enable);
            int totalPaths = Map.Paths.Count;
            int doorPoints = Map.Points.Count(p => p.IsAutoDoor);
            int rackPoints = Map.Points.Count(p => p.StationType == 1);
            int elevPoints = Map.Points.Count(p => p.StationType == 100);
            int batteryPoints = Map.Points.Count(p => p.StationType == 3);

            return $"✔ 地圖載入成功\r\n" +
                   $"  名稱: {Map.Name}\r\n" +
                   $"  版本: {Map.Note}\r\n" +
                   $"  ─────────────────\r\n" +
                   $"  總點位: {totalPoints}\r\n" +
                   $"  啟用點位: {enabledPoints}\r\n" +
                   $"  站點(非虛擬): {stationPoints}\r\n" +
                   $"  虛擬點: {virtualPoints}\r\n" +
                   $"  路徑數: {totalPaths}\r\n" +
                   $"  ─────────────────\r\n" +
                   $"  自動門點: {doorPoints}\r\n" +
                   $"  料架站: {rackPoints}\r\n" +
                   $"  電梯站: {elevPoints}\r\n" +
                   $"  換電站: {batteryPoints}";
        }

        #endregion

        #region 工具方法

        private static string GetString(Dictionary<string, object> d, string key, string def = "")
        {
            return d.ContainsKey(key) && d[key] != null ? Convert.ToString(d[key]) : def;
        }

        private static double GetDouble(Dictionary<string, object> d, string key, double def = 0)
        {
            return d.ContainsKey(key) && d[key] != null ? Convert.ToDouble(d[key]) : def;
        }

        private static int GetInt(Dictionary<string, object> d, string key, int def = 0)
        {
            return d.ContainsKey(key) && d[key] != null ? Convert.ToInt32(d[key]) : def;
        }

        private static bool GetBool(Dictionary<string, object> d, string key, bool def = false)
        {
            return d.ContainsKey(key) && d[key] != null ? Convert.ToBoolean(d[key]) : def;
        }

        #endregion
    }
}