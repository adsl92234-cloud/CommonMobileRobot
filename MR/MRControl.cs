using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AML;
using Newtonsoft.Json;

namespace MobileRobot
{
    public partial class MRControl : UserControl
    {
        // ==========================================
        // 核心屬性與外部相依
        // ==========================================
        //private MQTT mqtt;
        private MqttClient mqttClient;
        public AML_LogPage Log = new AML_LogPage();

        public enum MobileRobotVendor
        {
            GT,
            GPM,
        }

        //========狀態========
        private bool _connect;
        private string _brokerip;
        private int _brokerport;
        private string _clientid;
        private double _position_x;
        private double _position_y;
        private double _theta;
        private int _orderSeq;

        //========設定========
        private bool Connect
        {
            set
            {
                _connect = value;
                RunOnUI(() =>
                {
                    lblTopicStatusVal.Text = value ? "Active / Receiving" : "Disconnected / Idle";
                    lblTopicStatusVal.ForeColor = value ? Color.LimeGreen : Color.OrangeRed;
                });
            }
            get { return _connect; }
        }
        private string BrokerIP
        {
            set
            {
                _brokerip = value;
                RunOnUI(() => lblIpVal.Text = value);
            }
            get { return _brokerip; }
        }
        private int BrokerPort
        {
            set { _brokerport = value; }
            get { return _brokerport; }
        }
        private string ClientId
        {
            set { _clientid = value; }
            get { return _clientid; }
        }
        private double Position_X
        {
            set
            {
                _position_x = value;
                RunOnUI(() => lblPosXVal.Text = value.ToString("F4"));
            }
            get { return _position_x; }
        }
        private double Position_Y
        {
            set
            {
                _position_y = value;
                RunOnUI(() => lblPosYVal.Text = value.ToString("F4"));
            }
            get { return _position_y; }
        }
        private double Theta
        {
            set
            {
                _theta = value;
                RunOnUI(() => lblThetaVal.Text = (value * 180.0 / Math.PI).ToString("F4") + "°"); // 將弧度轉換為角度顯示
            }
            get { return _theta; }
        }
        /// <summary>MQTT 是否已連線</summary>
        public bool IsConnected => mqttClient != null && mqttClient.IsConnected;

        private void RunOnUI(Action action)
        {
            if (InvokeRequired)
            {
                if (IsHandleCreated && !IsDisposed)
                    this.BeginInvoke(action);
            }
            else
                action();
        }


        // ============ 變數 ============
        /// <summary>
        /// 當前控制的機器人 ID (例如 "AMR_01")
        /// </summary>
        public MobileRobotVendor MRVendor { get; private set; } = MobileRobotVendor.GT;
        public string RobotId { get; private set; } = "AGV_001";
        public string ModuleName = "Mobile_Robot";
        private string Manufacturer = "GreenTrans";
        private string SerialNumber = "carter01";
        private string MapId = "L1";

        // ==========================================
        // Topic 定義
        // ==========================================
        public string TopicCommandMission => $"mrmp/{RobotId}/remote_command/mission";
        public string TopicReportMissionStatus => $"mrmp/{RobotId}/report/mission/status";
        public string TopicReportRobotLoc => $"mrmp/{RobotId}/report/robot/loc";

        private string TopicCommandOrder => $"uagv/v2/{Manufacturer}/{SerialNumber}/order";
        private string TopicCommandInstantActions => $"uagv/v2/{Manufacturer}/{SerialNumber}/instantActions";
        private string TopicReportState => $"uagv/v2/{Manufacturer}/{SerialNumber}/state";
        private string TopicReportVisualization => $"uagv/v2/{Manufacturer}/{SerialNumber}/visualization";
        private string TopicReportConnection => $"uagv/v2/{Manufacturer}/{SerialNumber}/connection";
        private string TopicReportFactsheet => $"uagv/v2/{Manufacturer}/{SerialNumber}/factsheet";



        // ==========================================
        // 對外提供的 Event (通知 Core)
        // ==========================================
        public event Action<MissionStatusReport> OnMissionStatusUpdated;
        public event Action<RobotLocationReport> OnRobotLocationUpdated;
        /// <summary>MR 即時姿態 (x, y, theta[rad])，來源為 VDA state.agvPosition</summary>
        public event Action<double, double, double> OnPoseUpdated;
        /// <summary>收到 VDA state (含 orderId / lastNodeId / nodeStates / driving，供 Core 判斷到點)</summary>
        public event Action<VdaStateMessage> OnVdaStateUpdated;
        /// <summary>UR Robot Alarm (Code, Description)</summary>
        public event AlarmEvent Alarm;
        /// <summary>UR Robot GoBarcode/Get End 回報 (Action, Target, Slot, Barcode1, Barcode2)</summary>
        public event UR_Robot.BarcodeReportEvent OnURBarcodeReported;

