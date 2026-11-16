using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AML;
using AML_Library;

namespace MobileRobot
{
    public partial class UR_Robot : UserControl
    {
        public enum ToolControlState { Offline, Remote, Local };
        public enum URCommand { Remote = 1, Local, Home, AlmClear, GoBarcode, Get, Put, Clamp, Unclamp };

        public delegate void BarcodeReportEvent(string Action, string Target, string Slot, string Barcode1, string Barcode2);
        public event AlarmEvent Alarm;
        public event BarcodeReportEvent BarcodeReport;

        public AML_LogPage Log = new AML_LogPage();
        public string ModuleName = "UR_Robot";
        public int TimeOut = 60;

        private SocketServer ToolSocket;
        private Thread SendTool_CmdThread;
        private BlockQueue<string> SendCmdQueue = new BlockQueue<string>();
        private readonly StringBuilder RecvBuffer = new StringBuilder();
        private readonly object RecvLock = new object();
        private readonly object CmdLock = new object();
        private readonly Dictionary<URCommand, PendingCmd> PendingCmds = new Dictionary<URCommand, PendingCmd>();
        private const int RecvBufferLimit = 8192;

        private string UR_IP;
        private int UR_Port;

        private class PendingCmd
        {
            public URCommand Cmd;
            public string EndEvtId;
            public string Target;
            public string Slot;
            public bool AckReceived;
            public System.Threading.Timer Timer;
        }

        //========狀態========
        private bool _connect;
        private bool _busy;
        private string _currentAction = "Idle";
        private ToolControlState _controlState = ToolControlState.Offline;
        private bool? _gripperClosed;

        #region SECS Item
        private const int ALID_URDisconnect = 1000000;
        private const int ALID_URDisconnectInMotion = 1000001;
        private const int ALID_AckTimeoutBase = 2000000;
        private const int ALID_AckFailBase = 3000000;
        private const int ALID_EndTimeoutBase = 4000000;
        #endregion

        #region Tool EventID
        private const string EVT_HomeEnd = "20000";
        private const string EVT_AlmClearAll = "10001";
        private const string EVT_GoBarcodeStart = "20001";
        private const string EVT_GoBarcodeEnd = "20002";
        private const string EVT_GetStart = "20003";
        private const string EVT_GetEnd = "20004";
        private const string EVT_PutStart = "20005";
        private const string EVT_PutEnd = "20006";
        private const string EVT_ClampOpen = "20101";
        private const string EVT_ClampClose = "20102";
        private const string EVT_CCDAligment = "20201";
        #endregion

        private static readonly string[] Targets = { "Tool", "MR", "Station", "Buffer" };
        private const int SlotMin = 1;
        private const int SlotMax = 108;

        //========設定========
        public bool Connect
        {
            set
            {
                _connect = value;
                RunOnUI(() =>
                {
                    lblConnect.BackColor = value ? Color.LightGreen : Color.Red;
                    UpdateButtons();
                    picRobot.Invalidate();
                });
            }
            get { return _connect; }
        }
        public bool Busy
        {
            set
            {
                _busy = value;
                RunOnUI(() =>
                {
                    UpdateButtons();
                    picRobot.Invalidate();
                });
            }
            get { return _busy; }
        }
        public string CurrentAction
        {
            set
            {
                _currentAction = value;
                RunOnUI(() => lblAction.Text = value);
            }
            get { return _currentAction; }
        }
        public ToolControlState ControlState
        {
            set
            {
                _controlState = value;
                RunOnUI(() =>
                {
                    lblControlState.Text = value.ToString();
                    switch (value)
                    {
                        case ToolControlState.Offline:
                            lblControlState.BackColor = Color.LightPink;
                            break;
                        case ToolControlState.Remote:
                            lblControlState.BackColor = Color.LightGreen;
                            break;
                        case ToolControlState.Local:
                            lblControlState.BackColor = Color.LightYellow;
                            break;
                    }
                });
            }
            get { return _controlState; }
        }
        private void RunOnUI(Action action)
        {
            if (InvokeRequired)
            {
                if (IsHandleCreated && !IsDisposed)
                    this.BeginInvoke(action);
            }
            else
                action();
        }

