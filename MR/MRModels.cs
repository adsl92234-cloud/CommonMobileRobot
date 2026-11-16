using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileRobot
{
    #region ================= 3.2.2 任務下發 (Mission by Plan) =================

    /// <summary>
    /// 3.2.2 任務計畫中的單一步驟項目
    /// </summary>
    public class MissionPlanItem
    {
        /// <summary>
        /// 步驟類型："station" (站點), "recipe" (腳本), "action" (動作)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 站點名稱、腳本名稱或動作名稱
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// 附加參數 (可選，例如速度限制等)
        /// </summary>
        [JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Parameters { get; set; }
    }

    /// <summary>
    /// 3.2.2 任務下發命令本體
    /// Topic: mrmp/{robot_id}/remote_command/mission
    /// </summary>
    public class MissionByPlanCommand
    {
        /// <summary>
        /// 固定為 "MissionByPlan"
        /// </summary>
        [JsonProperty("command_type")]
        public string CommandType { get; set; } = "MissionByPlan";

        /// <summary>
        /// 任務唯一識別碼
        /// </summary>
        [JsonProperty("mission_id")]
        public string MissionId { get; set; }

        /// <summary>
        /// 依序執行的步驟清單
        /// </summary>
        [JsonProperty("mission_plan")]
        public List<MissionPlanItem> MissionPlan { get; set; } = new List<MissionPlanItem>();
    }

    #endregion

    #region ================= 3.5.4 任務狀態回報 (Mission Status Report) =================

    /// <summary>
    /// 3.5.4 任務狀態回報
    /// Topic: mrmp/{robot_id}/report/mission/status
    /// </summary>
    public class MissionStatusReport
    {
        /// <summary>
        /// 任務 ID
        /// </summary>
        [JsonProperty("mission_id")]
        public string MissionId { get; set; }

        /// <summary>
        /// 任務狀態碼 (0: IDLE, 1: PENDING, 2: STARTED, 3: EXECUTING, 5: COMPLETED, 6: FAILED 等)
        /// </summary>
        [JsonProperty("mission_status_code")]
        public int MissionStatusCode { get; set; }

        /// <summary>
        /// 狀態文字描述
        /// </summary>
        [JsonProperty("mission_status_message")]
        public string MissionStatusMessage { get; set; }

        /// <summary>
        /// 起始點 ID / 名稱
        /// </summary>
        [JsonProperty("start_point")]
        public string StartPoint { get; set; }

        /// <summary>
        /// 當前目標點 ID
        /// </summary>
        [JsonProperty("current_target")]
        public string CurrentTarget { get; set; }

        /// <summary>
        /// 預計剩餘時間 (單位：分鐘)
        /// </summary>
        [JsonProperty("remaining_time")]
        public double? RemainingTime { get; set; }
    }

    #endregion

    #region ================= 3.5.2 機器人位置回報 (Robot Location Report) =================

    /// <summary>
    /// 3.5.2 機器人位置回報
    /// Topic: mrmp/{robot_id}/report/robot/loc
    /// </summary>
    public class RobotLocationReport
    {
        /// <summary>
        /// X 座標
        /// </summary>
        [JsonProperty("robot_pos_x")]
        public double RobotPosX { get; set; }

        /// <summary>
        /// Y 座標
        /// </summary>
        [JsonProperty("robot_pos_y")]
        public double RobotPosY { get; set; }

        /// <summary>
        /// 朝向角度 (0 ~ 360)
        /// </summary>
        [JsonProperty("robot_theta")]
        public double RobotTheta { get; set; }

        /// <summary>
        /// 當前地圖 ID
        /// </summary>
        [JsonProperty("map_id")]
        public string MapId { get; set; }

        /// <summary>
        /// 當前所在站點 / 拓撲點 ID
        /// </summary>
        [JsonProperty("current_point")]
        public string CurrentPoint { get; set; }
    }

    #endregion
}