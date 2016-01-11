using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib.Plugins.Vault
{
    public class FruaVaultFileViewer : IGoldBoxViewer
    {
        private readonly FruaVaultFile _file;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public FruaVaultFileViewer(FruaVaultFile file)
        {
            _file = file;
        }

        public Control GetControl()
        {
            var control = ViewerHelper.CreateTextBox();
            control.Text = _file.ToString();
            return control;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}