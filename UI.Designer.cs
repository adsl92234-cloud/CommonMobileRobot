namespace AML
{
    partial class UI
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI));
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.ButtonSplit = new System.Windows.Forms.SplitContainer();
            this.MainSplit = new System.Windows.Forms.SplitContainer();
            this.ControlModeSplit = new System.Windows.Forms.SplitContainer();
            this.StatusSplit = new System.Windows.Forms.SplitContainer();
            this.lblControlMode = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.SystemLogOnTop = new AML.AML_LogPage();
            this.ToolBarSplit = new System.Windows.Forms.SplitContainer();
            this.SystemPanel = new System.Windows.Forms.TabControl();
            this.tabLogin = new System.Windows.Forms.TabPage();
            this.LoginSplit = new System.Windows.Forms.SplitContainer();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.LGAccessTab = new System.Windows.Forms.TabControl();
            this.Guest = new System.Windows.Forms.TabPage();
            this.LGLoginMessage = new System.Windows.Forms.Label();
            this.LGLoginGB = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.LGAccount = new System.Windows.Forms.TextBox();
            this.LGPassword = new System.Windows.Forms.TextBox();
            this.LGLoginBtn = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.Optab = new System.Windows.Forms.TabControl();
            this.PageMon = new System.Windows.Forms.TabPage();
            this.panelMonitorRobot = new System.Windows.Forms.Panel();
            this.lblRobotIcon = new System.Windows.Forms.Label();
            this.lblRobotName = new System.Windows.Forms.Label();
            this.panelMonitorStatus = new System.Windows.Forms.Panel();
            this.gbBattery = new System.Windows.Forms.GroupBox();
            this.progressBattery = new System.Windows.Forms.ProgressBar();
            this.lblBatteryPercent = new System.Windows.Forms.Label();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblCurrentLocation = new System.Windows.Forms.Label();
            this.lblCurrentLocationValue = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblSpeedValue = new System.Windows.Forms.Label();
            this.gbDispatchList = new System.Windows.Forms.GroupBox();
            this.dgvDispatchList = new System.Windows.Forms.DataGridView();
            this.colTaskID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbPlannedRoute = new System.Windows.Forms.GroupBox();
            this.lstPlannedRoute = new System.Windows.Forms.ListBox();
            this.PageOp = new System.Windows.Forms.TabPage();
            this.btnCycle = new System.Windows.Forms.Button();
            this.panelOpCenter = new System.Windows.Forms.Panel();
            this.lblOpRobotIcon = new System.Windows.Forms.Label();
            this.lblOpRobotLabel = new System.Windows.Forms.Label();
            this.btnMRDetail = new System.Windows.Forms.Button();
            this.btnNavigation = new System.Windows.Forms.Button();
            this.btnInterrupt = new System.Windows.Forms.Button();
            this.btnCharging = new System.Windows.Forms.Button();
            this.btnDispatch = new System.Windows.Forms.Button();
            this.btnEmergencyStop = new System.Windows.Forms.Button();
            this.btnManualMove = new System.Windows.Forms.Button();
            this.btnPayloadControl = new System.Windows.Forms.Button();
            this.btnDiagnostics = new System.Windows.Forms.Button();
            this.PageRobotDetail = new System.Windows.Forms.TabPage();
            this.mrControl1 = new MobileRobot.MRControl();
            this.PageDispatchDetail = new System.Windows.Forms.TabPage();
            this.agvMapControl1 = new AGVMap.AGVMapControl();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LogListSplit = new System.Windows.Forms.SplitContainer();
            this.LogRefreshBtn = new System.Windows.Forms.Button();
            this.LogOpenFolderBtn = new System.Windows.Forms.Button();
            this.GB_LogDataLoad = new System.Windows.Forms.GroupBox();
            this.HistorylogEnd = new System.Windows.Forms.DateTimePicker();
            this.HistorylogStart = new System.Windows.Forms.DateTimePicker();
            this.LabelLogDueDate = new System.Windows.Forms.Label();
            this.LabelLogDateFrom = new System.Windows.Forms.Label();
            this.LogTree = new System.Windows.Forms.TreeView();
            this.Log = new AML.AML_LogTab();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.SettingSplit = new System.Windows.Forms.SplitContainer();
            this.Core = new AML.CoreControl();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.TimeSplit = new System.Windows.Forms.SplitContainer();
            this.ShowTime = new System.Windows.Forms.TextBox();
            this.LOGOSplit = new System.Windows.Forms.SplitContainer();
            this.TopRightLogo = new System.Windows.Forms.Panel();
            this.AMLLogo = new System.Windows.Forms.PictureBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.BtExit = new System.Windows.Forms.Button();
            this.BtAbout = new System.Windows.Forms.Button();
            this.BtLogin = new System.Windows.Forms.Button();
            this.BtSetting = new System.Windows.Forms.Button();
            this.BtHistory = new System.Windows.Forms.Button();
            this.BtRefreshSEQ = new System.Windows.Forms.Button();
            this.BtOperation = new System.Windows.Forms.Button();
            this.BtMonitor = new System.Windows.Forms.Button();
            this.trash = new AML.CoreControl();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonSplit)).BeginInit();
            this.ButtonSplit.Panel1.SuspendLayout();
            this.ButtonSplit.Panel2.SuspendLayout();
            this.ButtonSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
            this.MainSplit.Panel1.SuspendLayout();
            this.MainSplit.Panel2.SuspendLayout();
            this.MainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ControlModeSplit)).BeginInit();
            this.ControlModeSplit.Panel1.SuspendLayout();
            this.ControlModeSplit.Panel2.SuspendLayout();
            this.ControlModeSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatusSplit)).BeginInit();
            this.StatusSplit.Panel1.SuspendLayout();
            this.StatusSplit.Panel2.SuspendLayout();
            this.StatusSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ToolBarSplit)).BeginInit();
            this.ToolBarSplit.Panel1.SuspendLayout();
            this.ToolBarSplit.Panel2.SuspendLayout();
            this.ToolBarSplit.SuspendLayout();
            this.SystemPanel.SuspendLayout();
            this.tabLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoginSplit)).BeginInit();
            this.LoginSplit.Panel1.SuspendLayout();
            this.LoginSplit.Panel2.SuspendLayout();
            this.LoginSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.LGAccessTab.SuspendLayout();
            this.Guest.SuspendLayout();
            this.LGLoginGB.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.Optab.SuspendLayout();
            this.PageMon.SuspendLayout();
            this.panelMonitorRobot.SuspendLayout();
            this.panelMonitorStatus.SuspendLayout();
            this.gbBattery.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.gbDispatchList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDispatchList)).BeginInit();
            this.gbPlannedRoute.SuspendLayout();
            this.PageOp.SuspendLayout();
            this.panelOpCenter.SuspendLayout();
            this.PageRobotDetail.SuspendLayout();
            this.PageDispatchDetail.SuspendLayout();
            this.tabLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogListSplit)).BeginInit();
            this.LogListSplit.Panel1.SuspendLayout();
            this.LogListSplit.Panel2.SuspendLayout();
            this.LogListSplit.SuspendLayout();
            this.GB_LogDataLoad.SuspendLayout();
            this.tabSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingSplit)).BeginInit();
            this.SettingSplit.Panel2.SuspendLayout();
            this.SettingSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeSplit)).BeginInit();
            this.TimeSplit.Panel1.SuspendLayout();
            this.TimeSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LOGOSplit)).BeginInit();
            this.LOGOSplit.Panel1.SuspendLayout();
            this.LOGOSplit.Panel2.SuspendLayout();
            this.LOGOSplit.SuspendLayout();
            this.TopRightLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AMLLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer1
            // 
            this.Timer1.Interval = 1000;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // ButtonSplit
            // 
            this.ButtonSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ButtonSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.ButtonSplit.IsSplitterFixed = true;
            this.ButtonSplit.Location = new System.Drawing.Point(0, 0);
            this.ButtonSplit.Name = "ButtonSplit";
            // 
            // ButtonSplit.Panel1
            // 
            this.ButtonSplit.Panel1.Controls.Add(this.MainSplit);
            // 
            // ButtonSplit.Panel2
            // 
            this.ButtonSplit.Panel2.Controls.Add(this.LOGOSplit);
            this.ButtonSplit.Size = new System.Drawing.Size(1920, 1080);
            this.ButtonSplit.SplitterDistance = 1750;
            this.ButtonSplit.TabIndex = 0;
            // 
            // MainSplit
            // 
            this.MainSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.MainSplit.IsSplitterFixed = true;
            this.MainSplit.Location = new System.Drawing.Point(0, 0);
            this.MainSplit.Name = "MainSplit";
            this.MainSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplit.Panel1
            // 
            this.MainSplit.Panel1.Controls.Add(this.ControlModeSplit);
            // 
            // MainSplit.Panel2
            // 
            this.MainSplit.Panel2.Controls.Add(this.ToolBarSplit);
            this.MainSplit.Size = new System.Drawing.Size(1750, 1080);
            this.MainSplit.SplitterDistance = 120;
            this.MainSplit.TabIndex = 0;
            // 
            // ControlModeSplit
            // 
            this.ControlModeSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ControlModeSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlModeSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ControlModeSplit.IsSplitterFixed = true;
            this.ControlModeSplit.Location = new System.Drawing.Point(0, 0);
            this.ControlModeSplit.Name = "ControlModeSplit";
            // 
            // ControlModeSplit.Panel1
            // 
            this.ControlModeSplit.Panel1.Controls.Add(this.StatusSplit);
            // 
            // ControlModeSplit.Panel2
            // 
            this.ControlModeSplit.Panel2.Controls.Add(this.SystemLogOnTop);
            this.ControlModeSplit.Size = new System.Drawing.Size(1750, 120);
            this.ControlModeSplit.SplitterDistance = 350;
            this.ControlModeSplit.TabIndex = 0;
            // 
            // StatusSplit
            // 
            this.StatusSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StatusSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusSplit.Location = new System.Drawing.Point(0, 0);
            this.StatusSplit.Name = "StatusSplit";
            this.StatusSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // StatusSplit.Panel1
            // 
            this.StatusSplit.Panel1.Controls.Add(this.lblControlMode);
            // 
            // StatusSplit.Panel2
            // 
            this.StatusSplit.Panel2.Controls.Add(this.lblConnectionStatus);
            this.StatusSplit.Size = new System.Drawing.Size(350, 120);
            this.StatusSplit.SplitterDistance = 55;
            this.StatusSplit.TabIndex = 0;
            // 
            // lblControlMode
            // 
            this.lblControlMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblControlMode.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblControlMode.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblControlMode.Location = new System.Drawing.Point(0, 0);
            this.lblControlMode.Name = "lblControlMode";
            this.lblControlMode.Size = new System.Drawing.Size(346, 51);
            this.lblControlMode.TabIndex = 0;
            this.lblControlMode.Text = "Mode: Local";
            this.lblControlMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnectionStatus.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectionStatus.Location = new System.Drawing.Point(0, 0);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(346, 57);
            this.lblConnectionStatus.TabIndex = 0;
            this.lblConnectionStatus.Text = "● Disconnected";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SystemLogOnTop
            // 
            this.SystemLogOnTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SystemLogOnTop.FolderLimit = 0;
            this.SystemLogOnTop.FolderPath = null;
            this.SystemLogOnTop.Location = new System.Drawing.Point(0, 0);
            this.SystemLogOnTop.LogLimit = 0;
            this.SystemLogOnTop.LogName = null;
            this.SystemLogOnTop.Name = "SystemLogOnTop";
            this.SystemLogOnTop.ReadOnly = true;
            this.SystemLogOnTop.SimpleTime = false;
            this.SystemLogOnTop.Size = new System.Drawing.Size(1392, 116);
            this.SystemLogOnTop.TabIndex = 0;
            this.SystemLogOnTop.Text = "";
            this.SystemLogOnTop.WriteLogFile = false;
            // 
            // ToolBarSplit
            // 
            this.ToolBarSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ToolBarSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolBarSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.ToolBarSplit.IsSplitterFixed = true;
            this.ToolBarSplit.Location = new System.Drawing.Point(0, 0);
            this.ToolBarSplit.Name = "ToolBarSplit";
            this.ToolBarSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ToolBarSplit.Panel1
            // 
            this.ToolBarSplit.Panel1.Controls.Add(this.SystemPanel);
            // 
            // ToolBarSplit.Panel2
            // 
            this.ToolBarSplit.Panel2.Controls.Add(this.TimeSplit);
            this.ToolBarSplit.Size = new System.Drawing.Size(1750, 956);
            this.ToolBarSplit.SplitterDistance = 922;
            this.ToolBarSplit.TabIndex = 0;
            // 
            // SystemPanel
            // 
            this.SystemPanel.Controls.Add(this.tabLogin);
            this.SystemPanel.Controls.Add(this.tabMain);
            this.SystemPanel.Controls.Add(this.tabLog);
            this.SystemPanel.Controls.Add(this.tabHistory);
            this.SystemPanel.Controls.Add(this.tabSetting);
            this.SystemPanel.Controls.Add(this.tabAbout);
            this.SystemPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SystemPanel.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.SystemPanel.ItemSize = new System.Drawing.Size(100, 29);
            this.SystemPanel.Location = new System.Drawing.Point(0, 0);
            this.SystemPanel.Name = "SystemPanel";
            this.SystemPanel.SelectedIndex = 0;
            this.SystemPanel.Size = new System.Drawing.Size(1746, 918);
            this.SystemPanel.TabIndex = 0;
            // 
            // tabLogin
            // 
            this.tabLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.tabLogin.Controls.Add(this.LoginSplit);
            this.tabLogin.Location = new System.Drawing.Point(4, 33);
            this.tabLogin.Name = "tabLogin";
            this.tabLogin.Size = new System.Drawing.Size(1738, 881);
            this.tabLogin.TabIndex = 0;
            this.tabLogin.Text = "Login";
            // 
            // LoginSplit
            // 
            this.LoginSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoginSplit.IsSplitterFixed = true;
            this.LoginSplit.Location = new System.Drawing.Point(0, 0);
            this.LoginSplit.Name = "LoginSplit";
            // 
            // LoginSplit.Panel1
            // 
            this.LoginSplit.Panel1.Controls.Add(this.picLogo);
            // 
            // LoginSplit.Panel2
            // 
            this.LoginSplit.Panel2.Controls.Add(this.LGAccessTab);
            this.LoginSplit.Size = new System.Drawing.Size(1738, 881);
            this.LoginSplit.SplitterDistance = 998;
            this.LoginSplit.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(264, 100);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(500, 500);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // LGAccessTab
            // 
            this.LGAccessTab.Controls.Add(this.Guest);
            this.LGAccessTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LGAccessTab.Location = new System.Drawing.Point(0, 0);
            this.LGAccessTab.Name = "LGAccessTab";
            this.LGAccessTab.SelectedIndex = 0;
            this.LGAccessTab.Size = new System.Drawing.Size(736, 881);
            this.LGAccessTab.TabIndex = 0;
            // 
            // Guest
            // 
            this.Guest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.Guest.Controls.Add(this.LGLoginMessage);
            this.Guest.Controls.Add(this.LGLoginGB);
            this.Guest.Location = new System.Drawing.Point(4, 29);
            this.Guest.Name = "Guest";
            this.Guest.Size = new System.Drawing.Size(728, 848);
            this.Guest.TabIndex = 0;
            this.Guest.Text = "Guest";
            // 
            // LGLoginMessage
            // 
            this.LGLoginMessage.Font = new System.Drawing.Font("微軟正黑體", 16F);
            this.LGLoginMessage.Location = new System.Drawing.Point(147, 219);
            this.LGLoginMessage.Name = "LGLoginMessage";
            this.LGLoginMessage.Size = new System.Drawing.Size(400, 28);
            this.LGLoginMessage.TabIndex = 0;
            this.LGLoginMessage.Text = "Please Login";
            // 
            // LGLoginGB
            // 
            this.LGLoginGB.Controls.Add(this.label23);
            this.LGLoginGB.Controls.Add(this.label24);
            this.LGLoginGB.Controls.Add(this.LGAccount);
            this.LGLoginGB.Controls.Add(this.LGPassword);
            this.LGLoginGB.Controls.Add(this.LGLoginBtn);
            this.LGLoginGB.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.LGLoginGB.Location = new System.Drawing.Point(152, 260);
            this.LGLoginGB.Name = "LGLoginGB";
            this.LGLoginGB.Size = new System.Drawing.Size(414, 235);
            this.LGLoginGB.TabIndex = 0;
            this.LGLoginGB.TabStop = false;
            this.LGLoginGB.Text = "Account";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(23, 39);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(93, 20);
            this.label23.TabIndex = 0;
            this.label23.Text = "User Name";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(23, 103);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(80, 20);
            this.label24.TabIndex = 1;
            this.label24.Text = "Password";
            // 
            // LGAccount
            // 
            this.LGAccount.Location = new System.Drawing.Point(27, 62);
            this.LGAccount.MaxLength = 40;
            this.LGAccount.Name = "LGAccount";
            this.LGAccount.Size = new System.Drawing.Size(364, 29);
            this.LGAccount.TabIndex = 1;
            // 
            // LGPassword
            // 
            this.LGPassword.Location = new System.Drawing.Point(27, 126);
            this.LGPassword.MaxLength = 12;
            this.LGPassword.Name = "LGPassword";
            this.LGPassword.PasswordChar = '*';
            this.LGPassword.Size = new System.Drawing.Size(364, 29);
            this.LGPassword.TabIndex = 2;
            this.LGPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LGPassword_KeyDown);
            // 
            // LGLoginBtn
            // 
            this.LGLoginBtn.Location = new System.Drawing.Point(27, 176);
            this.LGLoginBtn.Name = "LGLoginBtn";
            this.LGLoginBtn.Size = new System.Drawing.Size(364, 34);
            this.LGLoginBtn.TabIndex = 3;
            this.LGLoginBtn.Text = "Login";
            this.LGLoginBtn.UseVisualStyleBackColor = true;
            this.LGLoginBtn.Click += new System.EventHandler(this.LGLoginBtn_Click);
            // 
            // tabMain
            // 
            this.tabMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.tabMain.Controls.Add(this.Optab);
            this.tabMain.Location = new System.Drawing.Point(4, 33);
            this.tabMain.Name = "tabMain";
            this.tabMain.Size = new System.Drawing.Size(1738, 881);
            this.tabMain.TabIndex = 1;
            this.tabMain.Text = "Main";
            // 
            // Optab
            // 
            this.Optab.Controls.Add(this.PageMon);
            this.Optab.Controls.Add(this.PageOp);
            this.Optab.Controls.Add(this.PageRobotDetail);
            this.Optab.Controls.Add(this.PageDispatchDetail);
            this.Optab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Optab.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.Optab.Location = new System.Drawing.Point(0, 0);
            this.Optab.Name = "Optab";
            this.Optab.SelectedIndex = 0;
            this.Optab.Size = new System.Drawing.Size(1738, 881);
            this.Optab.TabIndex = 0;
            // 
            // PageMon
            // 
            this.PageMon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.PageMon.Controls.Add(this.panelMonitorRobot);
            this.PageMon.Controls.Add(this.panelMonitorStatus);
            this.PageMon.Location = new System.Drawing.Point(4, 29);
            this.PageMon.Name = "PageMon";
            this.PageMon.Size = new System.Drawing.Size(1730, 848);
            this.PageMon.TabIndex = 0;
            this.PageMon.Text = "Monitor";
            // 
            // panelMonitorRobot
            // 
            this.panelMonitorRobot.BackColor = System.Drawing.Color.White;
            this.panelMonitorRobot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMonitorRobot.Controls.Add(this.lblRobotIcon);
            this.panelMonitorRobot.Controls.Add(this.lblRobotName);
            this.panelMonitorRobot.Location = new System.Drawing.Point(10, 10);
            this.panelMonitorRobot.Name = "panelMonitorRobot";
            this.panelMonitorRobot.Size = new System.Drawing.Size(900, 828);
            this.panelMonitorRobot.TabIndex = 0;
            // 
            // lblRobotIcon
            // 
            this.lblRobotIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 120F);
            this.lblRobotIcon.Location = new System.Drawing.Point(300, 200);
            this.lblRobotIcon.Name = "lblRobotIcon";
            this.lblRobotIcon.Size = new System.Drawing.Size(300, 250);
            this.lblRobotIcon.TabIndex = 0;
            this.lblRobotIcon.Text = "🤖";
            this.lblRobotIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRobotName
            // 
            this.lblRobotName.Font = new System.Drawing.Font("微軟正黑體", 20F, System.Drawing.FontStyle.Bold);
            this.lblRobotName.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblRobotName.Location = new System.Drawing.Point(250, 460);
            this.lblRobotName.Name = "lblRobotName";
            this.lblRobotName.Size = new System.Drawing.Size(400, 40);
            this.lblRobotName.TabIndex = 0;
            this.lblRobotName.Text = "Mobile Robot - AMR01";
            this.lblRobotName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMonitorStatus
            // 
            this.panelMonitorStatus.Controls.Add(this.gbBattery);
            this.panelMonitorStatus.Controls.Add(this.gbStatus);
            this.panelMonitorStatus.Controls.Add(this.gbDispatchList);
            this.panelMonitorStatus.Controls.Add(this.gbPlannedRoute);
            this.panelMonitorStatus.Location = new System.Drawing.Point(920, 10);
            this.panelMonitorStatus.Name = "panelMonitorStatus";
            this.panelMonitorStatus.Size = new System.Drawing.Size(800, 828);
            this.panelMonitorStatus.TabIndex = 0;
            // 
            // gbBattery
            // 
            this.gbBattery.Controls.Add(this.progressBattery);
            this.gbBattery.Controls.Add(this.lblBatteryPercent);
            this.gbBattery.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gbBattery.Location = new System.Drawing.Point(5, 5);
            this.gbBattery.Name = "gbBattery";
            this.gbBattery.Size = new System.Drawing.Size(785, 80);
            this.gbBattery.TabIndex = 0;
            this.gbBattery.TabStop = false;
            this.gbBattery.Text = "🔋 Battery";
            // 
            // progressBattery
            // 
            this.progressBattery.Location = new System.Drawing.Point(15, 35);
            this.progressBattery.Name = "progressBattery";
            this.progressBattery.Size = new System.Drawing.Size(620, 30);
            this.progressBattery.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBattery.TabIndex = 0;
            this.progressBattery.Value = 85;
            // 
            // lblBatteryPercent
            // 
            this.lblBatteryPercent.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold);
            this.lblBatteryPercent.ForeColor = System.Drawing.Color.Green;
            this.lblBatteryPercent.Location = new System.Drawing.Point(645, 30);
            this.lblBatteryPercent.Name = "lblBatteryPercent";
            this.lblBatteryPercent.Size = new System.Drawing.Size(130, 35);
            this.lblBatteryPercent.TabIndex = 0;
            this.lblBatteryPercent.Text = "85 %";
            this.lblBatteryPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.lblStatus);
            this.gbStatus.Controls.Add(this.lblStatusValue);
            this.gbStatus.Controls.Add(this.lblCurrentLocation);
            this.gbStatus.Controls.Add(this.lblCurrentLocationValue);
            this.gbStatus.Controls.Add(this.lblSpeed);
            this.gbStatus.Controls.Add(this.lblSpeedValue);
            this.gbStatus.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gbStatus.Location = new System.Drawing.Point(5, 90);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(785, 130);
            this.gbStatus.TabIndex = 0;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "📊 Status";
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lblStatus.Location = new System.Drawing.Point(15, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(150, 25);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Robot State :";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold);
            this.lblStatusValue.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblStatusValue.Location = new System.Drawing.Point(170, 28);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(200, 25);
            this.lblStatusValue.TabIndex = 1;
            this.lblStatusValue.Text = "Idle";
            // 
            // lblCurrentLocation
            // 
            this.lblCurrentLocation.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lblCurrentLocation.Location = new System.Drawing.Point(15, 62);
            this.lblCurrentLocation.Name = "lblCurrentLocation";
            this.lblCurrentLocation.Size = new System.Drawing.Size(150, 25);
            this.lblCurrentLocation.TabIndex = 2;
            this.lblCurrentLocation.Text = "Location :";
            // 
            // lblCurrentLocationValue
            // 
            this.lblCurrentLocationValue.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrentLocationValue.Location = new System.Drawing.Point(170, 62);
            this.lblCurrentLocationValue.Name = "lblCurrentLocationValue";
            this.lblCurrentLocationValue.Size = new System.Drawing.Size(350, 25);
            this.lblCurrentLocationValue.TabIndex = 3;
            this.lblCurrentLocationValue.Text = "Station A (X:120, Y:350)";
            // 
            // lblSpeed
            // 
            this.lblSpeed.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.lblSpeed.Location = new System.Drawing.Point(15, 94);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(150, 25);
            this.lblSpeed.TabIndex = 4;
            this.lblSpeed.Text = "Speed :";
            // 
            // lblSpeedValue
            // 
            this.lblSpeedValue.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.lblSpeedValue.Location = new System.Drawing.Point(170, 94);
            this.lblSpeedValue.Name = "lblSpeedValue";
            this.lblSpeedValue.Size = new System.Drawing.Size(200, 25);
            this.lblSpeedValue.TabIndex = 5;
            this.lblSpeedValue.Text = "0.0 m/s";
            // 
            // gbDispatchList
            // 
            this.gbDispatchList.Controls.Add(this.dgvDispatchList);
            this.gbDispatchList.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gbDispatchList.Location = new System.Drawing.Point(5, 225);
            this.gbDispatchList.Name = "gbDispatchList";
            this.gbDispatchList.Size = new System.Drawing.Size(785, 330);
            this.gbDispatchList.TabIndex = 0;
            this.gbDispatchList.TabStop = false;
            this.gbDispatchList.Text = "📋 Dispatch List";
            // 
            // dgvDispatchList
            // 
            this.dgvDispatchList.AllowUserToAddRows = false;
            this.dgvDispatchList.AllowUserToDeleteRows = false;
            this.dgvDispatchList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDispatchList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDispatchList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTaskID,
            this.colFrom,
            this.colTo,
            this.colPriority,
            this.colTaskStatus});
            this.dgvDispatchList.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.dgvDispatchList.Location = new System.Drawing.Point(10, 30);
            this.dgvDispatchList.Name = "dgvDispatchList";
            this.dgvDispatchList.ReadOnly = true;
            this.dgvDispatchList.RowHeadersVisible = false;
            this.dgvDispatchList.RowHeadersWidth = 62;
            this.dgvDispatchList.RowTemplate.Height = 28;
            this.dgvDispatchList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDispatchList.Size = new System.Drawing.Size(765, 290);
            this.dgvDispatchList.TabIndex = 0;
            // 
            // colTaskID
            // 
            this.colTaskID.HeaderText = "Task ID";
            this.colTaskID.MinimumWidth = 8;
            this.colTaskID.Name = "colTaskID";
            this.colTaskID.ReadOnly = true;
            // 
            // colFrom
            // 
            this.colFrom.HeaderText = "From";
            this.colFrom.MinimumWidth = 8;
            this.colFrom.Name = "colFrom";
            this.colFrom.ReadOnly = true;
            // 
            // colTo
            // 
            this.colTo.HeaderText = "To";
            this.colTo.MinimumWidth = 8;
            this.colTo.Name = "colTo";
            this.colTo.ReadOnly = true;
            // 
            // colPriority
            // 
            this.colPriority.HeaderText = "Priority";
            this.colPriority.MinimumWidth = 8;
            this.colPriority.Name = "colPriority";
            this.colPriority.ReadOnly = true;
            // 
            // colTaskStatus
            // 
            this.colTaskStatus.HeaderText = "Status";
            this.colTaskStatus.MinimumWidth = 8;
            this.colTaskStatus.Name = "colTaskStatus";
            this.colTaskStatus.ReadOnly = true;
            // 
            // gbPlannedRoute
            // 
            this.gbPlannedRoute.Controls.Add(this.lstPlannedRoute);
            this.gbPlannedRoute.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gbPlannedRoute.Location = new System.Drawing.Point(5, 560);
            this.gbPlannedRoute.Name = "gbPlannedRoute";
            this.gbPlannedRoute.Size = new System.Drawing.Size(785, 260);
            this.gbPlannedRoute.TabIndex = 0;
            this.gbPlannedRoute.TabStop = false;
            this.gbPlannedRoute.Text = "🗺️ Planned Route";
            // 
            // lstPlannedRoute
            // 
            this.lstPlannedRoute.Font = new System.Drawing.Font("Consolas", 11F);
            this.lstPlannedRoute.ItemHeight = 18;
            this.lstPlannedRoute.Location = new System.Drawing.Point(10, 28);
            this.lstPlannedRoute.Name = "lstPlannedRoute";
            this.lstPlannedRoute.Size = new System.Drawing.Size(765, 184);
            this.lstPlannedRoute.TabIndex = 0;
            // 
            // PageOp
            // 
            this.PageOp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.PageOp.Controls.Add(this.btnCycle);
            this.PageOp.Controls.Add(this.panelOpCenter);
            this.PageOp.Controls.Add(this.btnMRDetail);
            this.PageOp.Controls.Add(this.btnNavigation);
            this.PageOp.Controls.Add(this.btnInterrupt);
            this.PageOp.Controls.Add(this.btnCharging);
            this.PageOp.Controls.Add(this.btnDispatch);
            this.PageOp.Controls.Add(this.btnEmergencyStop);
            this.PageOp.Controls.Add(this.btnManualMove);
            this.PageOp.Controls.Add(this.btnPayloadControl);
            this.PageOp.Controls.Add(this.btnDiagnostics);
            this.PageOp.Location = new System.Drawing.Point(4, 29);
            this.PageOp.Name = "PageOp";
            this.PageOp.Size = new System.Drawing.Size(1730, 848);
            this.PageOp.TabIndex = 1;
            this.PageOp.Text = "Operation";
            // 
            // btnCycle
            // 
            this.btnCycle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnCycle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycle.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnCycle.ForeColor = System.Drawing.Color.White;
            this.btnCycle.Location = new System.Drawing.Point(200, 635);
            this.btnCycle.Name = "btnCycle";
            this.btnCycle.Size = new System.Drawing.Size(200, 70);
            this.btnCycle.TabIndex = 10;
            this.btnCycle.Text = "∞ Cycle";
            this.btnCycle.UseVisualStyleBackColor = false;
            this.btnCycle.Click += new System.EventHandler(this.btnCycle_Click);
            // 
            // panelOpCenter
            // 
            this.panelOpCenter.BackColor = System.Drawing.Color.White;
            this.panelOpCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOpCenter.Controls.Add(this.lblOpRobotIcon);
            this.panelOpCenter.Controls.Add(this.lblOpRobotLabel);
            this.panelOpCenter.Location = new System.Drawing.Point(565, 224);
            this.panelOpCenter.Name = "panelOpCenter";
            this.panelOpCenter.Size = new System.Drawing.Size(600, 400);
            this.panelOpCenter.TabIndex = 0;
            // 
            // lblOpRobotIcon
            // 
            this.lblOpRobotIcon.Font = new System.Drawing.Font("Segoe UI Emoji", 80F);
            this.lblOpRobotIcon.Location = new System.Drawing.Point(190, 50);
            this.lblOpRobotIcon.Name = "lblOpRobotIcon";
            this.lblOpRobotIcon.Size = new System.Drawing.Size(220, 200);
            this.lblOpRobotIcon.TabIndex = 0;
            this.lblOpRobotIcon.Text = "🤖";
            this.lblOpRobotIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOpRobotLabel
            // 
            this.lblOpRobotLabel.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold);
            this.lblOpRobotLabel.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblOpRobotLabel.Location = new System.Drawing.Point(100, 270);
            this.lblOpRobotLabel.Name = "lblOpRobotLabel";
            this.lblOpRobotLabel.Size = new System.Drawing.Size(400, 40);
            this.lblOpRobotLabel.TabIndex = 1;
            this.lblOpRobotLabel.Text = "Mobile Robot - AMR01";
            this.lblOpRobotLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMRDetail
            // 
            this.btnMRDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnMRDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMRDetail.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnMRDetail.ForeColor = System.Drawing.Color.White;
            this.btnMRDetail.Location = new System.Drawing.Point(765, 130);
            this.btnMRDetail.Name = "btnMRDetail";
            this.btnMRDetail.Size = new System.Drawing.Size(200, 70);
            this.btnMRDetail.TabIndex = 1;
            this.btnMRDetail.Text = "MR Detail";
            this.btnMRDetail.UseVisualStyleBackColor = false;
            this.btnMRDetail.Click += new System.EventHandler(this.btnMRDetail_Click);
            // 
            // btnNavigation
            // 
            this.btnNavigation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnNavigation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavigation.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnNavigation.ForeColor = System.Drawing.Color.White;
            this.btnNavigation.Location = new System.Drawing.Point(200, 130);
            this.btnNavigation.Name = "btnNavigation";
            this.btnNavigation.Size = new System.Drawing.Size(200, 70);
            this.btnNavigation.TabIndex = 2;
            this.btnNavigation.Text = "🧭 Navigation";
            this.btnNavigation.UseVisualStyleBackColor = false;
            this.btnNavigation.Click += new System.EventHandler(this.btnNavigation_Click);
            // 
            // btnInterrupt
            // 
            this.btnInterrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnInterrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInterrupt.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnInterrupt.ForeColor = System.Drawing.Color.White;
            this.btnInterrupt.Location = new System.Drawing.Point(200, 210);
            this.btnInterrupt.Name = "btnInterrupt";
            this.btnInterrupt.Size = new System.Drawing.Size(200, 70);
            this.btnInterrupt.TabIndex = 3;
            this.btnInterrupt.Text = "⏹ Interrupt";
            this.btnInterrupt.UseVisualStyleBackColor = false;
            this.btnInterrupt.Click += new System.EventHandler(this.btnInterrupt_Click);
            // 
            // btnCharging
            // 
            this.btnCharging.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnCharging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharging.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnCharging.ForeColor = System.Drawing.Color.White;
            this.btnCharging.Location = new System.Drawing.Point(200, 370);
            this.btnCharging.Name = "btnCharging";
            this.btnCharging.Size = new System.Drawing.Size(200, 70);
            this.btnCharging.TabIndex = 3;
            this.btnCharging.Text = "🔌 Charging";
            this.btnCharging.UseVisualStyleBackColor = false;
            this.btnCharging.Click += new System.EventHandler(this.btnCharging_Click);
            // 
            // btnDispatch
            // 
            this.btnDispatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(58)))), ((int)(((byte)(183)))));
            this.btnDispatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDispatch.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnDispatch.ForeColor = System.Drawing.Color.White;
            this.btnDispatch.Location = new System.Drawing.Point(200, 530);
            this.btnDispatch.Name = "btnDispatch";
            this.btnDispatch.Size = new System.Drawing.Size(200, 70);
            this.btnDispatch.TabIndex = 4;
            this.btnDispatch.Text = "📦 Dispatch";
            this.btnDispatch.UseVisualStyleBackColor = false;
            this.btnDispatch.Click += new System.EventHandler(this.btnDispatch_Click);
            // 
            // btnEmergencyStop
            // 
            this.btnEmergencyStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnEmergencyStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmergencyStop.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold);
            this.btnEmergencyStop.ForeColor = System.Drawing.Color.White;
            this.btnEmergencyStop.Location = new System.Drawing.Point(620, 680);
            this.btnEmergencyStop.Name = "btnEmergencyStop";
            this.btnEmergencyStop.Size = new System.Drawing.Size(250, 80);
            this.btnEmergencyStop.TabIndex = 5;
            this.btnEmergencyStop.Text = "🛑 E-STOP";
            this.btnEmergencyStop.UseVisualStyleBackColor = false;
            this.btnEmergencyStop.Click += new System.EventHandler(this.btnEmergencyStop_Click);
            // 
            // btnManualMove
            // 
            this.btnManualMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnManualMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualMove.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnManualMove.ForeColor = System.Drawing.Color.White;
            this.btnManualMove.Location = new System.Drawing.Point(1330, 370);
            this.btnManualMove.Name = "btnManualMove";
            this.btnManualMove.Size = new System.Drawing.Size(200, 70);
            this.btnManualMove.TabIndex = 6;
            this.btnManualMove.Text = "🕹️ Manual Move";
            this.btnManualMove.UseVisualStyleBackColor = false;
            this.btnManualMove.Click += new System.EventHandler(this.btnManualMove_Click);
            // 
            // btnPayloadControl
            // 
            this.btnPayloadControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnPayloadControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayloadControl.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnPayloadControl.ForeColor = System.Drawing.Color.White;
            this.btnPayloadControl.Location = new System.Drawing.Point(1330, 530);
            this.btnPayloadControl.Name = "btnPayloadControl";
            this.btnPayloadControl.Size = new System.Drawing.Size(200, 70);
            this.btnPayloadControl.TabIndex = 7;
            this.btnPayloadControl.Text = "📥 Payload";
            this.btnPayloadControl.UseVisualStyleBackColor = false;
            this.btnPayloadControl.Click += new System.EventHandler(this.btnPayloadControl_Click);
            // 
            // btnDiagnostics
            // 
            this.btnDiagnostics.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.btnDiagnostics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiagnostics.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Bold);
            this.btnDiagnostics.ForeColor = System.Drawing.Color.White;
            this.btnDiagnostics.Location = new System.Drawing.Point(920, 700);
            this.btnDiagnostics.Name = "btnDiagnostics";
            this.btnDiagnostics.Size = new System.Drawing.Size(200, 70);
            this.btnDiagnostics.TabIndex = 8;
            this.btnDiagnostics.Text = "🔧 Diagnostics";
            this.btnDiagnostics.UseVisualStyleBackColor = false;
            this.btnDiagnostics.Click += new System.EventHandler(this.btnDiagnostics_Click);
            // 
            // PageRobotDetail
            // 
            this.PageRobotDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.PageRobotDetail.Controls.Add(this.mrControl1);
            this.PageRobotDetail.Location = new System.Drawing.Point(4, 29);
            this.PageRobotDetail.Name = "PageRobotDetail";
            this.PageRobotDetail.Size = new System.Drawing.Size(1730, 848);
            this.PageRobotDetail.TabIndex = 2;
            this.PageRobotDetail.Text = "RobotDetail";
            // 
            // mrControl1
            // 
            this.mrControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.mrControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mrControl1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mrControl1.Location = new System.Drawing.Point(0, 0);
            this.mrControl1.Name = "mrControl1";
            this.mrControl1.Size = new System.Drawing.Size(1730, 848);
            this.mrControl1.TabIndex = 5;
            // 
            // PageDispatchDetail
            // 
            this.PageDispatchDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.PageDispatchDetail.Controls.Add(this.agvMapControl1);
            this.PageDispatchDetail.Location = new System.Drawing.Point(4, 29);
            this.PageDispatchDetail.Name = "PageDispatchDetail";
            this.PageDispatchDetail.Padding = new System.Windows.Forms.Padding(3);
            this.PageDispatchDetail.Size = new System.Drawing.Size(1730, 848);
            this.PageDispatchDetail.TabIndex = 3;
            this.PageDispatchDetail.Text = "DispatchDetail";
            // 
            // agvMapControl1
            // 
            this.agvMapControl1.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.agvMapControl1.Location = new System.Drawing.Point(23, 6);
            this.agvMapControl1.MmPerPixel = 30D;
            this.agvMapControl1.Name = "agvMapControl1";
            this.agvMapControl1.Size = new System.Drawing.Size(1674, 815);
            this.agvMapControl1.TabIndex = 0;
            // 
            // tabLog
            // 
            this.tabLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.tabLog.Controls.Add(this.splitContainer1);
            this.tabLog.Location = new System.Drawing.Point(4, 33);
            this.tabLog.Name = "tabLog";
            this.tabLog.Size = new System.Drawing.Size(1738, 881);
            this.tabLog.TabIndex = 2;
            this.tabLog.Text = "Log";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LogListSplit);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Log);
            this.splitContainer1.Size = new System.Drawing.Size(1738, 881);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 0;
            // 
            // LogListSplit
            // 
            this.LogListSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogListSplit.Location = new System.Drawing.Point(0, 0);
            this.LogListSplit.Name = "LogListSplit";
            this.LogListSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // LogListSplit.Panel1
            // 
            this.LogListSplit.Panel1.Controls.Add(this.LogRefreshBtn);
            this.LogListSplit.Panel1.Controls.Add(this.LogOpenFolderBtn);
            this.LogListSplit.Panel1.Controls.Add(this.GB_LogDataLoad);
            // 
            // LogListSplit.Panel2
            // 
            this.LogListSplit.Panel2.Controls.Add(this.LogTree);
            this.LogListSplit.Size = new System.Drawing.Size(320, 881);
            this.LogListSplit.SplitterDistance = 150;
            this.LogListSplit.TabIndex = 0;
            // 
            // LogRefreshBtn
            // 
            this.LogRefreshBtn.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LogRefreshBtn.Location = new System.Drawing.Point(163, 101);
            this.LogRefreshBtn.Margin = new System.Windows.Forms.Padding(4);
            this.LogRefreshBtn.Name = "LogRefreshBtn";
            this.LogRefreshBtn.Size = new System.Drawing.Size(145, 32);
            this.LogRefreshBtn.TabIndex = 18;
            this.LogRefreshBtn.Text = "Refresh Range";
            this.LogRefreshBtn.UseVisualStyleBackColor = true;
            this.LogRefreshBtn.Click += new System.EventHandler(this.LogRefreshBtn_Click);
            // 
            // LogOpenFolderBtn
            // 
            this.LogOpenFolderBtn.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LogOpenFolderBtn.Location = new System.Drawing.Point(6, 101);
            this.LogOpenFolderBtn.Name = "LogOpenFolderBtn";
            this.LogOpenFolderBtn.Size = new System.Drawing.Size(150, 32);
            this.LogOpenFolderBtn.TabIndex = 17;
            this.LogOpenFolderBtn.Text = "Open Log Folder";
            this.LogOpenFolderBtn.UseVisualStyleBackColor = true;
            this.LogOpenFolderBtn.Click += new System.EventHandler(this.LogOpenFolderBtn_Click);
            // 
            // GB_LogDataLoad
            // 
            this.GB_LogDataLoad.Controls.Add(this.HistorylogEnd);
            this.GB_LogDataLoad.Controls.Add(this.HistorylogStart);
            this.GB_LogDataLoad.Controls.Add(this.LabelLogDueDate);
            this.GB_LogDataLoad.Controls.Add(this.LabelLogDateFrom);
            this.GB_LogDataLoad.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GB_LogDataLoad.Location = new System.Drawing.Point(6, 3);
            this.GB_LogDataLoad.Name = "GB_LogDataLoad";
            this.GB_LogDataLoad.Size = new System.Drawing.Size(308, 92);
            this.GB_LogDataLoad.TabIndex = 16;
            this.GB_LogDataLoad.TabStop = false;
            this.GB_LogDataLoad.Text = "Log data Search";
            // 
            // HistorylogEnd
            // 
            this.HistorylogEnd.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.HistorylogEnd.Location = new System.Drawing.Point(97, 56);
            this.HistorylogEnd.Name = "HistorylogEnd";
            this.HistorylogEnd.Size = new System.Drawing.Size(205, 29);
            this.HistorylogEnd.TabIndex = 6;
            // 
            // HistorylogStart
            // 
            this.HistorylogStart.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.HistorylogStart.Location = new System.Drawing.Point(97, 23);
            this.HistorylogStart.Name = "HistorylogStart";
            this.HistorylogStart.Size = new System.Drawing.Size(205, 29);
            this.HistorylogStart.TabIndex = 6;
            // 
            // LabelLogDueDate
            // 
            this.LabelLogDueDate.AutoSize = true;
            this.LabelLogDueDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabelLogDueDate.Location = new System.Drawing.Point(6, 62);
            this.LabelLogDueDate.Name = "LabelLogDueDate";
            this.LabelLogDueDate.Size = new System.Drawing.Size(80, 20);
            this.LabelLogDueDate.TabIndex = 4;
            this.LabelLogDueDate.Text = "Due Date";
            // 
            // LabelLogDateFrom
            // 
            this.LabelLogDateFrom.AutoSize = true;
            this.LabelLogDateFrom.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LabelLogDateFrom.Location = new System.Drawing.Point(6, 29);
            this.LabelLogDateFrom.Name = "LabelLogDateFrom";
            this.LabelLogDateFrom.Size = new System.Drawing.Size(85, 20);
            this.LabelLogDateFrom.TabIndex = 4;
            this.LabelLogDateFrom.Text = "Date from";
            // 
            // LogTree
            // 
            this.LogTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.LogTree.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LogTree.Location = new System.Drawing.Point(2, 4);
            this.LogTree.Margin = new System.Windows.Forms.Padding(4);
            this.LogTree.Name = "LogTree";
            this.LogTree.Size = new System.Drawing.Size(314, 715);
            this.LogTree.TabIndex = 18;
            this.LogTree.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.LogTree_AfterCollapse);
            this.LogTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.LogTree_BeforeExpand);
            this.LogTree.DoubleClick += new System.EventHandler(this.LogTree_DoubleClick);
            // 
            // Log
            // 
            this.Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Log.FolderPath = null;
            this.Log.Location = new System.Drawing.Point(0, 0);
            this.Log.Name = "Log";
            this.Log.SelectedIndex = 0;
            this.Log.Size = new System.Drawing.Size(1414, 881);
            this.Log.TabIndex = 0;
            this.Log.UserName = null;
            // 
            // tabHistory
            // 
            this.tabHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.tabHistory.Location = new System.Drawing.Point(4, 33);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Size = new System.Drawing.Size(1738, 881);
            this.tabHistory.TabIndex = 3;
            this.tabHistory.Text = "History";
            // 
            // tabSetting
            // 
            this.tabSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.tabSetting.Controls.Add(this.SettingSplit);
            this.tabSetting.Location = new System.Drawing.Point(4, 33);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Size = new System.Drawing.Size(1738, 881);
            this.tabSetting.TabIndex = 4;
            this.tabSetting.Text = "Setting";
            // 
            // SettingSplit
            // 
            this.SettingSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingSplit.Location = new System.Drawing.Point(0, 0);
            this.SettingSplit.Name = "SettingSplit";
            this.SettingSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SettingSplit.Panel2
            // 
            this.SettingSplit.Panel2.Controls.Add(this.Core);
            this.SettingSplit.Size = new System.Drawing.Size(1738, 881);
            this.SettingSplit.SplitterDistance = 120;
            this.SettingSplit.TabIndex = 0;
            // 
            // Core
            // 
            this.Core.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Core.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Core.Location = new System.Drawing.Point(0, 0);
            this.Core.Margin = new System.Windows.Forms.Padding(5);
            this.Core.Name = "Core";
            this.Core.Size = new System.Drawing.Size(1738, 757);
            this.Core.TabIndex = 0;
            // 
            // tabAbout
            // 
            this.tabAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.tabAbout.Location = new System.Drawing.Point(4, 33);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Size = new System.Drawing.Size(1738, 881);
            this.tabAbout.TabIndex = 5;
            this.tabAbout.Text = "About";
            // 
            // TimeSplit
            // 
            this.TimeSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TimeSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.TimeSplit.IsSplitterFixed = true;
            this.TimeSplit.Location = new System.Drawing.Point(0, 0);
            this.TimeSplit.Name = "TimeSplit";
            // 
            // TimeSplit.Panel1
            // 
            this.TimeSplit.Panel1.Controls.Add(this.ShowTime);
            this.TimeSplit.Size = new System.Drawing.Size(1746, 26);
            this.TimeSplit.SplitterDistance = 250;
            this.TimeSplit.TabIndex = 0;
            // 
            // ShowTime
            // 
            this.ShowTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowTime.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.ShowTime.Location = new System.Drawing.Point(0, 0);
            this.ShowTime.Name = "ShowTime";
            this.ShowTime.ReadOnly = true;
            this.ShowTime.Size = new System.Drawing.Size(250, 27);
            this.ShowTime.TabIndex = 0;
            this.ShowTime.Text = "yyyy-MM-dd HH:mm:ss";
            this.ShowTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LOGOSplit
            // 
            this.LOGOSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LOGOSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LOGOSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.LOGOSplit.IsSplitterFixed = true;
            this.LOGOSplit.Location = new System.Drawing.Point(0, 0);
            this.LOGOSplit.Name = "LOGOSplit";
            this.LOGOSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // LOGOSplit.Panel1
            // 
            this.LOGOSplit.Panel1.Controls.Add(this.TopRightLogo);
            // 
            // LOGOSplit.Panel2
            // 
            this.LOGOSplit.Panel2.Controls.Add(this.BtExit);
            this.LOGOSplit.Panel2.Controls.Add(this.BtAbout);
            this.LOGOSplit.Panel2.Controls.Add(this.BtLogin);
            this.LOGOSplit.Panel2.Controls.Add(this.BtSetting);
            this.LOGOSplit.Panel2.Controls.Add(this.BtHistory);
            this.LOGOSplit.Panel2.Controls.Add(this.BtRefreshSEQ);
            this.LOGOSplit.Panel2.Controls.Add(this.BtOperation);
            this.LOGOSplit.Panel2.Controls.Add(this.BtMonitor);
            this.LOGOSplit.Size = new System.Drawing.Size(166, 1080);
            this.LOGOSplit.SplitterDistance = 120;
            this.LOGOSplit.TabIndex = 0;
            // 
            // TopRightLogo
            // 
            this.TopRightLogo.Controls.Add(this.AMLLogo);
            this.TopRightLogo.Controls.Add(this.VersionLabel);
            this.TopRightLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopRightLogo.Location = new System.Drawing.Point(0, 0);
            this.TopRightLogo.Name = "TopRightLogo";
            this.TopRightLogo.Size = new System.Drawing.Size(162, 116);
            this.TopRightLogo.TabIndex = 0;
            // 
            // AMLLogo
            // 
            this.AMLLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AMLLogo.BackgroundImage")));
            this.AMLLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.AMLLogo.Location = new System.Drawing.Point(22, 15);
            this.AMLLogo.Name = "AMLLogo";
            this.AMLLogo.Size = new System.Drawing.Size(120, 49);
            this.AMLLogo.TabIndex = 0;
            this.AMLLogo.TabStop = false;
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Font = new System.Drawing.Font("微軟正黑體", 11F);
            this.VersionLabel.Location = new System.Drawing.Point(15, 72);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(136, 19);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "V1.0.20261015.09";
            // 
            // BtExit
            // 
            this.BtExit.AutoSize = true;
            this.BtExit.BackColor = System.Drawing.SystemColors.Control;
            this.BtExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtExit.BackgroundImage")));
            this.BtExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtExit.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtExit.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtExit.Location = new System.Drawing.Point(0, 833);
            this.BtExit.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtExit.Name = "BtExit";
            this.BtExit.Size = new System.Drawing.Size(162, 119);
            this.BtExit.TabIndex = 0;
            this.BtExit.TabStop = false;
            this.BtExit.Text = "Exit";
            this.BtExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtExit.UseVisualStyleBackColor = false;
            this.BtExit.Click += new System.EventHandler(this.BtExit_Click);
            // 
            // BtAbout
            // 
            this.BtAbout.AutoSize = true;
            this.BtAbout.BackColor = System.Drawing.SystemColors.Control;
            this.BtAbout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtAbout.BackgroundImage")));
            this.BtAbout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtAbout.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtAbout.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtAbout.Location = new System.Drawing.Point(0, 714);
            this.BtAbout.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtAbout.Name = "BtAbout";
            this.BtAbout.Size = new System.Drawing.Size(162, 119);
            this.BtAbout.TabIndex = 1;
            this.BtAbout.TabStop = false;
            this.BtAbout.Text = "About";
            this.BtAbout.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtAbout.UseVisualStyleBackColor = false;
            this.BtAbout.Click += new System.EventHandler(this.BtAbout_Click);
            // 
            // BtLogin
            // 
            this.BtLogin.AutoSize = true;
            this.BtLogin.BackColor = System.Drawing.SystemColors.Control;
            this.BtLogin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtLogin.BackgroundImage")));
            this.BtLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtLogin.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtLogin.Location = new System.Drawing.Point(0, 595);
            this.BtLogin.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtLogin.Name = "BtLogin";
            this.BtLogin.Size = new System.Drawing.Size(162, 119);
            this.BtLogin.TabIndex = 2;
            this.BtLogin.TabStop = false;
            this.BtLogin.Text = "Login";
            this.BtLogin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtLogin.UseVisualStyleBackColor = false;
            this.BtLogin.Click += new System.EventHandler(this.BtLogin_Click);
            // 
            // BtSetting
            // 
            this.BtSetting.AutoSize = true;
            this.BtSetting.BackColor = System.Drawing.SystemColors.Control;
            this.BtSetting.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtSetting.BackgroundImage")));
            this.BtSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtSetting.Enabled = false;
            this.BtSetting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtSetting.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtSetting.Location = new System.Drawing.Point(0, 476);
            this.BtSetting.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtSetting.Name = "BtSetting";
            this.BtSetting.Size = new System.Drawing.Size(162, 119);
            this.BtSetting.TabIndex = 3;
            this.BtSetting.TabStop = false;
            this.BtSetting.Text = "Setting";
            this.BtSetting.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtSetting.UseVisualStyleBackColor = false;
            this.BtSetting.Click += new System.EventHandler(this.BtSetting_Click);
            // 
            // BtHistory
            // 
            this.BtHistory.AutoSize = true;
            this.BtHistory.BackColor = System.Drawing.SystemColors.Control;
            this.BtHistory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtHistory.BackgroundImage")));
            this.BtHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtHistory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtHistory.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtHistory.Location = new System.Drawing.Point(0, 357);
            this.BtHistory.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtHistory.Name = "BtHistory";
            this.BtHistory.Size = new System.Drawing.Size(162, 119);
            this.BtHistory.TabIndex = 4;
            this.BtHistory.TabStop = false;
            this.BtHistory.Text = "History";
            this.BtHistory.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtHistory.UseVisualStyleBackColor = false;
            this.BtHistory.Click += new System.EventHandler(this.BtHistory_Click);
            // 
            // BtRefreshSEQ
            // 
            this.BtRefreshSEQ.AutoSize = true;
            this.BtRefreshSEQ.BackColor = System.Drawing.SystemColors.Control;
            this.BtRefreshSEQ.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtRefreshSEQ.BackgroundImage")));
            this.BtRefreshSEQ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtRefreshSEQ.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtRefreshSEQ.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtRefreshSEQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtRefreshSEQ.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtRefreshSEQ.Location = new System.Drawing.Point(0, 238);
            this.BtRefreshSEQ.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtRefreshSEQ.Name = "BtRefreshSEQ";
            this.BtRefreshSEQ.Size = new System.Drawing.Size(162, 119);
            this.BtRefreshSEQ.TabIndex = 5;
            this.BtRefreshSEQ.TabStop = false;
            this.BtRefreshSEQ.Text = "Log";
            this.BtRefreshSEQ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtRefreshSEQ.UseVisualStyleBackColor = false;
            this.BtRefreshSEQ.Click += new System.EventHandler(this.BtLog_Click);
            // 
            // BtOperation
            // 
            this.BtOperation.AutoSize = true;
            this.BtOperation.BackColor = System.Drawing.SystemColors.Control;
            this.BtOperation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtOperation.BackgroundImage")));
            this.BtOperation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtOperation.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtOperation.Enabled = false;
            this.BtOperation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtOperation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtOperation.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtOperation.Location = new System.Drawing.Point(0, 119);
            this.BtOperation.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtOperation.Name = "BtOperation";
            this.BtOperation.Size = new System.Drawing.Size(162, 119);
            this.BtOperation.TabIndex = 6;
            this.BtOperation.TabStop = false;
            this.BtOperation.Text = "Operation";
            this.BtOperation.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtOperation.UseVisualStyleBackColor = false;
            this.BtOperation.Click += new System.EventHandler(this.BtOperation_Click);
            // 
            // BtMonitor
            // 
            this.BtMonitor.AutoSize = true;
            this.BtMonitor.BackColor = System.Drawing.SystemColors.Control;
            this.BtMonitor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtMonitor.BackgroundImage")));
            this.BtMonitor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtMonitor.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtMonitor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BtMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtMonitor.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.BtMonitor.Location = new System.Drawing.Point(0, 0);
            this.BtMonitor.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtMonitor.Name = "BtMonitor";
            this.BtMonitor.Size = new System.Drawing.Size(162, 119);
            this.BtMonitor.TabIndex = 7;
            this.BtMonitor.TabStop = false;
            this.BtMonitor.Text = "Monitor";
            this.BtMonitor.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtMonitor.UseVisualStyleBackColor = false;
            this.BtMonitor.Click += new System.EventHandler(this.BtMonitor_Click);
            // 
            // trash
            // 
            this.trash.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.trash.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.trash.Location = new System.Drawing.Point(40, 40);
            this.trash.Margin = new System.Windows.Forms.Padding(5);
            this.trash.MaximumSize = new System.Drawing.Size(100, 100);
            this.trash.Name = "trash";
            this.trash.Size = new System.Drawing.Size(100, 80);
            this.trash.TabIndex = 0;
            this.trash.TabStop = false;
            // 
            // UI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.ButtonSplit);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(1362, 726);
            this.Name = "UI";
            this.Text = "Mobile Robot Control System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MobileRobotUI_Load);
            this.ButtonSplit.Panel1.ResumeLayout(false);
            this.ButtonSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonSplit)).EndInit();
            this.ButtonSplit.ResumeLayout(false);
            this.MainSplit.Panel1.ResumeLayout(false);
            this.MainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
            this.MainSplit.ResumeLayout(false);
            this.ControlModeSplit.Panel1.ResumeLayout(false);
            this.ControlModeSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ControlModeSplit)).EndInit();
            this.ControlModeSplit.ResumeLayout(false);
            this.StatusSplit.Panel1.ResumeLayout(false);
            this.StatusSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StatusSplit)).EndInit();
            this.StatusSplit.ResumeLayout(false);
            this.ToolBarSplit.Panel1.ResumeLayout(false);
            this.ToolBarSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ToolBarSplit)).EndInit();
            this.ToolBarSplit.ResumeLayout(false);
            this.SystemPanel.ResumeLayout(false);
            this.tabLogin.ResumeLayout(false);
            this.LoginSplit.Panel1.ResumeLayout(false);
            this.LoginSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LoginSplit)).EndInit();
            this.LoginSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.LGAccessTab.ResumeLayout(false);
            this.Guest.ResumeLayout(false);
            this.LGLoginGB.ResumeLayout(false);
            this.LGLoginGB.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.Optab.ResumeLayout(false);
            this.PageMon.ResumeLayout(false);
            this.panelMonitorRobot.ResumeLayout(false);
            this.panelMonitorStatus.ResumeLayout(false);
            this.gbBattery.ResumeLayout(false);
            this.gbStatus.ResumeLayout(false);
            this.gbDispatchList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDispatchList)).EndInit();
            this.gbPlannedRoute.ResumeLayout(false);
            this.PageOp.ResumeLayout(false);
            this.panelOpCenter.ResumeLayout(false);
            this.PageRobotDetail.ResumeLayout(false);
            this.PageDispatchDetail.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.LogListSplit.Panel1.ResumeLayout(false);
            this.LogListSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogListSplit)).EndInit();
            this.LogListSplit.ResumeLayout(false);
            this.GB_LogDataLoad.ResumeLayout(false);
            this.GB_LogDataLoad.PerformLayout();
            this.tabSetting.ResumeLayout(false);
            this.SettingSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SettingSplit)).EndInit();
            this.SettingSplit.ResumeLayout(false);
            this.TimeSplit.Panel1.ResumeLayout(false);
            this.TimeSplit.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeSplit)).EndInit();
            this.TimeSplit.ResumeLayout(false);
            this.LOGOSplit.Panel1.ResumeLayout(false);
            this.LOGOSplit.Panel2.ResumeLayout(false);
            this.LOGOSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LOGOSplit)).EndInit();
            this.LOGOSplit.ResumeLayout(false);
            this.TopRightLogo.ResumeLayout(false);
            this.TopRightLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AMLLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // ===== 全域控制項宣告 =====
        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.SplitContainer ButtonSplit;
        private System.Windows.Forms.SplitContainer MainSplit;
        private System.Windows.Forms.SplitContainer ControlModeSplit;
        private System.Windows.Forms.SplitContainer StatusSplit;
        private System.Windows.Forms.Label lblControlMode;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.SplitContainer ToolBarSplit;
        private System.Windows.Forms.SplitContainer TimeSplit;
        private System.Windows.Forms.TextBox ShowTime;

        private System.Windows.Forms.TabControl SystemPanel;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.TabPage tabLogin;
        private System.Windows.Forms.TabPage tabAbout;

        private System.Windows.Forms.TabControl Optab;
        private System.Windows.Forms.TabPage PageMon;
        private System.Windows.Forms.TabPage PageOp;
        private System.Windows.Forms.TabPage PageRobotDetail;

        // Monitor
        private System.Windows.Forms.Panel panelMonitorRobot;
        private System.Windows.Forms.Label lblRobotIcon;
        private System.Windows.Forms.Label lblRobotName;
        private System.Windows.Forms.Panel panelMonitorStatus;
        private System.Windows.Forms.GroupBox gbBattery;
        private System.Windows.Forms.ProgressBar progressBattery;
        private System.Windows.Forms.Label lblBatteryPercent;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblCurrentLocation;
        private System.Windows.Forms.Label lblCurrentLocationValue;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblSpeedValue;
        private System.Windows.Forms.GroupBox gbDispatchList;
        private System.Windows.Forms.DataGridView dgvDispatchList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskStatus;
        private System.Windows.Forms.GroupBox gbPlannedRoute;
        private System.Windows.Forms.ListBox lstPlannedRoute;

        // Operation
        private System.Windows.Forms.Panel panelOpCenter;
        private System.Windows.Forms.Label lblOpRobotIcon;
        private System.Windows.Forms.Label lblOpRobotLabel;
        private System.Windows.Forms.Button btnMRDetail;
        private System.Windows.Forms.Button btnNavigation;
        private System.Windows.Forms.Button btnInterrupt;
        private System.Windows.Forms.Button btnCharging;
        private System.Windows.Forms.Button btnDispatch;
        private System.Windows.Forms.Button btnEmergencyStop;
        private System.Windows.Forms.Button btnManualMove;
        private System.Windows.Forms.Button btnPayloadControl;
        private System.Windows.Forms.Button btnDiagnostics;

        // Login
        private System.Windows.Forms.SplitContainer LoginSplit;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.TabControl LGAccessTab;
        private System.Windows.Forms.TabPage Guest;
        private System.Windows.Forms.GroupBox LGLoginGB;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox LGAccount;
        private System.Windows.Forms.TextBox LGPassword;
        private System.Windows.Forms.Button LGLoginBtn;
        private System.Windows.Forms.Label LGLoginMessage;

        // 右側按鈕列
        private System.Windows.Forms.SplitContainer LOGOSplit;
        private System.Windows.Forms.Panel TopRightLogo;
        private System.Windows.Forms.PictureBox AMLLogo;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Button BtMonitor;
        private System.Windows.Forms.Button BtOperation;
        private System.Windows.Forms.Button BtRefreshSEQ;
        private System.Windows.Forms.Button BtHistory;
        private System.Windows.Forms.Button BtSetting;
        private System.Windows.Forms.Button BtLogin;
        private System.Windows.Forms.Button BtAbout;
        private System.Windows.Forms.Button BtExit;
        private System.Windows.Forms.TabPage PageDispatchDetail;
        private CoreControl trash;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer LogListSplit;
        private System.Windows.Forms.Button LogOpenFolderBtn;
        private System.Windows.Forms.GroupBox GB_LogDataLoad;
        private System.Windows.Forms.DateTimePicker HistorylogEnd;
        private System.Windows.Forms.DateTimePicker HistorylogStart;
        private System.Windows.Forms.Label LabelLogDueDate;
        private System.Windows.Forms.Label LabelLogDateFrom;
        private System.Windows.Forms.SplitContainer SettingSplit;
        private CoreControl Core;
        private System.Windows.Forms.Button btnCycle;
        private AGVMap.AGVMapControl agvMapControl1;
        private MobileRobot.MRControl mrControl1;
        private System.Windows.Forms.Button LogRefreshBtn;
        private System.Windows.Forms.TreeView LogTree;
        private AML_LogTab Log;
        private AML_LogPage SystemLogOnTop;
    }
}