        public UR_Robot()
        {
            InitializeComponent();
            foreach (URCommand cmd in Enum.GetValues(typeof(URCommand)))
                cbCommand.Items.Add(cmd);
            cbTarget.Items.AddRange(Targets);
            cbTarget.SelectedIndex = 0;
            for (int i = SlotMin; i <= SlotMax; i++)
                cbSlot.Items.Add(i.ToString());
            cbSlot.SelectedIndex = 0;
            cbCommand.SelectedIndex = 0;
            UpdateButtons();
        }

        public void Initial()
        {
            #region Setting
            string LogPath = XML.Read("Setting", "Setting/Form1/Logfolder", "C:\\AML");
            int LogCounterLimit = Convert.ToUInt16(XML.Read("Setting", "Setting/Form1/LogCountLimit", "200"));
            int LogFolderLimit = Convert.ToUInt16(XML.Read("Setting", "Setting/Form1/LogFolderLimit", "30"));
            string XmlNode = "Setting/UR_Robot/";
            UR_IP = XML.Read("Setting", XmlNode + "ListenIP", "0.0.0.0");
            UR_Port = Convert.ToInt32(XML.Read("Setting", XmlNode + "Port", "5000"));
            TimeOut = Convert.ToInt32(XML.Read("Setting", XmlNode + "TimeOut", "60"));
            Log.Initial(ModuleName, false, true, LogPath, LogCounterLimit, LogFolderLimit);
            WriteLog("ListenIP: " + UR_IP + ", Port: " + UR_Port + ", TimeOut: " + TimeOut + "s");
            #endregion

            //===== Tool Socket Initial =====
            try
            {
                lblConnect.Text = ModuleName;
                ToolSocket = new SocketServer(UR_IP, UR_Port);
                ToolSocket.ConnectEvent += ConnectEvent;
                ToolSocket.DisconnectEvent += DisconnectEvent;
                ToolSocket.DataArrivalEvent += DataArrivalEvent;
                ToolSocket.CloseEvent += CloseEvent;
                ToolSocket.ErrorEvent += ErrorEvent;

                SendTool_CmdThread = new Thread(SendCmdThread_Dowork);
                SendTool_CmdThread.IsBackground = true;
                SendTool_CmdThread.Start();

                ToolSocket.Listening(UR_IP, UR_Port);
                WriteLog("Socket Server listening " + UR_IP + ":" + UR_Port);
            }
            catch (Exception ex)
            { WriteLog(ModuleName + " Socket Initial Fail: " + ex.Message); }
        }

        //=====Socket=====
        public void SocketListen()
        {
            if (ToolSocket == null)
                return;
            try
            {
                ToolSocket.Socket_Close();
                ToolSocket.Listening();
                WriteLog("Socket Server re-listening " + UR_IP + ":" + UR_Port);
            }
            catch (Exception ex)
            { WriteLog("Socket Server re-listen fail: " + ex.Message); }
        }
        private void RestartListen()
        {
            Thread th = new Thread(() =>
            {
                Thread.Sleep(500);
                SocketListen();
            });
            th.IsBackground = true;
            th.Start();
        }
        private void ConnectEvent()
        {
            lock (RecvLock)
                RecvBuffer.Clear();
            Connect = true;
            WriteLog(ModuleName + " connect successful");
            Remote();
        }
        private void DisconnectEvent()
        {
            bool inMotion = Busy;
            ConnectionLost();
            WriteLog(ModuleName + " disconnect");
            if (inMotion)
                SendAlarm(ALID_URDisconnectInMotion.ToString(), ModuleName + " disconnect during motion");
            RestartListen();
        }
        private void ErrorEvent(string EX)
        {
            bool inMotion = Busy;
            ConnectionLost();
            WriteLog(ModuleName + " abnormal disconnect: " + EX);
            if (inMotion)
                SendAlarm(ALID_URDisconnectInMotion.ToString(), ModuleName + " disconnect during motion");
            else
                SendAlarm(ALID_URDisconnect.ToString(), ModuleName + " abnormal disconnect");
            RestartListen();
        }
        private void CloseEvent()
        {
            WriteLog("Stop " + ModuleName + " connection");
        }
        private void ConnectionLost()
        {
            Connect = false;
            ControlState = ToolControlState.Offline;
            ClearPending();
            SendCmdQueue.Clear();
            lock (RecvLock)
                RecvBuffer.Clear();
        }

