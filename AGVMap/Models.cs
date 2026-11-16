using System;
using System.Collections.Generic;
using System.Drawing;

namespace AGVMap
{
    public static class StationTypes
    {
        public const int Normal = 0;
        public const int Rack = 1;
        public const int Charging = 3;
        public const int Changing = 4;
        public const int Standby = 5;
        public const int Parking = 6;
        public const int Elevator = 100;
        // 新增 GT_Map 專用的類型對應，可自行擴充
        public const int KeepOut = 999;
    }

    public class DisplayOptions
    {
        public bool ShowLabels { get; set; } = true;
        public bool ShowPaths { get; set; } = true;
        public bool ShowPolygons { get; set; } = true;
        public bool ShowLegend { get; set; } = true;
        public bool ShowVirtualPoints { get; set; } = false;
        public bool ShowBackgroundImage { get; set; } = true;
    }

    public class MapPoint
    {
        public string ID { get; set; } // 改為 string
        public int TagNumber { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int StationType { get; set; }
        public bool Enable { get; set; } = true;
        public bool IsVirtualPoint { get; set; }
        public bool IsAutoDoor { get; set; }
        public Dictionary<string, double> Targets { get; set; } = new Dictionary<string, double>(); // int -> string

        public override string ToString()
        {
            return $"Tag:{TagNumber} ({Name})";
        }
    }

    public class MapPath
    {
        public string FromID { get; set; } // 改為 string
        public string ToID { get; set; }   // 改為 string
    }

    public class Edge
    {
        public string TargetPointID { get; set; } // 改為 string
        public double Weight { get; set; }
    }

    public class MapPolygon
    {
        public string Name { get; set; }
        public List<PointF> Points { get; set; } = new List<PointF>();
    }

    // 新增：禁制區 (GT_Map 專用)
    public class KeepOutZone
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<PointF> Polygon { get; set; } = new List<PointF>();
    }

    public class MapBoundary
    {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        public RectangleF ToRectangle()
        {
            return RectangleF.FromLTRB(MinX, MinY, MaxX, MaxY);
        }
    }

    public class MapInfo
    {
        public string MapName { get; set; }
        public string MapNote { get; set; }
        public string Format { get; set; }
        public int TotalPoints { get; set; }
        public int StationPoints { get; set; }
        public int PathCount { get; set; }
        public int KeepOutCount { get; set; }
    }
}
