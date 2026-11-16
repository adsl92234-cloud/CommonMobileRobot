using System;
using System.Collections.Generic;
using System.Drawing;

namespace AGVMap
{
    using AML;

    /// <summary>
    /// 路徑轉角 / 車頭角度變化過大時自動加點 (方式 B：在轉角前後插入過渡點，路徑幾何不變)。
    /// 所有角度為地圖座標 (影像座標 Y 向下) 的弧度。
    /// </summary>
    public static class RouteTurnRefiner
    {
        /// <param name="route">規劃結果 (地圖座標)</param>
        /// <param name="thresholdDeg">行進方向或車頭角度變化超過此值時加點</param>
        /// <param name="insertDistance">過渡點距轉角的距離 (地圖座標單位)，最多取該段長度 1/3</param>
        public static MapRouteData Refine(MapRouteData route, double thresholdDeg, double insertDistance)
        {
            if (route == null || !route.Success || route.PathCoordinates == null || route.PathCoordinates.Count < 2)
                return route;

            var pts = route.PathCoordinates;
            int edgeCount = pts.Count - 1;
            bool hasEo = route.EdgeOrientations != null && route.EdgeOrientations.Count == edgeCount;
            bool hasIds = route.PathNodeIds != null && route.PathNodeIds.Count == pts.Count;
            double threshold = thresholdDeg * Math.PI / 180.0;

            var dir = new double[edgeCount];
            var head = new double[edgeCount];
            var len = new double[edgeCount];
            for (int i = 0; i < edgeCount; i++)
            {
                double dx = pts[i + 1].X - pts[i].X, dy = pts[i + 1].Y - pts[i].Y;
                len[i] = Math.Sqrt(dx * dx + dy * dy);
                dir[i] = Math.Atan2(dy, dx);
                head[i] = hasEo && route.EdgeOrientations[i].HasValue ? route.EdgeOrientations[i].Value : dir[i];
            }

            var outPts = new List<PointF>();
            var outIds = new List<string>();
            var outTheta = new List<double?>();
            var outEo = new List<double?>();

            outPts.Add(pts[0]);
            outIds.Add(hasIds ? route.PathNodeIds[0] : null);
            outTheta.Add(head[0]);

            for (int k = 1; k < pts.Count; k++)
            {
                bool isLast = k == pts.Count - 1;
                string id = hasIds ? route.PathNodeIds[k] : null;
                double hin = head[k - 1];

                if (isLast)
                {
                    outEo.Add(hin);
                    outPts.Add(pts[k]);
                    outIds.Add(id);
                    outTheta.Add(route.EndTheta ?? hin);
                    break;
                }

                double hout = head[k];
                double dHead = Wrap(hout - hin);
                double dDir = Wrap(dir[k] - dir[k - 1]);
                if (Math.Abs(dHead) <= threshold && Math.Abs(dDir) <= threshold)
                {
                    outEo.Add(hin);
                    outPts.Add(pts[k]);
                    outIds.Add(id);
                    outTheta.Add(hout);
                    continue;
                }

                double dIn = Math.Min(insertDistance, len[k - 1] / 3.0);
                double dOut = Math.Min(insertDistance, len[k] / 3.0);
                var a = Along(pts[k], dir[k - 1] + Math.PI, dIn);
                var b = Along(pts[k], dir[k], dOut);

                // ... -> A : 維持進入方向
                outEo.Add(hin);
                outPts.Add(a); outIds.Add(null); outTheta.Add(hin);
                // A -> 轉角 : 轉一半
                outEo.Add(null);
                outPts.Add(pts[k]); outIds.Add(id); outTheta.Add(Wrap(hin + dHead / 2.0));
                // 轉角 -> B : 轉完
                outEo.Add(null);
                outPts.Add(b); outIds.Add(null); outTheta.Add(hout);
                // B -> 下一點 由下一輪加入
                head[k] = hout;
                // 下一段起點改為 B
                double ndx = pts[k + 1].X - b.X, ndy = pts[k + 1].Y - b.Y;
                len[k] = Math.Sqrt(ndx * ndx + ndy * ndy);
            }

            route.PathCoordinates = outPts;
            route.PathNodeIds = hasIds || outIds.Exists(s => s != null) ? outIds : null;
            route.EdgeOrientations = outEo;
            route.NodeThetas = outTheta;
            return route;
        }

        private static PointF Along(PointF p, double angle, double dist)
        {
            return new PointF((float)(p.X + Math.Cos(angle) * dist), (float)(p.Y + Math.Sin(angle) * dist));
        }

        public static double Wrap(double a)
        {
            while (a > Math.PI) a -= 2 * Math.PI;
            while (a < -Math.PI) a += 2 * Math.PI;
            return a;
        }
    }
}
