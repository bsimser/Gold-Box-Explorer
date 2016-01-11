using System.Drawing;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class EgaSpriteBlock : RenderBlock
    {
        public EgaSpriteBlock(FileBlockParameters parameters)
        {
            var data = parameters.Data;

            setBlockId(parameters.Id);
            uint frames = data[0];
            int offset = 1;
            if (frames > 8)
                return;

            Color[] clrs = new Color[16];
            for (int i = 0; i < 16; i++)
            {
                clrs[i] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
            }

            var filename = Path.GetFileName(parameters.Name).ToUpper();
            bool xorFrames = filename.StartsWith("PIC", true, System.Globalization.CultureInfo.CurrentCulture);
            xorFrames |= filename.StartsWith("FINAL", true, System.Globalization.CultureInfo.CurrentCulture);

            if (filename.StartsWith("SPRI", true, System.Globalization.CultureInfo.CurrentCulture))
            {
                clrs[0] = Color.FromArgb(0, 0, 0, 0);
                clrs[13] = Color.FromArgb((int)EgaVgaPalette.EgaColors[0]);
            }


            byte[] first_frame_ega_layout = null;

            for (int frame = 0; frame < frames; frame++)
            {
                if (data.Length < 21 + offset) return;

                uint delay = ArrayToUint(data, offset);
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
                    return;

                int egaDataSize = height * width * 4;

                if (data.Length < egaDataSize + offset) return;

                if (xorFrames)
                {
                    if (frame == 0)
                    {
                        first_frame_ega_layout = new byte[egaDataSize + 1];

                        System.Array.Copy(data, offset, first_frame_ega_layout, 0, egaDataSize);
                    }
                    else
                    {
                        for (int i = 0; i < egaDataSize; i++)
                        {
                            byte b = first_frame_ega_layout[i];
                            data[offset + i] ^= b;
                        }
                    }
                }

                var bitmap = new Bitmap((int)(width_px + x_pos_px), (int)(height_px + y_pos_px), System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                for (int y = 0; y < height_px; y++)
                {
                    for (int x = 0; x < width_px; x += 2)
                    {
                        byte b = data[(y * width * 4) + (x / 2) + offset];
                        int pxX = (int)(x + 0 + x_pos_px);
                        int pxY = (int)(y + y_pos_px);
                        bitmap.SetPixel(pxX, pxY, clrs[b >> 4]);
                        bitmap.SetPixel(pxX + 1, pxY, clrs[b & 0xF]);
                    }
                }

                Bitmaps.Add(bitmap);

                offset += egaDataSize;

            }
        }
    }
}
