namespace MobileRobot
{
    partial class UR_Robot
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
            this.lblConnect = new System.Windows.Forms.Label();
            this.lblControlState = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.cbCommand = new System.Windows.Forms.ComboBox();
            this.cbTarget = new System.Windows.Forms.ComboBox();
            this.cbSlot = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.picRobot = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // lblConnect
            // 
            this.lblConnect.BackColor = System.Drawing.Color.Red;
            this.lblConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblConnect.Location = new System.Drawing.Point(5, 5);
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(95, 28);
            this.lblConnect.TabIndex = 0;
            this.lblConnect.Text = "UR_Robot";
            this.lblConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblControlState
            // 
            this.lblControlState.BackColor = System.Drawing.Color.LightPink;
            this.lblControlState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblControlState.Location = new System.Drawing.Point(105, 5);
            this.lblControlState.Name = "lblControlState";
            this.lblControlState.Size = new System.Drawing.Size(95, 28);
            this.lblControlState.TabIndex = 1;
            this.lblControlState.Text = "Offline";
            this.lblControlState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAction
            // 
            this.lblAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAction.Location = new System.Drawing.Point(205, 5);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(300, 28);
            this.lblAction.TabIndex = 2;
            this.lblAction.Text = "Idle";
            this.lblAction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbCommand
            // 
            this.cbCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommand.Location = new System.Drawing.Point(5, 46);
            this.cbCommand.Name = "cbCommand";
            this.cbCommand.Size = new System.Drawing.Size(130, 23);
            this.cbCommand.TabIndex = 3;
            this.cbCommand.SelectedIndexChanged += new System.EventHandler(this.cbCommand_SelectedIndexChanged);
            // 
            // cbTarget
            // 
            this.cbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTarget.Location = new System.Drawing.Point(140, 46);
            this.cbTarget.Name = "cbTarget";
            this.cbTarget.Size = new System.Drawing.Size(100, 23);
            this.cbTarget.TabIndex = 4;
            // 
            // cbSlot
            // 
            this.cbSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSlot.Location = new System.Drawing.Point(245, 46);
            this.cbSlot.Name = "cbSlot";
            this.cbSlot.Size = new System.Drawing.Size(70, 23);
            this.cbSlot.TabIndex = 5;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(405, 42);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 32);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblBarcode
            // 
            this.lblBarcode.Location = new System.Drawing.Point(5, 82);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(500, 24);
            this.lblBarcode.TabIndex = 7;
            this.lblBarcode.Text = "Barcode : -";
            this.lblBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picRobot
            // 
            this.picRobot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picRobot.Location = new System.Drawing.Point(5, 110);
            this.picRobot.Name = "picRobot";
            this.picRobot.Size = new System.Drawing.Size(500, 245);
            this.picRobot.TabIndex = 8;
            this.picRobot.TabStop = false;
            this.picRobot.Paint += new System.Windows.Forms.PaintEventHandler(this.picRobot_Paint);
            // 
            // UR_Robot
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblConnect);
            this.Controls.Add(this.lblControlState);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.cbCommand);
            this.Controls.Add(this.cbTarget);
            this.Controls.Add(this.cbSlot);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.picRobot);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "UR_Robot";
            this.Size = new System.Drawing.Size(510, 360);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblConnect;
        private System.Windows.Forms.Label lblControlState;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.ComboBox cbCommand;
        private System.Windows.Forms.ComboBox cbTarget;
        private System.Windows.Forms.ComboBox cbSlot;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.PictureBox picRobot;
    }
}
