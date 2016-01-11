using System;
using System.Windows.Forms;
using GoldBoxExplorer.Lib.Plugins.Dax;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Tbl
{
    public class FruaTlbViewer : IGoldBoxViewer
    {
        private readonly FruaTlbFile _file;
        private readonly PictureBox _pictureBox;
        private int ccount = 0;
        private bool _wallTlb = false;
        UserControl _container;
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;

        public FruaTlbViewer(FruaTlbFile file, PluginParameter args)
        {
            if (args.Filename.ToUpper().Contains("8X8"))
                _wallTlb = true;

            _file = file;
            _pictureBox = new PictureBox();
            _pictureBox.Paint += PictureBoxPaint;
            ContainerWidth = args.ContainerWidth;
            Zoom = args.Zoom;
            _container = new UserControl{ Dock = DockStyle.Fill };
            _container.AutoScroll = true;
            var exportButton = new Button();
            exportButton.Text = "Export to Wall Template";
            exportButton.AutoSize = true;
            exportButton.MouseClick += wallTemplateExportForm;

            if (_wallTlb)
                _container.Controls.Add(exportButton);
            _container.Controls.Add(_pictureBox);

         }

        public void wallTemplateExportForm(object sender, MouseEventArgs e)
        {
            using (var form = new WallTemplateExportForm(_file.Bitmaps, _file.getImageOffsets()))
            {
                
                form.ShowDialog();
            }
            
        }
        private void PictureBoxPaint(object sender, PaintEventArgs e)
        {

            var bitmaps = _file.Bitmaps;

            const int padding = 6;
            var x = 0;
            var y = 25;
            var bitmapCount = bitmaps.Count;
            var rowImageHeight = 0;
            
            for (var i = 0; i < bitmapCount; i++)
            {
                var currentImage = bitmaps[i];
                // make sure walls are displayed a row at a time - ie start a new row at 10, 20, 29, 38 + 47 if we have a wallTLB
                if (x + (currentImage.Width * Zoom) > ContainerWidth || (_wallTlb && (i % 47 == 10 || i % 47 == 20 || i % 47 == 29 || i % 47 == 38 || i % 47 == 0)))
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
            ccount++;
            _file.LoadBitmaps();
            _container.Show();
            return _container;
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
    }
}