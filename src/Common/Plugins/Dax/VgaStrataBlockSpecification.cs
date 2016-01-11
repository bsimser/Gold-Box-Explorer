using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaStrataBlockSpecification : IFileBlockSpecification
    {
        public bool IsSatisfiedBy(FileBlockParameters parameters)
        {
            var data = parameters.Data;

            if (data.Length < 20)
                return false;

            int height = ArrayToUshort(data, 0);
            int width = ArrayToUshort(data, 2);

            var widthPx = width * 8;
            var heightPx = height;

            if (widthPx < 1 || heightPx < 1 || widthPx > 320 || heightPx > 200)
                return false;

            int picCount = data[8];

            var dataOffset = getDataOffset(9, data);
            var dataLen = (((5 * width * height) * picCount) + dataOffset);

            if (data.Length != dataLen)
                return false;

            return true;
        }

        public static int getDataOffset(int offset, byte[] data)
        {
            if ((data[offset] & 0xCC) == 0)
            {
                offset += 8;
            }
            else
            {
                var b = data[offset];
                offset += 1;

                if ((b & 1) != 0)
                {
                    offset += 8;
                }

                if ((b & 2) != 0)
                {
                    offset += 0x10;
                }

                if ((b & 8) != 0)
                {
                    offset += 0x18;
                }
            }

            return offset;
        }

        private static ushort ArrayToUshort(IList<byte> data, int offset)
        {
            return (ushort)(data[offset + 0] + (data[offset + 1] << 8));
        }
    }
}