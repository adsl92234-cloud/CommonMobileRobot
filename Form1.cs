using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AGVRoutePlanner
{
    public partial class Form1 : Form
    {
        private MapDataManager _mapData;
        private MapRenderer _renderer;
        private PathFinder _pathFinder;

        public Form1()
        {
            InitializeComponent();

            // 事件綑綁
            btnLoadMap.Click += BtnLoadMap_Click;
            btnPlan.Click += BtnPlan_Click;
            btnClear.Click += BtnClear_Click;
            chkWaypoint.CheckedChanged += ChkWaypoint_CheckedChanged;
            chkShowVirtual.CheckedChanged += ChkShowVirtual_CheckedChanged;
            panelMap.Paint += PanelMap_Paint;
            panelMap.Resize += (s, e) => panelMap.Invalidate();

            // 啟用雙緩衝
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, panelMap, new object[] { true });

            UpdateWaypointEnabled();

            // 嘗試自動載入同目錄下的 JSON
            TryAutoLoad();
        }

        private void TryAutoLoad()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            var jsonFiles = Directory.GetFiles(dir, "*.json");
            if (jsonFiles.Length > 0)
            {
                LoadMapFile(jsonFiles[0]);
            }
        }

        private void BtnLoadMap_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON 地圖檔 (*.json)|*.json|所有檔案 (*.*)|*.*";
                ofd.Title = "選擇 AGV 地圖 JSON 檔";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadMapFile(ofd.FileName);
                }
            }
        }

        private void LoadMapFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath, Encoding.UTF8);
                _mapData = new MapDataManager();
                _mapData.LoadFromJson(json);
                _pathFinder = new PathFinder(_mapData);
                _renderer = new MapRenderer(_mapData);

                PopulateComboBoxes();

                // ★ 改用詳細統計資訊
                txtResult.Text = _mapData.GetMapInfo();

                panelMap.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"地圖載入失敗:\r\n{ex.Message}\r\n\r\n{ex.StackTrace}",
                    "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateComboBoxes()
        {
            var points = chkShowVirtual.Checked
                ? _mapData.GetAllPoints()
                : _mapData.GetStationPoints();

            cboStart.DataSource = null;
            cboWaypoint.DataSource = null;
            cboEnd.DataSource = null;

            cboStart.DataSource = points.ToList();
            cboStart.DisplayMember = "ToString";
            cboStart.ValueMember = "ID";

            cboWaypoint.DataSource = points.ToList();
            cboWaypoint.DisplayMember = "ToString";
            cboWaypoint.ValueMember = "ID";

            cboEnd.DataSource = points.ToList();
            cboEnd.DisplayMember = "ToString";
            cboEnd.ValueMember = "ID";
        }

        private void ChkShowVirtual_CheckedChanged(object sender, EventArgs e)
        {
            if (_mapData != null)
                PopulateComboBoxes();
        }

        private void ChkWaypoint_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWaypointEnabled();
        }

        private void UpdateWaypointEnabled()
        {
            bool enabled = chkWaypoint.Checked;
            cboWaypoint.Enabled = enabled;
            lblWaypoint.Enabled = enabled;
        }

        private void BtnPlan_Click(object sender, EventArgs e)
        {
            if (_mapData == null || _pathFinder == null)
            {
                MessageBox.Show("請先載入地圖檔！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var startPt = cboStart.SelectedItem as MapPoint;
            var endPt = cboEnd.SelectedItem as MapPoint;
            var waypointPt = chkWaypoint.Checked ? cboWaypoint.SelectedItem as MapPoint : null;

            if (startPt == null || endPt == null)
            {
                MessageBox.Show("請選擇起點和終點！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int waypointId = waypointPt?.ID ?? -1;

            var result = _pathFinder.FindRoute(startPt.ID, waypointId, endPt.ID);

            if (!result.Success)
            {
                txtResult.Text = $"✘ 規劃失敗\r\n{result.ErrorMessage}";
                _renderer.ClearRoute();
                _renderer.HighlightStart = -1;
                _renderer.HighlightWaypoint = -1;
                _renderer.HighlightEnd = -1;
                panelMap.Invalidate();
                return;
            }

            // 更新繪製器
            _renderer.SetRoute(result);
            _renderer.HighlightStart = startPt.ID;
            _renderer.HighlightWaypoint = waypointId;
            _renderer.HighlightEnd = endPt.ID;

            // 顯示結果文字
            var sb = new StringBuilder();
            sb.AppendLine("✔ 路線規劃成功");
            sb.AppendLine($"總距離: {result.TotalDistance:F2} m");
            sb.AppendLine($"途經點數: {result.FullPath.Count}");
            sb.AppendLine("───────────────────");

            if (waypointId >= 0)
            {
                sb.AppendLine($"[段1] 起點→必經 ({result.DistanceToWaypoint:F2}m):");
                sb.AppendLine("  " + string.Join(" → ", result.PathToWaypoint.Select(id => GetPointLabel(id))));
                sb.AppendLine();
                sb.AppendLine($"[段2] 必經→終點 ({result.DistanceFromWaypoint:F2}m):");
                sb.AppendLine("  " + string.Join(" → ", result.PathFromWaypoint.Select(id => GetPointLabel(id))));
            }
            else
            {
                sb.AppendLine("完整路線:");
                sb.AppendLine("  " + string.Join(" → ", result.FullPath.Select(id => GetPointLabel(id))));
            }

            txtResult.Text = sb.ToString();
            panelMap.Invalidate();
        }

        private string GetPointLabel(int id)
        {
            if (_mapData.PointDict.TryGetValue(id, out var pt))
            {
                return string.IsNullOrEmpty(pt.Name) ? $"[{id}]" : $"{pt.Name}";
            }
            return $"[{id}]";
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (_renderer != null)
            {
                _renderer.ClearRoute();
                _renderer.HighlightStart = -1;
                _renderer.HighlightWaypoint = -1;
                _renderer.HighlightEnd = -1;
            }
            txtResult.Text = "";
            panelMap.Invalidate();
        }

        private void PanelMap_Paint(object sender, PaintEventArgs e)
        {
            if (_renderer != null)
            {
                _renderer.Render(e.Graphics, panelMap.Width, panelMap.Height);
            }
            else
            {
                // 未載入地圖時的提示
                using (var font = new Font("Microsoft JhengHei", 14f))
                using (var brush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.Clear(Color.FromArgb(30, 30, 35));
                    e.Graphics.DrawString("請點擊左側「載入地圖 JSON」開始",
                        font, brush,
                        new RectangleF(0, 0, panelMap.Width, panelMap.Height), sf);
                }
            }
        }
    }
}