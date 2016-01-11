using System.Collections.Generic;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public abstract class RenderBlock : IRenderBlock
    {
        protected List<Bitmap> Bitmaps = new List<Bitmap>();
        private int _blockId;
        public int getBlockId() { return _blockId; }
        public void setBlockId(int id) { _blockId = id; }

        public IEnumerable<Bitmap> GetBitmaps()
        {
            return Bitmaps.AsReadOnly();
        }

        protected static ushort ArrayToUint(IList<byte> data, int offset)
        {
            return (ushort)(data[offset + 0] + (data[offset + 1] << 8) + (data[offset + 2] << 16) + (data[offset + 3] << 24));
        }

        protected static ushort ArrayToUshort(IList<byte> data, int offset)
        {
            return (ushort)(data[offset + 0] + (data[offset + 1] << 8));
        }
    }
}