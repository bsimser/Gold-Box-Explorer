using System.Drawing;
using System.Drawing.Imaging;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaBlock : RenderBlock
    {
        public VgaBlock(FileBlockParameters parameters)
        {
            setBlockId(parameters.Id);

            var data = parameters.Data;

            int height = data[0];
            int width = data[1];
            const int xPos = 0;
            const int yPos = 0;
            var widthPx = width * 8;
            var heightPx = height;
            const int xPosPx = xPos * 8;
            const int yPosPx = yPos * 8;
            int clrBase = data[8];
            int clrCount = data[9] + 1;
            var paletteEnd = 10 + (clrCount * 3);
            var chunkSize = (widthPx * heightPx);
            var chunkCount = ArrayToUshort(data, 6);
            var dataStart = data.Length - (chunkCount * chunkSize);

            for (var chunk = 0; chunk < chunkCount; chunk++, dataStart += chunkSize)
            {
                var clrs = EgaVgaPalette.ExtractPalette(data, clrCount, clrBase, 10);

                if (clrs == null)
                {
                    return;
                }

                var bitmap = new Bitmap((widthPx + xPosPx), (heightPx + yPosPx), PixelFormat.Format16bppArgb1555);
                for (var y = 0; y < heightPx; y++)
                {
                    for (var x = 0; x < widthPx; x += 1)
                    {
                        var b = data[dataStart + (y * widthPx) + x];
                        var pxX = (x + 0 + xPosPx);
                        var pxY = (y + yPosPx);
                        bitmap.SetPixel(pxX, pxY, clrs[b]);
                    }
                }

                Bitmaps.Add(bitmap);
            }
        }
    }
}