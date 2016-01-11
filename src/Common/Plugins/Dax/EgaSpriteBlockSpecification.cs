using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class EgaSpriteBlockSpecification : IFileBlockSpecification
    {
        public bool IsSatisfiedBy(FileBlockParameters parameters)
        {
            var data = parameters.Data;
            int frames = data[0];

            int offset = 1;
            if (frames == 0 || frames > 8) 
                return false;

            for (int frame = 0; frame < frames; frame++)
            {
                if (data.Length < 21 + offset) 
                    return false;

                offset += 4;
                int height = ArrayToUshort(data, offset);
                offset += 2;
                int width = ArrayToUshort(data, offset);
                offset += 2;
                int x_pos = ArrayToUshort(data, offset);
                offset += 2;
                int y_pos = ArrayToUshort(data, offset);
                offset += 3;
                offset += 8;

                // skip 1 byte
                // skip 8 bytes
                int width_px = width * 8;
                int height_px = height;
                int x_pos_px = x_pos * 8;
                int y_pos_px = y_pos * 8;

                if (width_px < 1 || height_px < 1 || width_px > 320 || height_px > 200)
                    return false;

                int egaDataSize = height * width * 4;

                if (data.Length < egaDataSize + offset) 
                    return false;

                offset += egaDataSize;
            }

            if (data.Length > offset)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private static ushort ArrayToUshort(IList<byte> data, int offset)
        {
            return (ushort)(data[offset + 0] + (data[offset + 1] << 8));
        }
    }
}
