using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    public class HexFileViewer : IGoldBoxViewer
    {
        private readonly GoldBoxFile _goldBoxFile;
        private readonly string _fileName;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public HexFileViewer(GoldBoxFile goldBoxFile, PluginParameter args)
        {
            _goldBoxFile = goldBoxFile;
            _fileName = args.Filename;
            ContainerWidth = args.ContainerWidth;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
        
        public Control GetControl()
        {
            var provider = new DynamicFileByteProvider(_fileName, true);
            var control = new HexBox
                {
                    ByteProvider = provider,
                    Dock = DockStyle.Fill,
                    GroupSeparatorVisible = false,
                    ColumnInfoVisible = true,
                    LineInfoVisible = true,
                    StringViewVisible = true,
                    UseFixedBytesPerLine = true,
                    VScrollBarVisible = true,
                };
            return control;
        }
    }
}