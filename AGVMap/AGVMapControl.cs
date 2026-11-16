using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AML;

namespace AGVMap
{
    // ==========================================
    //  Delegate 定義 (已更新為 MapRouteData)
    // ==========================================
    public delegate void RoutePlanedEvent(MapRouteData routeData);
    public delegate void RouteProgressEvent(int currentTag, int nextTag, int currentIndex, int totalStops);
    public delegate void RouteCompletedEvent(int finalTag, List<int> fullRoute);
    public delegate void MapErrorEvent(string errorMessage);

    public partial class AGVMapControl : UserControl
    {
        private MapDataManager _mapData;
        private MapRenderer _renderer;
        private IPathPlanner _pathPlanner;

        public AML_LogPage Log = new AML_LogPage();

        // 導航狀態 (依然紀錄 TagNumber)
        private List<int> _navigationRoute;
        private int _navigationIndex;
        private bool _isNavigating;
        private bool _disposed;

        // ============ 事件 ============
        public event RoutePlanedEvent OnRoutePlaned;
        public event RoutePlanedEvent OnNavigationStarted;
        public event RouteProgressEvent OnRouteProgress;
        public event RouteCompletedEvent OnRouteCompleted;
        public event MapErrorEvent OnMapError;

        public bool IsMapLoaded => _mapData != null && _mapData.Points.Count > 0;
        public string ModuleName = "MR_PathFinder";

        /// <summary>地圖 1 個像素對應的實際距離 (mm)，讀取 Setting/AGVMap/MmPerPixel。</summary>
        public double MmPerPixel { get; set; } = 30.0;
        /// <summary>灰色未知區平移成本倍率，讀取 Setting/AGVMap/UnknownCostFactor。</summary>
        public double UnknownCostFactor { get; set; } = 3.0;
        /// <summary>1 像素 = 幾公尺 (VDA5050 座標單位)</summary>
        public double MetersPerPixel => MmPerPixel / 1000.0;
        /// <summary>地圖座標左邊界 (世界座標原點 X)</summary>
        public double MapMinX => _mapData?.Boundary?.MinX ?? 0;
        /// <summary>地圖座標下邊界 (影像座標 Y 向下，世界座標原點在左下)</summary>
        public double MapMaxY => _mapData?.Boundary != null ? _mapData.Boundary.MaxY : (_mapData?.BackgroundImage?.Height ?? 0);

        /// <summary>地圖座標 (左上原點, Y 向下) → 世界座標 (左下原點, m)</summary>
        public void MapToWorld(double mx, double my, out double wx, out double wy)
        {
            wx = (mx - MapMinX) * MetersPerPixel;
            wy = (MapMaxY - my) * MetersPerPixel;
        }

        /// <summary>世界座標 (左下原點, m) → 地圖座標 (左上原點, Y 向下)</summary>
        public void WorldToMap(double wx, double wy, out double mx, out double my)
        {
            mx = wx / MetersPerPixel + MapMinX;
            my = MapMaxY - wy / MetersPerPixel;
        }

        /// <summary>取得指定類型的站點 (例如 StationTypes.Rack 為橘色點)</summary>
        public List<MapPoint> GetStationsByType(int stationType)
        {
            if (_mapData == null) return new List<MapPoint>();
            return _mapData.Points.Where(p => p.Enable && !p.IsVirtualPoint && p.StationType == stationType).ToList();
        }

        /// <summary>隨機取得 MR 可任意朝向停放的已知空曠點 (地圖座標)</summary>
        public bool TryGetRandomFreePoint(Random rnd, out PointF point, int maxTry = 5000)
        {
            point = PointF.Empty;
            var planner = _pathPlanner as GridAStarPlanner;
            if (!IsMapLoaded || planner == null || _mapData.BackgroundImage == null) return false;

            double minX = MapMinX, maxY = MapMaxY;
            double maxX = _mapData.Boundary != null ? _mapData.Boundary.MaxX : _mapData.BackgroundImage.Width;
            double minY = _mapData.Boundary != null ? _mapData.Boundary.MinY : 0;
            for (int i = 0; i < maxTry; i++)
            {
                double x = minX + rnd.NextDouble() * (maxX - minX);
                double y = minY + rnd.NextDouble() * (maxY - minY);
                if (planner.IsFreeForAnyHeading(x, y))
                {
                    point = new PointF((float)x, (float)y);
                    return true;
                }
            }
            return false;
        }
        /// <summary>MR 外形 (mm)，讀取 Setting/AGVMap/MrLength、MrWidth、SafeMargin、RefOffsetX、RefOffsetY。</summary>
        public MrFootprint Footprint { get; } = new MrFootprint();

