using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AML; // 引用 RouteData

namespace AGVMap
{
    public class MapRenderer : IDisposable
    {
        private readonly MapDataManager _mapData;

        // 座標轉換
        private float _scale, _offsetX, _offsetY;
        private float _baseScale, _baseOffsetX, _baseOffsetY;
        private float _zoom = 1f, _panX, _panY;
        private const float MinZoom = 0.25f, MaxZoom = 30f;
        private bool _transformDirty = true;

        // 繪圖資源
        private Pen _pathPen = new Pen(Color.FromArgb(100, 150, 150, 150), 1.5f);
        private Pen _routePen = new Pen(Color.FromArgb(200, 0, 120, 255), 3f);
        private Pen _routeWaypointPen = new Pen(Color.FromArgb(200, 255, 165, 0), 3f);
        private Pen _polygonPen = new Pen(Color.FromArgb(60, 100, 100, 255), 1f);
        private Brush _polygonBrush = new SolidBrush(Color.FromArgb(20, 100, 100, 255));
        
        private Pen _keepOutPen = new Pen(Color.FromArgb(60, 255, 50, 50), 1.5f);
        private Brush _keepOutBrush = new SolidBrush(Color.FromArgb(40, 255, 50, 50));
        private Pen _boundaryPen = new Pen(Color.FromArgb(230, 255, 220, 0), 2.5f);
        
        private Font _labelFont = new Font("Microsoft JhengHei", 7f);
        private Font _legendFont = new Font("Microsoft JhengHei", 8f);

        // 高亮點位
        public string HighlightStart { get; set; } = null;
        public string HighlightEnd { get; set; } = null;
        public string HighlightWaypoint { get; set; } = null;
        
        // ==== 新增：MR 即時真實位置與角度 ====
        public bool ShowMR { get; set; } = false;
        public double MR_X { get; set; } = 0;
        public double MR_Y { get; set; } = 0;
        public double MR_Theta { get; set; } = 0; // 車體角度 (度數)

        // 路徑
        public List<string> CurrentRoutePath { get; set; } = null;
        public List<string> CurrentRoutePathToWaypoint { get; set; } = null;
        public List<string> CurrentRoutePathFromWaypoint { get; set; } = null;
        public List<PointF> CurrentRouteCoordinates { get; set; } = null;

        // 已派送給 MR (VDA 5050) 的節點與車頭朝向 (地圖座標)
        public List<DispatchNode> DispatchedNodes { get; set; } = null;

        // Free 模式點位 (地圖座標)，錯誤點索引為 -1 表示無
        public IList<RoutePoint> FreePoints { get; set; } = null;
        public int FreePointErrorIndex { get; set; } = -1;

        public DisplayOptions Options { get; } = new DisplayOptions();

        public MapRenderer(MapDataManager mapData)
        {
            _mapData = mapData;
            _routePen.DashStyle = DashStyle.Dash;
            _routeWaypointPen.DashStyle = DashStyle.Dash;
        }

        public void InvalidateTransform() => _transformDirty = true;

        private void CalculateTransform(int width, int height)
        {
            if (_mapData.Points.Count == 0) return;
            if (!_transformDirty) { ApplyView(); return; }
            var points = Options.ShowVirtualPoints ? _mapData.Points : _mapData.Points.Where(p => !p.IsVirtualPoint).ToList();
            if (points.Count == 0) points = _mapData.Points;

            double minX = points.Min(p => p.X); double maxX = points.Max(p => p.X);
            double minY = points.Min(p => p.Y); double maxY = points.Max(p => p.Y);

            if (_mapData.Boundary != null)
            {
                minX = Math.Min(minX, _mapData.Boundary.MinX);
                maxX = Math.Max(maxX, _mapData.Boundary.MaxX);
                minY = Math.Min(minY, _mapData.Boundary.MinY);
                maxY = Math.Max(maxY, _mapData.Boundary.MaxY);
            }
            double dataW = maxX - minX; double dataH = maxY - minY;
            if (dataW < 0.001) dataW = 1; if (dataH < 0.001) dataH = 1;

            int margin = 40;
            float scaleX = (width - margin * 2) / (float)dataW;
            float scaleY = (height - margin * 2) / (float)dataH;
            _baseScale = Math.Min(scaleX, scaleY);

            _baseOffsetX = margin + ((width - margin * 2) - (float)dataW * _baseScale) / 2 - (float)minX * _baseScale;
            _baseOffsetY = margin + ((height - margin * 2) - (float)dataH * _baseScale) / 2 - (float)minY * _baseScale;
            _transformDirty = false;
            ApplyView();
        }

