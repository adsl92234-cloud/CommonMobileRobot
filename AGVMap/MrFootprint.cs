using System;
using System.Collections.Generic;
using System.Drawing;

namespace AGVMap
{
    /// <summary>
    /// 規劃用的路徑點：站點或自由點 (地圖座標)。
    /// </summary>
    public class RoutePoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>站點 ID，自由點為 null。</summary>
        public string StationId { get; set; }

        /// <summary>是否為 MR 目前位置。</summary>
        public bool IsCurrentPosition { get; set; }

        /// <summary>指定朝向 (弧度，與 Atan2(dy, dx) 同座標系)，未指定為 null。僅終點有效。</summary>
        public double? Theta { get; set; }
    }

    /// <summary>
    /// MR 外形參數 (單位 mm)。車體座標系：U 為前進方向 (長邊)，V 為側向 (短邊)，
    /// 原點為定位參考點，RefOffset 為參考點相對車體中心的位置。
    /// </summary>
    public class MrFootprint
    {
        public double LengthMm { get; set; } = 940.0;
        public double WidthMm { get; set; } = 510.0;
        public double MarginMm { get; set; } = 50.0;
        public double RefOffsetXMm { get; set; } = 0.0;
        public double RefOffsetYMm { get; set; } = 0.0;
        public double MmPerPixel { get; set; } = 30.0;

        private double UMin => -RefOffsetXMm - LengthMm / 2.0 - MarginMm;
        private double UMax => -RefOffsetXMm + LengthMm / 2.0 + MarginMm;
        private double VMin => -RefOffsetYMm - WidthMm / 2.0 - MarginMm;
        private double VMax => -RefOffsetYMm + WidthMm / 2.0 + MarginMm;

        /// <summary>內切半徑 (pixel)：參考點到含邊距矩形最近邊的距離。</summary>
        public double InscribedRadiusPx
        {
            get
            {
                double r = Math.Min(Math.Min(Math.Abs(UMin), Math.Abs(UMax)),
                                    Math.Min(Math.Abs(VMin), Math.Abs(VMax)));
                return r / MmPerPixel;
            }
        }

        /// <summary>外接半徑 (pixel)：參考點到含邊距矩形最遠角的距離。</summary>
        public double CircumscribedRadiusPx
        {
            get
            {
                double[] us = { UMin, UMax };
                double[] vs = { VMin, VMax };
                double max = 0;
                foreach (double u in us)
                    foreach (double v in vs)
                        max = Math.Max(max, Math.Sqrt(u * u + v * v));
                return max / MmPerPixel;
            }
        }

        /// <summary>
        /// 產生車頭朝向 angleDeg (影像座標，0 度朝 +X) 時，含邊距矩形相對參考點所覆蓋的 pixel 偏移。
        /// </summary>
        public Point[] BuildOffsets(double angleDeg)
        {
            double rad = angleDeg * Math.PI / 180.0;
            double cos = Math.Cos(rad), sin = Math.Sin(rad);
            double step = MmPerPixel * 0.5;

            int nu = (int)Math.Ceiling((UMax - UMin) / step);
            int nv = (int)Math.Ceiling((VMax - VMin) / step);
            var set = new HashSet<Point>();

            for (int i = 0; i <= nu; i++)
            {
                double u = Math.Min(UMin + i * step, UMax);
                for (int j = 0; j <= nv; j++)
                {
                    double v = Math.Min(VMin + j * step, VMax);
                    double x = u * cos - v * sin;
                    double y = u * sin + v * cos;
                    set.Add(new Point((int)Math.Round(x / MmPerPixel), (int)Math.Round(y / MmPerPixel)));
                }
            }

            var result = new Point[set.Count];
            set.CopyTo(result);
            return result;
        }
    }
}
