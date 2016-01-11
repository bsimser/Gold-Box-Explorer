using System.Linq;
using System.Text;
using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib.Plugins.Text
{
    public class TextFileViewer : IGoldBoxViewer
    {
        private readonly GoldBoxFile _goldBoxFile;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public TextFileViewer(GoldBoxFile goldBoxFile, int containerWidth)
        {
            _goldBoxFile = goldBoxFile;
            ContainerWidth = containerWidth;
        }

        public Control GetControl()
        {
            var control = ViewerHelper.CreateTextBox();
            var bytes = _goldBoxFile.GetBytes();
            var textEncoder = new UTF8Encoding();
            control.Text = textEncoder.GetString(bytes.ToArray());
            return control;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}