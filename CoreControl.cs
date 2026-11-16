using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using MySql.Data.MySqlClient;
using AGVMap;
using MobileRobot;


namespace AML
{
    public enum CoreCJState { Wait, Run, LoadComplete, FirstHandle, End, Prussuring } //AbnormalEnd
    public enum AutoRunType { AutoRun, Monitor_Particle, Specified_MacroRun }
    public enum CoreFoupState
    {
        //Cart to Stocker
        Wait_Selected,
        WaitRobot_GetCartFOUP,
        RobotGet_CartFOUP,
        RobotPut_StockerInPort,
        FOUP_RFID_Dismatch,
        Complete_FOUPToStocker,

        //Stocker to Cart
        Wait_FOUPArrival,
        WaitRobot_GetOutPortFOUP,
        RobotGet_StockerOutPort,
        RobotPut_CartFOUP,
        Complete_FOUPInCart,

        Unknown
    }
    public enum CoreCJFromTo { Unknown, Cart_STK, STK_Cart, Take_Back_Board, Monitor_Cart, Monitor_STK, Take_Board_Shelf }
    public enum CoreMemberList { SECSAgent, Tool }
    public enum CoreActionItem { LotInfoChange }
    public enum CoreCartState
    {
        CartLoad, CartOpen, CloseExecuteLid, CloseNoUnload, CloseNoPower, OpenWithAllinNoClose, OpenWithAllOut, CloseWithPressure, OpenWithFdcUploadNoClose,
        ManualStkToCart_NoSpecified, ManualStkToCart_Specified, OpenWithAllinAndClose, CartMacro_OpenAndClose, OpenWithFdcUploadAndClose
    }
    /// <summary>
    ///  CartLoad : first proceed s3f17 Load - only cart up and lid close
    ///  CartOpen : CartLoad and open Cart , also using second proceed
    ///  CloseExecuteLid : Cancel Carrier also Unload Macro
    ///  CloseNoUnload : close until pressure OK no cart down
    ///  CloseNoPower : like CloseExecuteLid but no charge
    ///  OpenWithAllin : open and Cart to STK 
    ///  OpenWithAllOut : open and STK to Cart
    ///  CloseWithPressure : like Unload but pressure use Manual pressure
    ///  OpenWithFdcUpload : open and upload fdc
    /// </summary>

    public partial class CoreControl : UserControl
    {
        public delegate void CoreActionEvent(CoreActionItem Item);
        public event CoreActionEvent CoreAction;
        public event AlarmEvent Alarm;

        public struct ProcessFoup
        {
            public double[] CartOffset;
            public string CJID;
            public int Port;
            public int Position;
            public string CartID;
            public string FoupID;
            public CoreFoupState State;
            public string SourceInfo;
            public string TargetInfo;

