using System.Drawing;
using System.Drawing.Imaging;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class MonoBlock : RenderBlock
    {
        public MonoBlock(FileBlockParameters parameters)
        {
            setBlockId(parameters.Id);

            int[] monoBitMask = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 }; 
            var count = parameters.Data.Length / 8;

            for (var ch = 0; ch < count; ch++)
            {
                var bitmap = new Bitmap(8, 8, PixelFormat.Format16bppArgb1555);

                for (var y = 0; y < 8; y++)
                {
                    for (var x = 0; x < 8; x++)
                    {
                        var b = parameters.Data[(ch * 8) + y];
                        var c = ((b & monoBitMask[x]) != 0) ? Color.White : Color.Black;
                        bitmap.SetPixel(x, y, c);
                    }
                }
                
                Bitmaps.Add(bitmap);
            }
        }
    }
}