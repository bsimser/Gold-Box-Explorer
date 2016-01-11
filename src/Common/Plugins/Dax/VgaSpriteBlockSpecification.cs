using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaSpriteBlockSpecification : IFileBlockSpecification
    {
        public bool IsSatisfiedBy(FileBlockParameters parameters)
        {
            var data = parameters.Data;
            var origData = data;

            if (data.Length < 20)
                return false;

            if (data[1] != 0)
                return false;

            int height = data[0];
            int width = data[2];
            var widthPx = width*8;
            var heightPx = height;

            if (widthPx < 1 || heightPx < 1 || widthPx > 320 || heightPx > 200)
                return false;

            int frames = data[8];
            int clrBase = data[9];
            int clrCount = data[10];

            if (clrBase == 0)
                return false;

            if (clrCount == 0)
                return false;

            var dataStart = (clrCount*3) + 132 + (3*frames);

            if (Path.GetFileName(parameters.Name).ToUpper().StartsWith("SPRIT") && clrBase == 176 && clrCount == 80)
            {
                dataStart = 306;    
            }
            
            data = UnpackSpriteData(data, dataStart);

            if (data.Length <= 0)
                return false;

            var picSize = widthPx * heightPx;
            if (picSize <= 0)
                return false;

            if (data.Length % picSize != 0)
                return false;

            var frameCount = data.Length / picSize;
            if (picSize * frameCount > data.Length)
                return false;

            var clrs = EgaVgaPalette.ExtractPalette(origData, clrCount, clrBase, 11);
            if (clrs == null)
                return false;

            return true;
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