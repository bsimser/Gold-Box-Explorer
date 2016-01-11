using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using GoldBoxExplorer.Lib;
using System.Collections.Generic;
using GoldBoxExplorer.Lib.Frua;
namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public partial class WallTemplateExportForm : Form
    {
        static string templateFileName;
        static string targetFolder;
        private readonly List<Bitmap> _wallBitmaps;
        private Boolean _fileNameChanged = false;
        private Boolean _overlay = false;
        private readonly string _daxId;
        private Bitmap _templateBitmap;
        private Bitmap _exportBitmap;
        private bool _fromTLB = false;
        private List<Point> _TLBImageOffsets;
        public WallTemplateExportForm()
        {
            InitializeComponent();

        }
        public WallTemplateExportForm(IList<Bitmap> wallBitmaps, List<Point> xyoffsets)
            : this()
        {
            // these walls are coming from a TLB file, not a walldef one
            // so rearrange the walls so they're in the same order as the walldef ones
            _fromTLB = true;
            _TLBImageOffsets = new List<Point>();
            _wallBitmaps = new List<Bitmap>();
            int i = 0;

            Bitmap tmpBM = new Bitmap(7,7);
            Point tmpPoint = new Point(0, 0);
            foreach(var bm in wallBitmaps)
            {
                var wn = i % 47;
                if (wn < 20)
                {
                    wn %= 10;
                    if (wn == 1)
                        tmpBM = bm;
                    else
                    {
                        _wallBitmaps.Add(bm);
                        _TLBImageOffsets.Add(new Point(0, 0));

                    }
                    if (wn == 9)
                    {
                        _TLBImageOffsets.Add(new Point(0, 0));
                        _wallBitmaps.Add(tmpBM);
                    }

                }
                else
                {
                    wn = (wn - 20) % 9;
                    _wallBitmaps.Add(bm);
                    _TLBImageOffsets.Add(xyoffsets[i]);
                    if (wn == 8)
                    {
                        _wallBitmaps.Add(new Bitmap(8, 8));
                        _TLBImageOffsets.Add(new Point(0,0));
                    }
                }

                // put wallview 1 in wallview 10, to match the order
                i++;
            }
            _daxId = "tlb";
            initExportForm();
        }
        public WallTemplateExportForm(List<Bitmap> wallBitmaps, string daxId)
            : this()
        {
            _wallBitmaps = wallBitmaps;
            _daxId = daxId;

            initExportForm();


        }

        private void initExportForm()
        {
            if (targetFolder != null)
                exportTargetFolder.Text = targetFolder;
            else
            {
                exportTargetFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                targetFolder = exportTargetFolder.Text;
            }
            if (templateFileName != null)
                templateFile.Text = templateFileName;
            else
            {
                templateFile.Text = "template.bmp";
                templateFileName = templateFile.Text;
            }

            var wallNumbers = new List<String>();

            for (int i = 1; i <= _wallBitmaps.Count / 10; i++)
                wallNumbers.Add(i.ToString());


            selectWallNumber.DataSource = wallNumbers.ToArray();
            selectWallNumber.SelectedIndex = 0;
            selectTemplateWallNumber.DataSource = wallNumbers.ToArray();
            selectTemplateWallNumber.SelectedIndex = 0;
        }

        private void loadTemplate()
        {
            _templateBitmap = new Bitmap(new Bitmap(templateFile.Text));
        }

        private Bitmap compareBitmaps(Bitmap bm1, Bitmap bm2)
        {
            var transColor = Color.FromArgb(103, 247, 159);
            Bitmap newBitmap = new Bitmap(bm2);
            for (int x = 0; x < bm1.Width; x++)
            {
                for (int y = 0; y < bm1.Height; y++)
                {
                    if (bm2.GetPixel(x, y).Equals(bm1.GetPixel(x, y)))
                    {
                        newBitmap.SetPixel(x, y, transColor);
                    }
                }
            }

            return newBitmap;
        }
        private void copyWallsIntoOverlayTemplate()
        {
            var image = new Bitmap(_templateBitmap);
            var surface = Graphics.FromImage(image);

            // array of offsets, draw wall bitmaps to these locations
            int[] xOffsets = { 1, 66, 131, 196, 261, 1, 66, 131, 196, 261 };
            int[] yOffsets = { 1, 1, 1, 1, 1, 61, 61, 61, 61};
            int[] colCount = { 1, 1, 1, 3, 2, 2, 7, 2, 2, 1 };                 // seg600:0AE4
            int[] rowCount = { 1, 3, 3, 3, 7, 7, 7, 11, 11, 1 };               // seg600:0AEE
            int i = 0;
            int startBitmap = selectWallNumber.SelectedIndex * 10;
            if (startBitmap < 0) startBitmap = 0;
            int startOverlayBitmap = selectTemplateWallNumber.SelectedIndex * 10;
            if (startOverlayBitmap < 0) startOverlayBitmap = 0;

            List<Bitmap> selectedWallBitmaps = _wallBitmaps.GetRange(startBitmap, 9);
            List<Bitmap> selectedOverlayWallBitmaps = _wallBitmaps.GetRange(startOverlayBitmap, 9);

            foreach (var wallBitmap in selectedWallBitmaps)
            {
                int x = xOffsets[i];
                int y = yOffsets[i];
                int heightCrop = 8;
                if (i == 7 || i == 8 || _fromTLB) heightCrop = 0;
                Rectangle rectangle = new Rectangle(0, heightCrop, wallBitmap.Width, wallBitmap.Height - heightCrop);
                if (_fromTLB)
                {
                    int wallViewWidth = colCount[i]*8;
                    int wallViewHeight = rowCount[i]*8;
                    int xadjust = (wallViewWidth - selectedWallBitmaps[i].Width)/2 + (4*_TLBImageOffsets[i].X);// +overlayYOffset[i] * 8;
                    int yadjust = wallViewHeight - (selectedWallBitmaps[i].Height - (4*_TLBImageOffsets[i].Y));// +overlayXOffset[i] * 8;
                    
                    surface.DrawImage(selectedWallBitmaps[i], x + xadjust , y + yadjust, rectangle, GraphicsUnit.Pixel);
                }
                else
                {
                    Bitmap cmpBitmap = compareBitmaps(selectedOverlayWallBitmaps[i], selectedWallBitmaps[i]);
                    surface.DrawImage(cmpBitmap, x, y, rectangle, GraphicsUnit.Pixel);
                }
                i++;
            }
            _exportBitmap = convertBitmap(image);
            templatePreview.Image = _exportBitmap;
            templatePreview.Invalidate(true);
        }   
        private void copyWallsIntoTemplate()
        {
            if (_overlay)
            {
                copyWallsIntoOverlayTemplate();
                return;
            }
            var image = new Bitmap(_templateBitmap);
            var surface = Graphics.FromImage(image);

            // array of offsets, draw wall bitmaps to these locations
            /*
             * A_WALL_RECT = 392,146,424,357
B_WALL_RECT = 522,146,554,357
C_WALL_RECT = 262,146,294,280
D_WALL_RECT = 342,146,374,280
E_WALL_RECT = 262,146,374,280
F_WALL_RECT = 2,146,34,280
G_WALL_RECT = 132,146,164,280
!H_WALL_RECT = 522,2,570,60
I_WALL_RECT = 262,2,278,60
!J_WALL_RECT = 2,2,18,21
!K_WALL_RECT = 392,2,408,60
!L_WALL_RECT = 522,2,538,60
!M_WALL_RECT = 554,2,570,60
N_WALL_RECT = 18,146,34,280
O_WALL_RECT = 132,146,148,280
!P_WALL_RECT = 132,2,148,21
             */
            int[] xOffsets = { 66, 131, 196, 261, 1, 66, 131, 196, 261, 1 };
            int[] yOffsets = { 1, 1, 1, 1, 61, 61, 61, 61, 61, 1 };
            int i = 0;
            int startBitmap = selectWallNumber.SelectedIndex * 10;
            if (startBitmap < 0) startBitmap = 0;
            List<Bitmap> selectedWallBitmaps = _wallBitmaps.GetRange(startBitmap, 10);
            foreach (var wallBitmap in selectedWallBitmaps)
            {
                int x = xOffsets[i];
                int y = yOffsets[i];
                int heightCrop = 8;
                if (i == 7 || i == 8 || _fromTLB) heightCrop = 0;
                Rectangle rectangle = new Rectangle(0, heightCrop, wallBitmap.Width, wallBitmap.Height - heightCrop);
                surface.DrawImage(wallBitmap, x, y, rectangle, GraphicsUnit.Pixel);
                i++;
            }
            _exportBitmap = convertBitmap(image);
            templatePreview.Image = _exportBitmap;
            templatePreview.Invalidate(true);
        }   
        private void textBox1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = exportTargetFolder.Text;
            var result = folderBrowserDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                exportTargetFolder.Text = folderBrowserDialog1.SelectedPath;
                targetFolder = exportTargetFolder.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            using (new WaitCursor())
            {

                var targetFolder = exportTargetFolder.Text; 

                _exportBitmap.Save(string.Format(@"{0}\{1}", targetFolder, exportFileName.Text),ImageFormat.Bmp);

//                Hide();
                MessageBox.Show(string.Format("Exported wall template {0} to {1}",
                    exportFileName.Text, targetFolder));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // loop through the bitmap sets, unique name; add + save them
            var saveIndex = selectWallNumber.SelectedIndex;

            using (new WaitCursor())
            {

                var targetFolder = exportTargetFolder.Text;
                var i = 0;
                for (i = 0; i < _wallBitmaps.Count/10; i++) {
                    selectWallNumber.SelectedIndex = i;

                    _exportBitmap.Save(string.Format(@"{0}\{1}", targetFolder, exportFileName.Text), ImageFormat.Bmp);
                }

                //                Hide();
                MessageBox.Show(string.Format("Exported {2} wall templates to {1}",
                    exportFileName.Text, targetFolder, i));
            }
            selectWallNumber.SelectedIndex = saveIndex;
        }
        private void exportFileName_TextChanged(object sender, EventArgs e)
        {
            _fileNameChanged = true;
        }

        private void selectWallNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectWallNumber.SelectedIndex < 0) return;
            if (_fromTLB)
            {
                if ((selectWallNumber.SelectedIndex % 5) > 1)
                {
                    _overlay = true;
                    if (templateFile.Text == "template.bmp") templateFile.Text = "overlay.bmp";
                }
                else
                {
                    _overlay = false;
                    if (templateFile.Text == "overlay.bmp") templateFile.Text = "template.bmp";

                }
            }

            if (_fileNameChanged == false)
            {
                exportFileName.Text = "wa" + _daxId + "_" + (selectWallNumber.SelectedIndex+1).ToString() + ".bmp";
                if (_overlay)
                    exportFileName.Text = "wa" + _daxId + "_" + (selectTemplateWallNumber.SelectedIndex + 1).ToString() + "_" + (selectWallNumber.SelectedIndex + 1).ToString() + ".bmp";
                _fileNameChanged = false;
            }
            copyWallsIntoTemplate();

        }


        private void templateFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = templateFile.Text;
            var result = openFileDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                templateFile.Text = openFileDialog1.FileName;
                templateFileName = templateFile.Text;
            }

        }

        private void templateFile_TextChanged(object sender, EventArgs e)
        {
            loadTemplate();
            copyWallsIntoTemplate();
        }

        private static byte getColor(ColorPalette palette, Color color, Boolean paletteReadOnly = false)
        {
            if ((color.R == 255 && color.G == 82 && color.B == 255) ||(color.R == 103 && color.G == 247 && color.B == 159))
            {
                // transparency color
                return 255;
            }

            if (color.R == 255 && color.G == 87 && color.B == 255)
            {
                // template border color
                return 254;
            }
            if (color.R == 0 && color.G == 0 && color.B == 0)
            {
                return 0;
            }
            byte i;
            for (i = 1; i < 255; i++)
            {
                if (palette.Entries[i].R == color.R &&
                    palette.Entries[i].G == color.G &&
                    palette.Entries[i].B == color.B)
                {
                     return i;
                }
                if (i >= 32)
                {
                    if (palette.Entries[i].R == 0 &&
                        palette.Entries[i].G == 0 &&
                        palette.Entries[i].B == 0)
                    {
                        if (paletteReadOnly)
                        {
                            // couldn't find a palette entry for this color when there should be one already
                            return 0;
                        }

                        palette.Entries[i] = color;
                          return i;
                    }
                }
            }

            return 0; // out of palette entries!
        }

        private Bitmap convertBitmap(Bitmap bmsrc)
        {
            // convert a 32 bit bitmap into an 8 bit indexed palette one
            // to ensure all the walls in a wallset have the same palette, we build the palette from all the walls in the wallset
            // make a list of the group of 5 wallsets our wall has come from
            int firstBitmap = selectWallNumber.SelectedIndex/5;
      //      List<Bitmap> paletteBitmaps = _wallBitmaps.GetRange(firstBitmap*50, 50); // each wall consists of 10 bitmaps, so that's why it's 50 instead of 5
            List<Bitmap> paletteBitmaps = _wallBitmaps.GetRange(0, _wallBitmaps.Count);
            paletteBitmaps.Add(bmsrc);

            var bmdst = new Bitmap(bmsrc.Width, bmsrc.Height, PixelFormat.Format8bppIndexed);

            // build a palette out of the 5 walls
            var palette = bmdst.Palette;
            for (int i = 0; i <= 255; i++) palette.Entries[i] = Color.FromArgb(0, 0, 0);
            palette.Entries[254] = Color.FromArgb(255, 87, 255);
            palette.Entries[255] = Color.FromArgb(103, 247, 159);
            // loop through the bitmaps, to build up the palette
            foreach (var bm in paletteBitmaps)
            {
                for (int w = 0; w < bm.Width; w++)
                {
                    for (int h = 0; h < bm.Height; h++)
                    {
                        getColor(palette, bm.GetPixel(w, h));
                    }
                }

            }

            // now create the 8 bit indexed bitmap
            var lbm = new LockBitmap(bmdst);

            lbm.LockBits();
            // loop through pixels, if pixel color in color table set the pixel, if not add the color then set it
            for (int w = 0; w < bmdst.Width; w++)
            {
                for (int h = 0; h < bmdst.Height; h++)
                {
                    byte c = getColor(palette, bmsrc.GetPixel(w, h), true);
                    lbm.SetPixel(w, h, c);
                }
            }
            lbm.UnlockBits();
            bmdst.Palette = palette;
            return bmdst;
        }

        private void selectTemplateWallNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_overlay && _fileNameChanged == false)
            {
                exportFileName.Text = "wa" + _daxId + "_" + (selectTemplateWallNumber.SelectedIndex + 1).ToString() + "_" + (selectWallNumber.SelectedIndex + 1).ToString() + ".bmp";
                _fileNameChanged = false;
            }
            loadTemplate();
            copyWallsIntoTemplate();
        }

        private void exportAsOverlay_CheckedChanged(object sender, EventArgs e)
        {
            _overlay = exportAsOverlay.Checked;
            if (_overlay && templateFile.Text == "template.bmp") templateFile.Text = "overlay.bmp";
            if (!_overlay && templateFile.Text == "overlay.bmp") templateFile.Text = "template.bmp";
            if (_fileNameChanged == false)
            {
                exportFileName.Text = "wa" + _daxId + "_" + (selectWallNumber.SelectedIndex + 1).ToString() + ".bmp";
                if (_overlay)
                    exportFileName.Text = "wa" + _daxId + "_" + (selectTemplateWallNumber.SelectedIndex + 1).ToString() + "_" + (selectWallNumber.SelectedIndex + 1).ToString() + ".bmp";
                _fileNameChanged = false;
            }
            selectTemplateWallNumber.Enabled = _overlay; 
            loadTemplate();
            copyWallsIntoTemplate();
        }

        private void maybe(object sender, EventArgs e)
        {

        }


    }
}