            public void Set(string _CJID, string _FoupID)
            {
                try
                {
                    CJID = _CJID;
                    FoupID = _FoupID;

                    string condiction = "cjid ='" + CJID + "' and foupid ='" + FoupID + "'";
                    DataTable FoupInfoTable = MySQL.Read2DArray_Data("port,storage,cartid,sourceinfo,targetinfo,cart_offset,state", "pool_foupinfo", condiction);
                    if (FoupInfoTable == null || FoupInfoTable.Rows.Count == 0)
                        throw new Exception("FoupID: " + FoupID + " is NOT exist in Data Record, please check the foup list in setting page");
                    Port = int.Parse(FoupInfoTable.Rows[0]["port"].ToString());
                    Position = int.Parse(FoupInfoTable.Rows[0]["storage"].ToString());
                    CartID = FoupInfoTable.Rows[0]["cartid"].ToString();
                    if (!Enum.TryParse<CoreFoupState>(FoupInfoTable.Rows[0]["state"].ToString(), out State))
                        State = CoreFoupState.Unknown;
                    SourceInfo = FoupInfoTable.Rows[0]["sourceinfo"].ToString();
                    TargetInfo = FoupInfoTable.Rows[0]["targetinfo"].ToString();
                    string strCartOffset = FoupInfoTable.Rows[0]["cart_offset"].ToString();
                    CartOffset = (strCartOffset == string.Empty) ? null : strCartOffset.Split(',').Select(c => Convert.ToDouble(double.Parse(c))).ToArray();
                }
                catch
                {
                    //須補啥?

                }
            }
        }
        public AML_LogTab Log;
        private AGVMapControl MR_PathFinder;
        private MRControl Mobile_Robot;
        private WT_MsgBox MsgBox = new WT_MsgBox();
        public BackgroundWorker BG_AutoRun = new BackgroundWorker();
        public BackgroundWorker BG_CycleRun = new BackgroundWorker();
        public BackgroundWorker BG_CycleRun_STK = new BackgroundWorker();
        public BackgroundWorker BG_HandEyeCalibration = new BackgroundWorker();
        public BackgroundWorker BG_ManualMonitor = new BackgroundWorker();
        public BackgroundWorker BG_LP1CartAutoRun = new BackgroundWorker();
        public BackgroundWorker BG_LP2CartAutoRun = new BackgroundWorker();

        //=====變數=====
        public volatile bool BreakFlag = false;
        public string ModuleName = "Core";
        public string LabelSourcePath;
        public string LabelTargetPath;
        public int CycleTime = 0;
        public int CycleLimit = 0;
        private string LogPath = string.Empty;

        public bool Busy
        { get { return (BG_AutoRun.IsBusy || BG_CycleRun.IsBusy || BG_CycleRun_STK.IsBusy || BG_ManualMonitor.IsBusy || BG_HandEyeCalibration.IsBusy || BG_LP1CartAutoRun.IsBusy || BG_LP2CartAutoRun.IsBusy); } }
        public Button TsMacroBtn = new Button();
        //=====初始化=====
        public CoreControl()
        {
            InitializeComponent();
        }
        public void Initial(AML_LogTab Ref_Log, AGVMapControl Ref_MR_PathFinder, MRControl Ref_Mobile_Robot)
        {
            Log = Ref_Log;
            MR_PathFinder = Ref_MR_PathFinder;
            Mobile_Robot = Ref_Mobile_Robot;

            Mobile_Robot.Alarm += SendAlarm;

            MR_PathFinder.Initial();
            Mobile_Robot.Initial();

            LogPath = XML.Read("Setting/Form1/Logfolder", "C:\\AML");


            //===== 事件註冊 =====

            Mobile_Robot.OnMissionStatusUpdated += HandleMissionStatusUpdated;
            Mobile_Robot.OnRobotLocationUpdated += HandleRobotLocationUpdated;
            MR_PathFinder.OnNavigationStarted += HandleRoutePlaned;
            OnRouteDispatch += Mobile_Robot.DispatchRoute;
            OnRouteDispatch += MR_PathFinder.ShowDispatchedRoute;
            Mobile_Robot.SetMapFrame(MR_PathFinder.MetersPerPixel, MR_PathFinder.MapMinX, MR_PathFinder.MapMaxY);
            Mobile_Robot.OnPoseUpdated += (x, y, theta) =>
            {
                MR_PathFinder.WorldToMap(x, y, out double mx, out double my);
                MR_PathFinder.UpdateMRPose(mx, my, -theta * 180.0 / Math.PI);
            };
            Mobile_Robot.OnOrderCompleted += HandleOrderCompleted;


            BG_AutoRun.DoWork += new DoWorkEventHandler(BG_AutoRun_DoWork);
            BG_AutoRun.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BG_AutoRun_Completed);

            BG_CycleRun.DoWork += new DoWorkEventHandler(BG_CycleRun_DoWork);
            BG_CycleRun.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BG_CycleRun_Completed);

