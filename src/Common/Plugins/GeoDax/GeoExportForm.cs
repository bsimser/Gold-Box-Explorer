using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using GoldBoxExplorer.Lib;
using GoldBoxExplorer.Lib.Plugins;

//namespace GoldBoxExplorer
namespace GoldBoxExplorer.Lib.Plugins.GeoDax
{
    public partial class GeoExportForm : Form
    {
        private PictureBox _picturebox;

        private string _mapName;

        public GeoExportForm()
        {
            InitializeComponent();
        }

        public GeoExportForm(PictureBox picturebox,  string mapName = "")
            : this()
        {

            _picturebox = picturebox;
            _mapName = mapName;

            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox1.Text;
            var result = folderBrowserDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                var targetFolder = textBox1.Text;


                var exportFormat = ImageFormat.Png;
                if (formatBmp.Checked) exportFormat = ImageFormat.Bmp;
                else if (formatJpg.Checked) exportFormat = ImageFormat.Jpeg;


                    Bitmap bitmap = (Bitmap) _picturebox.Image;
                           var outputFilename = string.Format(@"{0}\\{1}.{2}",
                                                        targetFolder, _mapName,
                                                         exportFormat.ToString().ToLower());

                           var bm32bpp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                           using (var g = Graphics.FromImage(bm32bpp)) {
                                g.DrawImage(bitmap, 0, 0);
                                bm32bpp.Save(outputFilename, exportFormat);
                            }



                Hide();

                MessageBox.Show(string.Format("Exported {0} images as {1} to {2}", 
                    1, exportFormat.ToString().ToUpper(), targetFolder));
            }
        }
    }
}