        public MRControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Core 引用此 UserControl 時的初始化方法
        /// </summary>
        /// <param name="_mqtt">MQTT 管理實例</param>
        /// <param name="robotId">機器人 ID</param>
        /// <param name="log">日誌實例 (可選)</param>
        public void Initial()
        {
            string MapPath = XML.Read("Setting", "Setting/AGVMap/MapPath", "GT_Map1104.txt");
            string LogPath = XML.Read("Setting", "Setting/Form1/Logfolder", "C:\\AML");
            int LogCounterLimit = Convert.ToUInt16(XML.Read("Setting", "Setting/Form1/LogCountLimit", "200"));
            int LogFolderLimit = Convert.ToUInt16(XML.Read("Setting", "Setting/Form1/LogFolderLimit", "30"));
            BrokerIP = XML.Read("Setting", "Setting/MQTT/BrokerIP", "127.0.0.1"); // 192.168.68.60
            BrokerPort = int.TryParse(XML.Read("Setting", "Setting/MQTT/BrokerPort", "1883"), out int port) ? port : 1883;
            ClientId = XML.Read("Setting", "Setting/MQTT/ClientId", "MR001");
            Manufacturer = XML.Read("Setting", "Setting/MQTT/Manufacturer", "GreenTrans");
            SerialNumber = XML.Read("Setting", "Setting/MQTT/SerialNumber", "carter01");

            // 初始化 Log（不輸出Console, 輸出檔案, 路徑, 單檔行數上限, 資料夾數量上限）
            Log.Initial(ModuleName, false, true, LogPath, LogCounterLimit, LogFolderLimit);

            // 每個 Topic 各自一個 Log 檔
            _topicLogs.Clear();
            foreach (var kv in new Dictionary<string, string>
            {
                { TopicReportMissionStatus, "MissionStatus" },
                { TopicReportRobotLoc, "RobotLoc" },
                { TopicReportState, "State" },
                { TopicReportVisualization, "Visualization" },
                { TopicReportConnection, "Connection" },
                { TopicReportFactsheet, "Factsheet" },
                { TopicCommandOrder, "Order" },
                { TopicCommandInstantActions, "InstantActions" },
            })
            {
                var topicLog = new AML_LogPage();
                topicLog.Initial($"{ModuleName}_{kv.Value}", false, true, LogPath, LogCounterLimit, LogFolderLimit);
                _topicLogs[kv.Key] = topicLog;
            }

            UpdateTopicStatus(false);
            mqttClient = new MqttClient();
            mqttClient.DataArrival += (topic, payload) => OnMqttMessageReceived(topic, payload);
            mqttClient.ConnectionChanged += (isConnected) => UpdateTopicStatus(isConnected);
            _ = MQTTClientConnectAsync(BrokerIP, BrokerPort, ClientId);

            urRobot1.Alarm += (code, desc) => Alarm?.Invoke(code, desc);
            urRobot1.BarcodeReport += URRobot_BarcodeReport;
            urRobot1.Initial();
        }

        private void URRobot_BarcodeReport(string Action, string Target, string Slot, string Barcode1, string Barcode2)
        {
            Log.WriteSystemLog(ModuleName, $"UR {Action} {Target},{Slot} Barcode1={Barcode1}, Barcode2={Barcode2}");
            OnURBarcodeReported?.Invoke(Action, Target, Slot, Barcode1, Barcode2);
        }

        private async Task MQTTClientConnectAsync(string brokerIP, int brokerPort, string clientId)
        {
            if (mqttClient == null) return;
            try
            {
                Log.WriteSystemLog($"Connecting to {brokerIP}:{brokerPort}...");
                if (!await mqttClient.ConnectAsync(brokerIP, brokerPort, clientId))
                {
                    Log.WriteSystemLog($"[MRControl] MQTT 連線失敗: {brokerIP}:{brokerPort}");
                    return;
                }
                await SubscribeRobotTopicsAsync();
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog($"[MRControl] MQTT 連線失敗: {ex.Message}");
            }
        }

        /// <summary>
        /// 訂閱此機器人的相關主題
        /// </summary>
        private async Task SubscribeRobotTopicsAsync()
        {
            if (mqttClient == null || !mqttClient.IsConnected) return;

            Log.WriteSystemLog($" MRControl 正在訂閱機器人 ({RobotId}) 主題...");

            if (MRVendor == MobileRobotVendor.GPM)
            {
                Connect = await mqttClient.SubscribeAsync(TopicReportMissionStatus) &&          // 訂閱 3.5.4 任務狀態回報
                          await mqttClient.SubscribeAsync(TopicReportRobotLoc);                 // 訂閱 3.5.2 機器人位置回報
            }
            else if (MRVendor == MobileRobotVendor.GT)
            {
                // 訂閱 GT 的相關主題 (例如 Order, State, Visualization 等)
                Connect = await mqttClient.SubscribeAsync(TopicReportState) &&
                          await mqttClient.SubscribeAsync(TopicReportVisualization) &&
                          await mqttClient.SubscribeAsync(TopicReportConnection) &&
                          await mqttClient.SubscribeAsync(TopicReportFactsheet);
            }
        }

