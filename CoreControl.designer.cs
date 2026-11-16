namespace AML
{
    partial class CoreControl
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.GB_FoupList = new System.Windows.Forms.GroupBox();
            this.BT_RefreshFoupList = new System.Windows.Forms.Button();
            this.DGV_FoupList = new System.Windows.Forms.DataGridView();
            this.CoreTabControl = new System.Windows.Forms.TabControl();
            this.LotManageTabPage = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BT_RefreshCameraList = new System.Windows.Forms.Button();
            this.DGV_CameraList = new System.Windows.Forms.DataGridView();
            this.GB_CJList = new System.Windows.Forms.GroupBox();
            this.BT_DeleteCJList = new System.Windows.Forms.Button();
            this.BT_RefreshCJList = new System.Windows.Forms.Button();
            this.DGV_CJList = new System.Windows.Forms.DataGridView();
            this.AdjustmentTabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.BT_AdvisePressure = new System.Windows.Forms.Button();
            this.BT_SavePressure = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.AutoRun_DiffPressure = new System.Windows.Forms.TextBox();
            this.Manual_DiffPressure = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BT_AdviseEC = new System.Windows.Forms.Button();
            this.Humidity_textBox = new System.Windows.Forms.TextBox();
            this.BT_McuEC = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.L_DiffPressure_textBox = new System.Windows.Forms.TextBox();
            this.H_DiffPressure_textBox = new System.Windows.Forms.TextBox();
            this.GB_FoupList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_FoupList)).BeginInit();
            this.CoreTabControl.SuspendLayout();
            this.LotManageTabPage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CameraList)).BeginInit();
            this.GB_CJList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CJList)).BeginInit();
            this.AdjustmentTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_FoupList
            // 
            this.GB_FoupList.Controls.Add(this.BT_RefreshFoupList);
            this.GB_FoupList.Controls.Add(this.DGV_FoupList);
            this.GB_FoupList.Location = new System.Drawing.Point(13, 264);
            this.GB_FoupList.Name = "GB_FoupList";
            this.GB_FoupList.Size = new System.Drawing.Size(1700, 456);
            this.GB_FoupList.TabIndex = 6;
            this.GB_FoupList.TabStop = false;
            this.GB_FoupList.Text = "Foup List";
            // 
            // BT_RefreshFoupList
            // 
            this.BT_RefreshFoupList.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_RefreshFoupList.Location = new System.Drawing.Point(1566, 26);
            this.BT_RefreshFoupList.Name = "BT_RefreshFoupList";
            this.BT_RefreshFoupList.Size = new System.Drawing.Size(115, 30);
            this.BT_RefreshFoupList.TabIndex = 10;
            this.BT_RefreshFoupList.Text = "Refresh";
            this.BT_RefreshFoupList.UseVisualStyleBackColor = true;
            this.BT_RefreshFoupList.Click += new System.EventHandler(this.BT_RefreshFoupList_Click);
            // 
            // DGV_FoupList
            // 
            this.DGV_FoupList.AllowUserToAddRows = false;
            this.DGV_FoupList.AllowUserToDeleteRows = false;
            this.DGV_FoupList.AllowUserToResizeRows = false;
            this.DGV_FoupList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_FoupList.Location = new System.Drawing.Point(18, 60);
            this.DGV_FoupList.Name = "DGV_FoupList";
            this.DGV_FoupList.ReadOnly = true;
            this.DGV_FoupList.RowTemplate.Height = 24;
            this.DGV_FoupList.Size = new System.Drawing.Size(1663, 378);
            this.DGV_FoupList.TabIndex = 9;
            // 
            // CoreTabControl
            // 
            this.CoreTabControl.Controls.Add(this.LotManageTabPage);
            this.CoreTabControl.Controls.Add(this.AdjustmentTabPage);
            this.CoreTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CoreTabControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CoreTabControl.ItemSize = new System.Drawing.Size(200, 25);
            this.CoreTabControl.Location = new System.Drawing.Point(0, 0);
            this.CoreTabControl.Name = "CoreTabControl";
            this.CoreTabControl.SelectedIndex = 0;
            this.CoreTabControl.Size = new System.Drawing.Size(1734, 767);
            this.CoreTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.CoreTabControl.TabIndex = 8;
            // 
            // LotManageTabPage
            // 
            this.LotManageTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.LotManageTabPage.Controls.Add(this.groupBox3);
            this.LotManageTabPage.Controls.Add(this.GB_FoupList);
            this.LotManageTabPage.Controls.Add(this.GB_CJList);
            this.LotManageTabPage.Location = new System.Drawing.Point(4, 29);
            this.LotManageTabPage.Name = "LotManageTabPage";
            this.LotManageTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.LotManageTabPage.Size = new System.Drawing.Size(1726, 734);
            this.LotManageTabPage.TabIndex = 0;
            this.LotManageTabPage.Text = "Lot Manage";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BT_RefreshCameraList);
            this.groupBox3.Controls.Add(this.DGV_CameraList);
            this.groupBox3.Location = new System.Drawing.Point(873, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(840, 242);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Camera List";
            // 
            // BT_RefreshCameraList
            // 
            this.BT_RefreshCameraList.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_RefreshCameraList.Location = new System.Drawing.Point(633, 18);
            this.BT_RefreshCameraList.Name = "BT_RefreshCameraList";
            this.BT_RefreshCameraList.Size = new System.Drawing.Size(193, 30);
            this.BT_RefreshCameraList.TabIndex = 11;
            this.BT_RefreshCameraList.Text = "Refresh CameraList";
            this.BT_RefreshCameraList.UseVisualStyleBackColor = true;
            this.BT_RefreshCameraList.Click += new System.EventHandler(this.BT_RefreshCameraList_Click);
            // 
            // DGV_CameraList
            // 
            this.DGV_CameraList.AllowUserToAddRows = false;
            this.DGV_CameraList.AllowUserToDeleteRows = false;
            this.DGV_CameraList.AllowUserToResizeRows = false;
            this.DGV_CameraList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_CameraList.Location = new System.Drawing.Point(18, 52);
            this.DGV_CameraList.Name = "DGV_CameraList";
            this.DGV_CameraList.RowTemplate.Height = 24;
            this.DGV_CameraList.Size = new System.Drawing.Size(808, 172);
            this.DGV_CameraList.TabIndex = 8;
            // 
            // GB_CJList
            // 
            this.GB_CJList.Controls.Add(this.BT_DeleteCJList);
            this.GB_CJList.Controls.Add(this.BT_RefreshCJList);
            this.GB_CJList.Controls.Add(this.DGV_CJList);
            this.GB_CJList.Location = new System.Drawing.Point(13, 3);
            this.GB_CJList.Name = "GB_CJList";
            this.GB_CJList.Size = new System.Drawing.Size(840, 242);
            this.GB_CJList.TabIndex = 25;
            this.GB_CJList.TabStop = false;
            this.GB_CJList.Text = "CJ List";
            // 
            // BT_DeleteCJList
            // 
            this.BT_DeleteCJList.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_DeleteCJList.Location = new System.Drawing.Point(590, 18);
            this.BT_DeleteCJList.Name = "BT_DeleteCJList";
            this.BT_DeleteCJList.Size = new System.Drawing.Size(115, 30);
            this.BT_DeleteCJList.TabIndex = 12;
            this.BT_DeleteCJList.Text = "Delete";
            this.BT_DeleteCJList.UseVisualStyleBackColor = true;
            this.BT_DeleteCJList.Click += new System.EventHandler(this.BT_DeleteCJList_Click);
            // 
            // BT_RefreshCJList
            // 
            this.BT_RefreshCJList.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_RefreshCJList.Location = new System.Drawing.Point(711, 18);
            this.BT_RefreshCJList.Name = "BT_RefreshCJList";
            this.BT_RefreshCJList.Size = new System.Drawing.Size(115, 30);
            this.BT_RefreshCJList.TabIndex = 11;
            this.BT_RefreshCJList.Text = "Refresh";
            this.BT_RefreshCJList.UseVisualStyleBackColor = true;
            this.BT_RefreshCJList.Click += new System.EventHandler(this.BT_RefreshCJList_Click);
            // 
            // DGV_CJList
            // 
            this.DGV_CJList.AllowUserToAddRows = false;
            this.DGV_CJList.AllowUserToDeleteRows = false;
            this.DGV_CJList.AllowUserToResizeRows = false;
            this.DGV_CJList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_CJList.Location = new System.Drawing.Point(18, 52);
            this.DGV_CJList.Name = "DGV_CJList";
            this.DGV_CJList.ReadOnly = true;
            this.DGV_CJList.RowTemplate.Height = 24;
            this.DGV_CJList.Size = new System.Drawing.Size(808, 172);
            this.DGV_CJList.TabIndex = 8;
            // 
            // AdjustmentTabPage
            // 
            this.AdjustmentTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.AdjustmentTabPage.Controls.Add(this.groupBox2);
            this.AdjustmentTabPage.Controls.Add(this.groupBox1);
            this.AdjustmentTabPage.Location = new System.Drawing.Point(4, 29);
            this.AdjustmentTabPage.Name = "AdjustmentTabPage";
            this.AdjustmentTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AdjustmentTabPage.Size = new System.Drawing.Size(1726, 734);
            this.AdjustmentTabPage.TabIndex = 1;
            this.AdjustmentTabPage.Text = "Adjustment";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.BT_AdvisePressure);
            this.groupBox2.Controls.Add(this.BT_SavePressure);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.AutoRun_DiffPressure);
            this.groupBox2.Controls.Add(this.Manual_DiffPressure);
            this.groupBox2.Location = new System.Drawing.Point(599, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(511, 143);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "氣密籠車保壓閥值";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label8.Location = new System.Drawing.Point(343, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 18);
            this.label8.TabIndex = 13;
            this.label8.Text = "Pa";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label9.Location = new System.Drawing.Point(343, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 18);
            this.label9.TabIndex = 12;
            this.label9.Text = "Pa";
            // 
            // BT_AdvisePressure
            // 
            this.BT_AdvisePressure.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_AdvisePressure.Location = new System.Drawing.Point(382, 23);
            this.BT_AdvisePressure.Name = "BT_AdvisePressure";
            this.BT_AdvisePressure.Size = new System.Drawing.Size(75, 35);
            this.BT_AdvisePressure.TabIndex = 11;
            this.BT_AdvisePressure.Text = "修改";
            this.BT_AdvisePressure.UseVisualStyleBackColor = true;
            this.BT_AdvisePressure.Click += new System.EventHandler(this.BT_AdvisePressure_Click);
            // 
            // BT_SavePressure
            // 
            this.BT_SavePressure.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_SavePressure.Location = new System.Drawing.Point(382, 72);
            this.BT_SavePressure.Name = "BT_SavePressure";
            this.BT_SavePressure.Size = new System.Drawing.Size(75, 35);
            this.BT_SavePressure.TabIndex = 10;
            this.BT_SavePressure.Text = "Save";
            this.BT_SavePressure.UseVisualStyleBackColor = true;
            this.BT_SavePressure.Click += new System.EventHandler(this.BT_SavePressure_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label10.Location = new System.Drawing.Point(119, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 18);
            this.label10.TabIndex = 4;
            this.label10.Text = "Manual Run壓差:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label11.Location = new System.Drawing.Point(138, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(103, 18);
            this.label11.TabIndex = 5;
            this.label11.Text = "Auto Run壓差:";
            // 
            // AutoRun_DiffPressure
            // 
            this.AutoRun_DiffPressure.Enabled = false;
            this.AutoRun_DiffPressure.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.AutoRun_DiffPressure.Location = new System.Drawing.Point(251, 27);
            this.AutoRun_DiffPressure.Name = "AutoRun_DiffPressure";
            this.AutoRun_DiffPressure.Size = new System.Drawing.Size(88, 25);
            this.AutoRun_DiffPressure.TabIndex = 6;
            // 
            // Manual_DiffPressure
            // 
            this.Manual_DiffPressure.Enabled = false;
            this.Manual_DiffPressure.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.Manual_DiffPressure.Location = new System.Drawing.Point(251, 72);
            this.Manual_DiffPressure.Name = "Manual_DiffPressure";
            this.Manual_DiffPressure.Size = new System.Drawing.Size(88, 25);
            this.Manual_DiffPressure.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.BT_AdviseEC);
            this.groupBox1.Controls.Add(this.Humidity_textBox);
            this.groupBox1.Controls.Add(this.BT_McuEC);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.L_DiffPressure_textBox);
            this.groupBox1.Controls.Add(this.H_DiffPressure_textBox);
            this.groupBox1.Location = new System.Drawing.Point(23, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(511, 180);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "氣密籠車開蓋Alarm閥值";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label6.Location = new System.Drawing.Point(345, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label5.Location = new System.Drawing.Point(343, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "Pa";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label4.Location = new System.Drawing.Point(343, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Pa";
            // 
            // BT_AdviseEC
            // 
            this.BT_AdviseEC.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_AdviseEC.Location = new System.Drawing.Point(382, 43);
            this.BT_AdviseEC.Name = "BT_AdviseEC";
            this.BT_AdviseEC.Size = new System.Drawing.Size(75, 35);
            this.BT_AdviseEC.TabIndex = 11;
            this.BT_AdviseEC.Text = "修改";
            this.BT_AdviseEC.UseVisualStyleBackColor = true;
            this.BT_AdviseEC.Click += new System.EventHandler(this.BT_AdviseEC_Click);
            // 
            // Humidity_textBox
            // 
            this.Humidity_textBox.Enabled = false;
            this.Humidity_textBox.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.Humidity_textBox.Location = new System.Drawing.Point(251, 122);
            this.Humidity_textBox.Name = "Humidity_textBox";
            this.Humidity_textBox.Size = new System.Drawing.Size(88, 25);
            this.Humidity_textBox.TabIndex = 9;
            // 
            // BT_McuEC
            // 
            this.BT_McuEC.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_McuEC.Location = new System.Drawing.Point(382, 96);
            this.BT_McuEC.Name = "BT_McuEC";
            this.BT_McuEC.Size = new System.Drawing.Size(75, 35);
            this.BT_McuEC.TabIndex = 10;
            this.BT_McuEC.Text = "Save";
            this.BT_McuEC.UseVisualStyleBackColor = true;
            this.BT_McuEC.Click += new System.EventHandler(this.BT_McuEC_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label2.Location = new System.Drawing.Point(160, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "最高壓力差:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label1.Location = new System.Drawing.Point(160, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "最低壓力差:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label3.Location = new System.Drawing.Point(146, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "最大相對溼度:";
            // 
            // L_DiffPressure_textBox
            // 
            this.L_DiffPressure_textBox.Enabled = false;
            this.L_DiffPressure_textBox.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.L_DiffPressure_textBox.Location = new System.Drawing.Point(251, 27);
            this.L_DiffPressure_textBox.Name = "L_DiffPressure_textBox";
            this.L_DiffPressure_textBox.Size = new System.Drawing.Size(88, 25);
            this.L_DiffPressure_textBox.TabIndex = 6;
            // 
            // H_DiffPressure_textBox
            // 
            this.H_DiffPressure_textBox.Enabled = false;
            this.H_DiffPressure_textBox.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.H_DiffPressure_textBox.Location = new System.Drawing.Point(251, 72);
            this.H_DiffPressure_textBox.Name = "H_DiffPressure_textBox";
            this.H_DiffPressure_textBox.Size = new System.Drawing.Size(88, 25);
            this.H_DiffPressure_textBox.TabIndex = 7;
            // 
            // CoreControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CoreTabControl);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "CoreControl";
            this.Size = new System.Drawing.Size(1734, 767);
            this.GB_FoupList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_FoupList)).EndInit();
            this.CoreTabControl.ResumeLayout(false);
            this.LotManageTabPage.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CameraList)).EndInit();
            this.GB_CJList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_CJList)).EndInit();
            this.AdjustmentTabPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_FoupList;
        private System.Windows.Forms.DataGridView DGV_FoupList;
        public System.Windows.Forms.Button BT_RefreshFoupList;
        private System.Windows.Forms.TabPage LotManageTabPage;
        private System.Windows.Forms.TabPage AdjustmentTabPage;
        public System.Windows.Forms.TabControl CoreTabControl;
        private System.Windows.Forms.GroupBox GB_CJList;
        public System.Windows.Forms.Button BT_RefreshCJList;
        private System.Windows.Forms.DataGridView DGV_CJList;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.Button BT_RefreshCameraList;
        private System.Windows.Forms.DataGridView DGV_CameraList;
        public System.Windows.Forms.Button BT_DeleteCJList;
        private System.Windows.Forms.Button BT_McuEC;
        private System.Windows.Forms.TextBox Humidity_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox H_DiffPressure_textBox;
        private System.Windows.Forms.TextBox L_DiffPressure_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BT_AdviseEC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BT_AdvisePressure;
        private System.Windows.Forms.Button BT_SavePressure;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox AutoRun_DiffPressure;
        private System.Windows.Forms.TextBox Manual_DiffPressure;
    }
}
