using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AGVMap
{
    public class MapDataManager
    {
        // 地圖基本資訊與底圖
        public string MapName { get; set; } = "";
        public string MapNote { get; set; } = "";
        public string Format { get; set; } = "";
        public Image BackgroundImage { get; set; } = null;
        public MapBoundary Boundary { get; set; } = null;

        // 核心資料結構
        public List<MapPoint> Points { get; private set; } = new List<MapPoint>();
        public List<MapPath> Paths { get; private set; } = new List<MapPath>();
        public List<MapPolygon> Polygons { get; private set; } = new List<MapPolygon>();
        public List<KeepOutZone> KeepOutZones { get; private set; } = new List<KeepOutZone>();
        public Dictionary<string, List<Edge>> AdjacencyList { get; private set; } = new Dictionary<string, List<Edge>>();
        public Dictionary<string, MapPoint> PointDict { get; private set; } = new Dictionary<string, MapPoint>();

        // 支援的解析器列表
        private readonly List<IMapParser> _parsers = new List<IMapParser>();

        public MapDataManager()
        {
            // 註冊新舊格式的 Parser
            _parsers.Add(new GpmMapParser());
            _parsers.Add(new GtMapParser());
        }

        public void LoadFromJson(string json)
        {
            ClearData();

            IMapParser selectedParser = _parsers.FirstOrDefault(p => p.CanParse(json));
            if (selectedParser == null)
            {
                throw new Exception("無法辨識的地圖格式，找不到對應的 Parser");
            }

            bool success = selectedParser.Parse(json, this);
            if (!success)
            {
                throw new Exception($"地圖解析失敗 ({selectedParser.GetType().Name})");
            }
        }

        public void ClearData()
        {
            MapName = "";
            MapNote = "";
            Format = "";
            Boundary = null;
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
                BackgroundImage = null;
            }
            Points.Clear();
            Paths.Clear();
            Polygons.Clear();
            KeepOutZones.Clear();
            AdjacencyList.Clear();
            PointDict.Clear();
        }

        public void BuildPointDict()
        {
            PointDict.Clear();
            foreach (var point in Points)
            {
                PointDict[point.ID] = point;
            }
        }

        public List<MapPoint> GetStationPoints()
        {
            return Points.Where(p => !p.IsVirtualPoint && p.Enable).OrderBy(p => p.TagNumber).ToList();
        }

        public string GetMapInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"地圖名稱: {MapName}");
            sb.AppendLine($"格式: {Format}");
            sb.AppendLine($"總點位: {Points.Count}");
            sb.AppendLine($"路徑段數: {Paths.Count}");
            sb.AppendLine($"禁制區數: {KeepOutZones.Count}");
            return sb.ToString();
        }
    }
}