        // ==========================================
        // 對 Core 開放的命令介面 (Command 3.2.2)
        // ==========================================
        /// <summary>
        /// GPM 下發任務給機器人 (3.2.2)
        public async Task<bool> SendMissionByPlanAsync(string missionId, List<MissionPlanItem> planList)
        {
            if (mqttClient == null)
            {
                Log.WriteSystemLog($"[MRControl] 發送失敗: MQTT 未初始化");
                return false;
            }

            var cmd = new MissionByPlanCommand
            {
                CommandType = "MissionByPlan",
                MissionId = missionId,
                MissionPlan = planList
            };

            string topic = TopicCommandMission;
            string payload = JsonConvert.SerializeObject(cmd, Formatting.None);

            Log.Write($"[MRControl] 下發任務: Topic={topic}, MissionId={missionId}, Planlist={JsonConvert.SerializeObject(planList, Formatting.None)}");
            return await mqttClient.SendAsync(topic, payload);
        }

        /// <summary>
        /// GT Command 放這裡，未來可擴充其他命令
        /// // ==========================================
        // VDA 5050 專用 Command 發送介面 (Order & InstantActions)
        // ==========================================

        /// <summary>
        /// 發送 VDA 5050 標準 Order 派車指令 (對應 TopicCommandOrder)
        /// 將內部的 RouteData 轉換為標準的 Node 與 Edge 陣列
        /// </summary>
        /// <param name="routeData">由 AGVMap 計算出的路徑資料</param>
        /// <returns>發送是否成功</returns>
        /// 應該要重寫 routeData 不是 GT 的 map 結果
        public async Task<bool> SendVdaOrderAsync(RouteData routeData)
        {
            if (mqttClient == null)
            {
                Log.WriteSystemLog($"[MRControl] 發送 Order 失敗: MQTT 未初始化");
                return false;
            }

            if (routeData == null || !routeData.Success || routeData.FullPath == null || routeData.FullPath.Count == 0)
            {
                Log.WriteSystemLog($"[MRControl] 發送 Order 失敗: 路線資料無效或為空");
                return false;
            }

            // 1. 建立標準 VDA 訂單外框
            var vdaOrder = new VdaOrderMessage
            {
                headerId = NextVdaHeaderId(),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                manufacturer = Manufacturer,
                serialNumber = SerialNumber,
                orderId = "ORD_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + System.Threading.Interlocked.Increment(ref _orderSeq),
                orderUpdateId = 0
            };

            int seqId = 0; // VDA 規定 sequenceId 必須從 0 開始遞增，Node 為偶數，Edge 為奇數

            // 2. 走訪內部計算好的 RouteData.FullPath 來產生 Nodes 與 Edges
            for (int i = 0; i < routeData.FullPath.Count; i++)
            {
                string currentNodeStr = routeData.FullPath[i].ToString();

                // 建立 Node (偶數 SequenceId)
                vdaOrder.nodes.Add(new VdaNode
                {
                    nodeId = currentNodeStr,
                    sequenceId = seqId++,
                    released = true // 授權車子可以駛入該節點
                });

                // 建立 Edge (奇數 SequenceId) - 連接當前節點與下一個節點
                if (i < routeData.FullPath.Count - 1)
                {
                    string nextNodeStr = routeData.FullPath[i + 1].ToString();

                    vdaOrder.edges.Add(new VdaEdge
                    {
                        edgeId = $"Edge_{currentNodeStr}_{nextNodeStr}",
                        sequenceId = seqId++,
                        released = true, // 授權車子可以行駛該路徑
                        startNodeId = currentNodeStr,
                        endNodeId = nextNodeStr
                    });
                }
            }

            string topic = TopicCommandOrder;
            string payload = JsonConvert.SerializeObject(vdaOrder, Formatting.None);

            Log.WriteSystemLog($"[MRControl] 發送 VDA Order: Topic={topic}, OrderId={vdaOrder.orderId}, 總節點數={vdaOrder.nodes.Count}");
            LogPayload("TX", topic, payload);
            return await mqttClient.SendAsync(topic, payload);
        }

        private int _vdaHeaderId = 0;

        private int NextVdaHeaderId()
        {
            return System.Threading.Interlocked.Increment(ref _vdaHeaderId);
        }

