using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaMixedBlockSpecification : IFileBlockSpecification
    {
        public bool IsSatisfiedBy(FileBlockParameters parameters)
        {
            var data = parameters.Data;

            if (data.Length < 5)
                return false;

            uint frames = data[0];
            if (frames > 8)
                return false;

            int offset = 5;
            if (data.Length < 20 + offset)
                return false;

            int height = ArrayToUshort(data, 0 + offset);
            int width = ArrayToUshort(data, 2 + offset);
            
            int width_px = width * 8;

            int clr_count = data[9 + offset];
            int clr_start = data[10 + offset];

            if (width_px < 1 || height < 1 || width_px > 320 || height > 200)
                return false;

            bool egaBlock = (data[9 + offset] & 0xCC) == 0;
            int blocksize = egaBlock ? (4 * width * height) : ((5 * width * height) + 14);

            if (data.Length < blocksize + 9 + offset)
                return false;

            int data_offset = getDataOffset(9 + offset, data);

            if (data.Length < frames * blocksize)
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