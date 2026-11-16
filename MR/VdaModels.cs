using System;
using System.Collections.Generic;

namespace MobileRobot
{
    // VDA 5050 狀態回報結構
    public class VdaStateMessage
    {
        public int headerId { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; } = "2.0.0";
        public string manufacturer { get; set; }
        public string serialNumber { get; set; }
        public string orderId { get; set; } = "";
        public int orderUpdateId { get; set; }
        public string zoneSetId { get; set; } = "";
        public bool newBaseRequest { get; set; }
        public string lastNodeId { get; set; } = "";
        public int lastNodeSequenceId { get; set; }
        public bool driving { get; set; }
        public bool paused { get; set; }
        public string operatingMode { get; set; } = "AUTOMATIC";
        public double distanceSinceLastNode { get; set; }
        public List<VdaNodeState> nodeStates { get; set; } = new List<VdaNodeState>();
        public List<VdaEdgeState> edgeStates { get; set; } = new List<VdaEdgeState>();
        public List<VdaActionState> actionStates { get; set; } = new List<VdaActionState>();
        public List<VdaError> errors { get; set; } = new List<VdaError>();
        public SafetyState safetyState { get; set; }
        public AgvPosition agvPosition { get; set; }
        public VdaVelocity velocity { get; set; }
        public BatteryState batteryState { get; set; }
    }

    public class VdaNodeState
    {
        public string nodeId { get; set; }
        public int sequenceId { get; set; }
        public bool released { get; set; }
        public VdaNodePosition nodePosition { get; set; }
    }

    public class VdaEdgeState
    {
        public string edgeId { get; set; }
        public int sequenceId { get; set; }
        public bool released { get; set; }
    }

    public class VdaActionState
    {
        public string actionId { get; set; }
        public string actionType { get; set; }
        /// <summary>WAITING / INITIALIZING / RUNNING / PAUSED / FINISHED / FAILED</summary>
        public string actionStatus { get; set; }
        public string resultDescription { get; set; }
    }

    public class VdaError
    {
        public string errorType { get; set; }
        /// <summary>WARNING / FATAL</summary>
        public string errorLevel { get; set; }
        public string errorDescription { get; set; }
    }

    public class VdaVelocity
    {
        public double vx { get; set; }
        public double vy { get; set; }
        public double omega { get; set; }
    }

    // 模擬用 Lidar 回報 (自訂 topic: uagv/v2/{manufacturer}/{serialNumber}/lidar，欄位沿用 VDA header 風格)
    public class VdaLidarMessage
    {
        public int headerId { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; } = "2.0.0";
        public string manufacturer { get; set; }
        public string serialNumber { get; set; }
        public string mapId { get; set; }
        /// <summary>量測當下 MR 估測位姿 (m, m, rad)</summary>
        public AgvPosition sensorPose { get; set; }
        public double angleMin { get; set; }
        public double angleMax { get; set; }
        public double angleIncrement { get; set; }
        public double rangeMin { get; set; }
        public double rangeMax { get; set; }
        /// <summary>單位 m，解析度 0.1 mm；超出量測距離為 rangeMax</summary>
        public List<double> ranges { get; set; } = new List<double>();
    }

    public class SafetyState
    {
        public string eStop { get; set; }
        public bool fieldViolation { get; set; }
    }

    public class AgvPosition
    {
        public double x { get; set; }
        public double y { get; set; }
        public double theta { get; set; }
        public string mapId { get; set; }
        public bool positionInitialized { get; set; } = true;
        public double localizationScore { get; set; } = 1.0;
        public double deviationRange { get; set; }
        public string mapDescription { get; set; }
    }

    public class BatteryState
    {
        public double batteryCharge { get; set; }
        public double batteryVoltage { get; set; }
        public int batteryHealth { get; set; }
        public bool charging { get; set; }
        public double reach { get; set; }
        public double current { get; set; }
    }

    // VDA 5050 connection topic
    public class VdaConnectionMessage
    {
        public int headerId { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; }
        public string manufacturer { get; set; }
        public string serialNumber { get; set; }
        /// <summary>ONLINE / OFFLINE / CONNECTIONBROKEN</summary>
        public string connectionState { get; set; }
    }

    // VDA 5050 visualization topic
    public class VdaVisualizationMessage
    {
        public int headerId { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; }
        public string manufacturer { get; set; }
        public string serialNumber { get; set; }
        public AgvPosition agvPosition { get; set; }
        public VdaVelocity velocity { get; set; }
    }

    // VDA 5050 派車指令結構
    public class VdaOrderMessage
    {
        public int headerId { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; } = "2.0.0";
        public string manufacturer { get; set; }
        public string serialNumber { get; set; }
        public string orderId { get; set; }
        public int orderUpdateId { get; set; }
        public List<VdaNode> nodes { get; set; } = new List<VdaNode>();
        public List<VdaEdge> edges { get; set; } = new List<VdaEdge>();
    }

    public class VdaNode
    {
        public string nodeId { get; set; }
        public int sequenceId { get; set; }
        public bool released { get; set; }
        public List<VdaAction> actions { get; set; } = new List<VdaAction>();
        public VdaNodePosition nodePosition { get; set; }
    }

    public class VdaEdge
    {
        public string edgeId { get; set; }
        public int sequenceId { get; set; }
        public bool released { get; set; }
        public string startNodeId { get; set; }
        public string endNodeId { get; set; }

        // 車頭朝向 (弧度)；GLOBAL 時為地圖座標，倒車時與行進方向相反。未指定時不輸出。
        [Newtonsoft.Json.JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double? orientation { get; set; }
        [Newtonsoft.Json.JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string orientationType { get; set; }
        public List<VdaAction> actions { get; set; } = new List<VdaAction>();
    }
    // ==========================================
    // VDA 5050 即時動作指令 (InstantActions)
    // ==========================================
    public class VdaInstantActionsMessage
    {
        public int headerId { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; } = "2.0.0";
        public string manufacturer { get; set; }
        public string serialNumber { get; set; }
        public List<VdaAction> actions { get; set; } = new List<VdaAction>();
    }

    // VDA 5050 標準動作結構 (Node 抵達動作與 InstantAction 共用)
    public class VdaAction
    {
        public string actionId { get; set; }
        public string actionType { get; set; }
        public string actionDescription { get; set; } = "";

        // VDA 5050 阻擋類型：NONE (不阻擋), SOFT (軟阻擋), HARD (硬阻擋)
        public string blockingType { get; set; } = "HARD";

        public List<VdaActionParameter> actionParameters { get; set; } = new List<VdaActionParameter>();
    }

    // 動作夾帶的參數
    public class VdaActionParameter
    {
        public string key { get; set; }
        public object value { get; set; }
    }
    public class VdaNodePosition
    {
        private double _x, _y, _theta;

        public double x { get { return _x; } set { _x = Round3(value); } }
        public double y { get { return _y; } set { _y = Round3(value); } }
        public double theta { get { return _theta; } set { _theta = Round3(value); } }
        public string mapId { get; set; }

        private static double Round3(double v)
        {
            return Math.Round(v, 3, MidpointRounding.AwayFromZero);
        }

        // 容許
        //public double? allowedDeviationXY { get; set; }
        //public double? allowedDeviationTheta { get; set; }
    }
}