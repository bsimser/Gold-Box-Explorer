using System;
using System.Collections.Generic;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaMixedBlock : RenderBlock
    {
        public VgaMixedBlock(FileBlockParameters parameters)
        {
            setBlockId(parameters.Id);

            var data = parameters.Data;
            var origData = data;

            if (data.Length < 5)
                return;

            uint frames = data[0];
            int offset = 5;
            if (frames > 8)
                return;

            if (data.Length < 20 + offset)
                return;

            // try single frame
            int height = ArrayToUshort(data, 0 + offset);
            int width = ArrayToUshort(data, 2 + offset);
            int x_pos = ArrayToUshort(data, 4 + offset);
            int y_pos = ArrayToUshort(data, 6 + offset);
            int item_count = data[8 + offset];

            int width_px = width * 8;
            int height_px = height;
            int x_pos_px = x_pos * 8;
            int y_pos_px = y_pos * 8;

            if (width_px < 1 || height_px < 1 || width_px > 320 || height_px > 200)
                return;

            bool egaBlock = (data[9 + offset] & 0xCC) == 0;
            int blocksize = egaBlock ? (4 * width * height) : (5 * width * height);

            if (data.Length < blocksize + 9 + offset)
                return;


            byte[] tempPalette = new byte[0x3B];
            int data_offset = DecodePaletteData(tempPalette, 9 + offset, data);

            int[] dd = new int[48];
            int _base = 0x20;
            for (int i = 0; i < 24; i++)
            {
                int b = tempPalette[_base + i];
                int a1 = (b & 0x0f);
                int a2 = (b & 0xf0);
                dd[0 + i] = (b & 0x0f) * 4;
                dd[24 + i] = (b & 0xf0) / 4;
            }

            var clrs = PaletteBase();

            for (int i = 0; i < 14; i += 1)
            {
                int r = dd[(i * 3) + 6 + 0];
                int g = dd[(i * 3) + 6 + 1];
                int b = dd[(i * 3) + 6 + 2];
                clrs[i + 18] = Color.FromArgb(r * 4, g * 4, b * 4);
            }

            int[,] pxl_clrs = new int[height_px, width_px];

            for (int frame = 0; frame < frames; frame++)
            {
                if (egaBlock)
                {
                    int in_offset = data_offset;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width_px; x += 2)
                        {
                            pxl_clrs[y, x] = data[in_offset] >> 4;
                            pxl_clrs[y, x + 1] = data[in_offset] & 0x0F;

                            in_offset += 1;
                        }
                    }

                    data_offset = in_offset;
                }
                else
                {
                    int in_offset = data_offset;
                    int out_offset = 0;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width_px; x++)
                        {
                            int mask = 0x80 >> (x & 7);
                            int input_byte = x / 8;
                            byte px_clr = 0;

                            if ((data[in_offset + input_byte] & mask) != 0)
                            {
                                px_clr = 1;
                            }

                            if ((data[in_offset + input_byte + width] & mask) != 0)
                            {
                                px_clr += 2;
                            }

                            if ((data[in_offset + input_byte + (width * 2)] & mask) != 0)
                            {
                                px_clr += 4;
                            }

                            if ((data[in_offset + input_byte + (width * 3)] & mask) != 0)
                            {
                                px_clr += 8;
                            }

                            if ((data[in_offset + input_byte + (width * 4)] & mask) != 0)
                            {
                                px_clr += 0x10;
                            }

                            pxl_clrs[y, x] ^= px_clr;
                        }

                        in_offset += width * 5;
                        out_offset += width * 8;
                    }

                    data_offset = in_offset + 14;
                }

                var bitmap = new Bitmap(width_px + x_pos_px, height_px + y_pos_px, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                for (int y = 0; y < height_px; y++)
                {
                    for (int x = 0; x < width_px; x += 1)
                    {
                        int pxX = x + x_pos_px;
                        int pxY = y + y_pos_px;
                        bitmap.SetPixel(pxX, pxY, clrs[pxl_clrs[y, x]]);
                    }
                }

                Bitmaps.Add(bitmap);
            }
        }


        public static int DecodePaletteData(byte[] arg_0, int arg_4, byte[] arg_8)
        {
            byte[] var_6A = new byte[0x20];
            byte[] var_4A = new byte[0x20];
            byte[] var_2A = new byte[0x18];
            byte[] var_12 = new byte[0x10];
            byte var_1;

            if ((arg_8[arg_4] & 0xCC) == 0)
            {
                arg_0[0] = 3;

                Array.Copy(arg_8, arg_4, var_12, 0, 8);

                for (var_1 = 0; var_1 <= 0x0f; var_1++)
                {
                    var_4A[var_1] = var_1;

                    if ((var_1 & 1) != 0)
                    {
                        var_6A[var_1] = (byte)(var_12[var_1 / 2] & 0x0F);
                    }
                    else
                    {
                        var_6A[var_1] = (byte)((var_12[var_1 / 2] >> 4) & 0x0F);
                    }
                }
                arg_4 += 8;
            }
            else
            {
                arg_0[0] = arg_8[arg_4];
                arg_4 += 1;

                if ((arg_0[0] & 1) != 0)
                {
                    Array.Copy(arg_8, arg_4, var_12, 0, 8);
                    for (var_1 = 0; var_1 <= 0x1F; var_1++)
                    {
                        int dx = var_1 + var_1 + 6;

                        int dl = (var_12[var_1 / 4] >> dx) & 3;

                        var_6A[var_1] = (byte)dl;
                    }
                    arg_4 += 8;
                }

                if ((arg_0[0] & 2) != 0)
                {
                    Array.Copy(arg_8, arg_4, var_12, 0, 0x10);

                    for (var_1 = 0; var_1 < 0x10; var_1++)
                    {
                        var_4A[(var_1 * 2) + 0] = (byte)(var_12[var_1] & 0x0F);
                        var_4A[(var_1 * 2) + 1] = (byte)((var_12[var_1] >> 4) & 0x0F);
                    }
                    arg_4 += 0x10;
                }

                if ((arg_0[0] & 8) != 0)
                {
                    Array.Copy(arg_8, arg_4, var_2A, 0, 0x18);
                    arg_4 += 0x18;
                }
            }


            Array.Copy(var_2A, 0, arg_0, 0x20, 0x18);

            if ((arg_0[0] & 8) != 0)
            {
                arg_0[0x39] = 0xFF;
            }

            return arg_4;
        }


        static Color[] PaletteBase()
        {
            Color[] clrs = new Color[256];
            for (int i = 0; i < 16; i++)
            {
                clrs[i + 0x00] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x10] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x20] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x30] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x40] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x50] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x60] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x70] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x80] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0x90] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0xA0] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0xB0] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0xC0] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0xD0] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0xE0] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
                clrs[i + 0xF0] = Color.FromArgb((int)EgaVgaPalette.EgaColors[i]);
            }

            return clrs;
        }

        //private static ushort ArrayToUshort(IList<byte> data, int offset)
        //{
        //    return (ushort)(data[offset + 0] + (data[offset + 1] << 8));
        //}
    }
}