        // ============ Free 模式 ============
        private readonly List<RoutePoint> _freePoints = new List<RoutePoint>();
        private CheckBox chkFreeMode;
        private CheckBox chkStartFromMr;
        private Button btnFreeClear;
        private bool _freeDragging;
        private bool _manualFreePlanning;
        private bool _panning;
        private Point _lastMouse;

        public AGVMapControl()
        {
            InitializeComponent();
            InitFreeModeControls();
            panelMap.Paint += PanelMap_Paint;
            panelMap.Resize += PanelMap_Resize;
            panelMap.MouseDown += PanelMap_MouseDown;
            panelMap.MouseMove += PanelMap_MouseMove;
            panelMap.MouseUp += PanelMap_MouseUp;
            panelMap.MouseWheel += PanelMap_MouseWheel;
            panelMap.MouseEnter += (s, e) => panelMap.Focus();
            TryAutoLoad();
        }

        private void InitFreeModeControls()
        {
            var font = new Font("微軟正黑體", 11F);

            btnFreeClear = new Button { Text = "清除點", Font = font, Dock = DockStyle.Right, Width = 80 };
            btnFreeClear.Click += (s, e) => ClearFreePoints();

            chkStartFromMr = new CheckBox { Text = "起點=MR 位置", Font = font, Dock = DockStyle.Right, Width = 140, Checked = true };
            chkStartFromMr.CheckedChanged += (s, e) => UpdateFreeDisplay();

            chkFreeMode = new CheckBox { Text = "Free 模式", Font = font, Dock = DockStyle.Right, Width = 110 };
            chkFreeMode.CheckedChanged += (s, e) =>
            {
                panelMap.Cursor = chkFreeMode.Checked ? Cursors.Cross : Cursors.Default;
                ClearFreePoints();
                if (chkFreeMode.Checked)
                    txtResult.Text = "Free 模式：左鍵點選依序加入點 (最後一點按住拖曳可指定終點朝向)，右鍵移除最後一點。";
            };

            panelToolbar.Controls.Add(btnFreeClear);
            panelToolbar.Controls.Add(chkStartFromMr);
            panelToolbar.Controls.Add(chkFreeMode);

            var btnZoomReset = new Button { Text = "重設", Font = font, Dock = DockStyle.Right, Width = 60 };
            btnZoomReset.Click += (s, e) =>
            {
                if (_renderer != null)
                {
                    _renderer.ResetView();
                    _renderer.DispatchedNodes = null;
                    _renderer.CurrentRouteCoordinates = null;
                }
                panelMap.Invalidate();
            };
            var btnZoomOut = new Button { Text = "－", Font = font, Dock = DockStyle.Right, Width = 40 };
            btnZoomOut.Click += (s, e) => ZoomFromCenter(1 / 1.25f);
            var btnZoomIn = new Button { Text = "＋", Font = font, Dock = DockStyle.Right, Width = 40 };
            btnZoomIn.Click += (s, e) => ZoomFromCenter(1.25f);
            panelToolbar.Controls.Add(btnZoomReset);
            panelToolbar.Controls.Add(btnZoomOut);
            panelToolbar.Controls.Add(btnZoomIn);
        }

        private void ZoomFromCenter(float factor)
        {
            if (_renderer == null) return;
            _renderer.ZoomAt(new Point(panelMap.Width / 2, panelMap.Height / 2), factor);
            panelMap.Invalidate();
        }