            BG_CycleRun_STK.DoWork += new DoWorkEventHandler(BG_CycleRun_STK_DoWork);
            BG_CycleRun_STK.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BG_CycleRun_STK_Completed);

            BG_ManualMonitor.DoWork += new DoWorkEventHandler(BG_ManualMonitor_DoWork);
            BG_ManualMonitor.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BG_ManualMonitor_Completed);

            BG_LP1CartAutoRun.DoWork += new DoWorkEventHandler(BG_LP1AutoRun_DoWork);
            BG_LP1CartAutoRun.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BG_LP1AutoRun_Completed);

            BG_LP2CartAutoRun.DoWork += new DoWorkEventHandler(BG_LP2AutoRun_DoWork);
            BG_LP2CartAutoRun.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BG_LP2AutoRun_Completed);

            Initial_UI();
            RefreshSystem();
            RefreshCameraList();

            //BG_AutoRun.RunWorkerAsync();
        }
        private void TsRobotBtn_Click(object sender, EventArgs e)
        {
        }
        private void TsAdvanceBtn_Click(object sender, EventArgs e)
        {
        }
        private void TsMacroBtn_Click(object sender, EventArgs e)
        {
        }
        private void Initial_UI()
        {
            RefreshData();
        }
        public void CloseSeq()
        {
            Interrupt();
        }

        //=====流程處理=====
        private void BG_AutoRun_DoWork(object sender, DoWorkEventArgs e)
        {

            RefreshSystem();
            RefreshData();
            BreakFlag = false;
            e.Result = false;

            CycleTime = 0;
            CycleLimit = -1;
            int sinceRack = 0;
            int rackIndex = 0;

            while (!BreakFlag)
            {
                CycleTime++;
                if (CycleLimit != -1 && CycleLimit < CycleTime)
                {
                    Log.WriteSystemLog("Cycle", $"Cycle 結束共執行 {CycleTime} 次");
                    return;
                }
                string suffix = (CycleLimit == -1) ? "設定不停止" : $"預計總共執行 {CycleLimit} 次";

                var route = PlanRandomRoute(ref sinceRack, ref rackIndex);
                if (route == null)
                {
                    CycleTime--;
                    Thread.Sleep(1000);
                    continue;
                }
                Log.WriteSystemLog("Cycle", $"開始執行第 {CycleTime} 次 Cycle，{suffix}");

                _arrived.Reset();
                OnRouteDispatch?.Invoke(route);
                if (!WaitArrival(TimeSpan.FromMinutes(10)))
                {
                    if (!BreakFlag) Log.WriteSystemLog("Cycle", $"第 {CycleTime} 次 Cycle 等待 MR 到點逾時");
                    continue;
                }
                Log.WriteSystemLog("Cycle", $"第 {CycleTime} 次 Cycle 完成，MR 已到達 {route.EndName}");
                //var response = MRRandomrouteRequest();

                //Log.WriteSystemLog(response.RouteTagNumbers.ToString());

                //for (int i = 0; i < response.RouteTagNumbers.Count; i++)
                //{
                //    int current_target = response.RouteTagNumbers[i];
                //    Log.WriteSystemLog($"目標 {current_target.ToString()} 點正在派送給 MR 指令");
                //}
                //DispathToMR(response.RouteTagNumbers);
            }
        }
        private void BG_AutoRun_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.WriteSystemLog("System", "AutoRun Control close");
            RefreshSystem();
        }

        private void BG_CycleRun_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void BG_CycleRun_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Result is bool ok && ok))
                BreakFlag = true;
            Log.WriteSystemLog("System", "CycleRun Control close");
            RefreshSystem();
        }

        private void BG_CycleRun_STK_DoWork(object sender, DoWorkEventArgs e)
        { // need test 20230411 JAY
            CycleTime++;
        }
        private void BG_CycleRun_STK_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.WriteSystemLog("System", "CycleRun Control close, Cycle 結束");
            RefreshSystem();
        }


        private void BG_ManualMonitor_DoWork(object sender, DoWorkEventArgs e)
        {
            RefreshSystem();
        }
        private void BG_ManualMonitor_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.WriteSystemLog("System", "Particle (SPC) Monitor close, SPC 結束");
            RefreshSystem();
        }

        private void BG_LP1AutoRun_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void BG_LP1AutoRun_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.WriteSystemLog("System", "LoadPort 1 開始執行開關蓋完畢");
            RefreshSystem();
        }

        private void BG_LP2AutoRun_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void BG_LP2AutoRun_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.WriteSystemLog("System", "LoadPort 2 開始執行開關蓋完畢");
            RefreshSystem();
        }
        //=====Foup Flow Event=====

        public void RefreshSystem()
        {
        }
        public void RefreshData()
        {
            RefreshCJList();
            RefreshFoupList();
        }
        public void RefreshCJList()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { RefreshCJList(); }));
                return;
            }

            try
            {
                DGV_CJList.DataSource = MySQL.Read2DArray_Data(
                    "cjid,from_to,fouplist,state,createtime,starttime,endtime",
                    "pool_cj", "1");
                DGV_CJList.ReadOnly = true;
                for (int i = 0; i < DGV_CJList.Columns.Count; i++)
                    DGV_CJList.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog("Core", $"RefreshCJList failed: {ex.Message}");
            }
        }
        public void RefreshFoupList()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { RefreshFoupList(); }));
                return;
            }
            try
            {
                DGV_FoupList.DataSource = MySQL.Read2DArray_Data("*", "pool_foupinfo", "1");
                DGV_FoupList.ReadOnly = true;
                DGV_FoupList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog("Core", $"RefreshFoupList failed: {ex.Message}");
            }

            CoreAction?.Invoke(CoreActionItem.LotInfoChange);
        }
        public void RefreshCameraList()
        {
        }

        //=====傳送套件=====

        //=====Wait專用/有Timeout功能=====
        public void Interrupt()
        {
            BreakFlag = true;
            _arrived.Set();
        }

        //=====Event=====


        //=====Check ContinueRun Sequence =====


        //=====Lot Manage=====
        private void DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < ((DataGridView)sender).Rows.Count; i++)
                ((DataGridView)sender).Rows[i].HeaderCell.Value = (i + 1).ToString();
        }
        private void BT_RefreshCJList_Click(object sender, EventArgs e)
        {
            RefreshCJList();
        }
        private void BT_RefreshFoupList_Click(object sender, EventArgs e)
        {
            RefreshFoupList();
        }
        private void BT_RefreshCameraList_Click(object sender, EventArgs e)
        {
            Log.WriteOperationLog("Press Refresh CameraList Button");
            RefreshCameraList();
        }
        private void BT_DeleteCJList_Click(object sender, EventArgs e)
        {
            if (DGV_CJList.CurrentCell == null)
                return;
            int RowIndex = DGV_CJList.CurrentCell.RowIndex;
            DialogResult result = MsgBox.ShowYesNo("確定刪除第" + (RowIndex + 1).ToString() + "列之CJ 以及相關FOUP資料? (此動作無法復原)");
            if (result == DialogResult.Yes)
            {
                string DeleteCJ = DGV_CJList["cjid", RowIndex].Value.ToString();
                int PortNumber = Convert.ToInt16(DeleteCJ.Substring(1, 1));
                MySQL.Delete("pool_foupinfo", "cjid = '" + DeleteCJ + "'");
                MySQL.Delete("pool_cj", "cjid = '" + DeleteCJ + "'");
                Log.WriteOperationLog("Press delete control job:" + DeleteCJ);
                RefreshData();
            }
        }


        // ====== Function ======

        private List<MissionPlanItem> PathtoMQTTJson(List<int> PahtList)
        {
            // 透過 Select 帶索引 (nodeId, index) 轉換並直接塞入 Parameters
            List<MissionPlanItem> missionPlan = PahtList.Select((nodeId, index) =>
            {
                return new MissionPlanItem
                {
                    Type = "station",
                    Value = nodeId.ToString(),
                    Parameters = new Dictionary<string, object>
                    {
                        { "speed", 1.2 },                                    // 運行速度 (m/s)
                    }
                };
            }).ToList();
            return missionPlan;
        }

        private async void DispathToMR(List<int> PahtList)
        {
            var plan = PathtoMQTTJson(PahtList);
            await Mobile_Robot.SendMissionByPlanAsync($"MR_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"), plan);
        }

        //=====Alarm=====
        private void SendAlarm(string Code, string Description)
        {
            if (Alarm != null)
                Alarm(Code, Description);
        }


        #region ALID
        //====ALID====
        public string ALID_CoreAutoRunSequenceError = "9900000";    //自動排程異常

        #endregion
        #region ECID
        //====ECID====
        public int ECID_LP1_Enable = 6870100;




        //public string ECID_HighDiffPressure = "6010001";
        //public string ECID_LowDiffPressure = "6010002";
        //public string ECID_HumidityThres = "6020001";
        #endregion
        #region SVID
        //====SVID====
        public int SVID_ProcessState = 8000006;
        #endregion

        //=====Botton Click=====
        private void BT_McuEC_Click(object sender, EventArgs e)
        {

        }
        private void BT_AdviseEC_Click(object sender, EventArgs e)
        {
            DialogResult result = MsgBox.ShowYesNo("確定要修改設定?");
            if (result == DialogResult.No)
            {
                L_DiffPressure_textBox.Enabled = false;
                H_DiffPressure_textBox.Enabled = false;
                Humidity_textBox.Enabled = false;
                return;
            }

            L_DiffPressure_textBox.Enabled = true;
            H_DiffPressure_textBox.Enabled = true;
            Humidity_textBox.Enabled = true;

            Log.WriteOperationLog("按下確認修改開蓋閥值");
        }

        private void BT_AdvisePressure_Click(object sender, EventArgs e)
        {
            DialogResult result = MsgBox.ShowYesNo("確定要修改壓差設定?");
            if (result == DialogResult.No)
            {
                AutoRun_DiffPressure.Enabled = false;
                Manual_DiffPressure.Enabled = false;
                return;
            }

            AutoRun_DiffPressure.Enabled = true;
            Manual_DiffPressure.Enabled = true;

            Log.WriteOperationLog("按下修改充氣閥值");
        }
        private void BT_SavePressure_Click(object sender, EventArgs e)
        {

        }

        //private async void button1_Click(object sender, EventArgs e)
        //{
        //    // 如果還沒連線，先連線
        //    await MQTT_test.ConnectAsync();
        //    Thread.Sleep(500);
        //    await MQTT_test.SubscribeAsync("sensor/temperature", 0);

        //    Thread.Sleep(500);
        //    // 發佈
        //    await MQTT_test.PublishMessageAsync("cmd/control", "{\"action\":\"start\"}");
        //}


        // ===== MR Bridge =====
        private RouteData _currentRoute;
        private readonly object _routeLock = new object();

        public RouteData CurrentRoute
        {
            get { lock (_routeLock) { return _currentRoute; } }
        }



        // 暫代上位 MCS：路徑規劃完成後由 Core 以 RouteDispatch 事件派給 MobileRobot
        public delegate void RouteDispatchEvent(MapRouteData route);
        public event RouteDispatchEvent OnRouteDispatch;

        public const double TurnThresholdDeg = 60.0;
        public const double TurnInsertDistanceM = 0.3;
        public const int RackEveryN = 5;

        private readonly ManualResetEvent _arrived = new ManualResetEvent(false);
        private readonly Random _rnd = new Random();

        private void HandleRoutePlaned(MapRouteData route)
        {
            if (route == null || !route.Success) return;
            OnRouteDispatch?.Invoke(route);
        }

        private string _lastWaitMsg;
        private void LogOnce(string msg)
        {
            if (_lastWaitMsg == msg) return;
            _lastWaitMsg = msg;
            Log.WriteSystemLog("Cycle", msg);
        }

        private void HandleOrderCompleted(string orderId)
        {
            if (orderId == Mobile_Robot.LastOrderId)
                _arrived.Set();
        }

        private bool WaitArrival(TimeSpan timeout)
        {
            var sw = Stopwatch.StartNew();
            while (!BreakFlag && sw.Elapsed < timeout)
            {
                if (_arrived.WaitOne(50)) return !BreakFlag;
            }
            return false;
        }

        /// <summary>產生一筆可執行隨機路徑：起點為 MR 目前位置，每 RackEveryN 筆內至少一筆到 Rack (輪流)</summary>
        private MapRouteData PlanRandomRoute(ref int sinceRack, ref int rackIndex)
        {
            if (!MR_PathFinder.IsMapLoaded) { LogOnce("地圖尚未載入，等待中"); return null; }
            var pose = Mobile_Robot.LastState?.agvPosition;
            if (pose == null) { LogOnce("尚未收到 MR state，等待中"); return null; }
            _lastWaitMsg = null;

            MR_PathFinder.WorldToMap(pose.x, pose.y, out double sx, out double sy);
            var start = new RoutePoint { X = sx, Y = sy, IsCurrentPosition = true };

            var racks = MR_PathFinder.GetStationsByType(StationTypes.Rack);
            for (int attempt = 0; attempt < 20 && !BreakFlag; attempt++)
            {
                RoutePoint target;
                bool toRack = racks.Count > 0 && (sinceRack >= RackEveryN - 1 || _rnd.Next(RackEveryN) == 0);
                if (toRack)
                {
                    var rack = racks[rackIndex % racks.Count];
                    target = new RoutePoint { X = rack.X, Y = rack.Y, StationId = rack.ID };
                }
                else
                {
                    if (!MR_PathFinder.TryGetRandomFreePoint(_rnd, out PointF p)) continue;
                    target = new RoutePoint { X = p.X, Y = p.Y };
                }

                double dx = target.X - sx, dy = target.Y - sy;
                if (Math.Sqrt(dx * dx + dy * dy) * MR_PathFinder.MetersPerPixel < 1.0 && !toRack) continue;

                var route = MR_PathFinder.PlanFreeRoute(new List<RoutePoint> { start, target });
                if (route == null || !route.Success) continue;

                if (toRack) { rackIndex++; sinceRack = 0; } else sinceRack++;
                route.StartName = route.StartName ?? "MR";
                route.EndName = route.EndName ?? (target.StationId ?? $"Free({target.X:F0},{target.Y:F0})");
                Log.WriteSystemLog("Cycle", $"隨機路徑 -> {route.EndName}{(toRack ? " (Rack)" : "")}, 點數 {route.PathCoordinates.Count}");
                return route;
            }
            Log.WriteSystemLog("Cycle", "隨機路徑規劃失敗");
            return null;
        }

        // ====== 事件註冊 ======
        private void HandleMissionStatusUpdated(MissionStatusReport report)
        {
            // 機器人回報狀態 (0: IDLE, 3: EXECUTING, 5: COMPLETED, 6: FAILED 等)
            Log.WriteSystemLog($"[Core] 收到任務進度: ID={report.MissionId}, Code={report.MissionStatusCode}, Target={report.CurrentTarget}");

            if (report.MissionStatusCode == 5) // COMPLETED
            {
                Log.WriteSystemLog($"[Core] 任務 {report.MissionId} 已完成！");
                // 觸發下一階段邏輯...
            }
        }

        // ==========================================
        // 接收機器人座標回調處理 (3.5.2)
        // ==========================================
        private void HandleRobotLocationUpdated(RobotLocationReport loc)
        {
            // 即時座標，可用於 UI 地圖繪製或避障運算
            // X: loc.RobotPosX, Y: loc.RobotPosY, Point: loc.CurrentPoint
        }
    }
}