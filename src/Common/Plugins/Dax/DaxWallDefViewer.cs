using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class DaxWallDefViewer : IGoldBoxViewer
    {
        public event EventHandler<ChangeFileEventArgs> ChangeSelectedFile;
        private readonly List<List<Bitmap>> _wallBitmaps;
        private readonly List<int> _blockIds;
        private readonly int maxWallHeight = 11;
        private int maxWallSetHeight;
        private int initialYOffset = 32;
        private TabControl tab;

        public DaxWallDefViewer(List<List<Bitmap>> wallBitmaps, List<int> blockIds, float zoom, int containerWidth)
        {
            Zoom = zoom;
            ContainerWidth = containerWidth;
            _wallBitmaps = wallBitmaps;
            _blockIds = blockIds;
            maxWallSetHeight = (15 * 8 * (maxWallHeight + 1)) + initialYOffset;
        }

     
        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }

        void drawWallViews(Graphics surface, List<Bitmap> wallset)
        {
            float xoffset = 0;
            float yoffset = initialYOffset;
            int imagesDisplayed = 0;

            foreach (var wall in wallset)
            {

                surface.DrawImage(wall, xoffset, yoffset, wall.Width, wall.Height);

                imagesDisplayed++;
                xoffset += wall.Width + 8;

                // starts a new row of images, for a new wall, if we've displayed all 10 views
                if (imagesDisplayed > 9 && (imagesDisplayed % 10) == 0)
                {
                    xoffset = 0;
                    yoffset += (1 + maxWallHeight) * 8;
                }
            }

        }
        void InvalidateWalls(object sender, InvalidateEventArgs e)
        {
            var tab = sender as TabControl;
            if (tab == null) return;
            var page = tab.SelectedTab;
            var pictureBox = page.Controls["wallset"] as PictureBox;
            pictureBox.Width = (int)(ContainerWidth * Zoom);
            pictureBox.Height = (int)(maxWallSetHeight * Zoom);
        }
        public void wallTemplateExportForm(object sender, MouseEventArgs e)
        {
            string tabname = tab.SelectedTab.Text;
            using (var form = new WallTemplateExportForm(_wallBitmaps[tab.SelectedIndex], tabname))
            {
                form.ShowDialog();
            }

        }
        public Control GetControl()
        {
            var width = ContainerWidth;
            var height = maxWallSetHeight;
            
            tab = new TabControl { Dock = DockStyle.Fill };
            tab.Invalidated += InvalidateWalls;

            int i = 0;
            foreach (var wallset in _wallBitmaps)
            {
                int blockId = _blockIds[i++];
                var page = new TabPage(blockId.ToString());
                var pictureBox = new PictureBox();

                page.Size = new Size(width, height);
                pictureBox.Image = new Bitmap(width, height);
                tab.TabPages.Add(page);

                page.AutoScroll = true;
                var exportButton = new Button();
                exportButton.Text = "Export to Wall Template";
                exportButton.AutoSize = true;
                exportButton.MouseClick += wallTemplateExportForm;

                page.Controls.Add(exportButton);
                page.Controls.Add(pictureBox);
                pictureBox.Name = "wallset";
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                pictureBox.Size = new Size(width, height);
 
                Graphics surface = Graphics.FromImage(pictureBox.Image);
                drawWallViews(surface, wallset);
            }
            return tab;
        }
    }
}