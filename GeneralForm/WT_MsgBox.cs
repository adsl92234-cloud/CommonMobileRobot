using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AML
{
    public partial class WT_MsgBox : Form
    {
        public DialogResult Result;
        public WT_MsgBox()
        {
            InitializeComponent();
        }
        public void ShowMsg(string MsgString)
        {
            MessageString.Text = MsgString;
            BtOK.Visible = true;
            BtYes.Visible = false;
            BtNo.Visible = false;
            this.ShowDialog();
        }
        public DialogResult ShowYesNo(string MsgString)
        {
            MessageString.Text = MsgString;
            BtOK.Visible = false;
            BtYes.Visible = true;
            BtNo.Visible = true;
            this.ShowDialog();
            return Result;
        }

        //==================================================
        private void BtYes_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Yes;
            this.Hide();
        }
        private void BtNo_Click(object sender, EventArgs e)
        {
            Result = DialogResult.No;
            this.Hide();
        }
        private void BtOK_Click(object sender, EventArgs e)
        {
            Result = DialogResult.OK;
            this.Hide();
        }
    }
}
