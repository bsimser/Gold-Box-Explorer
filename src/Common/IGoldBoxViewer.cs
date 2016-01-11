using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib
{
    
    public class ChangeFileEventArgs : EventArgs
    {
        public string filename { get; set; }
        public int daxId { get; set; }
        public string place { get; set; }
        public static int currentDaxId = 0;
        public static string targetPlace = "";

    }
    public interface IGoldBoxViewer
    {
        Control GetControl();
        float Zoom { get; set; }
        int ContainerWidth { get; set; }
        event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
    }
}