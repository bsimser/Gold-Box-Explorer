using System.Drawing;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaTlbColorTable
    {
        public FruaTlbColorTableHeader Header { get; private set; }
        public Color[] Palette { get; private set; }

        public FruaTlbColorTable()
        {
            Header = new FruaTlbColorTableHeader();
            Palette = new Color[256];
            // default 16 EGA colors
            Palette[0] = Color.FromArgb(0, 0, 0);
            Palette[1] = Color.FromArgb(0, 0, 171);
            Palette[2] = Color.FromArgb(0, 171, 0);
            Palette[3] = Color.FromArgb(0, 171, 171);
            Palette[4] = Color.FromArgb(171, 0, 0);
            Palette[5] = Color.FromArgb(171, 0, 171);
            Palette[6] = Color.FromArgb(172, 87, 0);
            Palette[7] = Color.FromArgb(171, 171, 171);
            Palette[8] = Color.FromArgb(87, 87, 87);
            Palette[9] = Color.FromArgb(87, 87, 255);
            Palette[10] = Color.FromArgb(87, 255, 87);
            Palette[11] = Color.FromArgb(87, 255, 255);
            Palette[12] = Color.FromArgb(255, 87, 87);
            Palette[13] = Color.FromArgb(255, 87, 255);
            Palette[14] = Color.FromArgb(255, 255, 87);
            Palette[15] = Color.FromArgb(255, 255, 255);
            Palette[255] = Color.FromArgb(103, 247, 159);
        }
    }
}