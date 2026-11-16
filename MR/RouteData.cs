using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML
{
    public class RouteData
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
        public DateTime PlanTime { get; set; } = DateTime.Now;
        public string[] RouteArray { get; set; }

    }
}
