using System;
using System.Collections.Generic;
using System.Drawing; // 新增，為了使用 PointF
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML
{
    public class MapRouteData
    {
        public bool Success { get; set; }
        public int StartID { get; set; }
        public int EndID { get; set; }
        public int WaypointID { get; set; } = -1;
        public string StartName { get; set; }
        public string EndName { get; set; }
        public double TotalDistance { get; set; }
        public List<int> FullPath { get; set; } = new List<int>();
        public List<string> FullPathNames { get; set; } = new List<string>();
        
        // ==== 新增：提供給 Core 轉換 VDA5050 的實體座標路徑 ====
        public List<PointF> PathCoordinates { get; set; } = new List<PointF>();

        /// <summary>與 PathCoordinates 一一對應的 VDA nodeId，null 表示使用預設命名 (Free 模式使用)。</summary>
        public List<string> PathNodeIds { get; set; }

        /// <summary>操作者指定的終點朝向 (弧度)，未指定為 null。</summary>
        public double? EndTheta { get; set; }

        /// <summary>每段 edge 的車頭朝向 (弧度，地圖座標 GLOBAL)，長度為 PathCoordinates.Count-1；null 表示不指定。倒車時朝向與行進方向相反。</summary>
        public List<double?> EdgeOrientations { get; set; }

        /// <summary>每個節點的車頭朝向 (弧度，地圖座標)，長度為 PathCoordinates.Count；null 表示依 edge 方向推算。由 RouteTurnRefiner 產生。</summary>
        public List<double?> NodeThetas { get; set; }

        public DateTime PlanTime { get; set; } = DateTime.Now;
        public string[] RouteArray { get; set; }
    }
}
