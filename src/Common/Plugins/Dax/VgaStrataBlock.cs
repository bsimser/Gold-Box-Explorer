using System;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class VgaStrataBlock : RenderBlock
    {
        public VgaStrataBlock(FileBlockParameters parameters)
        {
            setBlockId(parameters.Id);

            var data = parameters.Data;
            int height = ArrayToUshort(data, 0);
            int width = ArrayToUshort(data, 2);
            int xPos = ArrayToUshort(data, 4);
            int yPos = ArrayToUshort(data, 6);
            int picCount = data[8];
            var widthPx = width * 8;
            var heightPx = height;
            var pxlClrs = new int[heightPx, widthPx];
            var xPosPx = xPos * 8;
            var yPosPx = yPos * 8;
            var tempPalette = new byte[59];
            var dataOffset = GetDataOffset(tempPalette, 9, data);

            var dd = new int[48];
            const int paletteBase = 0x20;
            for (var i = 0; i < 24; i++)
            {
                var b = tempPalette[paletteBase + i];
                dd[0 + i] = (b & 0x0f) * 4;
                dd[24 + i] = (b & 0xf0) / 4;
            }

            var egaPalette = EgaVgaPalette.PaletteBase();
            for (var i = 0; i < 14; i += 1)
            {
                var r = dd[(i * 3) + 6 + 0];
                var g = dd[(i * 3) + 6 + 1];
                var b = dd[(i * 3) + 6 + 2];
                egaPalette[i + 18] = Color.FromArgb(r * 4, g * 4, b * 4);
            }

            for (int pic = 0; pic < picCount; pic++)
            {
                var inOffset = dataOffset;

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < widthPx; x++)
                    {
                        var mask = 0x80 >> (x & 7);
                        var inputByte = x / 8;
                        byte pxClr = 0;

                        if ((data[inOffset + inputByte] & mask) != 0)
                        {
                            pxClr = 1;
                        }

                        if ((data[inOffset + inputByte + width] & mask) != 0)
                        {
                            pxClr += 2;
                        }

                        if ((data[inOffset + inputByte + (width * 2)] & mask) != 0)
                        {
                            pxClr += 4;
                        }

                        if ((data[inOffset + inputByte + (width * 3)] & mask) != 0)
                        {
                            pxClr += 8;
                        }

                        if ((data[inOffset + inputByte + (width * 4)] & mask) != 0)
                        {
                            pxClr += 0x10;
                        }

                        pxlClrs[y, x] = pxClr;
                    }

                    inOffset += width * 5;
                }

                dataOffset = inOffset;

                var bitmap = new Bitmap(widthPx + xPosPx, heightPx + yPosPx, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
                for (var y = 0; y < heightPx; y++)
                {
                    for (var x = 0; x < widthPx; x += 1)
                    {
                        var pxX = x + xPosPx;
                        var pxY = y + yPosPx;
                        bitmap.SetPixel(pxX, pxY, egaPalette[pxlClrs[y, x]]);
                    }
                }

                Bitmaps.Add(bitmap);
            }
        }

        public static int GetDataOffset(byte[] tempPalette, int offset, byte[] inputBytes)
        {
            var var6A = new byte[0x20];
            var var4A = new byte[0x20];
            var var2A = new byte[0x18];
            var var12 = new byte[0x10];
            byte var1;

            if ((inputBytes[offset] & 0xCC) == 0)
            {
                tempPalette[0] = 3;

                Array.Copy(inputBytes, offset, var12, 0, 8);

                for (var1 = 0; var1 <= 0x0f; var1++)
                {
                    var4A[var1] = var1;

                    if ((var1 & 1) != 0)
                    {
                        var6A[var1] = (byte)(var12[var1 / 2] & 0x0F);
                    }
                    else
                    {
                        var6A[var1] = (byte)((var12[var1 / 2] >> 4) & 0x0F);
                    }
                }
                offset += 8;
            }
            else
            {
                tempPalette[0] = inputBytes[offset];
                offset += 1;

                if ((tempPalette[0] & 1) != 0)
                {
                    Array.Copy(inputBytes, offset, var12, 0, 8);
                    for (var1 = 0; var1 <= 0x1F; var1++)
                    {
                        var dx = var1 + var1 + 6;
                        var dl = (var12[var1 / 4] >> dx) & 3;
                        var6A[var1] = (byte)dl;
                    }
                    offset += 8;
                }

                if ((tempPalette[0] & 2) != 0)
                {
                    Array.Copy(inputBytes, offset, var12, 0, 0x10);

                    for (var1 = 0; var1 < 0x10; var1++)
                    {
                        var4A[(var1 * 2) + 0] = (byte)(var12[var1] & 0x0F);
                        var4A[(var1 * 2) + 1] = (byte)((var12[var1] >> 4) & 0x0F);
                    }
                    offset += 0x10;
                }

                if ((tempPalette[0] & 8) != 0)
                {
                    Array.Copy(inputBytes, offset, var2A, 0, 0x18);
                    offset += 0x18;
                }
            }


            Array.Copy(var2A, 0, tempPalette, 0x20, 0x18);

            if ((tempPalette[0] & 8) != 0)
            {
                tempPalette[0x39] = 0xFF;
            }

            return offset;
        }
    }
}