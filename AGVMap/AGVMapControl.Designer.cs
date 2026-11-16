namespace AGVMap
{
    partial class AGVMapControl
    {
        private System.ComponentModel.IContainer components = null;

        // ===== 工具列 =====
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.ComboBox cboStart;
        private System.Windows.Forms.Label lblWaypoint;
        private System.Windows.Forms.ComboBox cboWaypoint;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.ComboBox cboEnd;
        private System.Windows.Forms.Button btnPlan;
        private System.Windows.Forms.Button btnStart;

        // ===== 左側面板 =====
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.GroupBox grpDisplay;
        private System.Windows.Forms.CheckBox chkShowLabels;
        private System.Windows.Forms.CheckBox chkShowPaths;
        private System.Windows.Forms.CheckBox chkShowPolygons;
        private System.Windows.Forms.CheckBox chkShowLegend;
        private System.Windows.Forms.CheckBox chkShowVirtualPoints;
        private System.Windows.Forms.TextBox txtResult;

        // ===== 地圖區 =====
        private DoubleBufferedPanel panelMap;

        private void InitializeComponent()
        {
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPlan = new System.Windows.Forms.Button();
            this.cboEnd = new System.Windows.Forms.ComboBox();
            this.lblEnd = new System.Windows.Forms.Label();
            this.cboWaypoint = new System.Windows.Forms.ComboBox();
            this.lblWaypoint = new System.Windows.Forms.Label();
            this.cboStart = new System.Windows.Forms.ComboBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.grpDisplay = new System.Windows.Forms.GroupBox();
            this.chkShowVirtualPoints = new System.Windows.Forms.CheckBox();
            this.chkShowLegend = new System.Windows.Forms.CheckBox();
            this.chkShowPolygons = new System.Windows.Forms.CheckBox();
            this.chkShowPaths = new System.Windows.Forms.CheckBox();
            this.chkShowLabels = new System.Windows.Forms.CheckBox();
            this.panelMap = new AGVMap.DoubleBufferedPanel();
            this.panelToolbar.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.grpDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.btnStart);
            this.panelToolbar.Controls.Add(this.btnPlan);
            this.panelToolbar.Controls.Add(this.cboEnd);
            this.panelToolbar.Controls.Add(this.lblEnd);
            this.panelToolbar.Controls.Add(this.cboWaypoint);
            this.panelToolbar.Controls.Add(this.lblWaypoint);
            this.panelToolbar.Controls.Add(this.cboStart);
            this.panelToolbar.Controls.Add(this.lblStart);
            this.panelToolbar.Controls.Add(this.btnLoad);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.panelToolbar.Size = new System.Drawing.Size(1281, 55);
            this.panelToolbar.TabIndex = 2;
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStart.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btnStart.Location = new System.Drawing.Point(826, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(110, 39);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "開始導航";
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnPlan
            // 
            this.btnPlan.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPlan.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btnPlan.Location = new System.Drawing.Point(716, 10);
            this.btnPlan.Name = "btnPlan";
            this.btnPlan.Size = new System.Drawing.Size(110, 39);
            this.btnPlan.TabIndex = 1;
            this.btnPlan.Text = "規劃路線";
            this.btnPlan.Click += new System.EventHandler(this.BtnPlan_Click);
            // 
            // cboEnd
            // 
            this.cboEnd.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnd.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.cboEnd.Location = new System.Drawing.Point(576, 10);
            this.cboEnd.Name = "cboEnd";
            this.cboEnd.Size = new System.Drawing.Size(140, 28);
            this.cboEnd.TabIndex = 2;
            // 
            // lblEnd
            // 
            this.lblEnd.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblEnd.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lblEnd.Location = new System.Drawing.Point(516, 10);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(60, 39);
            this.lblEnd.TabIndex = 3;
            this.lblEnd.Text = " 終點:";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboWaypoint
            // 
            this.cboWaypoint.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboWaypoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWaypoint.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.cboWaypoint.Location = new System.Drawing.Point(376, 10);
            this.cboWaypoint.Name = "cboWaypoint";
            this.cboWaypoint.Size = new System.Drawing.Size(140, 28);
            this.cboWaypoint.TabIndex = 4;
            // 
            // lblWaypoint
            // 
            this.lblWaypoint.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblWaypoint.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lblWaypoint.Location = new System.Drawing.Point(316, 10);
            this.lblWaypoint.Name = "lblWaypoint";
            this.lblWaypoint.Size = new System.Drawing.Size(60, 39);
            this.lblWaypoint.TabIndex = 5;
            this.lblWaypoint.Text = " 中繼:";
            this.lblWaypoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboStart
            // 
            this.cboStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStart.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.cboStart.Location = new System.Drawing.Point(176, 10);
            this.cboStart.Name = "cboStart";
            this.cboStart.Size = new System.Drawing.Size(140, 28);
            this.cboStart.TabIndex = 6;
            // 
            // lblStart
            // 
            this.lblStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStart.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lblStart.Location = new System.Drawing.Point(116, 10);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(60, 39);
            this.lblStart.TabIndex = 7;
            this.lblStart.Text = " 起點:";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLoad.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btnLoad.Location = new System.Drawing.Point(6, 10);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(110, 39);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "載入地圖";
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.txtResult);
            this.panelLeft.Controls.Add(this.grpDisplay);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 55);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(4);
            this.panelLeft.Size = new System.Drawing.Size(260, 645);
            this.panelLeft.TabIndex = 1;
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtResult.ForeColor = System.Drawing.Color.White;
            this.txtResult.Location = new System.Drawing.Point(4, 204);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(252, 437);
            this.txtResult.TabIndex = 0;
            // 
            // grpDisplay
            // 
            this.grpDisplay.Controls.Add(this.chkShowVirtualPoints);
            this.grpDisplay.Controls.Add(this.chkShowLegend);
            this.grpDisplay.Controls.Add(this.chkShowPolygons);
            this.grpDisplay.Controls.Add(this.chkShowPaths);
            this.grpDisplay.Controls.Add(this.chkShowLabels);
            this.grpDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDisplay.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.grpDisplay.Location = new System.Drawing.Point(4, 4);
            this.grpDisplay.Name = "grpDisplay";
            this.grpDisplay.Padding = new System.Windows.Forms.Padding(10);
            this.grpDisplay.Size = new System.Drawing.Size(252, 200);
            this.grpDisplay.TabIndex = 1;
            this.grpDisplay.TabStop = false;
            this.grpDisplay.Text = "顯示選項";
            // 
            // chkShowVirtualPoints
            // 
            this.chkShowVirtualPoints.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkShowVirtualPoints.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.chkShowVirtualPoints.Location = new System.Drawing.Point(10, 152);
            this.chkShowVirtualPoints.Name = "chkShowVirtualPoints";
            this.chkShowVirtualPoints.Size = new System.Drawing.Size(232, 30);
            this.chkShowVirtualPoints.TabIndex = 0;
            this.chkShowVirtualPoints.Text = "虛擬點";
            this.chkShowVirtualPoints.CheckedChanged += new System.EventHandler(this.DisplayOption_Changed);
            // 
            // chkShowLegend
            // 
            this.chkShowLegend.Checked = true;
            this.chkShowLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLegend.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkShowLegend.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.chkShowLegend.Location = new System.Drawing.Point(10, 122);
            this.chkShowLegend.Name = "chkShowLegend";
            this.chkShowLegend.Size = new System.Drawing.Size(232, 30);
            this.chkShowLegend.TabIndex = 1;
            this.chkShowLegend.Text = "圖例";
            this.chkShowLegend.CheckedChanged += new System.EventHandler(this.DisplayOption_Changed);
            // 
            // chkShowPolygons
            // 
            this.chkShowPolygons.Checked = true;
            this.chkShowPolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPolygons.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkShowPolygons.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.chkShowPolygons.Location = new System.Drawing.Point(10, 92);
            this.chkShowPolygons.Name = "chkShowPolygons";
            this.chkShowPolygons.Size = new System.Drawing.Size(232, 30);
            this.chkShowPolygons.TabIndex = 2;
            this.chkShowPolygons.Text = "區域 (多邊形)";
            this.chkShowPolygons.CheckedChanged += new System.EventHandler(this.DisplayOption_Changed);
            // 
            // chkShowPaths
            // 
            this.chkShowPaths.Checked = true;
            this.chkShowPaths.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPaths.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkShowPaths.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.chkShowPaths.Location = new System.Drawing.Point(10, 62);
            this.chkShowPaths.Name = "chkShowPaths";
            this.chkShowPaths.Size = new System.Drawing.Size(232, 30);
            this.chkShowPaths.TabIndex = 3;
            this.chkShowPaths.Text = "路徑連線";
            this.chkShowPaths.CheckedChanged += new System.EventHandler(this.DisplayOption_Changed);
            // 
            // chkShowLabels
            // 
            this.chkShowLabels.Checked = true;
            this.chkShowLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLabels.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkShowLabels.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.chkShowLabels.Location = new System.Drawing.Point(10, 32);
            this.chkShowLabels.Name = "chkShowLabels";
            this.chkShowLabels.Size = new System.Drawing.Size(232, 30);
            this.chkShowLabels.TabIndex = 4;
            this.chkShowLabels.Text = "點位標籤 (Tag)";
            this.chkShowLabels.CheckedChanged += new System.EventHandler(this.DisplayOption_Changed);
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(260, 55);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(1021, 645);
            this.panelMap.TabIndex = 0;
            // 
            // AGVMapControl
            // 
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelToolbar);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.Name = "AGVMapControl";
            this.Size = new System.Drawing.Size(1281, 700);
            this.panelToolbar.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.grpDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}