        //=====Receive=====
        private void DataArrivalEvent(string Data)
        {
            List<string> messages = new List<string>();
            lock (RecvLock)
            {
                RecvBuffer.Append(Data);
                string buffer = RecvBuffer.ToString();
                int end;
                while ((end = buffer.IndexOf('@')) >= 0)
                {
                    string segment = buffer.Substring(0, end);
                    buffer = buffer.Substring(end + 1);
                    int start = segment.LastIndexOf('~');
                    if (start < 0)
                    {
                        if (segment.Trim().Length > 0)
                            WriteLog("UR -> AML discard: " + segment.Trim());
                        continue;
                    }
                    messages.Add(segment.Substring(start + 1).Trim());
                }
                if (buffer.Length > RecvBufferLimit)
                {
                    WriteLog("UR -> AML buffer overflow, discard " + buffer.Length + " chars");
                    buffer = string.Empty;
                }
                RecvBuffer.Clear();
                RecvBuffer.Append(buffer);
            }

            foreach (string msg in messages)
            {
                Log.Write("UR -> AML", "~" + msg + "@");
                try
                {
                    ToolReceiveHandler(msg.Split(',').Select(s => s.Trim()).ToArray());
                }
                catch (Exception ex)
                {
                    WriteLog("UR -> AML handle fail: " + ex.Message);
                }
            }
        }
        private void ToolReceiveHandler(string[] Received)
        {
            switch (Received[0])
            {
                case "Ack":     // Ack,Action,[Param1],rslt
                    AckHandler(Received);
                    break;
                case "Evt":     // Evt,EventID,Action,...
                    EvtHandler(Received);
                    break;
                case "Alm":     // Alm,code,description
                    string code = Received.Length > 1 ? Received[1] : string.Empty;
                    string desc = Received.Length > 2 ? string.Join(",", Received.Skip(2)) : string.Empty;
                    SendAlarm(code, desc);
                    break;
                default:
                    WriteLog(ModuleName + " 回報 Unknown Message: " + string.Join(",", Received));
                    break;
            }
        }
        private void AckHandler(string[] Received)
        {
            if (Received.Length < 3)
            {
                WriteLog("Ack format error: " + string.Join(",", Received));
                return;
            }
            URCommand cmd;
            if (!Enum.TryParse(Received[1], out cmd))
            {
                WriteLog("Ack unknown action: " + Received[1]);
                return;
            }
            string rslt = Received[Received.Length - 1];

            PendingCmd pending;
            lock (CmdLock)
            {
                if (!PendingCmds.TryGetValue(cmd, out pending))
                {
                    WriteLog("Ack without pending command: " + cmd);
                    return;
                }
                pending.AckReceived = true;
            }

            if (rslt != "0")
            {
                CompletePending(cmd);
                SendAlarm((ALID_AckFailBase + (int)cmd).ToString(), ModuleName + " " + cmd + " Ack fail, rslt=" + rslt);
                return;
            }

            switch (cmd)
            {
                case URCommand.Remote:
                    ControlState = ToolControlState.Remote;
                    break;
                case URCommand.Local:
                    ControlState = ToolControlState.Local;
                    break;
            }

            if (pending.EndEvtId == null)
                CompletePending(cmd);
            else
                WriteLog(cmd + " Ack OK, wait Evt " + pending.EndEvtId);
        }
        private void EvtHandler(string[] Received)
        {
            if (Received.Length < 2)
            {
                WriteLog("Evt format error: " + string.Join(",", Received));
                return;
            }
            string evtId = Received[1];
            switch (evtId)
            {
                case EVT_GoBarcodeStart:
                case EVT_GetStart:
                case EVT_PutStart:
                    CurrentAction = Received[2] + " Start";
                    WriteLog(Received[2] + " Start");
                    break;
                case EVT_GoBarcodeEnd:
                case EVT_GetEnd:
                    {
                        string barcode1 = Received.Length > 4 ? Received[4] : string.Empty;
                        string barcode2 = Received.Length > 5 ? Received[5] : string.Empty;
                        URCommand cmd = evtId == EVT_GetEnd ? URCommand.Get : URCommand.GoBarcode;
                        PendingCmd pending = CompleteByEvt(evtId);
                        SetBarcodeLabel(barcode1, barcode2);
                        WriteLog(cmd + " End, Barcode1=" + barcode1 + ", Barcode2=" + barcode2);
                        if (BarcodeReport != null)
                            BarcodeReport(cmd.ToString(), pending?.Target ?? string.Empty, pending?.Slot ?? string.Empty, barcode1, barcode2);
                    }
                    break;
                case EVT_HomeEnd:
                case EVT_PutEnd:
                case EVT_ClampOpen:
                case EVT_ClampClose:
                    if (evtId == EVT_ClampOpen || evtId == EVT_ClampClose)
                    {
                        _gripperClosed = evtId == EVT_ClampClose;
                        RunOnUI(picRobot.Invalidate);
                    }
                    CompleteByEvt(evtId);
                    WriteLog("Evt " + evtId + " " + string.Join(",", Received.Skip(2)));
                    break;
                case EVT_AlmClearAll:
                    CompleteByEvt(evtId);
                    WriteLog("UR Alarm Clear All");
                    break;
                case EVT_CCDAligment:
                    WriteLog("CCDAligment: " + string.Join(",", Received.Skip(3)));
                    break;
                default:
                    WriteLog(ModuleName + " 回報 Unknown Event: " + string.Join(",", Received));
                    break;
            }
        }

