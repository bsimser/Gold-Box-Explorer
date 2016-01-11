using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class EgaBlockSpecification : IFileBlockSpecification
    {
        public bool IsSatisfiedBy(FileBlockParameters parameters)
        {
            var length = parameters.Data.Length;

            if( length < 9 ) return false;

            uint height = ArrayToUshort(parameters.Data, 0);
            uint width = ArrayToUshort(parameters.Data, 2);
            var widthPx = width*8;

            if( widthPx == 0 || height == 0 || widthPx > 320 || height > 200 )
                return false;

            uint itemCount = parameters.Data[8];

            const int egaDataOffset = 17;
            var egaDataSize = height * width * 4;

            if (parameters.Data.Length == (egaDataSize * (itemCount + 1)) + egaDataOffset)
            {
                // Death Knights of Krynn
                itemCount += 1;
            }

            return parameters.Data.Length == ((egaDataSize * itemCount) + egaDataOffset);
        }

        private static ushort ArrayToUshort(IList<byte> data, int offset)
        {
            return (ushort)(data[offset + 0] + (data[offset + 1] << 8));
        }
    }
}