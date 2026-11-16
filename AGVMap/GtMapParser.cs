using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AGVMap
{
    public class GtMapParser : IMapParser
    {
        public bool CanParse(string json)
        {
            try
            {
                var root = JObject.Parse(json);
                return root["map_id"] != null;
            }
            catch
            {
                return false;
            }
        }

        public bool Parse(string json, MapDataManager data)
        {
            var root = JObject.Parse(json);
            data.Format = "GT Map Format";
            data.MapName = root["map_name"]?.ToString() ?? "Unknown";

            var imageBase64 = root["image"]?.ToString();
            if (!string.IsNullOrEmpty(imageBase64))
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(imageBase64);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        // Image.FromStream 會依賴來源 stream 的生命週期；GT parser
                        // 返回後 stream 會被釋放，之後 LockBits/Render 可能讀到失效資料。
                        using (var source = Image.FromStream(ms))
                            data.BackgroundImage = new Bitmap(source);
                    }
                }
                catch { }
            }

            data.Boundary = ParseBoundary(root["map_boundary"] ?? root["boundary"] ?? root["mapBoundary"]);
            if (data.Boundary == null && data.BackgroundImage != null)
            {
                data.Boundary = new MapBoundary
                {
                    MinX = 0,
                    MinY = 0,
                    MaxX = data.BackgroundImage.Width,
                    MaxY = data.BackgroundImage.Height
                };
            }

            var stations = root["station_info"] as JArray;
            if (stations != null)
            {
                int virtualTag = 1000;
                foreach (var st in stations)
                {
                    string stType = st["type"]?.ToString() ?? "";
                    data.Points.Add(new MapPoint
                    {
                        ID = st["id"]?.ToString(),
                        Name = st["name"]?.ToString(),
                        X = st["node_x"]?.Value<double>() ?? 0,
                        Y = st["node_y"]?.Value<double>() ?? 0,
                        TagNumber = virtualTag++,
                        StationType = stType.Contains("Rack") ? StationTypes.Rack : StationTypes.Normal,
                        Enable = true
                    });
                }
            }

            var areas = root["area_info"] as JArray;
            if (areas != null)
            {
                foreach (var area in areas)
                {
                    if (area["type"]?.ToString() == "keepOut")
                    {
                        var zone = new KeepOutZone
                        {
                            ID = area["id"]?.ToString(),
                            Name = area["name"]?.ToString()
                        };

                        var nodes = area["nodes"] as JArray;
                        if (nodes != null)
                        {
                            foreach (var node in nodes)
                            {
                                var arr = node as JArray;
                                if (arr != null && arr.Count >= 2)
                                    zone.Polygon.Add(new PointF(arr[0].Value<float>(), arr[1].Value<float>()));
                            }
                        }
                        data.KeepOutZones.Add(zone);
                    }
                }
            }

            data.BuildPointDict();
            return true;
        }

        private MapBoundary ParseBoundary(JToken token)
        {
            var array = token as JArray;
            if (array != null && array.Count >= 4)
            {
                return new MapBoundary
                {
                    MinX = array[0].Value<float>(),
                    MinY = array[1].Value<float>(),
                    MaxX = array[2].Value<float>(),
                    MaxY = array[3].Value<float>()
                };
            }

            var obj = token as JObject;
            if (obj == null) return null;

            float minX = obj["min_x"]?.Value<float>() ?? obj["minX"]?.Value<float>() ?? obj["x"]?.Value<float>() ?? 0;
            float minY = obj["min_y"]?.Value<float>() ?? obj["minY"]?.Value<float>() ?? obj["y"]?.Value<float>() ?? 0;
            float maxX = obj["max_x"]?.Value<float>() ?? obj["maxX"]?.Value<float>() ?? float.NaN;
            float maxY = obj["max_y"]?.Value<float>() ?? obj["maxY"]?.Value<float>() ?? float.NaN;

            if (float.IsNaN(maxX) && obj["width"] != null) maxX = minX + obj["width"].Value<float>();
            if (float.IsNaN(maxY) && obj["height"] != null) maxY = minY + obj["height"].Value<float>();
            if (float.IsNaN(maxX) || float.IsNaN(maxY) || maxX <= minX || maxY <= minY) return null;

            return new MapBoundary { MinX = minX, MinY = minY, MaxX = maxX, MaxY = maxY };
        }
    }
}