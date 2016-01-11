using System;
using System.Collections.Generic;
using System.Text;

namespace GoldBoxExplorer.Lib.Frua
{
    public static class ByteHelper
    {
        public static ushort ArrayToDword(IList<byte> data, int offset)
        {
            return (ushort)(data[offset + 0] + (data[offset + 1] << 8) + (data[offset + 2] << 16) + (data[offset + 3] << 24));
        }

        public static string ArrayToString(byte[] src, int offset, int count)
        {
            var dest = new byte[count];
            Buffer.BlockCopy(src, offset, dest, 0, count);
            return Encoding.Default.GetString(dest);
        }

        public static ushort ArrayToWord(IList<byte> data, int offset)
        {
            return (ushort)(data[offset + 0] + (data[offset + 1] << 8));
        }
    }
}