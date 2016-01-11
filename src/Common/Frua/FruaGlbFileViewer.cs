using System.Windows.Forms;
using DaxFileLibrary.Frua;
using GoldBoxExplorer.Common.Plugins.Hex;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaGlbFileViewer : IGoldBoxViewer
    {
        private readonly FruaGlbFile _goldBoxFile;

        public FruaGlbFileViewer(GoldBoxFileViewerParameters parameters)
        {
            _goldBoxFile = (FruaGlbFile) parameters.GoldBoxFile;
            ContainerWidth = parameters.ContainerWidth;
            Zoom = parameters.Zoom;
        }

        public Control GetControl()
        {
            // TODO can't display GLB files yet so just return the hex viewer
            return new HexSplitContainer(ContainerWidth, _goldBoxFile.GetBytes());
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
        
        public string GetMode()
        {
            return "FRUA GLB File";
        }
    }
}