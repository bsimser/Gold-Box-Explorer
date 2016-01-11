using System;
using System.Drawing;
using System.Security;
using System.Windows.Forms;

namespace GoldBoxExplorer.Lib
{
    public partial class ExceptionDialog : Form
    {
        public ExceptionDialog()
        {
            InitializeComponent();
        }

        private void ExceptionDialog_Load(object sender, EventArgs e)
        {
            TopMost = true;
            TopMost = false;
            txtMore.Anchor = AnchorStyles.None;
            txtMore.Visible = false;
            SizeBox(ScopeBox);
            SizeBox(ActionBox);
            SizeBox(ErrorBox);
            lblScopeHeading.Top = (ErrorBox.Top + ErrorBox.Height) + 10;
            ScopeBox.Top = (lblScopeHeading.Top + lblScopeHeading.Height) + 10;
            lblActionHeading.Top = (ScopeBox.Top + ScopeBox.Height) + 10;
            ActionBox.Top = (lblActionHeading.Top + lblActionHeading.Height) + 10;
            lblMoreHeading.Top = (ActionBox.Top + ActionBox.Height) + 10;
            btnMore.Top = lblMoreHeading.Top - 3;
            Height = ((btnMore.Top + btnMore.Height) + 10) + 0x2d;
            CenterToScreen();
        }

        private static void SizeBox(Control ctl)
        {
            System.Drawing.Graphics g = null;
            try
            {
                g = System.Drawing.Graphics.FromHwnd(ctl.Handle);
                var newSize = new SizeF(ctl.Width, ctl.Height);
                var objSizeF = g.MeasureString(ctl.Text, ctl.Font, newSize);
                g.Dispose();
                ctl.Height = Convert.ToInt32(objSizeF.Height) + 5;
            }
            catch (SecurityException)
            {
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }
        }

        private void BtnMoreClick(object sender, EventArgs e)
        {
            if (btnMore.Text == ">>")
            {
                Height += 300;
                var textBox = txtMore;
                var location = new Point(lblMoreHeading.Left, (lblMoreHeading.Top + lblMoreHeading.Height) + 10);
                textBox.Location = location;
                textBox.Height = (ClientSize.Height - txtMore.Top) - 0x2d;
                textBox.Width = ClientSize.Width - 20;
                textBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                textBox.Visible = true;
                btn3.Focus();
                btnMore.Text = "<<";
            }
            else
            {
                SuspendLayout();
                btnMore.Text = ">>";
                Height = ((btnMore.Top + btnMore.Height) + 10) + 0x2d;
                txtMore.Visible = false;
                txtMore.Anchor = AnchorStyles.None;
                ResumeLayout();
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DetermineDialogResult(btn1.Text);
        }

        private static DialogResult DetermineDialogResult(string strButtonText)
        {
            var result = DialogResult.None;

            strButtonText = strButtonText.Replace("&", "");
            
            switch (strButtonText.ToLower())
            {
                case "abort":
                    return DialogResult.Abort;

                case "cancel":
                    return DialogResult.Cancel;

                case "ignore":
                    return DialogResult.Ignore;

                case "no":
                    return DialogResult.No;

                case "none":
                    return DialogResult.None;

                case "ok":
                    return DialogResult.OK;

                case "retry":
                    return DialogResult.Retry;

                case "yes":
                    result = DialogResult.Yes;
                    break;
            }

            return result;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DetermineDialogResult(btn2.Text);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DetermineDialogResult(btn3.Text);
        }
    }
}