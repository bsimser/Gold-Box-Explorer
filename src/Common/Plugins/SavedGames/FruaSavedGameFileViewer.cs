using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib.Plugins.SavedGames
{
    public class FruaSavedGameFileViewer : IGoldBoxViewer
    {
        private readonly FruaSavedGameFile _file;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public FruaSavedGameFileViewer(FruaSavedGameFile file, PluginParameter args)
        {
            _file = file;
            ContainerWidth = args.ContainerWidth;
            Zoom = args.Zoom;
        }

        public Control GetControl()
        {
            var control = ViewerHelper.CreateTextBox();
            control.Text = _file.Load().ToString();
            return control;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}