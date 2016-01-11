using System.Drawing;
using System.Windows.Forms;
using DaxFileLibrary.Frua;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaSavedGameFileViewer : IGoldBoxViewer
    {
        private readonly FruaSavedGameFile _file;

        public FruaSavedGameFileViewer(GoldBoxFileViewerParameters args)
        {
            _file = (FruaSavedGameFile) args.GoldBoxFile;
            ContainerWidth = args.ContainerWidth;
            Zoom = args.Zoom;
        }

        public Control GetControl()
        {
            var control = new TextBox
            {
                Dock = DockStyle.Fill,
                Top = 0,
                Left = 0,
                Width = ContainerWidth,
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ScrollBars = ScrollBars.Both,
                Text = _file.Load().ToString()
            };

            return control;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }

        public string GetMode()
        {
            return "FRUA Saved Game File";
        }
    }
}