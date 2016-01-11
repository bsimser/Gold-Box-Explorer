using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaTlbFile : GoldBoxFile
    {
        private readonly string _fullPath;
        private readonly FruaTlbFileHeader _header;
        private UInt32 _colorDataOffset;
        private UInt32 _imageIdTableOffset;
        private readonly IList<UInt32> _offsets;
        private readonly IList<FruaTlbImage> _images;
        private readonly FruaTlbColorTable _colorTable;

        public IList<Bitmap> Bitmaps { get; private set; }

        public FruaTlbFile(string fullPath)
        {
            _fullPath = fullPath;
            _header = new FruaTlbFileHeader();
            _offsets = new List<uint>();
            _images = new List<FruaTlbImage>();
            _colorTable = new FruaTlbColorTable();
            Bitmaps = new List<Bitmap>();
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_fullPath);
        }

        public override string GetStatusMessage()
        {
            return string.Format("{0} {1} {2} images using {3} colors starting at {4}", 
                _header.FileType, _header.ContentType, Bitmaps.Count, 
                _colorTable.Header.NumColorsInPalette,
                _colorTable.Header.FirstPaletteColorUsed);
        }

        public void LoadBitmaps()
        {
            using (var stream = new FileStream(_fullPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    ReadHeader(reader);

                    if (_header.ContentType == "TILE")
                    {
                        var imageCount = GetImageCount(reader);
                        ReadTileLibrary(reader, imageCount, 0);
                    }
                    else
                    {
                        ReadMasterLibrary(reader);
                    }
                }
            }
        }

        private void ReadMasterLibrary(BinaryReader reader)
        {
            var offsetToSetIdTable = reader.ReadUInt32();
            var imagePointers = new List<UInt32>();

            // don't read EOF pointer
            for (var i = 0; i < _header.Entries - 1; i++)
            {
                imagePointers.Add(reader.ReadUInt32());
            }

            foreach (var pointer in imagePointers)
            {
                reader.BaseStream.Seek(pointer, SeekOrigin.Begin);
                ReadHeader(reader);
                var imageCount = GetImageCount(reader);
                ReadTileLibrary(reader, imageCount, pointer);
            }
        }

        private void ReadTileLibrary(BinaryReader reader, int imageCount, uint startingPointForSeek)
        {
            _offsets.Clear();

            for (int i = 0; i < imageCount; i++)
            {
                var offset = reader.ReadUInt32();
                _offsets.Add(offset);
            }

            ReadColorTable(reader, startingPointForSeek);

            // read images
            for (var entry = 0; entry < imageCount; entry++)
            {
                var offset = _offsets[entry];
                reader.BaseStream.Seek(offset + startingPointForSeek, SeekOrigin.Begin);

                var image = new FruaTlbImage
                    {
                        Header =
                            {
                                ImageHeight = reader.ReadUInt16(),
                                VerticalOffset = reader.ReadInt16(),
                                HorizontalOffset = reader.ReadInt16(),
                                ImageWidth = reader.ReadByte()*4,
                                DrawingMethod = reader.ReadByte()
                            }
                    };

                // read image if we can
                switch (image.Header.DrawingMethod)
                {
                    case 16:
                        CreateBitmap16(image, reader);
                        break;

                    case 17:
                        CreateBitmap17(image, reader);
                        break;

                    case 18:
                        CreateBitmap18(image, reader);
                        break;

                    case 21:
                        CreateBitmap21(image, reader);
                        break;

                    case 23:
                        // TODO not working right yet
                        //CreateBitmap23(image, reader);
                        break;

                    case 24:
                        // TODO don't know how to read this yet
                        break;

                    case 25:
                        // TODO don't know how to read this yet
                        break;

                    default:
                        throw new Exception(string.Format("Unknown Drawing Method \"{0}\"", image.Header.DrawingMethod));
                }

                _images.Add(image);
            }
        }

        /// <summary>
        /// Uncompressed, non-transparent
        /// In transparent images, color 255 is not drawn to the screen
        /// </summary>
        /// <param name="image"></param>
        /// <param name="reader"></param>
        private void CreateBitmap16(FruaTlbImage image, BinaryReader reader)
        {
            var bitmap = CreateBitmap(image, reader);
            Bitmaps.Add(bitmap);
        }

        /// <summary>
        /// Uncompressed, transparent -- with image mask
        /// In transparent images, color 255 is not drawn to the screen
        /// </summary>
        /// <param name="image"></param>
        /// <param name="reader"></param>
        private void CreateBitmap17(FruaTlbImage image, BinaryReader reader)
        {
            var bitmap = CreateBitmap(image, reader);
            Bitmaps.Add(bitmap);
        }

        private void CreateBitmap18(FruaTlbImage image, BinaryReader reader)
        {
            var targetPixelCountToPlot = image.Header.ImageWidth*image.Header.ImageHeight;
            var width = image.Header.ImageWidth;
            var actualPixelsPlotted = 0;
            var pixels = new byte[targetPixelCountToPlot];
            var screenOffset = 0;
            var runCount = 0;

            while (actualPixelsPlotted < targetPixelCountToPlot)
            {
                // figure out how many pixels are represented by run
                var x = reader.ReadByte();

                if (x < 128)
                {
                    // run of non-consecutive colours
                    var length = x + 1;
                    for (int i = 0; i < length; i++)
                    {
                        var value = reader.ReadByte();
                        pixels[screenOffset] = value;
                        screenOffset += 4;
                        if (screenOffset >= width)
                        {
                            runCount++;
                            screenOffset = runCount;
                        }
                        actualPixelsPlotted++;
                    }
                }
                else
                {
                    // run of consecutive colours
                    var value = reader.ReadByte();
                    var length = 257 - x;
                    for (var i = 0; i < length; i++)
                    {
                        pixels[screenOffset] = value;
                        screenOffset += 4;
                        if (screenOffset >= width)
                        {
                            runCount++;
                            screenOffset = runCount;
                        }
                        actualPixelsPlotted++;
                    }
                }
            }

            /*
            var buffer = new byte[pixels.Length];
            var count = 0;
            for (int y = 0; y < image.Header.ImageHeight; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    var offset = 0;
                    for (int w = 0; w < width / 4; w++)
                    {
                        offset = offset + i*width/4;
                        var value = pixels[offset];
                        buffer[count++] = value;
                        offset += 4;
                    }
                }
            }
            */

            var bitmap = CreateBitmapFromBuffer(image, pixels);
            Bitmaps.Add(bitmap);
        }

        /// <summary>
        /// Uncompressed, transparent
        /// In transparent images, color 255 is not drawn to the screen
        /// </summary>
        /// <param name="image"></param>
        /// <param name="reader"></param>
        private void CreateBitmap21(FruaTlbImage image, BinaryReader reader)
        {
            var bitmap = CreateBitmap(image, reader);
            Bitmaps.Add(bitmap);
        }

        // TODO not working right yet
        private void CreateBitmap23(FruaTlbImage image, BinaryReader reader)
        {
            var buffer = new byte[image.Header.ImageHeight*image.Header.ImageWidth];
            var rowsToPlot = image.Header.ImageHeight;
            var row = 0;
            var pos = 0;

            while (row < rowsToPlot)
            {
                var value = reader.ReadByte();
                if (value == 0)
                {
                    row++;
                    pos = 0;
                }
                else
                {
                    if (value >= 128)
                    {
                        var x = (257 - value)*4;
                        pos += x;
                    }
                    else
                    {
                        for (int i = 0; i < value; i++)
                        {
                            var pixel = reader.ReadByte();
                            buffer[row*image.Header.ImageWidth+pos] = pixel;
                            // pos += 4; ??
                        }
                    }
                }
            }

            var bitmap = CreateBitmapFromBuffer(image, buffer);
            Bitmaps.Add(bitmap);
        }

        private Bitmap CreateBitmap(FruaTlbImage image, BinaryReader reader)
        {
            var pixelsToRead = image.Header.ImageHeight*image.Header.ImageWidth;
            var buffer = new byte[pixelsToRead];
            buffer = reader.ReadBytes(pixelsToRead);
            buffer = DeinterlacePixels(buffer);
            return CreateBitmapFromBuffer(image, buffer);
        }

        private Bitmap CreateBitmapFromBuffer(FruaTlbImage image, byte[] buffer)
        {
            var bitmap = new Bitmap(image.Header.ImageWidth, image.Header.ImageHeight);

            for (int y = 0; y < image.Header.ImageHeight; y++)
            {
                for (int x = 0; x < image.Header.ImageWidth; x++)
                {
                    var pixelIndex = buffer[y*image.Header.ImageWidth + x];
                    bitmap.SetPixel(x, y, _colorTable.Palette[pixelIndex]);
                }
            }

            return bitmap;
        }

        private void ReadHeader(BinaryReader reader)
        {
            _header.FileType = new string(reader.ReadChars(4));
            _header.FileSize = reader.ReadUInt32();
            _header.Entries = reader.ReadUInt16();
            _header.Unused = reader.ReadByte();
            _header.HasTable = reader.ReadByte() == 1;
            _header.ContentType = new string(reader.ReadChars(4));
        }

        private int GetImageCount(BinaryReader reader)
        {
            int imageCount;

            if (_header.HasTable)
            {
                _imageIdTableOffset = reader.ReadUInt32();
                _colorDataOffset = reader.ReadUInt32();
                imageCount = _header.Entries - 2;
            }
            else
            {
                // TODO cpicXXX.tlb has no color data
                _colorDataOffset = reader.ReadUInt32();
                imageCount = _header.Entries - 1;
            }

            return imageCount;
        }

        private void ReadColorTable(BinaryReader reader, uint startingPointForSeek)
        {
            reader.BaseStream.Seek(_colorDataOffset + startingPointForSeek, SeekOrigin.Begin);
            ReadColorTableHeader(reader);
            ReadColorTableData(reader);
            // TODO read color cycling data
        }

        private void ReadColorTableData(BinaryReader reader)
        {
            if (_fullPath.ToLower().Contains("topview.tlb")) return;

            var palOffset = _colorTable.Header.FirstPaletteColorUsed;

            for (int color = 0; color < _colorTable.Header.NumColorsInPalette; color++)
            {
                var red = reader.ReadByte();
                var green = reader.ReadByte();
                var blue = reader.ReadByte();
                _colorTable.Palette[color + palOffset] = Color.FromArgb(red, green, blue);
            }
        }

        private void ReadColorTableHeader(BinaryReader reader)
        {
            _colorTable.Header.ColorCyclingValue = reader.ReadUInt16();
            _colorTable.Header.FirstPaletteColorUsed = reader.ReadUInt16();
            _colorTable.Header.NumColorsInPalette = reader.ReadUInt16();
            _colorTable.Header.NumColorCyclingRanges = reader.ReadByte();
            _colorTable.Header.Magic = reader.ReadByte();
        }

        private static byte[] DeinterlacePixels(byte[] buffer)
        {
            var numberOfPixels = buffer.Length;
            var newBuffer = new byte[numberOfPixels];
            var index = 0;
            var pixelOffset = 0;
            var destPixel = 0;

            foreach (var b in buffer)
            {
                var destOffset = destPixel % numberOfPixels + pixelOffset;
                newBuffer[destOffset] = b;
                index++;
                destPixel = index * 4;
                if (destPixel % numberOfPixels == 0)
                {
                    pixelOffset++;
                }
            }

            return newBuffer;
        }
    }
}