        /// <summary>
        /// 將 AGVMap 規劃出的 PathCoordinates (含繞牆轉折點) 轉為 VDA 5050 Order 並發送。
        /// 起點/終點 nodeId 為站點 Tag，中間轉折點為 WP_n，座標沿用地圖座標單位。
        /// </summary>
        public async Task<bool> SendVdaOrderAsync(MapRouteData route, double metersPerUnit = 1.0)
        {
            if (mqttClient == null)
            {
                Log.WriteSystemLog("[MRControl] 發送 Order 失敗: MQTT 未初始化");
                return false;
            }

            if (route == null || !route.Success || route.PathCoordinates == null || route.PathCoordinates.Count < 2)
            {
                Log.WriteSystemLog("[MRControl] 發送 Order 失敗: 路徑座標無效或為空");
                return false;
            }

            var coords = route.PathCoordinates;
            var vdaOrder = new VdaOrderMessage
            {
                headerId = NextVdaHeaderId(),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                manufacturer = Manufacturer,
                serialNumber = SerialNumber,
                orderId = "ORD_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                orderUpdateId = 0
            };

            var nodeIds = new List<string>();
            bool useCustomIds = route.PathNodeIds != null && route.PathNodeIds.Count == coords.Count;
            for (int i = 0; i < coords.Count; i++)
            {
                string id = useCustomIds && route.PathNodeIds[i] != null ? route.PathNodeIds[i]
                          : i == 0 ? (route.StartID > 0 ? route.StartID.ToString() : "START")
                          : i == coords.Count - 1 ? route.EndID.ToString()
                          : "WP_" + i;
                nodeIds.Add(id);
            }

            int seqId = 0; //  VDA 規定 sequenceId 必須從 0 開始遞增，Node 為偶數，Edge 為奇數
            bool hasNodeTheta = route.NodeThetas != null && route.NodeThetas.Count == coords.Count;
            for (int i = 0; i < coords.Count; i++)
            {
                // theta 取該點沿路徑的切線方向 (弧度)；最後一點沿用前一段方向，若操作者指定終點朝向則優先使用
                int a = i < coords.Count - 1 ? i : i - 1;
                double theta = Math.Atan2(coords[a + 1].Y - coords[a].Y, coords[a + 1].X - coords[a].X);
                bool hasEo = route.EdgeOrientations != null && route.EdgeOrientations.Count == coords.Count - 1;
                if (hasEo && route.EdgeOrientations[a].HasValue) theta = route.EdgeOrientations[a].Value;
                if (i == coords.Count - 1 && route.EndTheta.HasValue) theta = route.EndTheta.Value;
                if (hasNodeTheta && route.NodeThetas[i].HasValue) theta = route.NodeThetas[i].Value;

                vdaOrder.nodes.Add(new VdaNode
                {
                    nodeId = nodeIds[i],
                    sequenceId = seqId++,
                    released = true,
                    nodePosition = ToVdaPosition(coords[i], theta, metersPerUnit)
                });

                if (i < coords.Count - 1)
                {
                    double? eo = hasEo ? route.EdgeOrientations[i] : null;
                    if (eo.HasValue && _hasMapFrame) eo = NormalizeAngle(-eo.Value);
                    vdaOrder.edges.Add(new VdaEdge
                    {
                        edgeId = $"Edge_{nodeIds[i]}_{nodeIds[i + 1]}",
                        sequenceId = seqId++,
                        released = true,
                        startNodeId = nodeIds[i],
                        endNodeId = nodeIds[i + 1],
                        orientation = eo,
                        orientationType = eo.HasValue ? "GLOBAL" : null
                    });
                }
            }

            string payload = JsonConvert.SerializeObject(vdaOrder, Formatting.None);
            LogPayload("TX", TopicCommandOrder, payload);
            bool sent = await mqttClient.SendAsync(TopicCommandOrder, payload);
            if (sent)
            {
                LastOrderId = vdaOrder.orderId;
                LastOrderEndNodeId = nodeIds[nodeIds.Count - 1];
                _trackOrderId = vdaOrder.orderId;
                _orderAccepted = _orderStarted = _orderCompleted = false;
                _orderNodeCount = vdaOrder.nodes.Count;
                _orderSentTime = DateTime.Now;
                var end = vdaOrder.nodes[vdaOrder.nodes.Count - 1].nodePosition;
                Log.WriteSystemLog(ModuleName, $"送出 Order {vdaOrder.orderId}：{route.StartName} → {route.EndName}，節點 {_orderNodeCount}，終點 X={end.x:F3} m, Y={end.y:F3} m");
            }
            else
            {
                Log.WriteSystemLog(ModuleName, $"送出 Order {vdaOrder.orderId} 失敗 (MQTT)");
            }
            return sent;
        }

        // ==========================================
        // 地圖座標 → VDA 世界座標 (左下原點, m, theta ±pi)
        // ==========================================
        private bool _hasMapFrame;
        private double _mapMinX;
        private double _mapMaxY;
        private double _metersPerUnit = 1.0;

        /// <summary>最後一筆成功送出的 VDA orderId</summary>
        public string LastOrderId { get; private set; }
        /// <summary>最後一筆 Order 的終點 nodeId</summary>
        public string LastOrderEndNodeId { get; private set; }
        /// <summary>最新一筆 VDA state</summary>
        public VdaStateMessage LastState { get; private set; }

