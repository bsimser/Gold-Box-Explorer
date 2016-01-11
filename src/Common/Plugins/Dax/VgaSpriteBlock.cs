using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaSpriteBlock : RenderBlock
    {
        public VgaSpriteBlock(FileBlockParameters parameters)
        {
            setBlockId(parameters.Id);
            var data = parameters.Data;
            var origData = data;

            int height = data[0];
            int width = data[2];
            var widthPx = width * 8;
            var heightPx = height;
            int frames = data[8];
            int clrBase = data[9];
            int clrCount = data[10];
            var picSize = widthPx * heightPx;
            var isSpriteFile = false;

            var clrs = EgaVgaPalette.ExtractPalette(origData, clrCount, clrBase, 11);
            var dataStart = (clrCount * 3) + 132 + (3 * frames);

            if (Path.GetFileName(parameters.Name).ToUpper().StartsWith("SPRIT"))
            {
                clrs[0] = Color.FromArgb(0);
                clrs[13] = Color.FromArgb(0, 0, 0);
                isSpriteFile = true;
                if (clrBase == 176 && clrCount == 80)
                    dataStart = 306;
            }

            data = UnpackSpriteData(data, dataStart);
            var frameCount = data.Length / picSize;

            int mframe = origData[(clrCount * 3) + 132 - 5];

            if (isSpriteFile)
            {
                mframe = frameCount - 1;
            }

            for (var frame = 0; frame < frameCount; frame++)
            {
                var bitmap = new Bitmap(widthPx, heightPx, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);

                for (var y = 0; y < heightPx; y++)
                {
                    for (var x = 0; x < widthPx; x += 1)
                    {
                        var b1 = data[(y * widthPx) + x + (picSize * frame)];
                        var b3 = data[(y * widthPx) + x + (picSize * mframe)];
                        bitmap.SetPixel(x, y, frame != mframe ? clrs[b1 ^ b3] : clrs[b1]);
                    }
                }

                Bitmaps.Add(bitmap);
            }
        }


        static byte[] UnpackSpriteData(IList<byte> data, int offset)
        {
            var nd = new List<byte>();

            var inputIndex = offset;
            while (inputIndex < data.Count)
            {
                int runLength = (sbyte)data[inputIndex];

                if (runLength >= 0)
                {
                    if (inputIndex + runLength + 1 >= data.Count)
                        return new byte[0];

                    for (var i = 0; i <= runLength; i++)
                    {
                        nd.Add(data[inputIndex + i + 1]);
                    }

                    inputIndex += runLength + 2;
                }
                else
                {
                    runLength = -runLength;

                    if (inputIndex + 1 >= data.Count)
                        return new byte[0];

                    for (var i = 0; i <= runLength; i++)
                    {
                        nd.Add(data[inputIndex + 1]);
                    }

                    inputIndex += 2;
                }
            }

            return nd.ToArray();
        }
    }
}