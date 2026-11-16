using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using AML; // 引用 MapRouteData

namespace AGVMap
{
    public class GridAStarPlanner : IPathPlanner, IFootprintPlanner
    {
        private readonly MapDataManager _mapData;

        // 網格與碰撞資訊
        private bool[,] _obstacleGrid; // 實體牆壁與禁制區
        private bool[,] _knownFreeGrid; // 已掃描的空曠區 (白色像素)，灰色未知區不列入
        private bool[,] _unknownGrid;   // 灰色未知區 (可通行但成本較高)
        private float[,] _distGrid;    // 每格到最近障礙的距離 (Pixels)
        private float _maxDist;        // _distGrid 最大值
        private int _gridWidth;
        private int _gridHeight;

        private bool _isGridInitialized = false;

        // MR 外形與碰撞快取
        private MrFootprint _footprint;
        private double _inscribedPx;
        private double _circumscribedPx;
        private int _borderPx;
        private sbyte[][,] _dirState;                       // 8 個朝向的可通行快取 (0 未知, 1 可行, 2 不可行)
        private Dictionary<int, Point[]> _offsetCache;      // 依整數角度快取足跡偏移

        private bool _ignoreRotation;   // 診斷用：暫時不要求轉向的旋轉空間
        private const int AnyDir = 8;       // 起點尚未決定朝向
        private const double TurnCost = 3.0; // 轉向懲罰 (Pixels)

        // 8 方向 (與 DirAngles 對應)
        private static readonly Point[] Directions = {
            new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0),
            new Point(-1, -1), new Point(1, -1), new Point(-1, 1), new Point(1, 1)
        };
        private static readonly int[] DirAngles = { 270, 90, 180, 0, 225, 315, 135, 45 };

        public string LastError { get; private set; }
        public AML_LogPage Log { get; set; }
        public int LastErrorPointIndex { get; private set; } = -1;
        /// <summary>灰色未知區的平移成本倍率 (>= 1)</summary>
        public double UnknownCostFactor { get; set; } = 3.0;

        public GridAStarPlanner(MapDataManager mapData, MrFootprint footprint = null)
        {
            _mapData = mapData;
            ApplyFootprint(footprint ?? new MrFootprint());
            InitializeGrid();
        }

        /// <summary>
        /// 該地圖座標是否為已知空曠區，且 MR 以任意朝向停放都不會碰撞 (隨機目標點用)。
        /// </summary>
        public bool IsFreeForAnyHeading(double x, double y)
        {
            if (!_isGridInitialized) return false;
            Point g = MapToGrid(x, y);
            if (g.X < 0 || g.Y < 0 || g.X >= _gridWidth || g.Y >= _gridHeight) return false;
            if (!_knownFreeGrid[g.X, g.Y] || _obstacleGrid[g.X, g.Y]) return false;
            return _distGrid[g.X, g.Y] >= _circumscribedPx + 1;
        }

        public void SetFootprint(MrFootprint footprint)
        {
            ApplyFootprint(footprint ?? new MrFootprint());
        }

        /// <summary>網格統計資訊 (驗證座標與未知區設定用)</summary>
        public string GetGridSummary()
        {
            if (!_isGridInitialized) return "網格尚未初始化";
            int obstacle = 0, unknown = 0, free = 0;
            for (int x = 0; x < _gridWidth; x++)
                for (int y = 0; y < _gridHeight; y++)
                {
                    if (_obstacleGrid[x, y]) obstacle++;
                    else if (_unknownGrid[x, y]) unknown++;
                    else if (_knownFreeGrid[x, y]) free++;
                }
            return $"網格 {_gridWidth}x{_gridHeight}，障礙 {obstacle}、未知 {unknown}、已知空曠 {free}，UnknownCostFactor={UnknownCostFactor:F1}";
        }

        private void ApplyFootprint(MrFootprint footprint)
        {
            _footprint = footprint;
            _inscribedPx = footprint.InscribedRadiusPx;
            _circumscribedPx = footprint.CircumscribedRadiusPx;
            _borderPx = (int)Math.Ceiling(_circumscribedPx) + 1;
            _dirState = new sbyte[8][,];
            _offsetCache = new Dictionary<int, Point[]>();
        }

        // GT Map 的站點座標是地圖座標，底圖則是像素座標。兩者只有在
        // boundary 恰好等於 image size 時才相同，不能直接 cast 成 int。
        private Point MapToGrid(double x, double y)
        {
            if (_mapData.Boundary == null)
                return new Point((int)Math.Round(x), (int)Math.Round(y));

            float width = _mapData.Boundary.MaxX - _mapData.Boundary.MinX;
            float height = _mapData.Boundary.MaxY - _mapData.Boundary.MinY;
            if (width <= 0 || height <= 0)
                return new Point((int)Math.Round(x), (int)Math.Round(y));

            return new Point(
                (int)Math.Round((x - _mapData.Boundary.MinX) / width * (_gridWidth - 1)),
                (int)Math.Round((y - _mapData.Boundary.MinY) / height * (_gridHeight - 1)));
        }

        private PointF GridToMap(Point p)
        {
            if (_mapData.Boundary == null)
                return new PointF(p.X, p.Y);

            float width = _mapData.Boundary.MaxX - _mapData.Boundary.MinX;
            float height = _mapData.Boundary.MaxY - _mapData.Boundary.MinY;
            return new PointF(
                _mapData.Boundary.MinX + p.X * width / Math.Max(1, _gridWidth - 1),
                _mapData.Boundary.MinY + p.Y * height / Math.Max(1, _gridHeight - 1));
        }

        // ==========================================
        //  1. 影像網格化與距離圖
        // ==========================================