        /// <summary>設定地圖座標與世界座標的轉換 (地圖影像 Y 向下，世界座標原點在左下)</summary>
        public void SetMapFrame(double metersPerUnit, double mapMinX, double mapMaxY)
        {
            _metersPerUnit = metersPerUnit;
            _mapMinX = mapMinX;
            _mapMaxY = mapMaxY;
            _hasMapFrame = true;
        }

        private VdaNodePosition ToVdaPosition(PointF p, double mapTheta, double metersPerUnit)
        {
            if (!_hasMapFrame)
                return new VdaNodePosition { x = p.X * metersPerUnit, y = p.Y * metersPerUnit, theta = mapTheta, mapId = MapId };
            return new VdaNodePosition
            {
                x = (p.X - _mapMinX) * metersPerUnit,
                y = (_mapMaxY - p.Y) * metersPerUnit,
                theta = NormalizeAngle(-mapTheta),
                mapId = MapId
            };
        }

        private static double NormalizeAngle(double a)
        {
            while (a > Math.PI) a -= 2 * Math.PI;
            while (a < -Math.PI) a += 2 * Math.PI;
            return a;
        }

        /// <summary>
        /// Core RouteDispatch 事件處理：將路徑以 VDA 5050 Order 派送給 MR
        /// </summary>
        public async void DispatchRoute(MapRouteData route)
        {
            try
            {
                await SendVdaOrderAsync(route, _metersPerUnit);
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog(ModuleName, $"派送路徑例外: {ex.Message}");
            }
        }

        /// <summary>
        /// 將 JSON payload 縮排後寫入 log (多行，便於閱讀)；非 JSON 則原樣輸出。
        /// </summary>
        private void LogPayload(string direction, string topic, string payload)
        {
            if (!_topicLogs.TryGetValue(topic, out var topicLog)) return;
            string body;
            try
            {
                body = Newtonsoft.Json.Linq.JToken.Parse(payload).ToString(Formatting.Indented);
            }
            catch
            {
                body = payload;
            }
            topicLog.Write(direction, $"Topic={topic}{Environment.NewLine}{body}");
        }

        /// <summary>
        /// 發送 VDA 5050 標準 InstantActions 即時動作指令 (對應 TopicCommandInstantActions)
        /// </summary>
        /// <param name="actionId">動作唯一識別碼</param>
        /// <param name="actionType">動作類型 (如: cancelOrder, pause, resume)</param>
        /// <returns>發送是否成功</returns>
        public async Task<bool> SendVdaInstantActionsAsync(string actionId, string actionType)
        {
            if (mqttClient == null)
            {
                Log.WriteSystemLog($"[MRControl] 發送 InstantActions 失敗: MQTT 未初始化");
                return false;
            }

            // 使用強型別 VdaInstantActionsMessage
            var instantActionsCmd = new VdaInstantActionsMessage
            {
                headerId = NextVdaHeaderId(),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                manufacturer = Manufacturer,
                serialNumber = SerialNumber
            };

            // 加入動作
            instantActionsCmd.actions.Add(new VdaAction
            {
                actionId = actionId,
                actionType = actionType,
                blockingType = "HARD" // 即時動作通常不阻擋當前路徑運算，依需求可改為 HARD 或 SOFT
            });

            string topic = TopicCommandInstantActions;
            string payload = JsonConvert.SerializeObject(instantActionsCmd, Formatting.None);
            LogPayload("InstantActions", topic, payload);

            return await mqttClient.SendAsync(topic, payload);
        }

