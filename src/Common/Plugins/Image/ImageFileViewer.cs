using System.Windows.Forms;
using System;

namespace GoldBoxExplorer.Lib.Plugins.Image
{
    public class ImageFileViewer : IGoldBoxViewer
    {
        private readonly string _filename;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public ImageFileViewer(string filename, int containerWidth)
        {
            _filename = filename;
            ContainerWidth = containerWidth;
        }

        public Control GetControl()
        {
            var bitmap = DevIL.DevIL.LoadBitmap(_filename);
            var pictureBox = new PictureBox
                                 {
                                     Top = 0,
                                     Left = 0,
                                     Image = bitmap,
                                     SizeMode = PictureBoxSizeMode.AutoSize,
                                 };
            return pictureBox;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}