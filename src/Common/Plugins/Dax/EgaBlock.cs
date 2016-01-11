using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class EgaBlock : RenderBlock
    {
        private uint[] EgaColors;
        private bool isCombatPicture = false; // images used in combat mode have a slightly different EGA palette (colors 0 and 8 are swapped)
        public EgaBlock(FileBlockParameters parameters)
        {
            setBlockId(parameters.Id);
            var data = parameters.Data;
            uint height = ArrayToUshort(data, 0);
            uint width = ArrayToUshort(data, 2);
            uint xPos = ArrayToUshort(data, 4);
            uint yPos = ArrayToUshort(data, 6);
            uint itemCount = data[8];
            var widthPx = width * 8;
            var heightPx = height;
            var xPosPx = xPos * 8;
            var yPosPx = yPos * 8;
            const int egaDataOffset = 17;
            var egaDataSize = height * width * 4;
            EgaColors = EgaVgaPalette.EgaColors;

            var filename = Path.GetFileName(parameters.Name).ToUpper();
            if (filename.Contains("CPIC") || filename.Contains("CHEAD") || filename.Contains("CBODY")
                || filename.Contains("DUNGCOM") || filename.Contains("WILDCOM") || filename.Contains("RANDCOM")
                || filename.Contains("COMSPR"))
            {
                isCombatPicture = true;
                EgaColors = EgaVgaPalette.EgaCombatColors;
            }

            if (data.Length == (egaDataSize * (itemCount + 1)) + egaDataOffset)
            {
                // Death Knights of Krynn
                itemCount += 1;
            }

            if (data.Length == (egaDataSize * itemCount) + egaDataOffset)
            {
                var offset = 0;
                for (var i = 0; i < itemCount; i++, offset += (int)egaDataSize)
                {
                    var bitmap = new Bitmap((int)(widthPx + xPosPx), (int)(heightPx + yPosPx),
                                            PixelFormat.Format16bppArgb1555);
                    for (var y = 0; y < heightPx; y++)
                    {
                        for (var x = 0; x < widthPx; x += 2)
                        {
                            var b = data[egaDataOffset + (y * width * 4) + (x / 2) + offset];
                            var pxX = (int)(x + 0 + xPosPx);
                            var pxY = (int)(y + yPosPx);
                            Color ctest = Color.FromArgb((int)EgaColors[b >> 4]);
                            bitmap.SetPixel(pxX, pxY, Color.FromArgb((int)EgaColors[b >> 4]));
                            bitmap.SetPixel(pxX + 1, pxY, Color.FromArgb((int)EgaColors[b & 0xF]));
                        }
                    }

                    Bitmaps.Add(bitmap);
                }
            }
        }
    }
}