        //=====Pending=====
        private PendingCmd CompleteByEvt(string evtId)
        {
            URCommand? cmd = null;
            lock (CmdLock)
            {
                foreach (var kv in PendingCmds)
                {
                    if (kv.Value.EndEvtId == evtId)
                    {
                        cmd = kv.Key;
                        break;
                    }
                }
            }
            if (cmd == null)
            {
                WriteLog("Evt " + evtId + " without pending command");
                return null;
            }
            return CompletePending(cmd.Value);
        }
        private PendingCmd CompletePending(URCommand cmd)
        {
            PendingCmd pending;
            bool motionLeft;
            lock (CmdLock)
            {
                if (!PendingCmds.TryGetValue(cmd, out pending))
                    return null;
                PendingCmds.Remove(cmd);
                pending.Timer?.Dispose();
                motionLeft = PendingCmds.Keys.Any(IsMotionCommand);
            }
            if (IsMotionCommand(cmd) && !motionLeft)
            {
                Busy = false;
                CurrentAction = "Idle";
            }
            WriteLog(cmd + " complete");
            return pending;
        }
        private void ClearPending()
        {
            lock (CmdLock)
            {
                foreach (var p in PendingCmds.Values)
                    p.Timer?.Dispose();
                PendingCmds.Clear();
            }
            Busy = false;
            CurrentAction = "Idle";
        }
        private void PendingTimeout(object state)
        {
            URCommand cmd = (URCommand)state;
            PendingCmd pending;
            lock (CmdLock)
            {
                if (!PendingCmds.TryGetValue(cmd, out pending))
                    return;
            }
            CompletePending(cmd);
            if (!pending.AckReceived)
                SendAlarm((ALID_AckTimeoutBase + (int)cmd).ToString(), ModuleName + " " + cmd + " Ack timeout");
            else
                SendAlarm((ALID_EndTimeoutBase + (int)cmd).ToString(), ModuleName + " " + cmd + " End timeout");
        }
        private static bool IsMotionCommand(URCommand cmd)
        {
            switch (cmd)
            {
                case URCommand.Home:
                case URCommand.GoBarcode:
                case URCommand.Get:
                case URCommand.Put:
                case URCommand.Clamp:
                case URCommand.Unclamp:
                    return true;
                default:
                    return false;
            }
        }

