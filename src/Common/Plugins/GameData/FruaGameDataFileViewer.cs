using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib.Plugins.GameData
{
    public class FruaGameDataFileViewer : IGoldBoxViewer
    {
        private readonly FruaGameData _data;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public FruaGameDataFileViewer(FruaGameData data)
        {
            _data = data;
        }

        public Control GetControl()
        {
            var viewer = ViewerHelper.CreateTextBox();
            viewer.Text = _data.ToString();
            return viewer;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}