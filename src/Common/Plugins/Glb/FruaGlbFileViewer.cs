using System.Drawing;
using System.Windows.Forms;
using GoldBoxExplorer.Lib.Plugins.Hex;
using System;

namespace GoldBoxExplorer.Lib.Plugins.Glb
{
    public class FruaGlbFileViewer : IGoldBoxViewer
    {
        private readonly FruaGlbFile _file;
        private readonly PluginParameter _args;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public FruaGlbFileViewer(FruaGlbFile file, PluginParameter args)
        {
            _file = file;
            _args = args;
            ContainerWidth = args.ContainerWidth;
            Zoom = args.Zoom;
        }

        public Control GetControl()
        {
            // TODO can't display GLB files yet so just return the hex viewer for now
            var provider = new DynamicFileByteProvider(_args.Filename, true);
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

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}