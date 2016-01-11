using System;
using System.Windows.Forms;
using DaxFileLibrary.Frua;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaTlbViewer : IGoldBoxViewer
    {
        private readonly FruaTlbFile _goldBoxFile;
        private readonly PictureBox _pictureBox;

        public FruaTlbViewer(GoldBoxFileViewerParameters parameters)
        {
            _goldBoxFile = (FruaTlbFile) parameters.GoldBoxFile;
            _pictureBox = new PictureBox();
            _pictureBox.Paint += PictureBoxPaint;
            ContainerWidth = parameters.ContainerWidth;
            Zoom = parameters.Zoom;
        }

        private void PictureBoxPaint(object sender, PaintEventArgs e)
        {
            var bitmaps = _goldBoxFile.Bitmaps;

            const int padding = 6;
            var x = 0;
            var y = 0;
            var bitmapCount = bitmaps.Count;
            var rowImageHeight = 0;

            for (var i = 0; i < bitmapCount; i++)
            {
                var currentImage = bitmaps[i];

                if (x + (currentImage.Width * Zoom) > ContainerWidth)
                {
                    x = 0;
                    y += rowImageHeight + (int)(padding * Zoom);
                    rowImageHeight = (int)(currentImage.Height * Zoom);
                }
                else
                {
                    rowImageHeight = Math.Max(rowImageHeight, (int)(currentImage.Height * Zoom));
                }

                e.Graphics.DrawImage(currentImage, x, y, currentImage.Width * Zoom, currentImage.Height * Zoom);
                x += (int)((currentImage.Width + padding) * Zoom);
            }

            _pictureBox.Width = ContainerWidth;
            _pictureBox.Height = y + (int)((rowImageHeight * Zoom));
        }

        public Control GetControl()
        {
            _goldBoxFile.LoadBitmaps();
            return _pictureBox;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }

        public string GetMode()
        {
            return "UA Art File";
        }
    }
}