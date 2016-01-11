using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class DaxImageViewer : IGoldBoxViewer
    {
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
        private readonly IList<Bitmap> _bitmaps;
        private readonly IList<int> _bitmapIds;
        private readonly bool _display35ImagesPerRow;
        private readonly bool _displayBorder;
        private readonly PictureBox _pictureBox;

        public DaxImageViewer(IList<Bitmap> bitmaps, float zoom, int containerWidth, bool display35ImagesPerRow, bool displayBorder, IList<int> bitmapIds)
        {
            Zoom = zoom;
            ContainerWidth = containerWidth;
            _bitmaps = bitmaps;
            _bitmapIds = bitmapIds;
            _display35ImagesPerRow = display35ImagesPerRow;
            _displayBorder = displayBorder;
            _pictureBox = new PictureBox();
            _pictureBox.Paint += PictureBoxPaint;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }

        public Control GetControl()
        {
            return _pictureBox;
        }

        private void PictureBoxPaint(object sender, PaintEventArgs e)
        {
            var x = 0;
            var y = 0;
            var bitmapCount = _bitmaps.Count;
            var lastImageHeight = 0;
            var pen = new Pen(Color.Fuchsia);
            int fontSize = (int) (14  * Zoom);
            var font = new Font("Courier New", fontSize);
            var brush = new SolidBrush(Color.FromArgb(85, 85, 85));
            int padding = fontSize * 3;

            for (int i = 0; i < bitmapCount; i++)
            {
                var currentImage = _bitmaps[i];
                var currentId = _bitmapIds[i];
                lastImageHeight = (int) Math.Max(lastImageHeight, currentImage.Height*Zoom);

                var newRow = false;

                if (_display35ImagesPerRow)
                {
                    if (i > 0 && i%35 == 0)
                        newRow = true;
                }
                else
                {
                    if (x + ((padding + currentImage.Width)*Zoom) > ContainerWidth)
                        newRow = true;
                }

                if(newRow)
                {
                    x = 0;
                    var ypad = (int) (padding*Zoom/2);
                    y += lastImageHeight + ypad;
                    if (i < bitmapCount - 1) lastImageHeight = 0;
                }

                e.Graphics.DrawImage(currentImage, x, y, currentImage.Width*Zoom, currentImage.Height*Zoom);
                e.Graphics.DrawString(currentId.ToString(), font, brush, x + (currentImage.Width*Zoom),y);
                if (_displayBorder)
                {
                    e.Graphics.DrawRectangle(pen, x, y, currentImage.Width*Zoom, currentImage.Height*Zoom);
                }

                x += (int) ((currentImage.Width + padding)*Zoom);
            }

            _pictureBox.Width = ContainerWidth;
            _pictureBox.Height = (int) (fontSize + y + (lastImageHeight*Zoom));
        }
    }
}