using AML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AML
{
    public partial class UI : Form
    {
        private string currentUser = "Guest";
        private string currentAccess = "Guest";
        

        private WT_MsgBox MsgBox = new WT_MsgBox();
        private AML_LogPage OperationLog = new AML_LogPage();


        //===== 變數 =====
        private string AML_Version = "V1.0.20261108.20"; // 版本號

        public UI()
        {
            InitializeComponent();
        }

        // ============================
        // Form Load — 啟用初始化 + MQTT 連線
        // ============================
        private void MobileRobotUI_Load(object sender, EventArgs e)
        {
            //MsgBox.Owner = this; //測試 JAY
            //if (Check_ExeOpen())
            //{
            //    MessageBox.Show("AML UI已開啟，請確認右下角Icon");
            //    Application.Exit();
            //    return;
            //}
            HidePanelTab();
            SystemPanel.SelectTab("tabMain");
            Optab.SelectTab("PageMon");
            BtChange("Monitor");
            LoadDemoData();

            string LogFolder = XML.Read("Setting/Form1/Logfolder", "C:\\AML");
            int LogCountLimit = Convert.ToUInt16(XML.Read("Setting/Form1/LogCountLimit", "200"));
            int LogFolderLimit = Convert.ToUInt16(XML.Read("Setting/Form1/LogFolderLimit", "30"));
            Log.Initial(false, true, LogFolder, LogCountLimit, LogFolderLimit);
            //Log.OutputSystemLog += new AML_LogTab.SystemLogEvent(WriteTopLog);
            OperationLog.Initial("Operation", false, true, LogFolder, LogCountLimit, LogFolderLimit);
            mrControl1.Log.Initial("MRControl", false, true, LogFolder, LogCountLimit, LogFolderLimit);
            agvMapControl1.Log.Initial("AVGMap", false, true, LogFolder, LogCountLimit, LogFolderLimit);
            Log.Add(OperationLog);
            Log.Add(mrControl1.Log);
            Log.Add(agvMapControl1.Log);
            // ✅ 啟用 Core 初始化（加上保護）
            try
            {
                Core.Initial(Log, agvMapControl1, mrControl1);
                Log.WriteSystemLog("System", "CoreControl initialized.");
            }
            catch (Exception ex)
            {
                WriteLog("ERROR", $"CoreControl init failed (non-fatal): {ex.Message}");
            }

            Timer1.Interval = 1000;
            Timer1.Start();
            VersionLabel.Text = AML_Version;

            Log.WriteSystemLog("System", "=================================================");
            Log.WriteSystemLog("System", "Mobile Robot UI Loaded.(" + VersionLabel.Text + ")");
            AutoLogIn(sender, e);
        }

        private bool Check_ExeOpen()
        {
            Process aProcess = Process.GetCurrentProcess();
            string aProcName = aProcess.ProcessName;
            if (Process.GetProcessesByName(aProcName).Length > 1)
                return true;
            else
                return false;
        }
        //=====Log Page=====
        private void LogOpenFolderBtn_Click(object sender, EventArgs e)
        {
            Process.Start(Log.FolderPath);
        }
        private void LogRefreshBtn_Click(object sender, EventArgs e)
        {
            LogTree.Nodes.Clear();
            if (HistorylogStart.Value.Date > HistorylogEnd.Value.Date)
            {
                MsgBox.ShowMsg("時間範圍選取錯誤");
                return;
            }
            double datecount = HistorylogEnd.Value.AddDays(1).Date.Subtract(HistorylogStart.Value.Date).TotalDays;
            if (datecount > 60)
            {
                MsgBox.ShowMsg("為避免資料量過大，選取時間範圍請在兩個月以內");
                return;
            }
            if (!Directory.Exists(Log.FolderPath))
            {
                MsgBox.ShowMsg("請確認設定路徑，設定不存在: " + Log.FolderPath);
                return;
            }
            for (int i = 0; i < datecount + 1; i++)
            {
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(Log.FolderPath, HistorylogStart.Value.Date.AddDays(i).ToString("yyyyMMdd")));
                foreach (var dir in dirs)
                {
                    TreeNode TestNode = new TreeNode();
                    TestNode.Nodes.Add("");
                    TestNode.Text = dir.Substring(dir.LastIndexOf("\\") + 1);
                    LogTree.Nodes.Add(TestNode);
                }
            }
        }
        private void LogTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string file_path = Log.FolderPath + "\\" + e.Node.Text;
            List<string> dirs1 = new List<string>(Directory.EnumerateFiles(file_path));
            foreach (var dir1 in dirs1)
                e.Node.Nodes.Add(dir1.Substring(dir1.LastIndexOf("\\") + 1));
        }
        private void LogTree_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.Nodes.Clear();
            e.Node.Nodes.Add("");
        }
        private void LogTree_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (LogTree.SelectedNode.Level == 1)
                {
                    string file = LogTree.SelectedNode.Text;
                    string folder = LogTree.SelectedNode.Parent.Text;
                    Process.Start(Log.FolderPath + "\\" + folder + "\\" + file);
                }
            }
            catch (Exception ex)
            {
                WriteLog("ERROR", $"Open log file failed: {ex.Message}");
            }
        }
        private void WriteTopLog(DateTime Time, string SubTitle, string Description)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { WriteTopLog(Time, SubTitle, Description); }));
                return;
            }
            SystemLogOnTop.Write(Time, SubTitle, Description);
        }
        // ============================
        // 隱藏 TabControl 頁籤
        // ============================
        private void HidePanelTab()
        {
            SystemPanel.Appearance = TabAppearance.FlatButtons;
            SystemPanel.ItemSize = new Size(0, 1);
            SystemPanel.SizeMode = TabSizeMode.Fixed;

            Optab.Appearance = TabAppearance.FlatButtons;
            Optab.ItemSize = new Size(0, 1);
            Optab.SizeMode = TabSizeMode.Fixed;

            LGAccessTab.Appearance = TabAppearance.FlatButtons;
            LGAccessTab.ItemSize = new Size(0, 1);
            LGAccessTab.SizeMode = TabSizeMode.Fixed;
        }

        // ============================
        // 時鐘 Timer
        // ============================
        private void Timer1_Tick(object sender, EventArgs e)
        {
            ShowTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // ============================
        // 右側導航按鈕 - 顏色切換
        // ============================
        private void BtChange(string selected)
        {
            Color defaultColor = SystemColors.Control;
            Color activeColor = Color.FromArgb(192, 255, 255);

            BtMonitor.BackColor = defaultColor;
            BtOperation.BackColor = defaultColor;
            BtRefreshSEQ.BackColor = defaultColor;
            BtHistory.BackColor = defaultColor;
            BtSetting.BackColor = defaultColor;
            BtLogin.BackColor = defaultColor;
            BtAbout.BackColor = defaultColor;

            switch (selected)
            {
                case "Monitor": BtMonitor.BackColor = activeColor; break;
                case "Operation": BtOperation.BackColor = activeColor; break;
                case "Log": BtRefreshSEQ.BackColor = activeColor; break;
                case "History": BtHistory.BackColor = activeColor; break;
                case "Setting": BtSetting.BackColor = activeColor; break;
                case "Login": BtLogin.BackColor = activeColor; break;
                case "About": BtAbout.BackColor = activeColor; break;
            }
        }

        // ============================
        // 右側導航按鈕 - 事件
        // ============================
        private void BtMonitor_Click(object sender, EventArgs e)
        {
            BtChange("Monitor");
            SystemPanel.SelectTab("tabMain");
            Optab.SelectTab("PageMon");
            WriteLog("UI", "Switched to Monitor page.");
        }

        private void BtOperation_Click(object sender, EventArgs e)
        {
            BtChange("Operation");
            SystemPanel.SelectTab("tabMain");
            Optab.SelectTab("PageOp");
            WriteLog("UI", "Switched to Operation page.");
        }

        private void BtLog_Click(object sender, EventArgs e)
        {
            BtChange("Log");
            SystemPanel.SelectTab("tabLog");
        }

        private void BtHistory_Click(object sender, EventArgs e)
        {
            BtChange("History");
            SystemPanel.SelectTab("tabHistory");
        }

        private void BtSetting_Click(object sender, EventArgs e)
        {
            BtChange("Setting");
            SystemPanel.SelectTab("tabSetting");
        }

        private void BtLogin_Click(object sender, EventArgs e)
        {
            BtChange("Login");
            SystemPanel.SelectTab("tabLogin");
        }

        private void BtAbout_Click(object sender, EventArgs e)
        {
            BtChange("About");
            SystemPanel.SelectTab("tabAbout");
        }

        private void BtExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // ============================
        // Login
        // ============================
        private void AutoLogIn(object sender, EventArgs e)
        {
            LGAccount.Text = "aml";
            LGPassword.Text = "00000000";
            LGLoginBtn_Click(sender, e);
        }
        private void LGLoginBtn_Click(object sender, EventArgs e)
        {
            string account = LGAccount.Text.Trim();
            string password = LGPassword.Text.Trim();

            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                LGLoginMessage.Text = "Please enter account and password.";
                LGLoginMessage.ForeColor = Color.Red;
                return;
            }

            // 簡易驗證 (實際可改為 DB 驗證)
            if (account == "aml" && password == "00000000")
            {
                currentUser = account;
                currentAccess = "Engineer";
                BtOperation.Enabled = true;
                BtSetting.Enabled = true;
                LGLoginMessage.Text = $"Welcome, {account}!";
                LGLoginMessage.ForeColor = Color.Green;
                WriteLog("Login", $"User [{account}] logged in as Engineer.");

                // 登入後跳到 Monitor
                BtMonitor_Click(null, null);
            }
            else
            {
                LGLoginMessage.Text = "Invalid account or password.";
                LGLoginMessage.ForeColor = Color.Red;
            }
        }

        private void LGPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LGLoginBtn_Click(null, null);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // ============================
        // Operation 頁面 - 周圍按鈕
        // ============================
        private void btnMRDetail_Click(object sender, EventArgs e)
        {
            Optab.SelectTab("PageRobotDetail");
            WriteLog("UI", "Navigated to Robot Detail page.");
        }

        private void btnRBBackToOp_Click(object sender, EventArgs e)
        {
            Optab.SelectTab("PageOp");
        }

        private void btnNavigation_Click(object sender, EventArgs e)
        {
            if (Core.BG_AutoRun.IsBusy)
            {
                WriteLog("Operation", "AutoRun stop requested.");
                Core.Interrupt();
                return;
            }
            WriteLog("Operation", "AutoRun (random route) started.");
            Core.BG_AutoRun.RunWorkerAsync("test");
        }

        private void btnInterrupt_Click(object sender, EventArgs e)
        {
            WriteLog("Operation", "Interrupt requested (BreakFlag = true).");
            Core.Interrupt();
        }

        private void btnCharging_Click(object sender, EventArgs e)
        {
            WriteLog("Operation", "Charging command sent - Go to charging station.");
            MessageBox.Show("Robot is heading to charging station.", "Charging",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDispatch_Click(object sender, EventArgs e)
        {
            Optab.SelectTab("PageDispatchDetail");
            WriteLog("Operation", "Dispatch management opened.");
        }

        // ============================
        // Cycle Run 按鈕 — 注入 Core
        // ============================
        private void btnCycle_Click(object sender, EventArgs e)
        {
            // ✅ 前置檢查
            if (Core == null)
            {
                MessageBox.Show("CoreControl 尚未初始化！", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ 檢查 MQTT 連線狀態（警告但不阻擋）
            if (!mrControl1.IsConnected)
            {
                var result = MessageBox.Show(
                    "MQTT 尚未連線到 Broker！\n\n" +
                    "路線規劃(Plan)仍可使用，但執行(Start)會無法發送指令。\n\n" +
                    "是否仍要開啟？",
                    "MQTT 未連線",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes) return;
            }
            WriteLog("Operation", "Cycle management opened.");
        }

        private void btnEmergencyStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("⚠ Are you sure to EMERGENCY STOP the robot?",
                "E-STOP", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                lblStatusValue.Text = "E-STOP";
                lblStatusValue.ForeColor = Color.Red;
                lblSpeedValue.Text = "0.0 m/s";
                WriteLog("ALARM", "🛑 Emergency Stop activated!");
            }
        }

        private void btnManualMove_Click(object sender, EventArgs e)
        {
            WriteLog("Operation", "Manual Move mode entered.");
            MessageBox.Show("Manual Move - 可在此用方向鍵控制 Robot 移動", "Manual Move",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPayloadControl_Click(object sender, EventArgs e)
        {
            WriteLog("Operation", "Payload control opened.");
            MessageBox.Show("Payload Control - 載具升降/夾取控制", "Payload",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDiagnostics_Click(object sender, EventArgs e)
        {
            WriteLog("Operation", "Diagnostics opened.");
            MessageBox.Show("Diagnostics - 機器人自我診斷", "Diagnostics",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // ============================
        // Setting 頁面 - 連線
        // ============================


        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            lblConnectionStatus.Text = "● Disconnected";
            lblConnectionStatus.ForeColor = Color.Red;
            WriteLog("Connection", "Disconnected from Robot.");
        }

        // ============================
        // 通用 Log 寫入
        // ============================
        private void WriteLog(string source, string message)
        {
            if (SystemLogOnTop.InvokeRequired)
            {
                SystemLogOnTop.Invoke(new Action(() => WriteLog(source, message)));
                return;
            }

            string logLine = $"[{DateTime.Now:HH:mm:ss}] [{source}] {message}\n";
            if (source == "Operation")
                OperationLog.Write("Operation", message);
            SystemLogOnTop.AppendText(logLine);
            SystemLogOnTop.ScrollToCaret();

            // 限制行數
            if (SystemLogOnTop.Lines.Length > 200)
            {
                SystemLogOnTop.Clear();
            }
        }

        // ============================
        // 載入示範資料
        // ============================
        private void LoadDemoData()
        {
            // Dispatch List 示範
            dgvDispatchList.Rows.Add("TASK-001", "Station A", "Station B", "High", "In Progress");
            dgvDispatchList.Rows.Add("TASK-002", "Station B", "Station C", "Medium", "Pending");
            dgvDispatchList.Rows.Add("TASK-003", "Station C", "Charging", "Low", "Queued");

            // Planned Route 示範
            lstPlannedRoute.Items.Add("1. Current Position → Corridor A");
            lstPlannedRoute.Items.Add("2. Corridor A → Intersection 3");
            lstPlannedRoute.Items.Add("3. Intersection 3 → Corridor B");
            lstPlannedRoute.Items.Add("4. Corridor B → Station B (Destination)");
            lstPlannedRoute.Items.Add("───────────────────────────");
            lstPlannedRoute.Items.Add($"Estimated Distance: 45.2 m");
            lstPlannedRoute.Items.Add($"Estimated Time: 2 min 15 sec");
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
