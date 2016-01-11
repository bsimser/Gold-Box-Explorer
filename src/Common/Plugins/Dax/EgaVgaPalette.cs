using System.Collections.Generic;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class EgaVgaPalette
    {
        public static readonly uint EGAtransparentColor = 0x67f79f;
        public static readonly uint EGAgreyColor = 0XFF525252;

        public static readonly uint EGABlackColor = 0xFF000000;
        public static readonly uint EGABlueColor = 0XFF0000AA;
        public static readonly uint EGAGreenColor = 0XFF00AA00;
        public static readonly uint EGACyanColor = 0XFF00AAAA;
        public static readonly uint EGARedColor = 0XFFAA0000;
        public static readonly uint EGAMagentaColor = 0XFFAA00AA;
        public static readonly uint EGABrownColor = 0XFFAA5500;
        public static readonly uint EGALightGrayColor = 0XFFAAAAAA;
        public static readonly uint EGADarkGrayColor = 0XFF555555;
        public static readonly uint EGABrightBlueColor = 0XFF5555FF;
        public static readonly uint EGABrightGreenColor = 0XFF55FF55;
        public static readonly uint EGABrightCyanColor = 0XFF55FFFF;
        public static readonly uint EGABrightRedColor = 0XFFFF5555;
        public static readonly uint EGABrightMagentaColor = 0XFFFF55FF;
        public static readonly uint EGABrightYellowColor = 0XFFFFFF55;
        public static readonly uint EGABrightWhiteColor = 0XFFFFFFFF;

        public static readonly uint[] EgaColors = {EGABlackColor, EGABlueColor, EGAGreenColor, EGACyanColor, EGARedColor,
                                                      EGAMagentaColor, EGABrownColor, EGALightGrayColor, EGADarkGrayColor,
                                                      EGABrightBlueColor, EGABrightGreenColor, EGABrightCyanColor,
                                                      EGABrightRedColor, EGABrightMagentaColor, EGABrightYellowColor,
                                                      EGABrightWhiteColor};
        public static readonly uint[] EgaCombatColors = {EGAtransparentColor, EGABlueColor, EGAGreenColor, EGACyanColor, EGARedColor,
                                                      EGAMagentaColor, EGABrownColor, EGALightGrayColor, EGABlackColor,
                                                      EGABrightBlueColor, EGABrightGreenColor, EGABrightCyanColor,
                                                      EGABrightRedColor, EGABrightMagentaColor, EGABrightYellowColor,
                                                      EGABrightWhiteColor};
            /*  public static readonly uint[] EgaColors = {
                                                       EGAtransparentColor, 0XFF0000AD, 0XFF00AD00, 0XFF00ADAD, 0XFFAD0000, 0XFFAD00AD,
                                                       0XFFAD5200, 0XFFADADAD,
                                                       EGABlackColor, 0XFF5252FF, 0XFF52FF52, 0XFF52FFFF, 0XFFFF5252, 0XFFFF52FF,
                                                       0XFFFFFF52, 0XFFFFFFFF
                                                   };*/
       /* public static readonly uint[] EgaColors = {
                                                 0XFF000000, 0XFF0000AD, 0XFF00AD00, 0XFF00ADAD, 0XFFAD0000, 0XFFAD00AD,
                                                 0XFFAD5200, 0XFFADADAD,
                                                 0XFF525252, 0XFF5252FF, 0XFF52FF52, 0XFF52FFFF, 0XFFFF5252, 0XFFFF52FF,
                                                 0XFFFFFF52, 0XFFFFFFFF
                                             };*/
        
        public static Color[] PaletteBase()
        {
            var clrs = new Color[256];
            for (var i = 0; i < 16; i++)
            {
                clrs[i + 0x00] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x10] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x20] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x30] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x40] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x50] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x60] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x70] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x80] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0x90] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0xA0] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0xB0] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0xC0] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0xD0] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0xE0] = Color.FromArgb((int)EgaColors[i]);
                clrs[i + 0xF0] = Color.FromArgb((int)EgaColors[i]);
            }

            return clrs;
        }

        public static Color[] ExtractPalette(IList<byte> data, int clrCount, int clrBase, int offset)
        {
            var clrs = PaletteBase();

            for (var i = 0; i < clrCount; i++)
            {
                var r = data[offset + (i * 3) + 0] * 4;
                var g = data[offset + (i * 3) + 1] * 4;
                var b = data[offset + (i * 3) + 2] * 4;

                if (r > 255 || g > 255 || b > 255)
                {
                    return null;
                }
                clrs[clrBase + i] = Color.FromArgb(r, g, b);
            }

            return clrs;
        }
    }
}