        //=====Send=====
        private bool SendCommand(URCommand cmd, string endEvtId, string target = null, string slot = null)
        {
            if (!Connect)
            {
                WriteLog(ModuleName + " is Disconnect 無法接受指令: " + cmd);
                return false;
            }
            bool isMotion = IsMotionCommand(cmd);
            lock (CmdLock)
            {
                if (isMotion && PendingCmds.Keys.Any(IsMotionCommand))
                {
                    WriteLog(ModuleName + " is Busy 無法接受指令: " + cmd);
                    return false;
                }
                if (PendingCmds.ContainsKey(cmd))
                {
                    WriteLog(cmd + " 尚未完成，無法重複下指令");
                    return false;
                }
                PendingCmd pending = new PendingCmd { Cmd = cmd, EndEvtId = endEvtId, Target = target, Slot = slot };
                PendingCmds[cmd] = pending;
                pending.Timer = new System.Threading.Timer(PendingTimeout, cmd, TimeOut * 1000, Timeout.Infinite);
            }
            if (isMotion)
            {
                Busy = true;
                CurrentAction = cmd.ToString();
            }

            string Cmd = "Cmd," + cmd;
            if (target != null)
                Cmd += "," + target + "," + slot;
            SendCmdQueue.EnQueue(Cmd);
            return true;
        }
        private void SendCmdThread_Dowork()
        {
            string Cmd;
            while (true)
            {
                Cmd = SendCmdQueue.DeQueue();
                if (!Connect)
                {
                    WriteLog(ModuleName + " is disconnect");
                    continue;
                }
                try
                {
                    Cmd = "~" + Cmd + "@";
                    Log.Write("AML -> UR", Cmd);
                    ToolSocket.SendData(Cmd);
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    WriteLog(ModuleName + " Send Data Fail: " + ex.Message);
                }
            }
        }
        private bool CheckTargetSlot(string target, int slot)
        {
            if (!Targets.Contains(target))
            {
                WriteLog("Target 錯誤: " + target);
                return false;
            }
            if (slot < SlotMin || slot > SlotMax)
            {
                WriteLog("Slot 超出範圍 (" + SlotMin + "~" + SlotMax + "): " + slot);
                return false;
            }
            return true;
        }

        //=====Tool 指令集=====
        public bool Remote() { return SendCommand(URCommand.Remote, null); }
        public bool Local() { return SendCommand(URCommand.Local, null); }
        public bool AlmClear() { return SendCommand(URCommand.AlmClear, EVT_AlmClearAll); }
        public bool Home() { return SendCommand(URCommand.Home, EVT_HomeEnd); }
        public bool Clamp() { return SendCommand(URCommand.Clamp, EVT_ClampClose); }
        public bool Unclamp() { return SendCommand(URCommand.Unclamp, EVT_ClampOpen); }
        public bool GoBarcode(string target, int slot)
        {
            if (!CheckTargetSlot(target, slot)) return false;
            return SendCommand(URCommand.GoBarcode, EVT_GoBarcodeEnd, target, slot.ToString());
        }
        public bool Get(string target, int slot)
        {
            if (!CheckTargetSlot(target, slot)) return false;
            return SendCommand(URCommand.Get, EVT_GetEnd, target, slot.ToString());
        }
        public bool Put(string target, int slot)
        {
            if (!CheckTargetSlot(target, slot)) return false;
            return SendCommand(URCommand.Put, EVT_PutEnd, target, slot.ToString());
        }

