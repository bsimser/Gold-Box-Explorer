using System.Drawing;
using System.Windows.Forms;
using DaxFileLibrary.Frua;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaCharacterViewer : IGoldBoxViewer
    {
        private readonly FruaCharacterFile _file;

        public FruaCharacterViewer(GoldBoxFileViewerParameters parameters)
        {
            _file = (FruaCharacterFile) parameters.GoldBoxFile;
            Zoom = parameters.Zoom;
            ContainerWidth = parameters.ContainerWidth;
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
                    Text = _file.LoadCharacter().ToString()
                };

            return control;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
        
        public string GetMode()
        {
            return "";
        }
    }
}