        private void InitializeGrid()
        {
            if (_mapData.Points.Count == 0 || _mapData.BackgroundImage == null) return;

            // 取得圖片寬高
            _gridWidth = _mapData.BackgroundImage.Width;
            _gridHeight = _mapData.BackgroundImage.Height;

            _obstacleGrid = new bool[_gridWidth, _gridHeight];
            _knownFreeGrid = new bool[_gridWidth, _gridHeight];
            _unknownGrid = new bool[_gridWidth, _gridHeight];

            // [修正]：強制建立一張統一為 32bppArgb 的新 Bitmap，避免原始圖片格式衝突
            using (Bitmap standardizedBmp = new Bitmap(_gridWidth, _gridHeight, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(standardizedBmp))
                {
                    // 將原始底圖畫上去
                    g.DrawImage(_mapData.BackgroundImage, 0, 0, _gridWidth, _gridHeight);
                }

                // 使用 LockBits 進行極速影像記憶體解析
                Rectangle rect = new Rectangle(0, 0, _gridWidth, _gridHeight);
                BitmapData bmpData = standardizedBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                // 計算總位元組
                int bytes = Math.Abs(bmpData.Stride) * _gridHeight;
                byte[] rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                standardizedBmp.UnlockBits(bmpData);

                // 解析像素建立碰撞網格
                for (int y = 0; y < _gridHeight; y++)
                {
                    for (int x = 0; x < _gridWidth; x++)
                    {
                        // 32bpp: 每個像素佔 4 bytes (B, G, R, A)
                        int p = (y * bmpData.Stride) + (x * 4);
                        byte b = rgbValues[p];
                        byte g = rgbValues[p + 1];
                        byte r = rgbValues[p + 2];

                        // 灰階值小於 100 視為黑色牆壁障礙物
                        double lum = r * 0.3 + g * 0.59 + b * 0.11;
                        _obstacleGrid[x, y] = lum < 100;
                        _knownFreeGrid[x, y] = lum > 200;
                        _unknownGrid[x, y] = lum >= 100 && lum <= 200;
                    }
                }
            } // standardizedBmp 被自動釋放

            if (_mapData.Boundary != null)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    for (int y = 0; y < _gridHeight; y++)
                    {
                        if (!IsInsideBoundary(x, y))
                            _obstacleGrid[x, y] = true;
                    }
                }
            }

            // 疊加 KeepOutZones 禁制區 (多邊形為地圖座標，逐格轉為地圖座標比對)
            foreach (var zone in _mapData.KeepOutZones)
            {
                if (zone.Polygon == null || zone.Polygon.Count < 3) continue;

                Point gMin = MapToGrid(zone.Polygon.Min(p => p.X), zone.Polygon.Min(p => p.Y));
                Point gMax = MapToGrid(zone.Polygon.Max(p => p.X), zone.Polygon.Max(p => p.Y));
                int x0 = Math.Max(0, Math.Min(gMin.X, gMax.X) - 1), x1 = Math.Min(_gridWidth - 1, Math.Max(gMin.X, gMax.X) + 1);
                int y0 = Math.Max(0, Math.Min(gMin.Y, gMax.Y) - 1), y1 = Math.Min(_gridHeight - 1, Math.Max(gMin.Y, gMax.Y) + 1);

                for (int x = x0; x <= x1; x++)
                {
                    for (int y = y0; y <= y1; y++)
                    {
                        if (!_obstacleGrid[x, y] && IsPointInPolygon(GridToMap(new Point(x, y)), zone.Polygon))
                            _obstacleGrid[x, y] = true;
                    }
                }
            }

            ComputeDistanceGrid();

            _isGridInitialized = true;
        }

        /// <summary>
        /// 精確歐氏距離轉換 (Felzenszwalb)，結果為每格到最近障礙像素的距離。
        /// </summary>
        private void ComputeDistanceGrid()
        {
            const double Inf = 1e12;
            int n = Math.Max(_gridWidth, _gridHeight);
            var f = new double[n];
            var d = new double[n];
            var v = new int[n];
            var z = new double[n + 1];

            var sq = new double[_gridWidth, _gridHeight];

            // 先處理每一欄 (Y 方向)
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                    f[y] = _obstacleGrid[x, y] ? 0 : Inf;
                Edt1D(f, d, v, z, _gridHeight);
                for (int y = 0; y < _gridHeight; y++)
                    sq[x, y] = d[y];
            }

            // 再處理每一列 (X 方向)
            _distGrid = new float[_gridWidth, _gridHeight];
            _maxDist = 0f;
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                    f[x] = sq[x, y];
                Edt1D(f, d, v, z, _gridWidth);
                for (int x = 0; x < _gridWidth; x++)
                {
                    _distGrid[x, y] = (float)Math.Sqrt(d[x]);
                    if (_distGrid[x, y] > _maxDist) _maxDist = _distGrid[x, y];
                }
            }
        }

        private static void Edt1D(double[] f, double[] d, int[] v, double[] z, int n)
        {
            int k = 0;
            v[0] = 0;
            z[0] = double.NegativeInfinity;
            z[1] = double.PositiveInfinity;
            for (int q = 1; q < n; q++)
            {
                double s;
                while (true)
                {
                    int p = v[k];
                    s = ((f[q] + (double)q * q) - (f[p] + (double)p * p)) / (2.0 * q - 2.0 * p);
                    if (s <= z[k]) k--;
                    else break;
                }
                k++;
                v[k] = q;
                z[k] = s;
                z[k + 1] = double.PositiveInfinity;
            }

            k = 0;
            for (int q = 0; q < n; q++)
            {
                while (z[k + 1] < q) k++;
                double diff = q - v[k];
                d[q] = diff * diff + f[v[k]];
            }
        }

        /// <summary>網格座標 (x, y) 換算為地圖座標後是否位於 Boundary 內。</summary>
        private bool IsInsideBoundary(int x, int y)
        {
            if (_mapData.Boundary == null) return true;

            PointF m = GridToMap(new Point(x, y));
            return m.X >= _mapData.Boundary.MinX && m.X <= _mapData.Boundary.MaxX &&
                   m.Y >= _mapData.Boundary.MinY && m.Y <= _mapData.Boundary.MaxY;
        }

        private bool IsPointInPolygon(PointF pt, List<PointF> polygon)
        {
            bool inside = false;
            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                if (((polygon[i].Y > pt.Y) != (polygon[j].Y > pt.Y)) &&
                    (pt.X < (polygon[j].X - polygon[i].X) * (pt.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        // ==========================================
        //  1.5 MR 外形碰撞檢查
        // ==========================================

        private bool InGrid(int x, int y)
        {
            return x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeight;
        }

        private Point[] GetOffsets(int angleDeg)
        {
            angleDeg = ((angleDeg % 360) + 360) % 360;
            if (!_offsetCache.TryGetValue(angleDeg, out var offsets))
            {
                offsets = _footprint.BuildOffsets(angleDeg);
                _offsetCache[angleDeg] = offsets;
            }
            return offsets;
        }

        /// <summary>
        /// 車頭朝 angleDeg 時，參考點位於 (x, y) 的車體 (含邊距) 是否不碰撞。
        /// </summary>
        private bool IsFootprintFree(int x, int y, int angleDeg)
        {
            if (!InGrid(x, y) || _obstacleGrid[x, y]) return false;

            float d = _distGrid[x, y];
            if (d < _inscribedPx) return false; // 內切圓內有障礙，必定碰撞

            if (d > _circumscribedPx + 1 &&
                x >= _borderPx && y >= _borderPx &&
                x < _gridWidth - _borderPx && y < _gridHeight - _borderPx)
                return true; // 外接圓內無障礙，必定安全

            foreach (var o in GetOffsets(angleDeg))
            {
                int px = x + o.X, py = y + o.Y;
                if (!InGrid(px, py) || _obstacleGrid[px, py]) return false;
            }
            return true;
        }

        private bool IsDirFree(int x, int y, int dirIndex)
        {
            if (!InGrid(x, y)) return false;

            var cache = _dirState[dirIndex];
            if (cache == null)
            {
                cache = new sbyte[_gridWidth, _gridHeight];
                _dirState[dirIndex] = cache;
            }

            sbyte s = cache[x, y];
            if (s == 0)
            {
                s = IsFootprintFree(x, y, DirAngles[dirIndex]) ? (sbyte)1 : (sbyte)2;
                cache[x, y] = s;
            }
            return s == 1;
        }

        /// <summary>
        /// 是否可在 (x, y) 以參考點為中心原地旋轉 (外接圓內無障礙)。
        /// </summary>
        private bool CanRotateAt(int x, int y)
        {
            if (!InGrid(x, y) || _obstacleGrid[x, y]) return false;
            return _distGrid[x, y] > _circumscribedPx + 1 &&
                   x >= _borderPx && y >= _borderPx &&
                   x < _gridWidth - _borderPx && y < _gridHeight - _borderPx;
        }

        /// <summary>
        /// 終點朝向與進入方向 (最後一點到終點的直線，容許倒車) 平行時，不需要旋轉空間。
        /// </summary>
        private bool FinalHeadingAlignedWithApproach(IList<RoutePoint> points, int angleDeg)
        {
            int n = points.Count;
            if (n < 2) return false;
            Point a = MapToGrid(points[n - 2].X, points[n - 2].Y);
            Point b = MapToGrid(points[n - 1].X, points[n - 1].Y);
            if (a == b) return false;
            double approach = Math.Atan2(b.Y - a.Y, b.X - a.X) * 180.0 / Math.PI;
            double diff = Math.Abs(((angleDeg - approach) % 180 + 270) % 180 - 90);
            return diff <= 5;
        }

        private double MinMarginHint(double clearanceMm)
        {
            return Math.Max(0, clearanceMm - _footprint.WidthMm / 2.0);
        }

        private string ValidateEndpoint(Point p, string label)
        {
            if (!InGrid(p.X, p.Y) || !IsInsideBoundary(p.X, p.Y))
                return $"{label}超出地圖範圍";
            if (_obstacleGrid[p.X, p.Y])
                return $"{label}位於障礙物內";
            if (_distGrid[p.X, p.Y] < _inscribedPx)
            {
                double actual = _distGrid[p.X, p.Y] * _footprint.MmPerPixel;
                double need = _inscribedPx * _footprint.MmPerPixel;
                return $"【車寬問題】{label}距障礙物僅 {actual:F0}mm，小於 MR 半寬加邊距所需的 {need:F0}mm，差 {need - actual:F0}mm" +
                       $" (邊距需降至約 {MinMarginHint(actual):F0}mm 才能通過)";
            }

            for (int h = 0; h < 8; h++)
            {
                if (IsDirFree(p.X, p.Y, h)) return null;
            }
            return $"【車寬問題】{label}通道寬度不足，MR 外形 (含邊距 {_footprint.MarginMm:F0}mm) 每 45 度的車頭朝向測試都會與障礙物重疊。" + FindMaxPassableMargin(p);
        }

        private string MeasureCorridorWidth(Point p)
        {
            double best = double.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                var d = Directions[i == 0 ? 0 : i == 1 ? 2 : i == 2 ? 4 : 5];
                double w = 1;
                for (int s = -1; s <= 1; s += 2)
                {
                    int k = 1;
                    while (InGrid(p.X + s * d.X * k, p.Y + s * d.Y * k) && !_obstacleGrid[p.X + s * d.X * k, p.Y + s * d.Y * k]) k++;
                    w += (k - 1) * ((d.X != 0 && d.Y != 0) ? 1.414 : 1.0);
                }
                if (w < best) best = w;
            }
            return $"該點最窄處通道寬約 {best * _footprint.MmPerPixel:F0}mm (±{_footprint.MmPerPixel:F0}mm)";
        }

        private string FindMaxPassableMargin(Point p)
        {
            var saved = _footprint;
            double orig = saved.MarginMm;
            try
            {
                for (int m = (int)orig - 1; m >= 0; m--)
                {
                    var t = new MrFootprint
                    {
                        LengthMm = saved.LengthMm,
                        WidthMm = saved.WidthMm,
                        MarginMm = m,
                        RefOffsetXMm = saved.RefOffsetXMm,
                        RefOffsetYMm = saved.RefOffsetYMm,
                        MmPerPixel = saved.MmPerPixel
                    };
                    ApplyFootprint(t);
                    for (int a = 0; a < 360; a += 15)
                    {
                        if (IsFootprintFree(p.X, p.Y, a))
                            return $" 邊距需降至 {m}mm 以下才能通過 (目前 {orig:F0}mm，至少需少 {orig - m:F0}mm)；{MeasureCorridorWidth(p)}，MR 車寬 {saved.WidthMm:F0}mm";
                    }
                }
                return $" 即使邊距為 0mm 也無法通過 (車身本身比通道寬，或終點貼近障礙物)；{MeasureCorridorWidth(p)}，MR 車寬 {saved.WidthMm:F0}mm";
            }
            finally
            {
                ApplyFootprint(saved);
            }
        }

        private bool ReachableWithClearance(Point s, Point e, float th)
        {
            var seen = new bool[_gridWidth, _gridHeight];
            var q = new Queue<Point>();
            q.Enqueue(s); seen[s.X, s.Y] = true;
            while (q.Count > 0)
            {
                var c = q.Dequeue();
                if (c == e) return true;
                foreach (var d in Directions)
                {
                    int nx = c.X + d.X, ny = c.Y + d.Y;
                    if (!InGrid(nx, ny) || seen[nx, ny] || _obstacleGrid[nx, ny]) continue;
                    if (_distGrid[nx, ny] < th && !(nx == e.X && ny == e.Y)) continue;
                    seen[nx, ny] = true;
                    q.Enqueue(new Point(nx, ny));
                }
            }
            return false;
        }

        private string DescribeBottleneck(Point s, Point e)
        {
            if (!InGrid(s.X, s.Y) || !InGrid(e.X, e.Y) || !ReachableWithClearance(s, e, 0f))
                return "，兩點不連通";
            float lo = 0f, hi = _maxDist;
            for (int i = 0; i < 20; i++)
            {
                float mid = (lo + hi) / 2f;
                if (ReachableWithClearance(s, e, mid)) lo = mid; else hi = mid;
            }
            double mm = _footprint.MmPerPixel;
            double minWidth = lo * 2 * mm;
            double need = _footprint.WidthMm + 2 * _footprint.MarginMm;
            return $"；沿途最小路寬約 {minWidth:F0}mm (±{mm:F0}mm)，需要 ≥ {need:F0}mm (車寬 {_footprint.WidthMm:F0} + 邊距 {_footprint.MarginMm:F0}×2)";
        }

        /// <summary>
        /// 找不到路徑時判斷原因
        /// </summary>
        private string DiagnoseNoPath(Point s, Point e, string from, string to)
        {
            List<PoseCell> relaxed;
            _ignoreRotation = true;
            try { relaxed = RunAStar(s, e); }
            finally { _ignoreRotation = false; }

            if (relaxed == null)
                return $"【車寬問題】{from} 到 {to} 沿途通道寬度不足 (MR 外形含邊距 {_footprint.MarginMm:F0}mm 無法通過)" + DescribeBottleneck(s, e);
            return $"【旋轉問題】{from} 到 {to} 途中需要轉向，但沿途沒有足夠的原地旋轉空間 (需距障礙 ≥ {_circumscribedPx * _footprint.MmPerPixel:F0}mm)";
        }


        // ==========================================
        //  2. 對外公開的路徑規劃介面
        // ==========================================
        public MapRouteData PlanRoute(string startId, string endId, string waypointId = null)
        {
            return PlanRouteCore(startId, endId, waypointId);
        }

        private MapRouteData PlanRouteCore(string startId, string endId, string waypointId)
        {
            var route = new MapRouteData { Success = false };
            LastError = null;
            LastErrorPointIndex = -1;
            if (!_isGridInitialized || !_mapData.PointDict.ContainsKey(startId) || !_mapData.PointDict.ContainsKey(endId))
            {
                LastError = "地圖尚未初始化或找不到起終點";
                return route;
            }

            bool hasWaypoint = !string.IsNullOrEmpty(waypointId) && _mapData.PointDict.ContainsKey(waypointId);

            var checks = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(startId, "起點")
            };
            if (hasWaypoint) checks.Add(new KeyValuePair<string, string>(waypointId, "中繼點"));
            checks.Add(new KeyValuePair<string, string>(endId, "終點"));

            foreach (var c in checks)
            {
                var mp = _mapData.PointDict[c.Key];
                string err = ValidateEndpoint(MapToGrid(mp.X, mp.Y), c.Value + " " + (mp.Name ?? mp.ID));
                if (err != null)
                {
                    LastError = err;
                    return route;
                }
            }

            List<PointF> finalPathCoords = new List<PointF>();
            List<string> fullStringPath = new List<string>();
            var orient = new List<double?>();
            double totalDist = 0;

            if (hasWaypoint)
            {
                var a = _mapData.PointDict[startId]; var w = _mapData.PointDict[waypointId]; var b = _mapData.PointDict[endId];
                var seg1 = FindAndSmoothPath(a.X, a.Y, w.X, w.Y, null, null, out var h1);
                List<double?> h2 = null;
                var seg2 = seg1 == null ? null : FindAndSmoothPath(w.X, w.Y, b.X, b.Y, h1.Count > 0 ? h1[h1.Count - 1] : null, null, out h2);

                if (seg1 == null || seg2 == null)
                {
                    var from = seg1 == null ? _mapData.PointDict[startId] : _mapData.PointDict[waypointId];
                    var to = seg1 == null ? _mapData.PointDict[waypointId] : _mapData.PointDict[endId];
                    LastError = DiagnoseNoPath(MapToGrid(from.X, from.Y), MapToGrid(to.X, to.Y), from.Name ?? from.ID, to.Name ?? to.ID);
                    return route; // 找不到路徑 (卡死)
                }

                finalPathCoords.AddRange(seg1);
                finalPathCoords.AddRange(seg2.Skip(1));
                orient.AddRange(h1);
                orient.AddRange(h2);
                fullStringPath = new List<string> { startId, waypointId, endId };
            }
            else
            {
                var a = _mapData.PointDict[startId]; var b = _mapData.PointDict[endId];
                var seg = FindAndSmoothPath(a.X, a.Y, b.X, b.Y, null, null, out var h);
                if (seg != null) orient.AddRange(h);
                if (seg == null)
                {
                    var from = _mapData.PointDict[startId];
                    var to = _mapData.PointDict[endId];
                    LastError = DiagnoseNoPath(MapToGrid(from.X, from.Y), MapToGrid(to.X, to.Y), from.Name ?? from.ID, to.Name ?? to.ID);
                    return route;
                }

                finalPathCoords = seg;
                fullStringPath = new List<string> { startId, endId };
            }

            for (int i = 0; i < finalPathCoords.Count - 1; i++)
            {
                totalDist += Math.Sqrt(Math.Pow(finalPathCoords[i].X - finalPathCoords[i + 1].X, 2) +
                                       Math.Pow(finalPathCoords[i].Y - finalPathCoords[i + 1].Y, 2));
            }

            route.Success = true;
            route.StartID = _mapData.PointDict[startId].TagNumber;
            route.EndID = _mapData.PointDict[endId].TagNumber;
            route.WaypointID = string.IsNullOrEmpty(waypointId) ? -1 : _mapData.PointDict[waypointId].TagNumber;
            route.StartName = _mapData.PointDict[startId].Name;
            route.EndName = _mapData.PointDict[endId].Name;
            route.TotalDistance = totalDist;
            route.PlanTime = DateTime.Now;

            foreach (var id in fullStringPath)
            {
                var pt = _mapData.PointDict[id];
                route.FullPath.Add(pt.TagNumber);
                route.FullPathNames.Add(pt.Name ?? pt.ID);
            }
            route.RouteArray = route.FullPathNames.ToArray();

            // 給 Core 送出精細的 2D 路徑
            route.PathCoordinates = finalPathCoords;
            route.EdgeOrientations = orient;

            return route;
        }

        // ==========================================
        //  3. A* 演算法核心 (狀態: 位置 + 朝向)
        // ==========================================
        public MapRouteData PlanFreeRoute(IList<RoutePoint> points)
        {
            var route = new MapRouteData { Success = false };
            LastError = null;
            LastErrorPointIndex = -1;

            if (!_isGridInitialized || points == null || points.Count < 2)
            {
                LastError = "地圖尚未初始化或路徑點不足 (至少需要起點與終點)";
                return route;
            }

            int n = points.Count;
            var labels = new string[n];
            var nodeIds = new string[n];
            var grids = new Point[n];
            int freeCount = 0;

            for (int i = 0; i < n; i++)
            {
                var p = points[i];
                MapPoint station = null;
                if (!string.IsNullOrEmpty(p.StationId)) _mapData.PointDict.TryGetValue(p.StationId, out station);

                if (station != null)
                {
                    labels[i] = "站點 " + (station.Name ?? station.ID);
                    nodeIds[i] = station.TagNumber > 0 ? station.TagNumber.ToString() : station.ID;
                }
                else if (p.IsCurrentPosition)
                {
                    labels[i] = "MR 目前位置";
                    nodeIds[i] = "START";
                }
                else
                {
                    freeCount++;
                    labels[i] = (i == 0 ? "起點" : i == n - 1 ? "終點" : "經過點") + " (自由點 " + freeCount + ")";
                    nodeIds[i] = "FREE_" + freeCount;
                }

                grids[i] = MapToGrid(p.X, p.Y);
                string err = ValidateEndpoint(grids[i], labels[i]);
                if (err != null)
                {
                    LastError = err;
                    LastErrorPointIndex = i;
                    return route;
                }
            }

            // 指定終點朝向時，該朝向的車體不可碰撞 (旋轉空間由 A* 依實際是否需要旋轉判斷)
            var last = points[n - 1];
            if (last.Theta.HasValue)
            {
                int angle = (int)Math.Round(last.Theta.Value * 180.0 / Math.PI);
                Point g = grids[n - 1];
                if (!IsFootprintFree(g.X, g.Y, angle))
                {
                    LastError = labels[n - 1] + "以指定朝向時 MR 外形會碰撞";
                    LastErrorPointIndex = n - 1;
                    return route;
                }
            }

            var coords = new List<PointF>();
            var ids = new List<string>();
            var orient = new List<double?>();
            double totalDist = 0;
            double? prevHeading = points[0].Theta;

            for (int i = 0; i < n - 1; i++)
            {
                double? segEnd = i == n - 2 ? last.Theta : null;
                var seg = FindAndSmoothPath(points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y,
                                            prevHeading, segEnd, out var segHeads);
                if (seg == null)
                {
                    LastError = DiagnoseNoPath(grids[i], grids[i + 1], labels[i], labels[i + 1]);
                    LastErrorPointIndex = i + 1;
                    return route;
                }
                orient.AddRange(segHeads);
                if (segHeads.Count > 0) prevHeading = segHeads[segHeads.Count - 1];

                var segPts = i == 0 ? seg : seg.Skip(1).ToList();
                for (int j = 0; j < segPts.Count; j++)
                {
                    coords.Add(segPts[j]);
                    if (i == 0 && j == 0) ids.Add(nodeIds[0]);
                    else if (j == segPts.Count - 1) ids.Add(nodeIds[i + 1]);
                    else ids.Add(null);
                }
            }

            for (int i = 0; i < coords.Count - 1; i++)
            {
                totalDist += Math.Sqrt(Math.Pow(coords[i].X - coords[i + 1].X, 2) +
                                       Math.Pow(coords[i].Y - coords[i + 1].Y, 2));
            }

            route.Success = true;
            route.TotalDistance = totalDist;
            route.PlanTime = DateTime.Now;
            route.StartID = -1;
            route.EndID = -1;
            for (int i = 0; i < n; i++)
            {
                MapPoint station = null;
                if (!string.IsNullOrEmpty(points[i].StationId)) _mapData.PointDict.TryGetValue(points[i].StationId, out station);
                int tag = station != null ? station.TagNumber : -1;

                if (i == 0) { route.StartID = tag; route.StartName = labels[i]; }
                if (i == n - 1) { route.EndID = tag; route.EndName = labels[i]; }
                if (tag > 0) route.FullPath.Add(tag);
                route.FullPathNames.Add(labels[i]);
            }
            route.RouteArray = route.FullPathNames.ToArray();
            route.PathCoordinates = coords;
            route.PathNodeIds = ids;
            route.EndTheta = last.Theta;
            route.EdgeOrientations = orient;
            return route;
        }

        private List<PointF> FindAndSmoothPath(MapPoint startPt, MapPoint endPt)
        {
            return FindAndSmoothPath(startPt.X, startPt.Y, endPt.X, endPt.Y, null, null, out _);
        }

        /// <summary>
        /// 回傳平滑後的座標，以及每段 edge 的車頭朝向 (弧度)。
        /// 起點/終點朝向：有指定用指定值 (操作員或 MR 目前朝向)，否則由規劃決定。
        /// </summary>
        private List<PointF> FindAndSmoothPath(double startX, double startY, double endX, double endY,
            double? startTheta, double? endTheta, out List<double?> edgeHeadings)
        {
            edgeHeadings = null;
            Point startGrid = MapToGrid(startX, startY);
            Point endGrid = MapToGrid(endX, endY);

            List<PoseCell> gridPath = RunAStar(startGrid, endGrid, HeadingIndex(startTheta), HeadingIndex(endTheta));
            if (gridPath == null || gridPath.Count == 0) return null;

            // 去除位置相同的連續點 (原地旋轉)，保留最後者：H 即為離開該格時的車頭朝向
            var cells = new List<PoseCell>();
            foreach (var c in gridPath)
            {
                if (cells.Count > 0 && cells[cells.Count - 1].P == c.P) cells[cells.Count - 1] = c;
                else cells.Add(c);
            }
            if (cells.Count < 2) cells.Add(cells[0]);

            double? startDeg = startTheta.HasValue ? startTheta.Value * 180.0 / Math.PI : (double?)null;
            double? endDeg = endTheta.HasValue ? endTheta.Value * 180.0 / Math.PI : (double?)null;
            BuildHeadedPath(cells, startDeg, endDeg, out var nodes, out var heads);
            MergeClosePoints(nodes, heads, startDeg);

            var worldPath = nodes.Select(p => GridToMap(p)).ToList();
            worldPath[0] = new PointF((float)startX, (float)startY);
            worldPath[worldPath.Count - 1] = new PointF((float)endX, (float)endY);

            // edge i 的朝向 = 從 nodes[i] 出發 (旋轉後) 行駛時的車頭朝向；終點朝向由 EndTheta 另行指定
            edgeHeadings = heads.Select(h => (double?)RouteTurnRefiner.Wrap(h * Math.PI / 180.0)).ToList();
            return worldPath;
        }

        private const double MinWaypointSpacingMm = 500.0;   // 過近的點合併門檻
        private const double ForceHeadingLengthRatio = 0.5;  // 直線段 > 半個車長 時強制車頭朝行進方向
        private const double ReverseMaxLengthRatio = 3.0;    // 直線段 <= 3 倍車長 且安全時鼓勵倒車

        private double MmPerPx => Math.Max(_footprint.MmPerPixel, 1e-6);

        private static double PxDist(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /// <summary>直線段以車頭朝向 headDeg 行駛時，沿途車體 (含邊距) 皆不碰撞。</summary>
        private bool IsSegmentSafe(Point a, Point b, double headDeg)
        {
            return HasLineOfSight(a, b, (int)Math.Round(headDeg));
        }

        /// <summary>在 p 原地由 fromDeg 轉到 toDeg，掃過的角度 (每 15°) 皆不碰撞。</summary>
        private bool CanRotateSweep(Point p, double? fromDeg, double toDeg)
        {
            if (!IsFootprintFree(p.X, p.Y, (int)Math.Round(toDeg))) return false;
            if (!fromDeg.HasValue || _ignoreRotation || CanRotateAt(p.X, p.Y)) return true;
            double d = ((toDeg - fromDeg.Value) % 360 + 540) % 360 - 180;
            int steps = (int)Math.Ceiling(Math.Abs(d) / 15.0);
            for (int k = 1; k < steps; k++)
            {
                if (!IsFootprintFree(p.X, p.Y, (int)Math.Round(fromDeg.Value + d * k / steps)))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 直線段可用的車頭朝向 (依優先順序)：
        /// 長度 &gt; 半車長：只能車頭朝前 (或 &lt;= 3 倍車長時倒車，旋轉量較小者優先)；
        /// 長度 &lt;= 半車長：先嘗試維持目前朝向平移，其次車頭朝前。
        /// </summary>
        private IEnumerable<double> CandidateHeadings(Point a, Point b, double? cur)
        {
            double len = PxDist(a, b);
            double forward = Math.Atan2(b.Y - a.Y, b.X - a.X) * 180.0 / Math.PI;
            double reverse = forward + 180.0;
            bool reverseOk = len <= _footprint.LengthMm * ReverseMaxLengthRatio / MmPerPx;

            if (len > _footprint.LengthMm * ForceHeadingLengthRatio / MmPerPx)
            {
                if (reverseOk && cur.HasValue && AngleDiffDeg(reverse, cur.Value) < AngleDiffDeg(forward, cur.Value))
                {
                    yield return reverse;
                    yield return forward;
                }
                else
                {
                    yield return forward;
                    if (reverseOk) yield return reverse;
                }
                // 接受斜向移動 (行進方向與車頭夾角 <= 45° 或 >= 135°)，但不接受純側移
                if (cur.HasValue)
                {
                    double rel = AngleDiffDeg(forward, cur.Value);
                    if (rel <= 45.0 + 1e-6 || rel >= 135.0 - 1e-6) yield return cur.Value;
                }
            }
            else
            {
                if (cur.HasValue) yield return cur.Value;
                yield return forward;
            }
        }

        /// <summary>
        /// 由 A* 格點路徑產生節點與每段車頭朝向：
        /// 從目前點盡量直線 (含斜線) 連到最遠且安全的點，車頭依 CandidateHeadings 決定；
        /// 找不到安全的直線段時，退回 A* 原本的朝向 (例如窄巷內只能側移)。
        /// </summary>
        private void BuildHeadedPath(List<PoseCell> cells, double? startDeg, double? endDeg,
            out List<Point> nodes, out List<double> heads)
        {
            nodes = new List<Point> { cells[0].P };
            heads = new List<double>();
            double? cur = startDeg;
            int n = cells.Count;
            int i = 0;

            while (i < n - 1)
            {
                int bestJ = -1;
                double bestH = 0;
                List<string> rejects = null;
                for (int j = n - 1; j > i && bestJ < 0; j--)
                {
                    rejects = new List<string>();
                    double fwd = Math.Atan2(cells[j].P.Y - cells[i].P.Y, cells[j].P.X - cells[i].P.X) * 180.0 / Math.PI;
                    foreach (double h in CandidateHeadings(cells[i].P, cells[j].P, cur))
                    {
                        string reason = null;
                        if (j == n - 1 && endDeg.HasValue && AngleDiffDeg(h, fwd) > 90 && AngleDiffDeg(h, endDeg.Value) > 90)
                            reason = "倒車抵達終點後需大角度旋轉";
                        else if (!CanRotateSweep(cells[i].P, cur, h)) reason = "起點旋轉碰撞";
                        else if (!IsSegmentSafe(cells[i].P, cells[j].P, h)) reason = "直線段碰撞";
                        else if (j == n - 1 && endDeg.HasValue && !CanRotateSweep(cells[j].P, h, endDeg.Value)) reason = "終點旋轉碰撞";

                        if (reason != null)
                        {
                            rejects.Add($"{h:F0}°:{reason}");
                            continue;
                        }
                        bestJ = j;
                        bestH = h;
                        break;
                    }
                }

                if (bestJ >= 0 && rejects.Count > 0)
                    Log?.WriteSystemLog($"[路徑朝向] 段 ({cells[i].P.X},{cells[i].P.Y})->({cells[bestJ].P.X},{cells[bestJ].P.Y}) 採用 {bestH:F0}°，被拒絕: {string.Join(", ", rejects)}");

                if (bestJ < 0)
                {
                    Log?.WriteSystemLog($"[路徑朝向] 由 ({cells[i].P.X},{cells[i].P.Y}) 找不到安全直線段，退回 A* 朝向 {DirAngles[cells[i].H]}°");
                    int hIdx = cells[i].H;
                    bestH = DirAngles[hIdx];
                    bestJ = i + 1;
                    for (int m = i + 2; m < n; m++)
                    {
                        if (cells[m - 1].H != hIdx) break;
                        if (!HasLineOfSight(cells[i].P, cells[m].P, DirAngles[hIdx])) break;
                        bestJ = m;
                    }
                }

                nodes.Add(cells[bestJ].P);
                heads.Add(bestH);
                cur = bestH;
                i = bestJ;
            }
        }

        /// <summary>
        /// 中間點與下一點 (或與起點) 距離小於 MinWaypointSpacingMm 時移除，保留較後者；
        /// 合併後的直線段以較長子段的車頭朝向做碰撞與旋轉檢查，不通過則保留原點。
        /// </summary>
        private void MergeClosePoints(List<Point> nodes, List<double> heads, double? startDeg)
        {
            double minPx = MinWaypointSpacingMm / MmPerPx;
            int i = 1;
            while (i < nodes.Count - 1)
            {
                double dPrev = PxDist(nodes[i - 1], nodes[i]);
                double dNext = PxDist(nodes[i], nodes[i + 1]);
                if (dNext < minPx || (i == 1 && dPrev < minPx))
                {
                    double h = dPrev >= dNext ? heads[i - 1] : heads[i];
                    double? before = i - 1 > 0 ? heads[i - 2] : startDeg;
                    bool ok = IsSegmentSafe(nodes[i - 1], nodes[i + 1], h)
                              && CanRotateSweep(nodes[i - 1], before, h)
                              && (i + 1 >= heads.Count || CanRotateSweep(nodes[i + 1], h, heads[i + 1]));
                    if (ok)
                    {
                        heads[i - 1] = h;
                        heads.RemoveAt(i);
                        nodes.RemoveAt(i);
                        continue;
                    }
                }
                i++;
            }
        }

        private List<PointF> FindAndSmoothPath(double startX, double startY, double endX, double endY)
        {
            return FindAndSmoothPath(startX, startY, endX, endY, null, null, out _);
        }

		private struct Node
		{
			public Point Position;
			public int Dir;        // 車頭朝向 (DirAngles 索引)，AnyDir 表示起點尚未決定
			public double G;
			public double H;
			public double F => G + H;
		}

		private struct StateInfo
		{
			public double G;
			public long Parent;    // 父狀態 StateKey，-1 表示起點
			public bool Closed;
		}

		/// <summary>A* 待處理清單 (binary heap)，排序與原 SortedSet 相同：F → H → X → Y → Dir。</summary>
		private sealed class MinHeap
		{
			private readonly List<Node> _items = new List<Node>();
			public int Count => _items.Count;

			private static int Compare(Node a, Node b)
			{
				int cmp = a.F.CompareTo(b.F);
				if (cmp == 0) cmp = a.H.CompareTo(b.H);
				if (cmp == 0) cmp = a.Position.X.CompareTo(b.Position.X);
				if (cmp == 0) cmp = a.Position.Y.CompareTo(b.Position.Y);
				if (cmp == 0) cmp = a.Dir.CompareTo(b.Dir);
				return cmp;
			}

			public void Push(Node n)
			{
				_items.Add(n);
				int i = _items.Count - 1;
				while (i > 0)
				{
					int p = (i - 1) / 2;
					if (Compare(_items[i], _items[p]) >= 0) break;
					var t = _items[i]; _items[i] = _items[p]; _items[p] = t;
					i = p;
				}
			}

			public Node Pop()
			{
				var top = _items[0];
				int last = _items.Count - 1;
				_items[0] = _items[last];
				_items.RemoveAt(last);
				int i = 0, n = _items.Count;
				while (true)
				{
					int l = i * 2 + 1, r = l + 1, m = i;
					if (l < n && Compare(_items[l], _items[m]) < 0) m = l;
					if (r < n && Compare(_items[r], _items[m]) < 0) m = r;
					if (m == i) break;
					var t = _items[i]; _items[i] = _items[m]; _items[m] = t;
					i = m;
				}
				return top;
			}
		}

		private struct PoseCell
		{
			public Point P;
			public int H;
		}

		private static double AngleDiffDeg(double a, double b)
		{
			double d = Math.Abs(((a - b) % 360 + 540) % 360 - 180);
			return d;
		}

		/// <summary>弧度朝向轉為最接近的 8 方向索引，null 回傳 -1。</summary>
		private static int HeadingIndex(double? rad)
		{
			if (!rad.HasValue) return -1;
			double deg = rad.Value * 180.0 / Math.PI;
			int best = 0;
			for (int i = 1; i < 8; i++)
				if (AngleDiffDeg(DirAngles[i], deg) < AngleDiffDeg(DirAngles[best], deg)) best = i;
			return best;
		}

		/// <summary>
		/// 原地旋轉 45° 是否可行：只檢查旋轉過程掃過的角度 (每 15°)，不要求整個外接圓淨空。
		/// </summary>
		private bool CanRotateStep(Point p, int from, int to)
		{
			if (_ignoreRotation || CanRotateAt(p.X, p.Y)) return true;
			double a0 = DirAngles[from];
			double d = ((DirAngles[to] - a0) % 360 + 540) % 360 - 180;
			for (int k = 1; k <= 2; k++)
			{
				if (!IsFootprintFree(p.X, p.Y, (int)Math.Round(a0 + d * k / 3.0)))
					return false;
			}
			return true;
		}

		/// <summary>
		/// 位置 + 車頭朝向的 A*。全向輪可在保持車頭朝向下往任意方向平移 (含倒車/側移)，
		/// 旋轉為獨立動作，每次 45°。endHead &lt; 0 表示終點朝向不限。
		/// </summary>
		private List<PoseCell> RunAStar(Point start, Point end, int startHead = -1, int endHead = -1)
		{
			if (!InGrid(start.X, start.Y) || !InGrid(end.X, end.Y) ||
				!IsInsideBoundary(start.X, start.Y) || !IsInsideBoundary(end.X, end.Y))
				return null;

			if (startHead >= 0 && !IsDirFree(start.X, start.Y, startHead))
				startHead = -1; // 實際朝向與 45° 量化不符時，改由規劃決定

			// 待處理清單：binary heap + lazy deletion；排序規則 F → H → X → Y → Dir
			var openList = new MinHeap();
			var states = new Dictionary<long, StateInfo>();

			int sDir = startHead >= 0 ? startHead : AnyDir;
			long startKey = StateKey(start, sDir);
			states[startKey] = new StateInfo { G = 0, Parent = -1 };
			openList.Push(new Node { Position = start, Dir = sDir, G = 0, H = GetHeuristic(start, end) });

			Action<Node, Point, int, double> relax = (cur, pos, dir, cost) =>
			{
				long key = StateKey(pos, dir);
				double g = cur.G + cost;
				if (states.TryGetValue(key, out var info))
				{
					if (info.Closed || g >= info.G) return;
				}
				states[key] = new StateInfo { G = g, Parent = StateKey(cur.Position, cur.Dir) };
				openList.Push(new Node { Position = pos, Dir = dir, G = g, H = GetHeuristic(pos, end) });
			};

			while (openList.Count > 0)
			{
				var current = openList.Pop();
				long curKey = StateKey(current.Position, current.Dir);
				var curInfo = states[curKey];
				if (curInfo.Closed || current.G > curInfo.G) continue; // 已過期的項目

				if (current.Position == end && current.Dir != AnyDir && (endHead < 0 || current.Dir == endHead))
				{
					var path = new List<PoseCell>();
					long k = curKey;
					while (k >= 0)
					{
						int dir = (int)(k % 9);
						long cell = k / 9;
						if (dir != AnyDir)
							path.Add(new PoseCell { P = new Point((int)(cell % _gridWidth), (int)(cell / _gridWidth)), H = dir });
						k = states[k].Parent;
					}
					path.Reverse();
					return path;
				}

				curInfo.Closed = true;
				states[curKey] = curInfo;

				if (current.Dir == AnyDir)
				{
					// 起點朝向未知：可選擇任何不碰撞的朝向
					for (int h = 0; h < 8; h++)
						if (IsDirFree(start.X, start.Y, h))
							relax(current, current.Position, h, 0);
					continue;
				}

				// 原地旋轉 ±45°：任何有旋轉空間的格點皆可旋轉，讓長距離移動以車頭方向前進
				for (int h = 0; h < 8; h++)
				{
					if (AngleDiffDeg(DirAngles[h], DirAngles[current.Dir]) != 45) continue;
					if (!IsDirFree(current.Position.X, current.Position.Y, h)) continue;
					if (!CanRotateStep(current.Position, current.Dir, h)) continue;
					relax(current, current.Position, h, TurnCost);
				}

				// 保持車頭朝向平移 (前進、倒車、側移、斜移)
				for (int di = 0; di < 8; di++)
				{
					var dir = Directions[di];
					Point np = new Point(current.Position.X + dir.X, current.Position.Y + dir.Y);
					if (!InGrid(np.X, np.Y)) continue;
					if (!IsDirFree(np.X, np.Y, current.Dir)) continue;

					double step = (dir.X != 0 && dir.Y != 0) ? 1.414 : 1.0;
					double rel = AngleDiffDeg(DirAngles[di], DirAngles[current.Dir]);
					// 前進 1.0、倒車 1.2、斜移 3、橫移 5：能轉車頭的地方就不橫移
										double factor = rel < 1 ? 1.0 : rel > 179 ? 1.2 : (Math.Abs(rel - 90) < 1 ? 5.0 : 3.0);
					if (_unknownGrid[np.X, np.Y]) factor *= Math.Max(1.0, UnknownCostFactor);
					relax(current, np, current.Dir, step * factor);
				}
			}

			return null;
		}

        private long StateKey(Point p, int dir)
        {
            return ((long)p.Y * _gridWidth + p.X) * 9 + dir;
        }

        private double GetHeuristic(Point a, Point b)
        {
            int dx = Math.Abs(a.X - b.X);
            int dy = Math.Abs(a.Y - b.Y);
            return (dx + dy) + (1.414 - 2) * Math.Min(dx, dy);
        }

        // ==========================================
        //  4. 視線測試與平滑化
        // ==========================================
        private bool HasLineOfSight(Point a, Point b, int angle)
        {
            int x0 = a.X, y0 = a.Y, x1 = b.X, y1 = b.Y;

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;

            for (; ; )
            {
                if (!IsFootprintFree(x0, y0, angle))
                    return false;

                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
            return true;
        }
    }
}
