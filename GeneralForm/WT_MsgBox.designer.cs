namespace AML
{
    partial class WT_MsgBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtYes = new System.Windows.Forms.Button();
            this.BtOK = new System.Windows.Forms.Button();
            this.BtNo = new System.Windows.Forms.Button();
            this.MsgSplit = new System.Windows.Forms.SplitContainer();
            this.MessageString = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MsgSplit)).BeginInit();
            this.MsgSplit.Panel1.SuspendLayout();
            this.MsgSplit.Panel2.SuspendLayout();
            this.MsgSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtYes
            // 
            this.BtYes.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtYes.Location = new System.Drawing.Point(34, 5);
            this.BtYes.Margin = new System.Windows.Forms.Padding(4);
            this.BtYes.Name = "BtYes";
            this.BtYes.Size = new System.Drawing.Size(80, 30);
            this.BtYes.TabIndex = 0;
            this.BtYes.TabStop = false;
            this.BtYes.Text = "Yes";
            this.BtYes.UseVisualStyleBackColor = true;
            this.BtYes.Click += new System.EventHandler(this.BtYes_Click);
            // 
            // BtOK
            // 
            this.BtOK.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtOK.Location = new System.Drawing.Point(178, 5);
            this.BtOK.Margin = new System.Windows.Forms.Padding(4);
            this.BtOK.Name = "BtOK";
            this.BtOK.Size = new System.Drawing.Size(80, 30);
            this.BtOK.TabIndex = 1;
            this.BtOK.TabStop = false;
            this.BtOK.Text = "OK";
            this.BtOK.UseVisualStyleBackColor = true;
            this.BtOK.Click += new System.EventHandler(this.BtOK_Click);
            // 
            // BtNo
            // 
            this.BtNo.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtNo.Location = new System.Drawing.Point(322, 5);
            this.BtNo.Margin = new System.Windows.Forms.Padding(4);
            this.BtNo.Name = "BtNo";
            this.BtNo.Size = new System.Drawing.Size(80, 30);
            this.BtNo.TabIndex = 2;
            this.BtNo.TabStop = false;
            this.BtNo.Text = "No";
            this.BtNo.UseVisualStyleBackColor = true;
            this.BtNo.Click += new System.EventHandler(this.BtNo_Click);
            // 
            // MsgSplit
            // 
            this.MsgSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MsgSplit.IsSplitterFixed = true;
            this.MsgSplit.Location = new System.Drawing.Point(0, 0);
            this.MsgSplit.Name = "MsgSplit";
            this.MsgSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MsgSplit.Panel1
            // 
            this.MsgSplit.Panel1.Controls.Add(this.MessageString);
            // 
            // MsgSplit.Panel2
            // 
            this.MsgSplit.Panel2.Controls.Add(this.BtYes);
            this.MsgSplit.Panel2.Controls.Add(this.BtNo);
            this.MsgSplit.Panel2.Controls.Add(this.BtOK);
            this.MsgSplit.Size = new System.Drawing.Size(437, 113);
            this.MsgSplit.SplitterDistance = 70;
            this.MsgSplit.TabIndex = 0;
            this.MsgSplit.TabStop = false;
            // 
            // MessageString
            // 
            this.MessageString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageString.Font = new System.Drawing.Font("新細明體", 12F);
            this.MessageString.Location = new System.Drawing.Point(0, 0);
            this.MessageString.Name = "MessageString";
            this.MessageString.Size = new System.Drawing.Size(437, 70);
            this.MessageString.TabIndex = 0;
            this.MessageString.Text = "MessageString";
            this.MessageString.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WT_MsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 113);
            this.ControlBox = false;
            this.Controls.Add(this.MsgSplit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WT_MsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Notice";
            this.TopMost = true;
            this.MsgSplit.Panel1.ResumeLayout(false);
            this.MsgSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MsgSplit)).EndInit();
            this.MsgSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtYes;
        private System.Windows.Forms.Button BtOK;
        private System.Windows.Forms.Button BtNo;
        private System.Windows.Forms.SplitContainer MsgSplit;
        private System.Windows.Forms.Label MessageString;
    }
}