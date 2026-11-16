using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace AGVRoutePlanner
{
    /// <summary>
    /// 負責將地圖繪製到 Panel 上
    /// </summary>
    public class MapRenderer
    {
        private readonly MapDataManager _mapData;
        private RouteResult _currentRoute;

        // 繪製參數
        private float _margin = 40f;
        private float _scale = 1f;
        private float _offsetX = 0f;
        private float _offsetY = 0f;

        // 高亮的起點、必經點、終點 ID
        public int HighlightStart { get; set; } = -1;
        public int HighlightWaypoint { get; set; } = -1;
        public int HighlightEnd { get; set; } = -1;

        public MapRenderer(MapDataManager mapData)
        {
            _mapData = mapData;
        }

        public void SetRoute(RouteResult route)
        {
            _currentRoute = route;
        }

        public void ClearRoute()
        {
            _currentRoute = null;
        }

        /// <summary>
        /// 根據面板大小計算縮放和偏移
        /// </summary>
        private void CalculateTransform(int panelWidth, int panelHeight)
        {
            double rangeX = _mapData.MaxX - _mapData.MinX;
            double rangeY = _mapData.MaxY - _mapData.MinY;

            if (rangeX <= 0) rangeX = 1;
            if (rangeY <= 0) rangeY = 1;

            float availW = panelWidth - 2 * _margin;
            float availH = panelHeight - 2 * _margin;

            _scale = (float)Math.Min(availW / rangeX, availH / rangeY);
            _offsetX = _margin;
            _offsetY = _margin;
        }

        /// <summary>
        /// 將地圖座標轉換為畫面像素座標
        /// </summary>
        private PointF ToScreen(double mapX, double mapY)
        {
            float sx = _offsetX + (float)(mapX - _mapData.MinX) * _scale;
            // Y 軸翻轉（地圖 Y 往上為正，螢幕 Y 往下為正）
            float sy = _offsetY + (float)(_mapData.MaxY - mapY) * _scale;
            return new PointF(sx, sy);
        }

        /// <summary>
        /// 主繪製方法
        /// </summary>
        public void Render(Graphics g, int panelWidth, int panelHeight)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(30, 30, 35));

            if (_mapData?.Map == null) return;

            CalculateTransform(panelWidth, panelHeight);

            DrawPolygons(g);
            DrawPaths(g);
            DrawRoute(g);
            DrawPoints(g);
            DrawLegend(g, panelWidth);
        }

        private void DrawPolygons(Graphics g)
        {
            if (_mapData.Map.Polygons == null) return;

            using (var brush = new SolidBrush(Color.FromArgb(40, 255, 200, 0)))
            using (var pen = new Pen(Color.FromArgb(120, 255, 200, 0), 1.5f) { DashStyle = DashStyle.Dash })
            {
                foreach (var poly in _mapData.Map.Polygons)
                {
                    if (poly.Vertices == null || poly.Vertices.Count < 3) continue;

                    var pts = poly.Vertices.Select(v => ToScreen(v.X, v.Y)).ToArray();
                    g.FillPolygon(brush, pts);
                    g.DrawPolygon(pen, pts);

                    // 標註名稱
                    float cx = pts.Average(p => p.X);
                    float cy = pts.Average(p => p.Y);
                    using (var font = new Font("Microsoft JhengHei", 7f))
                    using (var sb = new SolidBrush(Color.FromArgb(200, 255, 200, 0)))
                    {
                        var sf = new StringFormat { Alignment = StringAlignment.Center };
                        g.DrawString(poly.Name, font, sb, cx, cy, sf);
                    }
                }
            }
        }

        private void DrawPaths(Graphics g)
        {
            using (var arrowPen = new Pen(Color.FromArgb(80, 100, 140, 180), 1.2f))
            {
                arrowPen.CustomEndCap = new AdjustableArrowCap(3, 3);

                foreach (var path in _mapData.Map.Paths)
                {
                    if (!_mapData.PointDict.ContainsKey(path.StartPointID) ||
                        !_mapData.PointDict.ContainsKey(path.EndPointID))
                        continue;

                    var p1 = _mapData.PointDict[path.StartPointID];
                    var p2 = _mapData.PointDict[path.EndPointID];

                    PointF s1 = ToScreen(p1.X, p1.Y);
                    PointF s2 = ToScreen(p2.X, p2.Y);

                    g.DrawLine(arrowPen, s1, s2);
                }
            }
        }

        private void DrawRoute(Graphics g)
        {
            if (_currentRoute == null || !_currentRoute.Success) return;

            // 繪製 起點→必經點 段（藍色）
            if (_currentRoute.PathToWaypoint != null && _currentRoute.PathToWaypoint.Count > 1)
            {
                DrawPathSegment(g, _currentRoute.PathToWaypoint, Color.FromArgb(220, 0, 180, 255), 3.5f);
            }

            // 繪製 必經點→終點 段（綠色）
            if (_currentRoute.PathFromWaypoint != null && _currentRoute.PathFromWaypoint.Count > 1)
            {
                DrawPathSegment(g, _currentRoute.PathFromWaypoint, Color.FromArgb(220, 0, 255, 100), 3.5f);
            }

            // 若無必經點，整段用橙色
            if (_currentRoute.PathToWaypoint == null || _currentRoute.PathToWaypoint.Count == 0)
            {
                if (_currentRoute.FullPath.Count > 1)
                    DrawPathSegment(g, _currentRoute.FullPath, Color.FromArgb(230, 255, 165, 0), 3.5f);
            }
        }

        private void DrawPathSegment(Graphics g, List<int> pointIds, Color color, float width)
        {
            using (var pen = new Pen(color, width) { LineJoin = LineJoin.Round })
            {
                pen.CustomEndCap = new AdjustableArrowCap(4, 4);

                for (int i = 0; i < pointIds.Count - 1; i++)
                {
                    if (!_mapData.PointDict.ContainsKey(pointIds[i]) ||
                        !_mapData.PointDict.ContainsKey(pointIds[i + 1]))
                        continue;

                    var pa = _mapData.PointDict[pointIds[i]];
                    var pb = _mapData.PointDict[pointIds[i + 1]];
                    g.DrawLine(pen, ToScreen(pa.X, pa.Y), ToScreen(pb.X, pb.Y));
                }
            }

            // 沿路徑繪製序號
            using (var font = new Font("Consolas", 7f, FontStyle.Bold))
            using (var brush = new SolidBrush(color))
            {
                for (int i = 0; i < pointIds.Count; i++)
                {
                    if (!_mapData.PointDict.ContainsKey(pointIds[i])) continue;
                    var pt = _mapData.PointDict[pointIds[i]];
                    var sp = ToScreen(pt.X, pt.Y);
                    g.DrawString((i + 1).ToString(), font, brush, sp.X + 6, sp.Y - 14);
                }
            }
        }

        private void DrawPoints(Graphics g)
        {
            var fontNormal = new Font("Microsoft JhengHei", 6.5f);
            var fontBold = new Font("Microsoft JhengHei", 8f, FontStyle.Bold);

            foreach (var pt in _mapData.Map.Points)
            {
                PointF sp = ToScreen(pt.X, pt.Y);
                float r = 4f;
                Color pointColor;
                float radius = r;

                bool isOnRoute = _currentRoute != null && _currentRoute.Success &&
                                 _currentRoute.FullPath.Contains(pt.ID);

                if (pt.ID == HighlightStart)
                {
                    pointColor = Color.Lime;
                    radius = 8f;
                }
                else if (pt.ID == HighlightWaypoint)
                {
                    pointColor = Color.Gold;
                    radius = 8f;
                }
                else if (pt.ID == HighlightEnd)
                {
                    pointColor = Color.Red;
                    radius = 8f;
                }
                else if (pt.IsVirtualPoint)
                {
                    pointColor = Color.FromArgb(140, 128, 128, 128);
                    radius = 2.5f;
                }
                else if (pt.StationType == 3) // 換電站
                {
                    pointColor = Color.FromArgb(220, 255, 100, 255);
                    radius = 6f;
                }
                else if (pt.StationType == 100) // 電梯
                {
                    pointColor = Color.FromArgb(220, 255, 200, 50);
                    radius = 6f;
                }
                else if (pt.StationType == 1) // 料架
                {
                    pointColor = Color.FromArgb(220, 100, 200, 255);
                    radius = 6f;
                }
                else
                {
                    pointColor = isOnRoute ? Color.White : Color.FromArgb(180, 200, 200, 200);
                    radius = isOnRoute ? 5f : 3.5f;
                }

                // 繪製點
                using (var brush = new SolidBrush(pointColor))
                {
                    g.FillEllipse(brush, sp.X - radius, sp.Y - radius, radius * 2, radius * 2);
                }

                // 特殊點外框
                if (pt.ID == HighlightStart || pt.ID == HighlightWaypoint || pt.ID == HighlightEnd)
                {
                    using (var pen = new Pen(Color.White, 2f))
                        g.DrawEllipse(pen, sp.X - radius - 2, sp.Y - radius - 2, (radius + 2) * 2, (radius + 2) * 2);
                }

                // 標註名稱（僅非虛擬點）
                if (!pt.IsVirtualPoint && !string.IsNullOrEmpty(pt.Name))
                {
                    bool isHighlight = pt.ID == HighlightStart || pt.ID == HighlightWaypoint || pt.ID == HighlightEnd;
                    var f = isHighlight ? fontBold : fontNormal;
                    var c = isHighlight ? Color.White : Color.FromArgb(180, 200, 200, 200);
                    using (var sb = new SolidBrush(c))
                    {
                        g.DrawString(pt.Name, f, sb, sp.X + radius + 2, sp.Y - 4);
                    }
                }
            }

            fontNormal.Dispose();
            fontBold.Dispose();
        }

        private void DrawLegend(Graphics g, int panelWidth)
        {
            int x = panelWidth - 180;
            int y = 10;
            int boxSize = 10;
            int lineH = 18;

            using (var bgBrush = new SolidBrush(Color.FromArgb(180, 20, 20, 25)))
            using (var font = new Font("Microsoft JhengHei", 7.5f))
            using (var whiteBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(bgBrush, x - 8, y - 4, 176, lineH * 7 + 8);

                var legends = new (Color color, string text)[]
                {
                    (Color.Lime, "起點"),
                    (Color.Gold, "必經點"),
                    (Color.Red, "終點"),
                    (Color.FromArgb(220, 0, 180, 255), "路線段 1 (→必經)"),
                    (Color.FromArgb(220, 0, 255, 100), "路線段 2 (→終點)"),
                    (Color.FromArgb(220, 255, 165, 0), "路線 (無必經點)"),
                    (Color.FromArgb(140, 128, 128, 128), "虛擬點"),
                };

                foreach (var (c, t) in legends)
                {
                    using (var brush = new SolidBrush(c))
                        g.FillRectangle(brush, x, y + 2, boxSize, boxSize);
                    g.DrawString(t, font, whiteBrush, x + boxSize + 4, y - 1);
                    y += lineH;
                }
            }
        }
    }
}