        private void ApplyView()
        {
            _scale = _baseScale * _zoom;
            _offsetX = _baseOffsetX * _zoom + _panX;
            _offsetY = _baseOffsetY * _zoom + _panY;
        }

        public float Zoom => _zoom;

        /// <summary>以螢幕位置為中心縮放，游標下的地圖位置不變。</summary>
        public void ZoomAt(Point screen, float factor)
        {
            if (_baseScale <= 0) return;
            float newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, _zoom * factor));
            if (Math.Abs(newZoom - _zoom) < 1e-6f) return;

            float ratio = newZoom / _zoom;
            _panX = screen.X - (screen.X - _panX) * ratio;
            _panY = screen.Y - (screen.Y - _panY) * ratio;
            _zoom = newZoom;
            ApplyView();
        }

        public void PanBy(float dx, float dy)
        {
            _panX += dx;
            _panY += dy;
            ApplyView();
        }

        public void ResetView()
        {
            _zoom = 1f;
            _panX = 0f;
            _panY = 0f;
            ApplyView();
        }

        private PointF Transform(double x, double y) => new PointF((float)x * _scale + _offsetX, (float)y * _scale + _offsetY);

        /// <summary>螢幕座標轉地圖座標 (需已繪製過至少一次)</summary>
        public PointF ScreenToMap(Point screen)
        {
            if (_scale <= 0) return PointF.Empty;
            return new PointF((screen.X - _offsetX) / _scale, (screen.Y - _offsetY) / _scale);
        }

        public void Render(Graphics g, Control panel)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(30, 30, 30));
            if (_mapData == null || _mapData.Points.Count == 0) return;

            CalculateTransform(panel.Width, panel.Height);

            if (Options.ShowBackgroundImage && _mapData.BackgroundImage != null)
            {
                var imageRect = _mapData.Boundary == null
                    ? new RectangleF(_offsetX, _offsetY, _mapData.BackgroundImage.Width * _scale, _mapData.BackgroundImage.Height * _scale)
                    : new RectangleF(Transform(_mapData.Boundary.MinX, _mapData.Boundary.MinY).X,
                        Transform(_mapData.Boundary.MinX, _mapData.Boundary.MinY).Y,
                        (_mapData.Boundary.MaxX - _mapData.Boundary.MinX) * _scale,
                        (_mapData.Boundary.MaxY - _mapData.Boundary.MinY) * _scale);
                g.DrawImage(_mapData.BackgroundImage, imageRect);
            }

            DrawBoundary(g);

            if (Options.ShowPolygons) DrawPolygons(g);
            DrawKeepOutZones(g);
            if (Options.ShowPaths) DrawPaths(g);
            DrawRoute(g);
            DrawPoints(g);
            DrawFreePoints(g);
            DrawDispatchedNodes(g);
            if (Options.ShowLabels) DrawLabels(g);
            DrawAGVMarker(g);
            if (Options.ShowLegend) DrawLegend(g, panel);
        }

        private void DrawBoundary(Graphics g)
        {
            if (_mapData.Boundary == null) return;

            var boundary = _mapData.Boundary;
            var topLeft = Transform(boundary.MinX, boundary.MinY);
            var bottomRight = Transform(boundary.MaxX, boundary.MaxY);
            var rect = RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);

            g.DrawRectangle(_boundaryPen, rect.X, rect.Y, rect.Width, rect.Height);
            using (var labelBrush = new SolidBrush(Color.FromArgb(230, 255, 220, 0)))
                g.DrawString("地圖邊界", _legendFont, labelBrush, rect.Left + 4, rect.Top + 4);
        }

        private void DrawPolygons(Graphics g)
        {
            foreach (var polygon in _mapData.Polygons.Where(p => p.Points.Count >= 3))
            {
                var pts = polygon.Points.Select(p => Transform(p.X, p.Y)).ToArray();
                g.FillPolygon(_polygonBrush, pts); g.DrawPolygon(_polygonPen, pts);
            }
        }

        private void DrawKeepOutZones(Graphics g)
        {
            foreach (var zone in _mapData.KeepOutZones.Where(z => z.Polygon.Count >= 3))
            {
                var pts = zone.Polygon.Select(p => Transform(p.X, p.Y)).ToArray();
                g.FillPolygon(_keepOutBrush, pts); g.DrawPolygon(_keepOutPen, pts);
            }
        }

        private void DrawPaths(Graphics g)
        {
            foreach (var path in _mapData.Paths)
            {
                var from = _mapData.Points.FirstOrDefault(p => p.ID == path.FromID);
                var to = _mapData.Points.FirstOrDefault(p => p.ID == path.ToID);
                if (from == null || to == null) continue;
                if (!Options.ShowVirtualPoints && (from.IsVirtualPoint || to.IsVirtualPoint)) continue;
                g.DrawLine(_pathPen, Transform(from.X, from.Y), Transform(to.X, to.Y));
            }
        }

        private void DrawRoute(Graphics g)
        {
            // A* 的細部路徑才包含繞牆轉折；FullPath 只有站點，直接連線會把
            // dock_6 與 dock_2 畫成穿過障礙物的直線。
            if (CurrentRouteCoordinates != null && CurrentRouteCoordinates.Count > 1)
            {
                var points = CurrentRouteCoordinates.Select(p => Transform(p.X, p.Y)).ToArray();
                g.DrawLines(_routePen, points);
                return;
            }

            if (CurrentRoutePathToWaypoint != null && CurrentRoutePathFromWaypoint != null)
            {
                DrawRouteSegment(g, _routeWaypointPen, CurrentRoutePathToWaypoint);
                DrawRouteSegment(g, _routePen, CurrentRoutePathFromWaypoint);
            }
            else if (CurrentRoutePath != null)
            {
                DrawRouteSegment(g, _routePen, CurrentRoutePath);
            }
        }

        private void DrawDispatchedNodes(Graphics g)
        {
            var nodes = DispatchedNodes;
            if (nodes == null || nodes.Count == 0) return;

            using (var linePen = new Pen(Color.FromArgb(220, 0, 230, 120), 2f))
            using (var headPen = new Pen(Color.Yellow, 2f) { CustomEndCap = new AdjustableArrowCap(4f, 4f) })
            using (var outline = new Pen(Color.Black, 1f))
            using (var textBrush = new SolidBrush(Color.White))
            {
                if (nodes.Count > 1)
                    g.DrawLines(linePen, nodes.Select(n => Transform(n.Position.X, n.Position.Y)).ToArray());

                for (int i = 0; i < nodes.Count; i++)
                {
                    var n = nodes[i];
                    var c = Transform(n.Position.X, n.Position.Y);
                    if (n.Theta.HasValue)
                    {
                        float len = 24f;
                        var tip = new PointF(c.X + (float)Math.Cos(n.Theta.Value) * len, c.Y + (float)Math.Sin(n.Theta.Value) * len);
                        g.DrawLine(headPen, c, tip);
                    }

                    float r = 5f;
                    Brush fill = i == 0 ? Brushes.LimeGreen : i == nodes.Count - 1 ? Brushes.Red : Brushes.Cyan;
                    g.FillEllipse(fill, c.X - r, c.Y - r, r * 2, r * 2);
                    g.DrawEllipse(outline, c.X - r, c.Y - r, r * 2, r * 2);
                    g.DrawString(n.Id, _labelFont, textBrush, c.X + r + 2, c.Y + r);
                }
            }
        }

        private void DrawRouteSegment(Graphics g, Pen pen, List<string> pathIds)
        {
            for (int i = 0; i < pathIds.Count - 1; i++)
            {
                var from = _mapData.Points.FirstOrDefault(p => p.ID == pathIds[i]);
                var to = _mapData.Points.FirstOrDefault(p => p.ID == pathIds[i + 1]);
                if (from == null || to == null) continue;
                g.DrawLine(pen, Transform(from.X, from.Y), Transform(to.X, to.Y));
            }
        }

        private void DrawPoints(Graphics g)
        {
            foreach (var point in _mapData.Points)
            {
                if (!Options.ShowVirtualPoints && point.IsVirtualPoint && point.ID != "MR_CURRENT") continue;

                var pos = Transform(point.X, point.Y);
                float r = GetPointRadius(point);
                Brush brush = GetPointBrush(point);

                g.FillEllipse(brush, pos.X - r, pos.Y - r, r * 2, r * 2);

                if (point.ID == HighlightStart || point.ID == HighlightEnd || point.ID == HighlightWaypoint)
                {
                    using (var pen = new Pen(Color.White, 2f))
                        g.DrawEllipse(pen, pos.X - r - 1, pos.Y - r - 1, (r + 1) * 2, (r + 1) * 2);
                }
            }
        }

        private float GetPointRadius(MapPoint point)
        {
            if (point.ID == HighlightStart || point.ID == HighlightEnd || point.ID == HighlightWaypoint) return 7f;
            if (point.IsVirtualPoint) return 3f;
            return 5f;
        }

        private Brush GetPointBrush(MapPoint point)
        {
            if (point.ID == HighlightStart) return Brushes.LimeGreen;
            if (point.ID == HighlightWaypoint) return Brushes.Gold;
            if (point.ID == HighlightEnd) return Brushes.Red;
            if (!point.Enable) return Brushes.Gray;
            if (point.IsVirtualPoint) return Brushes.DimGray;
            switch (point.StationType)
            {
                case StationTypes.Rack: return Brushes.Orange;
                case StationTypes.Charging: return Brushes.LimeGreen;
                case StationTypes.Changing: return Brushes.DeepPink;
                case StationTypes.Standby: return Brushes.SteelBlue;
                case StationTypes.Parking: return Brushes.SlateGray;
                case StationTypes.Elevator: return Brushes.MediumPurple;
                default: return Brushes.DodgerBlue;
            }
        }

        // ==== 新增：繪製帶有方向性的即時車體 ====
        private void DrawFreePoints(Graphics g)
        {
            if (FreePoints == null || FreePoints.Count == 0) return;

            if (CurrentRouteCoordinates == null && FreePoints.Count >= 2)
            {
                using (var pen = new Pen(Color.FromArgb(160, 0, 200, 255), 1.5f) { DashStyle = DashStyle.Dot })
                    g.DrawLines(pen, FreePoints.Select(p => Transform(p.X, p.Y)).ToArray());
            }

            for (int i = 0; i < FreePoints.Count; i++)
            {
                var p = FreePoints[i];
                var c = Transform(p.X, p.Y);
                bool isError = i == FreePointErrorIndex;
                Color fill = isError ? Color.Red
                           : i == 0 ? Color.LimeGreen
                           : i == FreePoints.Count - 1 ? Color.Orange
                           : Color.DeepSkyBlue;

                if (p.Theta.HasValue)
                {
                    float len = 28f;
                    var tip = new PointF(c.X + (float)Math.Cos(p.Theta.Value) * len, c.Y + (float)Math.Sin(p.Theta.Value) * len);
                    using (var arrow = new Pen(fill, 3f) { EndCap = LineCap.ArrowAnchor })
                        g.DrawLine(arrow, c, tip);
                }

                using (var brush = new SolidBrush(fill))
                    g.FillEllipse(brush, c.X - 7, c.Y - 7, 14, 14);
                g.DrawEllipse(isError ? Pens.White : Pens.Black, c.X - 7, c.Y - 7, 14, 14);
                g.DrawString((i + 1).ToString(), _labelFont, Brushes.White, c.X + 8, c.Y - 16);
            }
        }

        private void DrawAGVMarker(Graphics g)
        {
            if (!ShowMR) return;

            var pos = Transform(MR_X, MR_Y);

            // 備份當前繪圖狀態
            var state = g.Save();

            // 將畫布原點移動到 MR 座標
            g.TranslateTransform(pos.X, pos.Y);
            // 依照 Theta 旋轉 (這取決於地圖座標系，可能需要加負號或補角)
            g.RotateTransform((float)MR_Theta); 

            // 外圈（半透明大圓）
            using (var brush = new SolidBrush(Color.FromArgb(60, 0, 255, 255)))
                g.FillEllipse(brush, -16, -16, 32, 32);

            // 內圈主體
            using (var brush = new SolidBrush(Color.FromArgb(150, 0, 255, 255)))
                g.FillEllipse(brush, -10, -10, 20, 20);

            // 方向箭頭 (紅色三角形)
            PointF[] arrow = { new PointF(12, 0), new PointF(2, -6), new PointF(2, 6) };
            g.FillPolygon(Brushes.Red, arrow);

            // 外框
            using (var pen = new Pen(Color.White, 2f))
                g.DrawEllipse(pen, -10, -10, 20, 20);

            // 恢復畫布狀態
            g.Restore(state);

            // 標籤
            string label = "AGV";
            var labelSize = g.MeasureString(label, _legendFont);
            g.DrawString(label, _legendFont, Brushes.Cyan, pos.X - labelSize.Width / 2, pos.Y - 26);
        }

        private void DrawLabels(Graphics g)
        {
            foreach (var point in _mapData.Points)
            {
                if (!Options.ShowVirtualPoints && point.IsVirtualPoint) continue;
                if (point.ID == "MR_CURRENT") continue; // 虛擬起點不畫文字

                var pos = Transform(point.X, point.Y);
                string label = $"{point.TagNumber}";
                var size = g.MeasureString(label, _labelFont);
                g.DrawString(label, _labelFont, Brushes.White, pos.X - size.Width / 2, pos.Y + 8);
            }
        }

        private void DrawLegend(Graphics g, Control panel)
        {
            int x = 10, y = 10, spacing = 20, boxSize = 12;
            var items = new List<(string text, Brush brush)>
            {
                ("一般站點", Brushes.DodgerBlue),
                ("貨架", Brushes.Orange),
                ("充電站", Brushes.LimeGreen),
                ("禁制區", Brushes.LightCoral),
                ("地圖邊界", Brushes.Gold),
            };

            if (HighlightStart != null) items.Add(("起點", Brushes.LimeGreen));
            if (HighlightEnd != null) items.Add(("終點", Brushes.Red));
            if (ShowMR) items.Add(("AGV 位置", Brushes.Cyan));

            int bgHeight = items.Count * spacing + 10;
            using (var bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                g.FillRectangle(bgBrush, x - 5, y - 5, 120, bgHeight);

            foreach (var (text, brush) in items)
            {
                g.FillRectangle(brush, x, y, boxSize, boxSize);
                g.DrawString(text, _legendFont, Brushes.White, x + boxSize + 5, y - 2);
                y += spacing;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _pathPen?.Dispose(); _routePen?.Dispose(); _routeWaypointPen?.Dispose();
                _polygonPen?.Dispose(); _polygonBrush?.Dispose();
                _keepOutPen?.Dispose(); _keepOutBrush?.Dispose();
            _boundaryPen?.Dispose();
                _labelFont?.Dispose(); _legendFont?.Dispose();
                _disposed = true;
            }
        }
        private bool _disposed;
    }

    /// <summary>已派送給 MR 的節點 (地圖座標)，Theta 為車頭朝向 (弧度，地圖座標)</summary>
    public class DispatchNode
    {
        public string Id { get; set; }
        public PointF Position { get; set; }
        public double? Theta { get; set; }
    }
}