        //=====Alarm=====
        private void SendAlarm(string Code, string Description)
        {
            WriteLog("Alarm " + Code + ": " + Description);
            if (Alarm != null)
                Alarm(Code, Description);
        }

        //=====UI=====
        private void UpdateButtons()
        {
            if (!(cbCommand.SelectedItem is URCommand))
            {
                btnSend.Enabled = false;
                return;
            }
            URCommand cmd = (URCommand)cbCommand.SelectedItem;
            bool needTarget = cmd == URCommand.GoBarcode || cmd == URCommand.Get || cmd == URCommand.Put;
            cbTarget.Enabled = needTarget;
            cbSlot.Enabled = needTarget;
            btnSend.Enabled = _connect && (!IsMotionCommand(cmd) || !_busy);
        }
        private void SetBarcodeLabel(string barcode1, string barcode2)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate () { SetBarcodeLabel(barcode1, barcode2); }));
                return;
            }
            lblBarcode.Text = "Barcode : " + barcode1 + " / " + barcode2;
        }
        private void WriteLog(string msg)
        {
            Log.WriteSystemLog(ModuleName, msg);
        }

        //=====Robot 示意圖=====
        private void picRobot_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int W = picRobot.ClientSize.Width, H = picRobot.ClientSize.Height;
            float s = Math.Min(W / 500f, H / 245f);
            g.TranslateTransform((W - 500 * s) / 2, (H - 245 * s) / 2);
            g.ScaleTransform(s, s);

            Color body = Color.FromArgb(232, 236, 241);
            Color outline = Color.FromArgb(70, 80, 95);
            Color jointBlue = _connect ? Color.FromArgb(0, 150, 214) : Color.FromArgb(150, 160, 170);

            using (Pen outlinePen = new Pen(outline, 2.5f))
            using (SolidBrush bodyBrush = new SolidBrush(body))
            using (SolidBrush blueBrush = new SolidBrush(jointBlue))
            {
                // 地面陰影
                using (SolidBrush shadow = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
                    g.FillEllipse(shadow, 95, 222, 150, 16);

                // 底座
                FillRound(g, bodyBrush, outlinePen, new RectangleF(110, 175, 120, 52), 14);
                // 紅綠燈 (紅:Offline / 黃:Busy / 綠:Ready)
                int active = !_connect ? 0 : _busy ? 1 : 2;
                Color[] lampOn = { Color.FromArgb(235, 60, 60), Color.FromArgb(255, 190, 30), Color.FromArgb(50, 205, 100) };
                using (SolidBrush housing = new SolidBrush(Color.FromArgb(55, 60, 70)))
                    FillRound(g, housing, outlinePen, new RectangleF(132, 191, 76, 26), 13);
                for (int i = 0; i < 3; i++)
                {
                    float cx = 145 + i * 25, cy = 204;
                    Color c = i == active ? lampOn[i] : Color.FromArgb(70, lampOn[i]);
                    if (i == active)
                        using (SolidBrush glow = new SolidBrush(Color.FromArgb(90, lampOn[i])))
                            g.FillEllipse(glow, cx - 12, cy - 12, 24, 24);
                    using (SolidBrush lampBrush = new SolidBrush(c))
                        g.FillEllipse(lampBrush, cx - 8, cy - 8, 16, 16);
                }

                // 姿態：動作中手臂前伸
                PointF shoulder = new PointF(170, 160);
                PointF elbow = _busy ? new PointF(240, 70) : new PointF(215, 60);
                PointF wrist = _busy ? new PointF(350, 100) : new PointF(315, 85);
                PointF tool = new PointF(wrist.X + 8, wrist.Y + 55);

                DrawLink(g, bodyBrush, outlinePen, shoulder, elbow, 34);
                DrawLink(g, bodyBrush, outlinePen, elbow, wrist, 28);
                DrawLink(g, bodyBrush, outlinePen, wrist, tool, 20);

                DrawJoint(g, blueBrush, outlinePen, shoulder, 27);
                DrawJoint(g, blueBrush, outlinePen, elbow, 23);
                DrawJoint(g, blueBrush, outlinePen, wrist, 19);

                // 夷爪
                float gap = _gripperClosed == true ? 4 : 16;
                using (SolidBrush gripBrush = new SolidBrush(Color.FromArgb(90, 100, 115)))
                {
                    FillRound(g, gripBrush, outlinePen, new RectangleF(tool.X - 24, tool.Y - 4, 48, 14), 5);
                    FillRound(g, gripBrush, outlinePen, new RectangleF(tool.X - gap - 8, tool.Y + 10, 8, 30), 3);
                    FillRound(g, gripBrush, outlinePen, new RectangleF(tool.X + gap, tool.Y + 10, 8, 30), 3);
                }
            }

            // 狀態文字
            string state = !_connect ? "Offline" : _busy ? _currentAction : "Ready";
            string grip = _gripperClosed == null ? "Unknown" : _gripperClosed == true ? "Clamp" : "Unclamp";
            using (Font f = new Font("微軟正黑體", 11F, FontStyle.Bold))
            using (Font f2 = new Font("微軟正黑體", 9F))
            using (SolidBrush txt = new SolidBrush(Color.FromArgb(70, 80, 95)))
            {
                g.DrawString("UR Robot", f, txt, 390, 165);
                g.DrawString("State : " + state, f2, txt, 390, 190);
                g.DrawString("Gripper : " + grip, f2, txt, 390, 208);
            }
        }
        private static void DrawLink(Graphics g, Brush brush, Pen pen, PointF a, PointF b, float width)
        {
            float dx = b.X - a.X, dy = b.Y - a.Y;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);
            float angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
            var state = g.Save();
            g.TranslateTransform(a.X, a.Y);
            g.RotateTransform(angle);
            FillRound(g, brush, pen, new RectangleF(0, -width / 2, len, width), width / 2);
            g.Restore(state);
        }
        private static void DrawJoint(Graphics g, Brush brush, Pen pen, PointF c, float r)
        {
            g.FillEllipse(brush, c.X - r, c.Y - r, r * 2, r * 2);
            g.DrawEllipse(pen, c.X - r, c.Y - r, r * 2, r * 2);
            using (SolidBrush shine = new SolidBrush(Color.FromArgb(110, 255, 255, 255)))
                g.FillEllipse(shine, c.X - r * 0.6f, c.Y - r * 0.7f, r * 0.7f, r * 0.5f);
        }
        private static void FillRound(Graphics g, Brush brush, Pen pen, RectangleF r, float radius)
        {
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        }

        private void cbCommand_SelectedIndexChanged(object sender, EventArgs e) { UpdateButtons(); }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!(cbCommand.SelectedItem is URCommand))
                return;
            string target = cbTarget.Text;
            int slot = Convert.ToInt32(cbSlot.Text);
            switch ((URCommand)cbCommand.SelectedItem)
            {
                case URCommand.Remote: Remote(); break;
                case URCommand.Local: Local(); break;
                case URCommand.Home: Home(); break;
                case URCommand.AlmClear: AlmClear(); break;
                case URCommand.GoBarcode: GoBarcode(target, slot); break;
                case URCommand.Get: Get(target, slot); break;
                case URCommand.Put: Put(target, slot); break;
                case URCommand.Clamp: Clamp(); break;
                case URCommand.Unclamp: Unclamp(); break;
            }
        }
    }
}
