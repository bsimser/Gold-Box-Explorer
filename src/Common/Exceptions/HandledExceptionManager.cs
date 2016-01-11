using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GoldBoxExplorer.Lib.Exceptions
{
    public class HandledExceptionManager
    {
        private static string ExceptionToMore(Exception objException)
        {
            var sb = new StringBuilder();

            sb.Append("Detailed technical information follows: " + Environment.NewLine);
            sb.Append("---" + Environment.NewLine);
            var x = UnhandledExceptionManager.ExceptionToString(objException);
            sb.Append(x);
            
            return sb.ToString();
        }

        private static string GetDefaultMore(string strMoreDetails)
        {
            if (strMoreDetails == "")
            {
                var objStringBuilder = new StringBuilder();
                
                objStringBuilder.Append("No further information is available. If the problem persists, contact (contact).");
                objStringBuilder.Append(Environment.NewLine);
                objStringBuilder.Append(Environment.NewLine);
                objStringBuilder.Append("Basic technical information follows: " + Environment.NewLine);
                objStringBuilder.Append("---" + Environment.NewLine);
                objStringBuilder.Append(UnhandledExceptionManager.SysInfoToString(true));
                
                return objStringBuilder.ToString();
            }
            return strMoreDetails;
        }

        private static void ProcessStrings(ref string strWhatHappened, ref string strHowUserAffected, ref string strWhatUserCanDo, ref string strMoreDetails)
        {
            strWhatHappened = ReplaceStringVals(strWhatHappened);
            strHowUserAffected = ReplaceStringVals(strHowUserAffected);
            strWhatUserCanDo = ReplaceStringVals(strWhatUserCanDo);
            strMoreDetails = ReplaceStringVals(GetDefaultMore(strMoreDetails));
        }

        private static string ReplaceStringVals(string strOutput)
        {
            string strTemp = strOutput ?? "";
            return strTemp.Replace("(app)", AppSettings.AppProduct).Replace("(contact)", AppSettings.GetString("UnhandledExceptionManager/ContactInfo"));
        }

        public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo)
        {
            return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, UserErrorDefaultButton.Default);
        }

        public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, Exception objException, MessageBoxButtons Buttons, MessageBoxIcon Icon, UserErrorDefaultButton DefaultButton)
        {
            return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, ExceptionToMore(objException), Buttons, Icon, DefaultButton);
        }

        public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails, MessageBoxButtons Buttons, MessageBoxIcon Icon, UserErrorDefaultButton DefaultButton)
        {
            return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, strMoreDetails, Buttons, Icon, DefaultButton);
        }

        private static DialogResult ShowDialogInternal(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails, MessageBoxButtons Buttons, MessageBoxIcon Icon, UserErrorDefaultButton DefaultButton)
        {
            ProcessStrings(ref strWhatHappened, ref strHowUserAffected, ref strWhatUserCanDo, ref strMoreDetails);

            var objForm = new ExceptionDialog();
            
            objForm.Text = ReplaceStringVals(objForm.Text);
            objForm.ErrorBox.Text = strWhatHappened;
            objForm.ScopeBox.Text = strHowUserAffected;
            objForm.ActionBox.Text = strWhatUserCanDo;
            objForm.txtMore.Text = strMoreDetails;

            switch (((int) Buttons))
            {
                case 0:
                    objForm.btn3.Text = "OK";
                    objForm.btn2.Visible = false;
                    objForm.btn1.Visible = false;
                    objForm.AcceptButton = objForm.btn3;
                    break;

                case 1:
                    objForm.btn3.Text = "Cancel";
                    objForm.btn2.Text = "OK";
                    objForm.btn1.Visible = false;
                    objForm.AcceptButton = objForm.btn2;
                    objForm.CancelButton = objForm.btn3;
                    break;

                case 2:
                    objForm.btn1.Text = "&Abort";
                    objForm.btn2.Text = "&Retry";
                    objForm.btn3.Text = "&Ignore";
                    objForm.AcceptButton = objForm.btn2;
                    objForm.CancelButton = objForm.btn3;
                    break;

                case 3:
                    objForm.btn3.Text = "Cancel";
                    objForm.btn2.Text = "&No";
                    objForm.btn1.Text = "&Yes";
                    objForm.CancelButton = objForm.btn3;
                    break;

                case 4:
                    objForm.btn3.Text = "&No";
                    objForm.btn2.Text = "&Yes";
                    objForm.btn1.Visible = false;
                    break;

                case 5:
                    objForm.btn3.Text = "Cancel";
                    objForm.btn2.Text = "&Retry";
                    objForm.btn1.Visible = false;
                    objForm.AcceptButton = objForm.btn2;
                    objForm.CancelButton = objForm.btn3;
                    break;
            }

            if (Icon == MessageBoxIcon.Hand)
            {
                objForm.PictureBox1.Image = SystemIcons.Error.ToBitmap();
            }
            else if (Icon == MessageBoxIcon.Hand)
            {
                objForm.PictureBox1.Image = SystemIcons.Error.ToBitmap();
            }
            else if (Icon == MessageBoxIcon.Exclamation)
            {
                objForm.PictureBox1.Image = SystemIcons.Exclamation.ToBitmap();
            }
            else if (Icon == MessageBoxIcon.Asterisk)
            {
                objForm.PictureBox1.Image = SystemIcons.Information.ToBitmap();
            }
            else if (Icon == MessageBoxIcon.Question)
            {
                objForm.PictureBox1.Image = SystemIcons.Question.ToBitmap();
            }
            else
            {
                objForm.PictureBox1.Image = SystemIcons.Error.ToBitmap();
            }
            
            switch (((int) DefaultButton))
            {
                case 1:
                    objForm.AcceptButton = objForm.btn1;
                    objForm.btn1.TabIndex = 0;
                    break;

                case 2:
                    objForm.AcceptButton = objForm.btn2;
                    objForm.btn2.TabIndex = 0;
                    break;

                case 3:
                    objForm.AcceptButton = objForm.btn3;
                    objForm.btn3.TabIndex = 0;
                    break;
            }

            return objForm.ShowDialog();
        }

        public enum UserErrorDefaultButton
        {
            Default,
            Button1,
            Button2,
            Button3
        }
    }
}