        // ==========================================
        // MQTT 訊息接收與路由處理 (Event 3.5.4 & 3.5.2)
        // ==========================================
        private void OnMqttMessageReceived(string topic, string payload)
        {
            //LogPayload("RX", topic, payload);
            try
            {
                // UI 跨執行緒安全呼叫
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => ProcessMessage(topic, payload)));
                }
                else
                {
                    ProcessMessage(topic, payload);
                }
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog($"[MRControl] 處理訊息例外: {ex.Message}");
            }
        }

        private readonly Dictionary<string, AML_LogPage> _topicLogs = new Dictionary<string, AML_LogPage>();

        private void ProcessMessage(string topic, string payload)
        {
            if (_topicLogs.TryGetValue(topic, out var topicLog))
            {
                string body;
                try { body = Newtonsoft.Json.Linq.JToken.Parse(payload).ToString(Formatting.Indented); }
                catch { body = payload; }
                topicLog.Write("RX", $"Topic={topic}{Environment.NewLine}{body}");
            }

            // 判斷是否為此機器人的主題
            switch (topic)
            {
                case string t when t == TopicReportMissionStatus:
                    var locationStatus = JsonConvert.DeserializeObject<MissionStatusReport>(payload);
                    if (locationStatus != null)
                    {
                        Log.WriteSystemLog(ModuleName, $"收到任務狀態: {locationStatus.MissionId}, 狀態碼 {locationStatus.MissionStatusCode}");
                        UpdateMissionStatusUI(locationStatus);
                        OnMissionStatusUpdated?.Invoke(locationStatus);
                    }
                    break;
                case string t when t == TopicReportRobotLoc:
                    var locationReport = JsonConvert.DeserializeObject<RobotLocationReport>(payload);
                    if (locationReport != null)
                    {
                        UpdateLocationUI(locationReport);
                        OnRobotLocationUpdated?.Invoke(locationReport);
                    }
                    break;
                case string t when t == TopicReportState:
                    var stateReport = JsonConvert.DeserializeObject<VdaStateMessage>(payload);
                    if (stateReport != null)
                    {
                        LastState = stateReport;
                        UpdateStateUI(stateReport);
                        TrackOrderState(stateReport);
                        OnVdaStateUpdated?.Invoke(stateReport);
                    }
                    break;
                case string t when t == TopicReportVisualization:
                    var visReport = JsonConvert.DeserializeObject<VdaVisualizationMessage>(payload);
                    if (visReport?.agvPosition != null)
                    {
                        Position_X = visReport.agvPosition.x;
                        Position_Y = visReport.agvPosition.y;
                        Theta = visReport.agvPosition.theta;
                        if (!string.IsNullOrEmpty(visReport.agvPosition.mapId))
                            lblMapIdVal.Text = visReport.agvPosition.mapId;
                        OnPoseUpdated?.Invoke(Position_X, Position_Y, Theta);
                    }
                    break;
                case string t when t == TopicReportConnection:
                    var connReport = JsonConvert.DeserializeObject<VdaConnectionMessage>(payload);
                    if (connReport != null)
                        UpdateConnectionUI(connReport);
                    break;
                case string t when t == TopicReportFactsheet:
                    UpdateFactsheetUI(payload);
                    break;
                default:
                    Log.WriteSystemLog($"[MRControl] 收到未知主題訊息: Topic={topic}, Payload={payload}");
                    return;
            }
        }

        // ==========================================
        // UI 更新方法
        // ==========================================
        private void UpdateMissionStatusUI(MissionStatusReport report)
        {
            lblMissionIdVal.Text = report.MissionId;
            lblMissionStatusVal.Text = $"{report.MissionStatusCode} ({report.MissionStatusMessage})";
            lblStartPointVal.Text = report.StartPoint ?? "-";
            lblCurrentTargetVal.Text = report.CurrentTarget ?? "-";
        }

        private void UpdateLocationUI(RobotLocationReport report)
        {
            lblPosXVal.Text = $"{report.RobotPosX:F2}";
            lblPosYVal.Text = $"{report.RobotPosY:F2}";
            lblThetaVal.Text = $"{report.RobotTheta:F1}°";
            lblMapIdVal.Text = report.MapId ?? "-";
            lblCurrentPointVal.Text = report.CurrentPoint ?? "-";
        }

        private void UpdateStateUI(VdaStateMessage stateMsg)
        {
            if (stateMsg?.agvPosition != null)
            {
                Position_X = stateMsg.agvPosition.x;
                Position_Y = stateMsg.agvPosition.y;
                Theta = stateMsg.agvPosition.theta; // 順便取得車頭角度（若需要）

                OnPoseUpdated?.Invoke(Position_X, Position_Y, Theta);

                lblMapIdVal.Text = string.IsNullOrEmpty(stateMsg.agvPosition.mapId) ? "-" : stateMsg.agvPosition.mapId;
            }
            if (stateMsg == null) return;

            // Mission / Order
            lblMissionIdVal.Text = string.IsNullOrEmpty(stateMsg.orderId) ? "-" : $"{stateMsg.orderId} (#{stateMsg.orderUpdateId})";
            lblMissionStatusVal.Text = $"{stateMsg.operatingMode} / {(stateMsg.paused ? "Paused" : stateMsg.driving ? "Driving" : "Idle")}";
            lblCurrentPointVal.Text = string.IsNullOrEmpty(stateMsg.lastNodeId) ? "-" : $"{stateMsg.lastNodeId} (seq {stateMsg.lastNodeSequenceId})";
            lblStartPointVal.Text = lblCurrentPointVal.Text;
            var nextNode = stateMsg.nodeStates != null && stateMsg.nodeStates.Count > 0 ? stateMsg.nodeStates[stateMsg.nodeStates.Count - 1] : null;
            lblCurrentTargetVal.Text = nextNode != null ? nextNode.nodeId : "-";

            // Velocity
            if (stateMsg.velocity != null)
            {
                lblLeftMotorTitle.Text = "Velocity :";
                lblLeftMotorVal.Text = $"vx {stateMsg.velocity.vx:F2} / vy {stateMsg.velocity.vy:F2} m/s";
                lblRightMotorTitle.Text = "Omega :";
                lblRightMotorVal.Text = $"{stateMsg.velocity.omega:F3} rad/s";
            }
            lblTemperatureTitle.Text = "Distance :";
            lblTemperatureVal.Text = $"{stateMsg.distanceSinceLastNode:F2} m (since last node)";

            // Battery
            if (stateMsg.batteryState != null)
            {
                var b = stateMsg.batteryState;
                lblBatteryVal.Text = $"{b.batteryCharge:F1} % / {b.batteryVoltage:F1} V / {b.current:F2} A / Health {b.batteryHealth} %";
                lblBatteryVal.ForeColor = b.batteryCharge < 20 ? Color.OrangeRed : b.batteryCharge < 50 ? Color.DarkOrange : Color.ForestGreen;
                lblChargingVal.Text = b.charging ? "● Charging" : "● Not Charging";
                lblChargingVal.ForeColor = b.charging ? Color.DodgerBlue : SystemColors.ControlText;
            }

            // Errors
            if (stateMsg.errors == null || stateMsg.errors.Count == 0)
            {
                lblErrorCodeVal.Text = "None";
                lblErrorCodeVal.ForeColor = Color.ForestGreen;
            }
            else
            {
                var e = stateMsg.errors[0];
                lblErrorCodeVal.Text = $"[{e.errorLevel}] {e.errorType}" + (stateMsg.errors.Count > 1 ? $" (+{stateMsg.errors.Count - 1})" : "");
                lblErrorCodeVal.ForeColor = e.errorLevel == "FATAL" ? Color.Red : Color.DarkOrange;
            }

            if (!string.IsNullOrEmpty(stateMsg.serialNumber))
                lblMrNumberVal.Text = $"{stateMsg.manufacturer} / {stateMsg.serialNumber}";
        }

        private void UpdateConnectionUI(VdaConnectionMessage conn)
        {
            string state = conn.connectionState ?? "-";
            lblTopicStatusVal.Text = $"MR {state}";
            lblTopicStatusVal.ForeColor = state == "ONLINE" ? Color.LimeGreen : Color.OrangeRed;
        }

        private void UpdateFactsheetUI(string payload)
        {
            try
            {
                var fs = Newtonsoft.Json.Linq.JObject.Parse(payload);
                string serial = (string)fs["serialNumber"];
                string manufacturer = (string)fs["manufacturer"];
                string series = (string)fs.SelectToken("typeSpecification.seriesName");
                if (!string.IsNullOrEmpty(serial))
                    lblMrNumberVal.Text = $"{manufacturer} / {serial}";
                string version = (string)fs["version"];
                lblFirmwareVal.Text = string.IsNullOrEmpty(series) ? (version ?? "-") : $"{series} (VDA {version})";
            }
            catch (Exception ex)
            {
                Log.WriteSystemLog($"[MRControl] Factsheet 解析失敗: {ex.Message}");
            }
        }

        // ==========================================
        // Order 生命週期追蹤：只在狀態轉換時寫 System log (即時座標等明細寫在 State topic log)
        // ==========================================
        private string _trackOrderId;
        private bool _orderAccepted;
        private bool _orderStarted;
        private bool _orderCompleted;
        private int _orderNodeCount;
        private DateTime _orderSentTime;

        /// <summary>目前 Order 完成 (MR 已到達終點並停止)，參數為 orderId</summary>
        public event Action<string> OnOrderCompleted;

        private void TrackOrderState(VdaStateMessage st)
        {
            if (string.IsNullOrEmpty(_trackOrderId) || _orderCompleted) return;
            if (st.orderId != _trackOrderId) return;

            int remain = st.nodeStates?.Count ?? 0;
            if (!_orderAccepted)
            {
                _orderAccepted = true;
                Log.WriteSystemLog(ModuleName, $"MR 已接收 Order {_trackOrderId}");
            }
            if (!_orderStarted && st.driving)
            {
                _orderStarted = true;
                Log.WriteSystemLog(ModuleName, $"MR 開始移動 (Order {_trackOrderId})");
            }
            if (remain == 0 && !st.driving)
            {
                _orderCompleted = true;
                double sec = (DateTime.Now - _orderSentTime).TotalSeconds;
                var p = st.agvPosition;
                string pos = p != null ? $"，位置 X={p.x:F3} m, Y={p.y:F3} m, θ={p.theta * 180.0 / Math.PI:F1}°" : "";
                Log.WriteSystemLog(ModuleName, $"MR 已完成 Order {_trackOrderId}，到達終點 {st.lastNodeId}，耗時 {sec:F1} s{pos}");
                OnOrderCompleted?.Invoke(_trackOrderId);
            }
        }

        private void UpdateTopicStatus(bool isConnected)
        {
            Connect = isConnected;
        }

        // ==========================================
        // 控制按鈕事件
        // ==========================================
        private void btnStart_Click(object sender, EventArgs e) => Log.WriteSystemLog("[UI] Start Clicked");
        private void btnPause_Click(object sender, EventArgs e) => Log.WriteSystemLog("[UI] Pause Clicked");
        private void btnResume_Click(object sender, EventArgs e) => Log.WriteSystemLog("[UI] Resume Clicked");
        private void btnStop_Click(object sender, EventArgs e) => Log.WriteSystemLog("[UI] Stop Clicked");
        private void btnReset_Click(object sender, EventArgs e) => Log.WriteSystemLog("[UI] Reset Clicked");
        private void btnHome_Click(object sender, EventArgs e) => Log.WriteSystemLog("[UI] Home Clicked");

        private async void BT_MqttReconnect_Click(object sender, EventArgs e)
        {
            Log.WriteOperationLog("點選 MR 重新連線...");
            await mqttClient.DisconnectAsync();
            await Task.Delay(1000); // 等待一秒再重新連線   
            await MQTTClientConnectAsync(BrokerIP, BrokerPort, ClientId);
        }

        private async void BT_MqttDisconnect_Click(object sender, EventArgs e)
        {
            Log.WriteOperationLog("點選 MR 切斷連線...");
            await mqttClient.DisconnectAsync();
        }

        private async void BT_OrderTest_Click(object sender, EventArgs e)
        {
            // 測試點位 (x, y, theta)，起點為目前 MR 位置，依序前往 P1~P5
            var waypoints = new List<(double x, double y, double theta)>
            {
                //(7.3795, 8.5448, 0.064), // Point 1
                //(9.6, 7.3, 1.57),        // Point 6
                (9.217, 12.05, 0),          // Point 3
                (12.988, 13.68, 1.70),   // Point 5
                (9.6, 7.3, -2.5),        // Point 6

                //(2.00, 3.00, 1.57)
            };
            var testOrder = BuildVdaOrder(waypoints);
            await SendTestOrderAsync(testOrder);
        }

        /// <summary>
        /// 以目前 MR 位置為起點 (Node 0)，依序加入多個點位，轉換成 VDA5050 Order。
        /// </summary>
        private VdaOrderMessage BuildVdaOrder(IList<(double x, double y, double theta)> points)
        {
            var order = new VdaOrderMessage
            {
                headerId = NextVdaHeaderId(),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                orderId = "TEST_" + DateTime.Now.ToString("HHmmss"),
                orderUpdateId = 0,
                manufacturer = Manufacturer,
                serialNumber = SerialNumber
            };

            order.nodes.Add(new VdaNode
            {
                nodeId = "Station_Start",
                sequenceId = 0,
                released = true,
                nodePosition = new VdaNodePosition { x = Position_X, y = Position_Y, theta = Theta, mapId = MapId }
                //nodePosition = new VdaNodePosition { x = 9.61, y = 6.31, theta = -2.71, mapId = MapId }
            });

            string prevNodeId = "Station_Start";
            int seq = 1;
            for (int i = 0; i < points.Count; i++)
            {
                string nodeId = $"P{i + 1}";
                string edgeId = $"Edge_{prevNodeId}_To_{nodeId}";

                order.edges.Add(new VdaEdge
                {
                    edgeId = edgeId,
                    sequenceId = seq++,
                    released = true,
                    startNodeId = prevNodeId,
                    endNodeId = nodeId
                });
                order.nodes.Add(new VdaNode
                {
                    nodeId = nodeId,
                    sequenceId = seq++,
                    released = true,
                    nodePosition = new VdaNodePosition { x = points[i].x, y = points[i].y, theta = points[i].theta, mapId = MapId }
                });
                prevNodeId = nodeId;
            }
            return order;
        }

        private async Task SendTestOrderAsync(VdaOrderMessage testOrder)
        {

            // 5. 序列化為 JSON 字串
            string payload = JsonConvert.SerializeObject(testOrder, Formatting.None);
            LogPayload("TX", TopicCommandOrder, payload);
            // 6. 透過 MQTT 發送
            if (mqttClient != null)
            {
                bool result = await mqttClient.SendAsync(TopicCommandOrder, payload);
                if (result)
                {
                    MessageBox.Show("完整的 Node-Edge-Node 派車指令發送成功！", "通知");
                }
                else
                {
                    MessageBox.Show("發送失敗，請檢查 MQTT 連線。", "錯誤");
                }
            }
        }

        private async void BT_InstantTest_Click(object sender, EventArgs e)
        {
            // 1. 動態產生一個獨立的 Action ID (VDA 要求每次發送的 Action ID 最好不重複)
            string testActionId = "ACT_" + DateTime.Now.ToString("HHmmss");

            // 2. 寫死你要測試的 VDA 動作類型 (例如取消任務)
            string testActionType = "cancelOrder"; // 你也可以改成 "pause" 或 "resume" 測試車子反應

            // 3. 呼叫發送 Function
            bool result = await SendVdaInstantActionsAsync(testActionId, testActionType);

            if (result)
            {
                MessageBox.Show($"InstantAction ({testActionType}) 指令發送成功！", "通知");
            }
            else
            {
                MessageBox.Show("InstantAction 指令發送失敗，請檢查 MQTT 連線。", "錯誤");
            }
        }
    }
}