        private void PanelMap_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_renderer == null || !IsMapLoaded) return;
            _renderer.ZoomAt(e.Location, e.Delta > 0 ? 1.25f : 1 / 1.25f);
            panelMap.Invalidate();
        }

        public void Initial()
        {
            string MapPath = XML.Read("Setting", "Setting/AGVMap/MapPath", "GT_Map1104.txt");
            string LogPath = XML.Read("Setting", "Setting/Form1/Logfolder", "D:\\AML");
            int LogCounterLimit = Convert.ToUInt16(XML.Read("Setting", "Setting/Form1/LogCountLimit", "200"));
            int LogFolderLimit = Convert.ToUInt16(XML.Read("Setting", "Setting/Form1/LogFolderLimit", "30"));

            double mmPerPixel;
            if (double.TryParse(XML.Read("Setting", "Setting/AGVMap/MmPerPixel", "30"), out mmPerPixel) && mmPerPixel > 0)
                MmPerPixel = mmPerPixel;
            Footprint.LengthMm = ReadSettingDouble("Setting/AGVMap/MrLength", Footprint.LengthMm, 1);
            Footprint.WidthMm = ReadSettingDouble("Setting/AGVMap/MrWidth", Footprint.WidthMm, 1);
            Footprint.MarginMm = ReadSettingDouble("Setting/AGVMap/SafeMargin", Footprint.MarginMm, 0);
            Footprint.RefOffsetXMm = ReadSettingDouble("Setting/AGVMap/RefOffsetX", Footprint.RefOffsetXMm, double.MinValue);
            Footprint.RefOffsetYMm = ReadSettingDouble("Setting/AGVMap/RefOffsetY", Footprint.RefOffsetYMm, double.MinValue);
            UnknownCostFactor = ReadSettingDouble("Setting/AGVMap/UnknownCostFactor", UnknownCostFactor, 1);
            bool MapSuccess = LoadMap(MapPath);
            Log.Initial(ModuleName, false, true, LogPath, LogCounterLimit, LogFolderLimit);
            if (MapSuccess)
                Log.WriteSystemLog(ModuleName, "啟用 MR Path Finding Functn，預設地圖為 : " + MapPath);
            else
                Log.WriteSystemLog(ModuleName, "Map 載入 Fail，無法執行路徑規劃");
        }

        private static double ReadSettingDouble(string path, double defaultValue, double minValue)
        {
            double v;
            if (double.TryParse(XML.Read("Setting", path, defaultValue.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                    System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out v) && v >= minValue)
                return v;
            return defaultValue;
        }

        public bool LoadMap(string jsonOrFilePath)
        {
            try
            {
                if (File.Exists(jsonOrFilePath))
                    jsonOrFilePath = File.ReadAllText(jsonOrFilePath);
                LoadMapInternal(jsonOrFilePath);
                Log.WriteSystemLog(ModuleName, "地圖載入成功");
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog(ModuleName, $"地圖載入失敗: {ex.Message}");
                OnMapError?.Invoke($"載入地圖失敗: {ex.Message}");
                return false;
            }
        }

        private void LoadMapInternal(string json)
        {
            _mapData = new MapDataManager();
            _mapData.LoadFromJson(json);

            _renderer = new MapRenderer(_mapData);

            if (_mapData.Format == "GT Map Format")
                {
                    Footprint.MmPerPixel = MmPerPixel;
                    var gridPlanner = new GridAStarPlanner(_mapData, Footprint) { Log = Log, UnknownCostFactor = UnknownCostFactor };
                    _pathPlanner = gridPlanner;
                    Log.WriteSystemLog(ModuleName, gridPlanner.GetGridSummary());
                }
            else
                _pathPlanner = new DijkstraPlanner(_mapData);

            SyncDisplayOptions();
            PopulateComboBoxes();
            ClearRouteDisplay();

            txtResult.Text = _mapData.GetMapInfo();
            panelMap.Invalidate();
        }

        // ==========================================
        //  顯示已派送給 MR (VDA 5050) 的節點與車頭朝向
        // ==========================================
        public void ShowDispatchedRoute(MapRouteData route)
        {
            if (_renderer == null || route == null || !route.Success || route.PathCoordinates == null || route.PathCoordinates.Count < 2)
                return;

            var coords = route.PathCoordinates;
            int last = coords.Count - 1;
            bool hasIds = route.PathNodeIds != null && route.PathNodeIds.Count == coords.Count;
            bool hasEo = route.EdgeOrientations != null && route.EdgeOrientations.Count == last;
            bool hasNodeTheta = route.NodeThetas != null && route.NodeThetas.Count == coords.Count;

            var nodes = new List<DispatchNode>();
            for (int i = 0; i <= last; i++)
            {
                string id = hasIds && route.PathNodeIds[i] != null ? route.PathNodeIds[i]
                          : i == 0 ? (route.StartID > 0 ? route.StartID.ToString() : "START")
                          : i == last ? route.EndID.ToString()
                          : "WP_" + i;

                int a = i < last ? i : i - 1;
                double theta = Math.Atan2(coords[a + 1].Y - coords[a].Y, coords[a + 1].X - coords[a].X);
                if (hasEo && route.EdgeOrientations[a].HasValue) theta = route.EdgeOrientations[a].Value;
                if (i == last && route.EndTheta.HasValue) theta = route.EndTheta.Value;
                if (hasNodeTheta && route.NodeThetas[i].HasValue) theta = route.NodeThetas[i].Value;

                nodes.Add(new DispatchNode { Id = id, Position = coords[i], Theta = theta });
            }

            _renderer.CurrentRouteCoordinates = coords;
            _renderer.DispatchedNodes = nodes;
            UpdateUI(() => panelMap.Invalidate());
        }

        // ==========================================
        //  從 Core 接收即時座標
        // ==========================================
        public void UpdateMRPose(double x, double y, double theta)
        {
            if (_renderer != null)
            {
                _renderer.MR_X = x;
                _renderer.MR_Y = y;
                _renderer.MR_Theta = theta;
                _renderer.ShowMR = true;
            }

            if (_mapData != null)
            {
                bool isNewPoint = false;
                if (!_mapData.PointDict.ContainsKey("MR_CURRENT"))
                {
                    var mrPt = new MapPoint
                    {
                        ID = "MR_CURRENT",
                        Name = "目前位置",
                        TagNumber = -999, // 虛擬 Tag
                        Enable = true,
                        StationType = StationTypes.Normal,
                        IsVirtualPoint = true
                    };
                    _mapData.Points.Add(mrPt);
                    _mapData.PointDict["MR_CURRENT"] = mrPt;
                    isNewPoint = true;
                }

                var pt = _mapData.PointDict["MR_CURRENT"];
                pt.X = x;
                pt.Y = y;

                if (isNewPoint)
                {
                    UpdateUI(() => PopulateComboBoxes());
                }
            }

            UpdateUI(() => panelMap.Invalidate());
        }

        public MapRouteData PlanRoute(int startTag, int endTag, int waypointTag = -1)
        {
            var route = new MapRouteData { Success = false };
            if (!IsMapLoaded)
            {
                OnMapError?.Invoke("地圖尚未載入");
                return route;
            }

            try
            {
                string startId = FindIdByTag(startTag);
                string endId = FindIdByTag(endTag);
                string waypointId = waypointTag > 0 ? FindIdByTag(waypointTag) : null;

                if (startId == null) { OnMapError?.Invoke($"找不到起點 Tag:{startTag}"); return route; }
                if (endId == null) { OnMapError?.Invoke($"找不到終點 Tag:{endTag}"); return route; }

                route = _pathPlanner.PlanRoute(startId, endId, waypointId);

                if (route.Success)
                {
                    _renderer.HighlightStart = startId;
                    _renderer.HighlightWaypoint = waypointId;
                    _renderer.HighlightEnd = endId;
                    _renderer.CurrentRoutePath = route.FullPathNames;
                    _renderer.CurrentRouteCoordinates = route.PathCoordinates;

                    UpdateUI(() =>
                    {
                        txtResult.Text = FormatRouteResult(route);
                        panelMap.Invalidate();
                    });
                    Log.WriteSystemLog(ModuleName, $"路徑規劃成功: 起點 Tag {startTag} -> 終點 Tag {endTag}");
                    OnRoutePlaned?.Invoke(route);
                }
                else
                {
                    string reason = (_pathPlanner as IFootprintPlanner)?.LastError;
                    Log.WriteSystemLog(ModuleName, $"路徑規劃失敗: 起點 Tag {startTag} -> 終點 Tag {endTag}" + (string.IsNullOrEmpty(reason) ? "" : " (" + reason + ")"));
                    if (!string.IsNullOrEmpty(reason)) OnMapError?.Invoke("路徑規劃失敗: " + reason);
                }
                return route;
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog(ModuleName, $"路徑規劃例外: {ex.Message}");
                OnMapError?.Invoke($"路徑規劃失敗: {ex.Message}");
                return route;
            }
        }

        public MapRouteData PlanFreeRoute(IList<RoutePoint> points)
        {
            var route = new MapRouteData { Success = false };
            if (!IsMapLoaded)
            {
                OnMapError?.Invoke("地圖尚未載入");
                return route;
            }

            var planner = _pathPlanner as IFootprintPlanner;
            if (planner == null)
            {
                OnMapError?.Invoke("目前地圖格式不支援 Free 模式");
                return route;
            }

            try
            {
                route = planner.PlanFreeRoute(points);

                // 手動 Free 模式中，外部 (如 AutoRun) 的規劃不覆蓋使用者點選的畫面
                if (chkFreeMode.Checked && !_manualFreePlanning)
                {
                    if (route.Success) OnRoutePlaned?.Invoke(route);
                    return route;
                }

                if (route.Success)
                {
                    _renderer.HighlightStart = null;
                    _renderer.HighlightWaypoint = null;
                    _renderer.HighlightEnd = null;
                    _renderer.CurrentRoutePath = route.FullPathNames;
                    _renderer.CurrentRouteCoordinates = route.PathCoordinates;
                    _renderer.FreePoints = points;
                    _renderer.FreePointErrorIndex = -1;

                    UpdateUI(() =>
                    {
                        txtResult.Text = FormatRouteResult(route);
                        panelMap.Invalidate();
                    });
                    Log.WriteSystemLog(ModuleName, $"Free 路徑規劃成功: 點數 {points.Count}");
                    OnRoutePlaned?.Invoke(route);
                }
                else
                {
                    string reason = planner.LastError;
                    _renderer.CurrentRouteCoordinates = null;
                    _renderer.FreePoints = points;
                    _renderer.FreePointErrorIndex = planner.LastErrorPointIndex;
                    UpdateUI(() =>
                    {
                        txtResult.Text = "❌ 路徑規劃失敗" + (string.IsNullOrEmpty(reason) ? "" : Environment.NewLine + reason);
                        panelMap.Invalidate();
                    });
                    Log.WriteSystemLog(ModuleName, "Free 路徑規劃失敗" + (string.IsNullOrEmpty(reason) ? "" : " (" + reason + ")"));
                    if (!string.IsNullOrEmpty(reason)) OnMapError?.Invoke("路徑規劃失敗: " + reason);
                }
                return route;
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog(ModuleName, $"Free 路徑規劃例外: {ex.Message}");
                OnMapError?.Invoke($"路徑規劃失敗: {ex.Message}");
                return route;
            }
        }

        public bool StartFreeNavigation(IList<RoutePoint> points)
        {
            var response = PlanFreeRoute(points);
            if (!response.Success) return false;

            // Free 路徑不依站點到達回報追蹤進度
            _navigationRoute = null;
            _navigationIndex = 0;
            _isNavigating = false;
            Log.WriteSystemLog(ModuleName, $"開始 Free 導航: 點數 {points.Count}");

            OnNavigationStarted?.Invoke(response);
            return true;
        }

        public bool StartNavigation(int startTag, int endTag, int waypointTag = -1)
        {
            var response = PlanRoute(startTag, endTag, waypointTag);
            if (!response.Success) return false;

            _navigationRoute = response.FullPath;
            _navigationIndex = 0;
            _isNavigating = true;
            Log.WriteSystemLog(ModuleName, $"開始導航: 起點 Tag {startTag} -> 終點 Tag {endTag}");

            OnNavigationStarted?.Invoke(response);
            ReportNavigationProgress();
            return true;
        }

        public void ReportArrival(int tagNumber)
        {
            if (!_isNavigating || _navigationRoute == null) return;

            int expectedIndex = _navigationIndex + 1;
            if (expectedIndex >= _navigationRoute.Count) return;

            if (_navigationRoute[expectedIndex] != tagNumber)
            {
                OnMapError?.Invoke($"預期 Tag:{_navigationRoute[expectedIndex]}，收到 Tag:{tagNumber}");
                return;
            }

            _navigationIndex = expectedIndex;
            Log.WriteSystemLog(ModuleName, $"到達 Tag {tagNumber}");
            UpdateAGVPositionFallback(tagNumber);
            ReportNavigationProgress();
        }

        public void CancelNavigation()
        {
            _isNavigating = false;
            _navigationRoute = null;
            _navigationIndex = 0;
            Log.WriteSystemLog(ModuleName, "取消導航");
            UpdateUI(() => panelMap.Invalidate());
        }

        public List<int> GetAvailableTagNumbers()
        {
            return _mapData?.GetStationPoints().Select(p => p.TagNumber).Where(t => t > 0).ToList() ?? new List<int>();
        }

        private void ReportNavigationProgress()
        {
            if (_navigationRoute == null) return;

            int currentTag = _navigationRoute[_navigationIndex];
            bool isComplete = _navigationIndex >= _navigationRoute.Count - 1;

            if (isComplete)
            {
                _isNavigating = false;
                OnRouteCompleted?.Invoke(currentTag, _navigationRoute);
            }
            else
            {
                int nextTag = _navigationRoute[_navigationIndex + 1];
                OnRouteProgress?.Invoke(currentTag, nextTag, _navigationIndex, _navigationRoute.Count);
            }
        }

        private void UpdateAGVPositionFallback(int tagNumber)
        {
            string pointId = FindIdByTag(tagNumber);
            if (pointId != null && _renderer != null && _mapData.PointDict.ContainsKey(pointId))
            {
                var pt = _mapData.PointDict[pointId];
                _renderer.MR_X = pt.X;
                _renderer.MR_Y = pt.Y;
                _renderer.ShowMR = true;
                UpdateUI(() => panelMap.Invalidate());
            }
        }

        private string FindIdByTag(int tagNumber)
        {
            return _mapData.Points.FirstOrDefault(p => p.TagNumber == tagNumber)?.ID;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "JSON/TXT Files|*.json;*.txt" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try { LoadMapInternal(File.ReadAllText(ofd.FileName)); }
                    catch (Exception ex) { MessageBox.Show($"載入地圖失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
        }

        private void BtnPlan_Click(object sender, EventArgs e)
        {
            if (!IsMapLoaded) return;

            btnPlan.Enabled = false;
            Cursor = Cursors.WaitCursor;
            txtResult.Text = "⏳ 正在規劃路線，運算中，請稍候...";
            txtResult.Refresh();
            try
            {
                if (chkFreeMode.Checked)
                {
                    var pts = BuildFreeRouteList();
                    if (pts != null)
                    {
                        _manualFreePlanning = true;
                        try { PlanFreeRoute(pts); }
                        finally { _manualFreePlanning = false; }
                    }
                    return;
                }
                var start = cboStart.SelectedItem as MapPoint;
                var end = cboEnd.SelectedItem as MapPoint;
                var waypoint = cboWaypoint.SelectedItem as MapPoint;

                if (start != null && end != null)
                    PlanRoute(start.TagNumber, end.TagNumber, waypoint?.TagNumber ?? -1);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnPlan.Enabled = true;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (!IsMapLoaded) return;
            if (chkFreeMode.Checked)
            {
                var pts = BuildFreeRouteList();
                if (pts != null)
                {
                    bool freeOk;
                    _manualFreePlanning = true;
                    try { freeOk = StartFreeNavigation(pts); }
                    finally { _manualFreePlanning = false; }
                    txtResult.AppendText(freeOk ? Environment.NewLine + "🚀 導航已開始" : Environment.NewLine + "❌ 導航啟動失敗");
                }
                return;
            }
            var start = cboStart.SelectedItem as MapPoint;
            var end = cboEnd.SelectedItem as MapPoint;
            var waypoint = cboWaypoint.SelectedItem as MapPoint;

            if (start != null && end != null)
            {
                bool ok = StartNavigation(start.TagNumber, end.TagNumber, waypoint?.TagNumber ?? -1);
                txtResult.AppendText(ok ? Environment.NewLine + "🚀 導航已開始" : Environment.NewLine + "❌ 導航啟動失敗");
            }
        }

        private void PanelMap_Paint(object sender, PaintEventArgs e) => _renderer?.Render(e.Graphics, panelMap);

        // ==========================================
        //  Free 模式：地圖點選
        // ==========================================
        private void PanelMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (_renderer == null || !IsMapLoaded) return;

            // 中鍵拖曳，或非 Free 模式時左鍵拖曳，平移地圖
            if (e.Button == MouseButtons.Middle || (e.Button == MouseButtons.Left && !chkFreeMode.Checked))
            {
                _panning = true;
                _lastMouse = e.Location;
                panelMap.Cursor = Cursors.SizeAll;
                return;
            }

            if (!chkFreeMode.Checked) return;

            if (e.Button == MouseButtons.Right)
            {
                if (_freePoints.Count > 0) _freePoints.RemoveAt(_freePoints.Count - 1);
                UpdateFreeDisplay();
                return;
            }

            if (e.Button != MouseButtons.Left) return;

            var m = _renderer.ScreenToMap(e.Location);
            _freePoints.Add(new RoutePoint { X = m.X, Y = m.Y });
            _freeDragging = true;
            UpdateFreeDisplay();
        }

        private void PanelMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (_panning && _renderer != null)
            {
                _renderer.PanBy(e.X - _lastMouse.X, e.Y - _lastMouse.Y);
                _lastMouse = e.Location;
                panelMap.Invalidate();
                return;
            }

            if (!_freeDragging || _freePoints.Count == 0 || _renderer == null) return;

            var last = _freePoints[_freePoints.Count - 1];
            var m = _renderer.ScreenToMap(e.Location);
            double dx = m.X - last.X, dy = m.Y - last.Y;
            if (Math.Sqrt(dx * dx + dy * dy) < 3) return;

            last.Theta = Math.Atan2(dy, dx);
            UpdateFreeDisplay();
        }

        private void PanelMap_MouseUp(object sender, MouseEventArgs e)
        {
            _freeDragging = false;
            if (_panning)
            {
                _panning = false;
                panelMap.Cursor = chkFreeMode.Checked ? Cursors.Cross : Cursors.Default;
            }
        }

        /// <summary>組出規劃用的點清單 (含 MR 目前位置作為起點)，無法組成時回傳 null。</summary>
        private List<RoutePoint> BuildFreeRouteList(bool showMessage = true)
        {
            var list = new List<RoutePoint>();
            if (chkStartFromMr.Checked)
            {
                if (_renderer == null || !_renderer.ShowMR)
                {
                    if (showMessage) txtResult.Text = "尚未收到 MR 目前位置，無法以 MR 位置作為起點";
                    return null;
                }
                list.Add(new RoutePoint { X = _renderer.MR_X, Y = _renderer.MR_Y, IsCurrentPosition = true, Theta = _renderer.MR_Theta * Math.PI / 180.0 });
            }
            list.AddRange(_freePoints);

            if (list.Count < 2)
            {
                if (showMessage) txtResult.Text = "Free 模式至少需要起點與終點";
                return null;
            }
            return list;
        }

        private void UpdateFreeDisplay()
        {
            if (_renderer == null) return;
            var preview = BuildFreeRouteList(false);
            if (preview == null)
            {
                // 不足兩點時仍顯示已選的點
                preview = new List<RoutePoint>();
                if (chkStartFromMr.Checked && _renderer.ShowMR)
                    preview.Add(new RoutePoint { X = _renderer.MR_X, Y = _renderer.MR_Y, IsCurrentPosition = true });
                preview.AddRange(_freePoints);
            }
            _renderer.CurrentRouteCoordinates = null;
            _renderer.CurrentRoutePath = null;
            _renderer.FreePoints = preview;
            _renderer.FreePointErrorIndex = -1;
            panelMap.Invalidate();
        }

        private void ClearFreePoints()
        {
            _freePoints.Clear();
            _freeDragging = false;
            if (_renderer != null)
            {
                _renderer.FreePoints = null;
                _renderer.FreePointErrorIndex = -1;
                _renderer.CurrentRouteCoordinates = null;
                _renderer.CurrentRoutePath = null;
            }
            panelMap.Invalidate();
        }
        private void PanelMap_Resize(object sender, EventArgs e) { _renderer?.InvalidateTransform(); panelMap.Invalidate(); }
        private void DisplayOption_Changed(object sender, EventArgs e) { SyncDisplayOptions(); panelMap.Invalidate(); }
        private void TryAutoLoad() { try { string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map.json"); if (File.Exists(p)) LoadMapInternal(File.ReadAllText(p)); } catch { } }
        private void SyncDisplayOptions() { if (_renderer != null) { _renderer.Options.ShowLabels = chkShowLabels.Checked; _renderer.Options.ShowPaths = chkShowPaths.Checked; _renderer.Options.ShowPolygons = chkShowPolygons.Checked; _renderer.Options.ShowLegend = chkShowLegend.Checked; _renderer.Options.ShowVirtualPoints = chkShowVirtualPoints.Checked; _renderer.InvalidateTransform(); } }

        private void PopulateComboBoxes()
        {
            if (_mapData == null) return;
            var stations = _mapData.GetStationPoints();

            var mrPt = _mapData.PointDict.ContainsKey("MR_CURRENT") ? _mapData.PointDict["MR_CURRENT"] : null;

            cboStart.DataSource = null; cboStart.Items.Clear();
            if (mrPt != null) cboStart.Items.Add(mrPt);
            stations.ForEach(s => cboStart.Items.Add(s));
            if (cboStart.Items.Count > 0) cboStart.SelectedIndex = 0;

            cboEnd.DataSource = null; cboEnd.Items.Clear();
            stations.ForEach(s => cboEnd.Items.Add(s));

            cboWaypoint.DataSource = null; cboWaypoint.Items.Clear(); cboWaypoint.Items.Add("(無)");
            stations.ForEach(s => cboWaypoint.Items.Add(s));
            cboWaypoint.SelectedIndex = 0;
        }

        private string FormatRouteResult(MapRouteData route)
        {
            var lines = new List<string> { "✔ 路徑規劃成功", "", $"總距離: {route.TotalDistance:F2}", $"路徑節點數: {route.FullPath.Count}", "" };
            lines.Add("路徑站點:");
            foreach (var pt in route.FullPathNames) lines.Add($"  → {pt}");
            return string.Join(Environment.NewLine, lines);
        }

        private void ClearRouteDisplay()
        {
            if (_renderer != null)
            {
                _renderer.HighlightStart = null;
                _renderer.HighlightWaypoint = null;
                _renderer.HighlightEnd = null;
                _renderer.CurrentRoutePath = null;
                _renderer.CurrentRouteCoordinates = null;
                _renderer.FreePoints = null;
                _renderer.FreePointErrorIndex = -1;
            }
            _freePoints.Clear();
            panelMap.Invalidate();
        }

        private void UpdateUI(Action action) { if (!IsHandleCreated) return; if (InvokeRequired) BeginInvoke(action); else action(); }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) { _isNavigating = false; _navigationRoute = null; _renderer?.Dispose(); components?.Dispose(); }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}