using System.Collections.Generic;
using AML; // 假設 MapRouteData 定義在此命名空間

namespace AGVMap
{
    public interface IMapParser
    {
        /// <summary>
        /// 判斷這個 Parser 是否支援該 JSON 格式
        /// </summary>
        bool CanParse(string json);

        /// <summary>
        /// 解析 JSON 並將資料填入 MapDataManager
        /// </summary>
        bool Parse(string json, MapDataManager data);
    }

    public interface IPathPlanner
    {
        /// <summary>
        /// 規劃路徑，回傳標準 MapRouteData
        /// </summary>
        MapRouteData PlanRoute(string startId, string endId, string waypointId = null);
    }

    /// <summary>
    /// 支援依 MR 外形做碰撞檢查的規劃器
    /// </summary>
    public interface IFootprintPlanner
    {
        void SetFootprint(MrFootprint footprint);

        /// <summary>最近一次規劃失敗的原因</summary>
        string LastError { get; }

        /// <summary>最近一次失敗對應的點索引，無則為 -1</summary>
        int LastErrorPointIndex { get; }

        /// <summary>
        /// 依多個座標點規劃 (第一點為起點，最後一點為終點，中間為經過點)
        /// </summary>
        MapRouteData PlanFreeRoute(IList<RoutePoint> points);
    }

    /// <summary>
    /// 即時障礙來源 (預留，例如 VDA5050 Lidar)，回傳地圖 pixel 座標的障礙點
    /// </summary>
    public interface IObstacleSource
    {
        IEnumerable<System.Drawing.Point> GetObstaclePixels();
    }
}