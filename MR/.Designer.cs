namespace MobileRobot
{
    partial class MRControl
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.gbRobotInfo = new System.Windows.Forms.GroupBox();
            this.lblIpTitle = new System.Windows.Forms.Label();
            this.lblIpVal = new System.Windows.Forms.Label();
            this.lblMrNumberTitle = new System.Windows.Forms.Label();
            this.lblMrNumberVal = new System.Windows.Forms.Label();
            this.lblFirmwareTitle = new System.Windows.Forms.Label();
            this.lblFirmwareVal = new System.Windows.Forms.Label();
            this.lblOdometerTitle = new System.Windows.Forms.Label();
            this.lblOdometerVal = new System.Windows.Forms.Label();
            this.lblErrorCodeTitle = new System.Windows.Forms.Label();
            this.lblErrorCodeVal = new System.Windows.Forms.Label();
            this.gbRobotControl = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.gbMotorStatus = new System.Windows.Forms.GroupBox();
            this.lblLeftMotorTitle = new System.Windows.Forms.Label();
            this.lblLeftMotorVal = new System.Windows.Forms.Label();
            this.lblRightMotorTitle = new System.Windows.Forms.Label();
            this.lblRightMotorVal = new System.Windows.Forms.Label();
            this.lblTemperatureTitle = new System.Windows.Forms.Label();
            this.lblTemperatureVal = new System.Windows.Forms.Label();
            this.lblBatteryTitle = new System.Windows.Forms.Label();
            this.lblBatteryVal = new System.Windows.Forms.Label();
            this.lblChargingTitle = new System.Windows.Forms.Label();
            this.lblChargingVal = new System.Windows.Forms.Label();
            this.gbSensorStatus = new System.Windows.Forms.GroupBox();
            this.urRobot1 = new MobileRobot.UR_Robot();
            this.gbMqttAndMission = new System.Windows.Forms.GroupBox();
            this.lblOperation = new System.Windows.Forms.Label();
            this.lblTopicStatusTitle = new System.Windows.Forms.Label();
            this.lblTopicStatusVal = new System.Windows.Forms.Label();
            this.lblMissionIdTitle = new System.Windows.Forms.Label();
            this.lblMissionIdVal = new System.Windows.Forms.Label();
            this.lblMissionStatusTitle = new System.Windows.Forms.Label();
            this.lblMissionStatusVal = new System.Windows.Forms.Label();
            this.lblStartPointTitle = new System.Windows.Forms.Label();
            this.lblStartPointVal = new System.Windows.Forms.Label();
            this.lblCurrentTargetTitle = new System.Windows.Forms.Label();
            this.lblCurrentTargetVal = new System.Windows.Forms.Label();
            this.PelMqttOperation = new System.Windows.Forms.Panel();
            this.BT_MqttDisconnect = new System.Windows.Forms.Button();
            this.BT_MqttReconnect = new System.Windows.Forms.Button();
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.lblPosXTitle = new System.Windows.Forms.Label();
            this.lblPosXVal = new System.Windows.Forms.Label();
            this.lblPosYTitle = new System.Windows.Forms.Label();
            this.lblPosYVal = new System.Windows.Forms.Label();
            this.lblThetaTitle = new System.Windows.Forms.Label();
            this.lblThetaVal = new System.Windows.Forms.Label();
            this.lblMapIdTitle = new System.Windows.Forms.Label();
            this.lblMapIdVal = new System.Windows.Forms.Label();
            this.lblCurrentPointTitle = new System.Windows.Forms.Label();
            this.lblCurrentPointVal = new System.Windows.Forms.Label();
            this.BT_OrderTest = new System.Windows.Forms.Button();
            this.BT_InstantTest = new System.Windows.Forms.Button();
            this.gbRobotInfo.SuspendLayout();
            this.gbRobotControl.SuspendLayout();
            this.gbMotorStatus.SuspendLayout();
            this.gbSensorStatus.SuspendLayout();
            this.gbMqttAndMission.SuspendLayout();
            this.PelMqttOperation.SuspendLayout();
            this.gbLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRobotInfo
            // 
            this.gbRobotInfo.Controls.Add(this.lblIpTitle);
            this.gbRobotInfo.Controls.Add(this.lblIpVal);
            this.gbRobotInfo.Controls.Add(this.lblMrNumberTitle);
            this.gbRobotInfo.Controls.Add(this.lblMrNumberVal);
            this.gbRobotInfo.Controls.Add(this.lblFirmwareTitle);
            this.gbRobotInfo.Controls.Add(this.lblFirmwareVal);
            this.gbRobotInfo.Controls.Add(this.lblOdometerTitle);
            this.gbRobotInfo.Controls.Add(this.lblOdometerVal);
            this.gbRobotInfo.Controls.Add(this.lblErrorCodeTitle);
            this.gbRobotInfo.Controls.Add(this.lblErrorCodeVal);
            this.gbRobotInfo.Location = new System.Drawing.Point(30, 20);
            this.gbRobotInfo.Name = "gbRobotInfo";
            this.gbRobotInfo.Size = new System.Drawing.Size(520, 380);
            this.gbRobotInfo.TabIndex = 0;
            this.gbRobotInfo.TabStop = false;
            this.gbRobotInfo.Text = "MR Information";
            // 
            // lblIpTitle
            // 
            this.lblIpTitle.Location = new System.Drawing.Point(30, 45);
            this.lblIpTitle.Name = "lblIpTitle";
            this.lblIpTitle.Size = new System.Drawing.Size(150, 30);
            this.lblIpTitle.TabIndex = 0;
            this.lblIpTitle.Text = "IP Address :";
            // 
            // lblIpVal
            // 
            this.lblIpVal.Location = new System.Drawing.Point(200, 45);
            this.lblIpVal.Name = "lblIpVal";
            this.lblIpVal.Size = new System.Drawing.Size(300, 30);
            this.lblIpVal.TabIndex = 1;
            this.lblIpVal.Text = "192.168.1.100";
            // 
            // lblMrNumberTitle
            // 
            this.lblMrNumberTitle.Location = new System.Drawing.Point(30, 105);
            this.lblMrNumberTitle.Name = "lblMrNumberTitle";
            this.lblMrNumberTitle.Size = new System.Drawing.Size(150, 30);
            this.lblMrNumberTitle.TabIndex = 2;
            this.lblMrNumberTitle.Text = "MR Number :";
            // 
            // lblMrNumberVal
            // 
            this.lblMrNumberVal.Location = new System.Drawing.Point(200, 105);
            this.lblMrNumberVal.Name = "lblMrNumberVal";
            this.lblMrNumberVal.Size = new System.Drawing.Size(300, 30);
            this.lblMrNumberVal.TabIndex = 3;
            this.lblMrNumberVal.Text = "AMR-2000";
            // 
            // lblFirmwareTitle
            // 
            this.lblFirmwareTitle.Location = new System.Drawing.Point(30, 165);
            this.lblFirmwareTitle.Name = "lblFirmwareTitle";
            this.lblFirmwareTitle.Size = new System.Drawing.Size(150, 30);
            this.lblFirmwareTitle.TabIndex = 4;
            this.lblFirmwareTitle.Text = "Firmware :";
            // 
            // lblFirmwareVal
            // 
            this.lblFirmwareVal.Location = new System.Drawing.Point(200, 165);
            this.lblFirmwareVal.Name = "lblFirmwareVal";
            this.lblFirmwareVal.Size = new System.Drawing.Size(300, 30);
            this.lblFirmwareVal.TabIndex = 5;
            this.lblFirmwareVal.Text = "v3.2.1";
            // 
            // lblOdometerTitle
            // 
            this.lblOdometerTitle.Location = new System.Drawing.Point(30, 225);
            this.lblOdometerTitle.Name = "lblOdometerTitle";
            this.lblOdometerTitle.Size = new System.Drawing.Size(150, 30);
            this.lblOdometerTitle.TabIndex = 6;
            this.lblOdometerTitle.Text = "Odometer :";
            // 
            // lblOdometerVal
            // 
            this.lblOdometerVal.Location = new System.Drawing.Point(200, 225);
            this.lblOdometerVal.Name = "lblOdometerVal";
            this.lblOdometerVal.Size = new System.Drawing.Size(300, 30);
            this.lblOdometerVal.TabIndex = 7;
            this.lblOdometerVal.Text = "12,345 m";
            // 
            // lblErrorCodeTitle
            // 
            this.lblErrorCodeTitle.Location = new System.Drawing.Point(30, 285);
            this.lblErrorCodeTitle.Name = "lblErrorCodeTitle";
            this.lblErrorCodeTitle.Size = new System.Drawing.Size(150, 30);
            this.lblErrorCodeTitle.TabIndex = 8;
            this.lblErrorCodeTitle.Text = "Error Code :";
            // 
            // lblErrorCodeVal
            // 
            this.lblErrorCodeVal.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblErrorCodeVal.Location = new System.Drawing.Point(200, 285);
            this.lblErrorCodeVal.Name = "lblErrorCodeVal";
            this.lblErrorCodeVal.Size = new System.Drawing.Size(300, 30);
            this.lblErrorCodeVal.TabIndex = 9;
            this.lblErrorCodeVal.Text = "None";
            // 
            // gbRobotControl
            // 
            this.gbRobotControl.Controls.Add(this.BT_InstantTest);
            this.gbRobotControl.Controls.Add(this.BT_OrderTest);
            this.gbRobotControl.Controls.Add(this.label1);
            this.gbRobotControl.Controls.Add(this.btnStart);
            this.gbRobotControl.Controls.Add(this.btnPause);
            this.gbRobotControl.Controls.Add(this.btnResume);
            this.gbRobotControl.Controls.Add(this.btnStop);
            this.gbRobotControl.Controls.Add(this.btnReset);
            this.gbRobotControl.Controls.Add(this.btnHome);
            this.gbRobotControl.Location = new System.Drawing.Point(580, 20);
            this.gbRobotControl.Name = "gbRobotControl";
            this.gbRobotControl.Size = new System.Drawing.Size(520, 380);
            this.gbRobotControl.TabIndex = 1;
            this.gbRobotControl.TabStop = false;
            this.gbRobotControl.Text = "Robot Control";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 30);
            this.label1.TabIndex = 10;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(34, 277);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(34, 329);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 1;
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(115, 277);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(75, 23);
            this.btnResume.TabIndex = 2;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(115, 330);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(220, 277);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(220, 337);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(75, 23);
            this.btnHome.TabIndex = 5;
            // 
            // gbMotorStatus
            // 
            this.gbMotorStatus.Controls.Add(this.lblLeftMotorTitle);
            this.gbMotorStatus.Controls.Add(this.lblLeftMotorVal);
            this.gbMotorStatus.Controls.Add(this.lblRightMotorTitle);
            this.gbMotorStatus.Controls.Add(this.lblRightMotorVal);
            this.gbMotorStatus.Controls.Add(this.lblTemperatureTitle);
            this.gbMotorStatus.Controls.Add(this.lblTemperatureVal);
            this.gbMotorStatus.Controls.Add(this.lblBatteryTitle);
            this.gbMotorStatus.Controls.Add(this.lblBatteryVal);
            this.gbMotorStatus.Controls.Add(this.lblChargingTitle);
            this.gbMotorStatus.Controls.Add(this.lblChargingVal);
            this.gbMotorStatus.Location = new System.Drawing.Point(30, 420);
            this.gbMotorStatus.Name = "gbMotorStatus";
            this.gbMotorStatus.Size = new System.Drawing.Size(520, 390);
            this.gbMotorStatus.TabIndex = 2;
            this.gbMotorStatus.TabStop = false;
            this.gbMotorStatus.Text = "MR Status";
            // 
            // lblLeftMotorTitle
            // 
            this.lblLeftMotorTitle.Location = new System.Drawing.Point(30, 60);
            this.lblLeftMotorTitle.Name = "lblLeftMotorTitle";
            this.lblLeftMotorTitle.Size = new System.Drawing.Size(150, 30);
            this.lblLeftMotorTitle.TabIndex = 0;
            this.lblLeftMotorTitle.Text = "Left Motor :";
            // 
            // lblLeftMotorVal
            // 
            this.lblLeftMotorVal.Location = new System.Drawing.Point(200, 60);
            this.lblLeftMotorVal.Name = "lblLeftMotorVal";
            this.lblLeftMotorVal.Size = new System.Drawing.Size(300, 30);
            this.lblLeftMotorVal.TabIndex = 1;
            this.lblLeftMotorVal.Text = "0 RPM";
            // 
            // lblRightMotorTitle
            // 
            this.lblRightMotorTitle.Location = new System.Drawing.Point(30, 130);
            this.lblRightMotorTitle.Name = "lblRightMotorTitle";
            this.lblRightMotorTitle.Size = new System.Drawing.Size(150, 30);
            this.lblRightMotorTitle.TabIndex = 2;
            this.lblRightMotorTitle.Text = "Right Motor :";
            // 
            // lblRightMotorVal
            // 
            this.lblRightMotorVal.Location = new System.Drawing.Point(200, 130);
            this.lblRightMotorVal.Name = "lblRightMotorVal";
            this.lblRightMotorVal.Size = new System.Drawing.Size(300, 30);
            this.lblRightMotorVal.TabIndex = 3;
            this.lblRightMotorVal.Text = "0 RPM";
            // 
            // lblTemperatureTitle
            // 
            this.lblTemperatureTitle.Location = new System.Drawing.Point(30, 200);
            this.lblTemperatureTitle.Name = "lblTemperatureTitle";
            this.lblTemperatureTitle.Size = new System.Drawing.Size(150, 30);
            this.lblTemperatureTitle.TabIndex = 4;
            this.lblTemperatureTitle.Text = "Temperature :";
            // 
            // lblTemperatureVal
            // 
            this.lblTemperatureVal.Location = new System.Drawing.Point(200, 200);
            this.lblTemperatureVal.Name = "lblTemperatureVal";
            this.lblTemperatureVal.Size = new System.Drawing.Size(300, 30);
            this.lblTemperatureVal.TabIndex = 5;
            this.lblTemperatureVal.Text = "35.2 °C";
            // 
            // lblBatteryTitle
            // 
            this.lblBatteryTitle.Location = new System.Drawing.Point(30, 270);
            this.lblBatteryTitle.Name = "lblBatteryTitle";
            this.lblBatteryTitle.Size = new System.Drawing.Size(150, 30);
            this.lblBatteryTitle.TabIndex = 6;
            this.lblBatteryTitle.Text = "Battery :";
            // 
            // lblBatteryVal
            // 
            this.lblBatteryVal.Location = new System.Drawing.Point(200, 270);
            this.lblBatteryVal.Name = "lblBatteryVal";
            this.lblBatteryVal.Size = new System.Drawing.Size(300, 30);
            this.lblBatteryVal.TabIndex = 7;
            this.lblBatteryVal.Text = "- %";
            // 
            // lblChargingTitle
            // 
            this.lblChargingTitle.Location = new System.Drawing.Point(30, 330);
            this.lblChargingTitle.Name = "lblChargingTitle";
            this.lblChargingTitle.Size = new System.Drawing.Size(150, 30);
            this.lblChargingTitle.TabIndex = 8;
            this.lblChargingTitle.Text = "Charging :";
            // 
            // lblChargingVal
            // 
            this.lblChargingVal.Location = new System.Drawing.Point(200, 330);
            this.lblChargingVal.Name = "lblChargingVal";
            this.lblChargingVal.Size = new System.Drawing.Size(300, 30);
            this.lblChargingVal.TabIndex = 9;
            this.lblChargingVal.Text = "-";
            // 
            // gbSensorStatus
            // 
            this.gbSensorStatus.Controls.Add(this.urRobot1);
            this.gbSensorStatus.Location = new System.Drawing.Point(580, 420);
            this.gbSensorStatus.Name = "gbSensorStatus";
            this.gbSensorStatus.Size = new System.Drawing.Size(520, 390);
            this.gbSensorStatus.TabIndex = 3;
            this.gbSensorStatus.TabStop = false;
            this.gbSensorStatus.Text = "UR Robot";
            // 
            // urRobot1
            // 
            this.urRobot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.urRobot1.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.urRobot1.Name = "urRobot1";
            this.urRobot1.TabIndex = 0;
            // 
            // gbMqttAndMission
            // 
            this.gbMqttAndMission.Controls.Add(this.lblOperation);
            this.gbMqttAndMission.Controls.Add(this.lblTopicStatusTitle);
            this.gbMqttAndMission.Controls.Add(this.lblTopicStatusVal);
            this.gbMqttAndMission.Controls.Add(this.lblMissionIdTitle);
            this.gbMqttAndMission.Controls.Add(this.lblMissionIdVal);
            this.gbMqttAndMission.Controls.Add(this.lblMissionStatusTitle);
            this.gbMqttAndMission.Controls.Add(this.lblMissionStatusVal);
            this.gbMqttAndMission.Controls.Add(this.lblStartPointTitle);
            this.gbMqttAndMission.Controls.Add(this.lblStartPointVal);
            this.gbMqttAndMission.Controls.Add(this.lblCurrentTargetTitle);
            this.gbMqttAndMission.Controls.Add(this.lblCurrentTargetVal);
            this.gbMqttAndMission.Controls.Add(this.PelMqttOperation);
            this.gbMqttAndMission.Location = new System.Drawing.Point(1130, 20);
            this.gbMqttAndMission.Name = "gbMqttAndMission";
            this.gbMqttAndMission.Size = new System.Drawing.Size(560, 380);
            this.gbMqttAndMission.TabIndex = 4;
            this.gbMqttAndMission.TabStop = false;
            this.gbMqttAndMission.Text = "MQTT & Mission Status (3.5.4)";
            // 
            // lblOperation
            // 
            this.lblOperation.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblOperation.Location = new System.Drawing.Point(25, 322);
            this.lblOperation.Name = "lblOperation";
            this.lblOperation.Size = new System.Drawing.Size(160, 30);
            this.lblOperation.TabIndex = 10;
            this.lblOperation.Text = "Operation";
            // 
            // lblTopicStatusTitle
            // 
            this.lblTopicStatusTitle.Location = new System.Drawing.Point(25, 45);
            this.lblTopicStatusTitle.Name = "lblTopicStatusTitle";
            this.lblTopicStatusTitle.Size = new System.Drawing.Size(160, 30);
            this.lblTopicStatusTitle.TabIndex = 0;
            this.lblTopicStatusTitle.Text = "Topic Status :";
            // 
            // lblTopicStatusVal
            // 
            this.lblTopicStatusVal.Location = new System.Drawing.Point(200, 45);
            this.lblTopicStatusVal.Name = "lblTopicStatusVal";
            this.lblTopicStatusVal.Size = new System.Drawing.Size(340, 30);
            this.lblTopicStatusVal.TabIndex = 1;
            this.lblTopicStatusVal.Text = "Disconnected / Idle";
            // 
            // lblMissionIdTitle
            // 
            this.lblMissionIdTitle.Location = new System.Drawing.Point(25, 100);
            this.lblMissionIdTitle.Name = "lblMissionIdTitle";
            this.lblMissionIdTitle.Size = new System.Drawing.Size(160, 30);
            this.lblMissionIdTitle.TabIndex = 2;
            this.lblMissionIdTitle.Text = "Mission ID :";
            // 
            // lblMissionIdVal
            // 
            this.lblMissionIdVal.Location = new System.Drawing.Point(200, 100);
            this.lblMissionIdVal.Name = "lblMissionIdVal";
            this.lblMissionIdVal.Size = new System.Drawing.Size(340, 30);
            this.lblMissionIdVal.TabIndex = 3;
            this.lblMissionIdVal.Text = "-";
            // 
            // lblMissionStatusTitle
            // 
            this.lblMissionStatusTitle.Location = new System.Drawing.Point(25, 155);
            this.lblMissionStatusTitle.Name = "lblMissionStatusTitle";
            this.lblMissionStatusTitle.Size = new System.Drawing.Size(160, 30);
            this.lblMissionStatusTitle.TabIndex = 4;
            this.lblMissionStatusTitle.Text = "Mission Status :";
            // 
            // lblMissionStatusVal
            // 
            this.lblMissionStatusVal.Location = new System.Drawing.Point(200, 155);
            this.lblMissionStatusVal.Name = "lblMissionStatusVal";
            this.lblMissionStatusVal.Size = new System.Drawing.Size(340, 30);
            this.lblMissionStatusVal.TabIndex = 5;
            this.lblMissionStatusVal.Text = "-";
            // 
            // lblStartPointTitle
            // 
            this.lblStartPointTitle.Location = new System.Drawing.Point(25, 210);
            this.lblStartPointTitle.Name = "lblStartPointTitle";
            this.lblStartPointTitle.Size = new System.Drawing.Size(160, 30);
            this.lblStartPointTitle.TabIndex = 6;
            this.lblStartPointTitle.Text = "Start Point :";
            // 
            // lblStartPointVal
            // 
            this.lblStartPointVal.Location = new System.Drawing.Point(200, 210);
            this.lblStartPointVal.Name = "lblStartPointVal";
            this.lblStartPointVal.Size = new System.Drawing.Size(340, 30);
            this.lblStartPointVal.TabIndex = 7;
            this.lblStartPointVal.Text = "-";
            // 
            // lblCurrentTargetTitle
            // 
            this.lblCurrentTargetTitle.Location = new System.Drawing.Point(25, 265);
            this.lblCurrentTargetTitle.Name = "lblCurrentTargetTitle";
            this.lblCurrentTargetTitle.Size = new System.Drawing.Size(160, 30);
            this.lblCurrentTargetTitle.TabIndex = 8;
            this.lblCurrentTargetTitle.Text = "Current Target :";
            // 
            // lblCurrentTargetVal
            // 
            this.lblCurrentTargetVal.Location = new System.Drawing.Point(200, 265);
            this.lblCurrentTargetVal.Name = "lblCurrentTargetVal";
            this.lblCurrentTargetVal.Size = new System.Drawing.Size(340, 30);
            this.lblCurrentTargetVal.TabIndex = 9;
            this.lblCurrentTargetVal.Text = "-";
            // 
            // PelMqttOperation
            // 
            this.PelMqttOperation.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PelMqttOperation.Controls.Add(this.BT_MqttDisconnect);
            this.PelMqttOperation.Controls.Add(this.BT_MqttReconnect);
            this.PelMqttOperation.Location = new System.Drawing.Point(0, 315);
            this.PelMqttOperation.Name = "PelMqttOperation";
            this.PelMqttOperation.Size = new System.Drawing.Size(560, 65);
            this.PelMqttOperation.TabIndex = 14;
            // 
            // BT_MqttDisconnect
            // 
            this.BT_MqttDisconnect.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_MqttDisconnect.Location = new System.Drawing.Point(369, 29);
            this.BT_MqttDisconnect.Name = "BT_MqttDisconnect";
            this.BT_MqttDisconnect.Size = new System.Drawing.Size(115, 30);
            this.BT_MqttDisconnect.TabIndex = 14;
            this.BT_MqttDisconnect.Text = "Disconnect";
            this.BT_MqttDisconnect.UseVisualStyleBackColor = true;
            this.BT_MqttDisconnect.Click += new System.EventHandler(this.BT_MqttDisconnect_Click);
            // 
            // BT_MqttReconnect
            // 
            this.BT_MqttReconnect.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_MqttReconnect.Location = new System.Drawing.Point(225, 29);
            this.BT_MqttReconnect.Name = "BT_MqttReconnect";
            this.BT_MqttReconnect.Size = new System.Drawing.Size(115, 30);
            this.BT_MqttReconnect.TabIndex = 13;
            this.BT_MqttReconnect.Text = "Reconnect";
            this.BT_MqttReconnect.UseVisualStyleBackColor = true;
            this.BT_MqttReconnect.Click += new System.EventHandler(this.BT_MqttReconnect_Click);
            // 
            // gbLocation
            // 
            this.gbLocation.Controls.Add(this.lblPosXTitle);
            this.gbLocation.Controls.Add(this.lblPosXVal);
            this.gbLocation.Controls.Add(this.lblPosYTitle);
            this.gbLocation.Controls.Add(this.lblPosYVal);
            this.gbLocation.Controls.Add(this.lblThetaTitle);
            this.gbLocation.Controls.Add(this.lblThetaVal);
            this.gbLocation.Controls.Add(this.lblMapIdTitle);
            this.gbLocation.Controls.Add(this.lblMapIdVal);
            this.gbLocation.Controls.Add(this.lblCurrentPointTitle);
            this.gbLocation.Controls.Add(this.lblCurrentPointVal);
            this.gbLocation.Location = new System.Drawing.Point(1130, 420);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Size = new System.Drawing.Size(560, 390);
            this.gbLocation.TabIndex = 5;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "MR Location";
            // 
            // lblPosXTitle
            // 
            this.lblPosXTitle.Location = new System.Drawing.Point(25, 55);
            this.lblPosXTitle.Name = "lblPosXTitle";
            this.lblPosXTitle.Size = new System.Drawing.Size(160, 30);
            this.lblPosXTitle.TabIndex = 0;
            this.lblPosXTitle.Text = "MR Pos X :";
            // 
            // lblPosXVal
            // 
            this.lblPosXVal.Location = new System.Drawing.Point(200, 55);
            this.lblPosXVal.Name = "lblPosXVal";
            this.lblPosXVal.Size = new System.Drawing.Size(340, 30);
            this.lblPosXVal.TabIndex = 1;
            this.lblPosXVal.Text = "0.0";
            // 
            // lblPosYTitle
            // 
            this.lblPosYTitle.Location = new System.Drawing.Point(25, 115);
            this.lblPosYTitle.Name = "lblPosYTitle";
            this.lblPosYTitle.Size = new System.Drawing.Size(160, 30);
            this.lblPosYTitle.TabIndex = 2;
            this.lblPosYTitle.Text = "MR Pos Y :";
            // 
            // lblPosYVal
            // 
            this.lblPosYVal.Location = new System.Drawing.Point(200, 115);
            this.lblPosYVal.Name = "lblPosYVal";
            this.lblPosYVal.Size = new System.Drawing.Size(340, 30);
            this.lblPosYVal.TabIndex = 3;
            this.lblPosYVal.Text = "0.0";
            // 
            // lblThetaTitle
            // 
            this.lblThetaTitle.Location = new System.Drawing.Point(25, 175);
            this.lblThetaTitle.Name = "lblThetaTitle";
            this.lblThetaTitle.Size = new System.Drawing.Size(160, 30);
            this.lblThetaTitle.TabIndex = 4;
            this.lblThetaTitle.Text = "MR Theta :";
            // 
            // lblThetaVal
            // 
            this.lblThetaVal.Location = new System.Drawing.Point(200, 175);
            this.lblThetaVal.Name = "lblThetaVal";
            this.lblThetaVal.Size = new System.Drawing.Size(340, 30);
            this.lblThetaVal.TabIndex = 5;
            this.lblThetaVal.Text = "0.0°";
            // 
            // lblMapIdTitle
            // 
            this.lblMapIdTitle.Location = new System.Drawing.Point(25, 235);
            this.lblMapIdTitle.Name = "lblMapIdTitle";
            this.lblMapIdTitle.Size = new System.Drawing.Size(160, 30);
            this.lblMapIdTitle.TabIndex = 6;
            this.lblMapIdTitle.Text = "Map ID :";
            // 
            // lblMapIdVal
            // 
            this.lblMapIdVal.Location = new System.Drawing.Point(200, 235);
            this.lblMapIdVal.Name = "lblMapIdVal";
            this.lblMapIdVal.Size = new System.Drawing.Size(340, 30);
            this.lblMapIdVal.TabIndex = 7;
            this.lblMapIdVal.Text = "-";
            // 
            // lblCurrentPointTitle
            // 
            this.lblCurrentPointTitle.Location = new System.Drawing.Point(25, 295);
            this.lblCurrentPointTitle.Name = "lblCurrentPointTitle";
            this.lblCurrentPointTitle.Size = new System.Drawing.Size(160, 30);
            this.lblCurrentPointTitle.TabIndex = 8;
            this.lblCurrentPointTitle.Text = "Current Point :";
            // 
            // lblCurrentPointVal
            // 
            this.lblCurrentPointVal.Location = new System.Drawing.Point(200, 295);
            this.lblCurrentPointVal.Name = "lblCurrentPointVal";
            this.lblCurrentPointVal.Size = new System.Drawing.Size(340, 30);
            this.lblCurrentPointVal.TabIndex = 9;
            this.lblCurrentPointVal.Text = "-";
            // 
            // BT_OrderTest
            // 
            this.BT_OrderTest.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_OrderTest.Location = new System.Drawing.Point(34, 66);
            this.BT_OrderTest.Name = "BT_OrderTest";
            this.BT_OrderTest.Size = new System.Drawing.Size(115, 30);
            this.BT_OrderTest.TabIndex = 14;
            this.BT_OrderTest.Text = "OrderTest";
            this.BT_OrderTest.UseVisualStyleBackColor = true;
            this.BT_OrderTest.Click += new System.EventHandler(this.BT_OrderTest_Click);
            // 
            // BT_InstantTest
            // 
            this.BT_InstantTest.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.BT_InstantTest.Location = new System.Drawing.Point(204, 66);
            this.BT_InstantTest.Name = "BT_InstantTest";
            this.BT_InstantTest.Size = new System.Drawing.Size(115, 30);
            this.BT_InstantTest.TabIndex = 15;
            this.BT_InstantTest.Text = "InstantTest";
            this.BT_InstantTest.UseVisualStyleBackColor = true;
            this.BT_InstantTest.Click += new System.EventHandler(this.BT_InstantTest_Click);
            // 
            // MRControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.gbRobotInfo);
            this.Controls.Add(this.gbRobotControl);
            this.Controls.Add(this.gbMotorStatus);
            this.Controls.Add(this.gbSensorStatus);
            this.Controls.Add(this.gbMqttAndMission);
            this.Controls.Add(this.gbLocation);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MRControl";
            this.Size = new System.Drawing.Size(1720, 840);
            this.gbRobotInfo.ResumeLayout(false);
            this.gbRobotControl.ResumeLayout(false);
            this.gbMotorStatus.ResumeLayout(false);
            this.gbSensorStatus.ResumeLayout(false);
            this.gbMqttAndMission.ResumeLayout(false);
            this.PelMqttOperation.ResumeLayout(false);
            this.gbLocation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void SetupButton(System.Windows.Forms.Button btn, string text, System.Drawing.Color backColor, System.Drawing.Point loc, System.EventHandler onClick)
        {
            btn.Text = text;
            btn.Location = loc;
            btn.Size = new System.Drawing.Size(140, 100);
            btn.BackColor = backColor;
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            btn.UseVisualStyleBackColor = false;
            btn.Click += onClick;
        }

        #endregion

        // UI 元件宣告
        private System.Windows.Forms.GroupBox gbRobotInfo;
        private System.Windows.Forms.Label lblIpTitle;
        private System.Windows.Forms.Label lblIpVal;
        private System.Windows.Forms.Label lblMrNumberTitle;
        private System.Windows.Forms.Label lblMrNumberVal;
        private System.Windows.Forms.Label lblFirmwareTitle;
        private System.Windows.Forms.Label lblFirmwareVal;
        private System.Windows.Forms.Label lblOdometerTitle;
        private System.Windows.Forms.Label lblOdometerVal;
        private System.Windows.Forms.Label lblErrorCodeTitle;
        private System.Windows.Forms.Label lblErrorCodeVal;

        private System.Windows.Forms.GroupBox gbRobotControl;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnHome;

        private System.Windows.Forms.GroupBox gbMotorStatus;
        private System.Windows.Forms.Label lblLeftMotorTitle;
        private System.Windows.Forms.Label lblLeftMotorVal;
        private System.Windows.Forms.Label lblRightMotorTitle;
        private System.Windows.Forms.Label lblRightMotorVal;
        private System.Windows.Forms.Label lblTemperatureTitle;
        private System.Windows.Forms.Label lblTemperatureVal;
        private System.Windows.Forms.Label lblBatteryTitle;
        private System.Windows.Forms.Label lblBatteryVal;
        private System.Windows.Forms.Label lblChargingTitle;
        private System.Windows.Forms.Label lblChargingVal;

        private System.Windows.Forms.GroupBox gbSensorStatus;
        public MobileRobot.UR_Robot urRobot1;

        private System.Windows.Forms.GroupBox gbMqttAndMission;
        private System.Windows.Forms.Label lblTopicStatusTitle;
        private System.Windows.Forms.Label lblTopicStatusVal;
        private System.Windows.Forms.Label lblMissionIdTitle;
        private System.Windows.Forms.Label lblMissionIdVal;
        private System.Windows.Forms.Label lblMissionStatusTitle;
        private System.Windows.Forms.Label lblMissionStatusVal;
        private System.Windows.Forms.Label lblStartPointTitle;
        private System.Windows.Forms.Label lblStartPointVal;
        private System.Windows.Forms.Label lblCurrentTargetTitle;
        private System.Windows.Forms.Label lblCurrentTargetVal;

        private System.Windows.Forms.GroupBox gbLocation;
        private System.Windows.Forms.Label lblPosXTitle;
        private System.Windows.Forms.Label lblPosXVal;
        private System.Windows.Forms.Label lblPosYTitle;
        private System.Windows.Forms.Label lblPosYVal;
        private System.Windows.Forms.Label lblThetaTitle;
        private System.Windows.Forms.Label lblThetaVal;
        private System.Windows.Forms.Label lblMapIdTitle;
        private System.Windows.Forms.Label lblMapIdVal;
        private System.Windows.Forms.Label lblCurrentPointTitle;
        private System.Windows.Forms.Label lblCurrentPointVal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOperation;
        public System.Windows.Forms.Button BT_MqttReconnect;
        private System.Windows.Forms.Panel PelMqttOperation;
        public System.Windows.Forms.Button BT_MqttDisconnect;
        public System.Windows.Forms.Button BT_InstantTest;
        public System.Windows.Forms.Button BT_OrderTest;
    }
}