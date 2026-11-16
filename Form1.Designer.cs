namespace AGVRoutePlanner
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMap = new System.Windows.Forms.Panel();
            this.panelControl = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.cboStart = new System.Windows.Forms.ComboBox();
            this.lblWaypoint = new System.Windows.Forms.Label();
            this.cboWaypoint = new System.Windows.Forms.ComboBox();
            this.chkWaypoint = new System.Windows.Forms.CheckBox();
            this.lblEnd = new System.Windows.Forms.Label();
            this.cboEnd = new System.Windows.Forms.ComboBox();
            this.btnPlan = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLoadMap = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.chkShowVirtual = new System.Windows.Forms.CheckBox();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();

            // panelMap
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(280, 0);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(820, 650);
            this.panelMap.TabIndex = 0;

            // panelControl
            this.panelControl.BackColor = System.Drawing.Color.FromArgb(35, 35, 40);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl.Width = 280;
            this.panelControl.Padding = new System.Windows.Forms.Padding(12);
            this.panelControl.Controls.Add(this.lblTitle);
            this.panelControl.Controls.Add(this.btnLoadMap);
            this.panelControl.Controls.Add(this.lblStart);
            this.panelControl.Controls.Add(this.cboStart);
            this.panelControl.Controls.Add(this.chkWaypoint);
            this.panelControl.Controls.Add(this.lblWaypoint);
            this.panelControl.Controls.Add(this.cboWaypoint);
            this.panelControl.Controls.Add(this.lblEnd);
            this.panelControl.Controls.Add(this.cboEnd);
            this.panelControl.Controls.Add(this.chkShowVirtual);
            this.panelControl.Controls.Add(this.btnPlan);
            this.panelControl.Controls.Add(this.btnClear);
            this.panelControl.Controls.Add(this.lblResult);
            this.panelControl.Controls.Add(this.txtResult);

            int yPos = 12;

            // lblTitle
            this.lblTitle.Text = "AGV 路線規劃工具";
            this.lblTitle.Font = new System.Drawing.Font("Microsoft JhengHei", 14f, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, yPos);
            this.lblTitle.AutoSize = true;
            yPos += 40;

            // btnLoadMap
            this.btnLoadMap.Text = "📂 載入地圖 JSON";
            this.btnLoadMap.Location = new System.Drawing.Point(12, yPos);
            this.btnLoadMap.Size = new System.Drawing.Size(252, 32);
            this.btnLoadMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadMap.BackColor = System.Drawing.Color.FromArgb(60, 60, 70);
            this.btnLoadMap.ForeColor = System.Drawing.Color.White;
            this.btnLoadMap.Font = new System.Drawing.Font("Microsoft JhengHei", 9.5f);
            yPos += 44;

            // lblStart
            this.lblStart.Text = "🟢 起始點";
            this.lblStart.ForeColor = System.Drawing.Color.Lime;
            this.lblStart.Font = new System.Drawing.Font("Microsoft JhengHei", 9.5f, System.Drawing.FontStyle.Bold);
            this.lblStart.Location = new System.Drawing.Point(12, yPos);
            this.lblStart.AutoSize = true;
            yPos += 22;

            // cboStart
            this.cboStart.Location = new System.Drawing.Point(12, yPos);
            this.cboStart.Size = new System.Drawing.Size(252, 26);
            this.cboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStart.Font = new System.Drawing.Font("Microsoft JhengHei", 9f);
            yPos += 36;

            // chkWaypoint
            this.chkWaypoint.Text = "啟用必經點";
            this.chkWaypoint.ForeColor = System.Drawing.Color.Gold;
            this.chkWaypoint.Font = new System.Drawing.Font("Microsoft JhengHei", 9f);
            this.chkWaypoint.Location = new System.Drawing.Point(12, yPos);
            this.chkWaypoint.Checked = true;
            this.chkWaypoint.AutoSize = true;
            yPos += 26;

            // lblWaypoint
            this.lblWaypoint.Text = "🟡 必經站點";
            this.lblWaypoint.ForeColor = System.Drawing.Color.Gold;
            this.lblWaypoint.Font = new System.Drawing.Font("Microsoft JhengHei", 9.5f, System.Drawing.FontStyle.Bold);
            this.lblWaypoint.Location = new System.Drawing.Point(12, yPos);
            this.lblWaypoint.AutoSize = true;
            yPos += 22;

            // cboWaypoint
            this.cboWaypoint.Location = new System.Drawing.Point(12, yPos);
            this.cboWaypoint.Size = new System.Drawing.Size(252, 26);
            this.cboWaypoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWaypoint.Font = new System.Drawing.Font("Microsoft JhengHei", 9f);
            yPos += 36;

            // lblEnd
            this.lblEnd.Text = "🔴 終點站";
            this.lblEnd.ForeColor = System.Drawing.Color.FromArgb(255, 80, 80);
            this.lblEnd.Font = new System.Drawing.Font("Microsoft JhengHei", 9.5f, System.Drawing.FontStyle.Bold);
            this.lblEnd.Location = new System.Drawing.Point(12, yPos);
            this.lblEnd.AutoSize = true;
            yPos += 22;

            // cboEnd
            this.cboEnd.Location = new System.Drawing.Point(12, yPos);
            this.cboEnd.Size = new System.Drawing.Size(252, 26);
            this.cboEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnd.Font = new System.Drawing.Font("Microsoft JhengHei", 9f);
            yPos += 36;

            // chkShowVirtual
            this.chkShowVirtual.Text = "下拉顯示虛擬點";
            this.chkShowVirtual.ForeColor = System.Drawing.Color.Gray;
            this.chkShowVirtual.Font = new System.Drawing.Font("Microsoft JhengHei", 8.5f);
            this.chkShowVirtual.Location = new System.Drawing.Point(12, yPos);
            this.chkShowVirtual.AutoSize = true;
            yPos += 30;

            // btnPlan
            this.btnPlan.Text = "▶ 開始規劃路線";
            this.btnPlan.Location = new System.Drawing.Point(12, yPos);
            this.btnPlan.Size = new System.Drawing.Size(252, 38);
            this.btnPlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlan.BackColor = System.Drawing.Color.FromArgb(0, 120, 200);
            this.btnPlan.ForeColor = System.Drawing.Color.White;
            this.btnPlan.Font = new System.Drawing.Font("Microsoft JhengHei", 11f, System.Drawing.FontStyle.Bold);
            yPos += 44;

            // btnClear
            this.btnClear.Text = "✕ 清除路線";
            this.btnClear.Location = new System.Drawing.Point(12, yPos);
            this.btnClear.Size = new System.Drawing.Size(252, 30);
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(80, 50, 50);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Font = new System.Drawing.Font("Microsoft JhengHei", 9f);
            yPos += 40;

            // lblResult
            this.lblResult.Text = "📋 規劃結果";
            this.lblResult.ForeColor = System.Drawing.Color.White;
            this.lblResult.Font = new System.Drawing.Font("Microsoft JhengHei", 9.5f, System.Drawing.FontStyle.Bold);
            this.lblResult.Location = new System.Drawing.Point(12, yPos);
            this.lblResult.AutoSize = true;
            yPos += 22;

            // txtResult
            this.txtResult.Location = new System.Drawing.Point(12, yPos);
            this.txtResult.Size = new System.Drawing.Size(252, 150);
            this.txtResult.Multiline = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.ReadOnly = true;
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(25, 25, 30);
            this.txtResult.ForeColor = System.Drawing.Color.FromArgb(200, 220, 200);
            this.txtResult.Font = new System.Drawing.Font("Consolas", 8.5f);
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Form1
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.panelControl);
            this.Text = "AGV 路線規劃工具 — 3F DemoRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.ComboBox cboStart;
        private System.Windows.Forms.Label lblWaypoint;
        private System.Windows.Forms.ComboBox cboWaypoint;
        private System.Windows.Forms.CheckBox chkWaypoint;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.ComboBox cboEnd;
        private System.Windows.Forms.Button btnPlan;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnLoadMap;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.CheckBox chkShowVirtual;
    }
}