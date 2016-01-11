using System;


using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{

    public class DaxWallDefFile : DaxFile, IRenderBlock
    {
        public List<Bitmap> wallBitmaps = new List<Bitmap>();
        public int _blockId;

        public int getBlockId() { return _blockId; }
        public void setBlockId(int id) { _blockId = id; }

        public string Name { get; private set; }

        private readonly List<string> _strings = new List<string>();


        public List<List<Bitmap>> wallsets = new List<List<Bitmap>>();
        public List<int> _blockIds = new List<int>();
        public List<int> _bitmapIds = new List<int>();

                public DaxWallDefFile(string path, int blockId = -1)
            : base(path)
        {
            _blockId = blockId;
            ProcessBlocks();
        }


        protected override sealed void ProcessBlocks()
        {
            foreach (var block in Blocks)
            {
                if (_blockId == -1 || _blockId == block.Id) 
                    processWallSet(block);
            }
        }


        private void processWallSet(DaxFileBlock daxFileBlock)
        {
            byte[] data = daxFileBlock.Data;
            _blockIds.Add(daxFileBlock.Id);
            wallsets.Add(assembleWallSet(daxFileBlock, FileName, daxFileBlock.Id));

        }

        public IEnumerable<Bitmap> GetBitmaps()
        {
            // flatten the list of lists into a list
            List<Bitmap> bml = new List<Bitmap>();
            foreach (var wall in wallsets)
            {
                foreach (var bm in wall)
                {
                    bml.Add(bm);
                }
            }
            return bml.AsReadOnly();
        }


    

        public void Draw8x8Tiles(Bitmap wallImage, byte[,] wall, List<Bitmap> bitmaps)
        {
            var rows = wall.GetLength(0);
            var cols = wall.GetLength(1);
            var x = 0;
            var y = 0;
            var i = 0;
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var tileIndex = wall[r, c];

                    if (tileIndex >= bitmaps.Count) continue;


                    var currentImage = bitmaps[tileIndex];
                    x = (int)(c * currentImage.Width);
                    y = (int)(r * currentImage.Height);

                    var surface = Graphics.FromImage(wallImage);

                    surface.DrawImage(currentImage, x, y, currentImage.Width, currentImage.Height);
                    i++;
                }
            }
        }

        private static void fillBitmap(Bitmap bitmap, Color c)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, c);
                }
            }
        }
        static Bitmap bitmapZero()
        {
            var bm = new Bitmap(8, 8);
            fillBitmap(bm, Color.FromArgb(255, 82, 255));
            return bm;
        }

        List<Bitmap> assembleWallSet(DaxFileBlock daxFileBlock, string fileName, int blockId)
        {
            string tileFileName = fileName.ToLower().Replace("walldef", "8x8d");
            var dir = Path.GetDirectoryName(fileName);
 
            // first the 8x8 tiles which are universal to all wallsets
            List<Bitmap> tileBitmaps = load8x8Bitmaps(daxFileBlock, fileName, tileFileName, blockId); // return bitmaps
            List<byte[,]>  wallSetData = loadWallDefs(daxFileBlock); // return walldef data
            for (int i = 0; i < wallSetData.Count; i++)
                _bitmapIds.Add(daxFileBlock.Id);

            return aggregateWalls(wallSetData, tileBitmaps); // return bitmap list


        }

        List<Bitmap> load8x8Bitmaps(DaxFileBlock daxFileBlock, string fileName, string tileFileName, int blockId)
        {
            var bitmapcount = getBitmapCount(blockId, tileFileName);
            List<Bitmap> bitmaps8x8 = new List<Bitmap>();
            bitmaps8x8.Add(bitmapZero());

            if (bitmapcount >= 255)     // the bitmaps are in a single large file, load them all at once
            {
                if (bitmapcount == 256) bitmaps8x8.RemoveAt(0); //bitmapIdxOffset = 1;
                // just load the 8x8 with the same id as the wall def
                loadBitmaps(blockId, tileFileName, bitmaps8x8);
            }
            else
                // the bitmaps are in small chunks in a universal file, and one or more wallset specific files
            {
                loadBitmaps(getUniversalTileBlockId(), getUniversalTileFileName(fileName), bitmaps8x8);

                // next load the ones which are specific to this wallset
                // how many wallsets are there in this walldef daxblock? We will need to load one 8x8 daxblock for each wallset in the walldef
                int wallsetCount = daxFileBlock.Data.Length / 780;

                // if there's just one wallset, the 8x8 daxblock needed has the same id as the walldef daxblock
                if (wallsetCount >= 1)
                {
                    loadBitmaps(blockId, tileFileName, bitmaps8x8);
                }

                // if there's 2 or 3, the Id's of 8x8 daxblocks are given by the current id*10 +1, +2 or +3
                // and if the current daxblock id is 0, consider to be 10 before multiplying by 10
                if (bitmaps8x8.Count < 255 && wallsetCount > 1)
                {
                    var baseBlockId = 10 * blockId;
                    if (blockId == 0) { baseBlockId = 10 * 10; }
                    var block1 = baseBlockId + 1;
                    var block2 = baseBlockId + 2;
                    var block3 = baseBlockId + 3;

                    loadBitmaps(block1, tileFileName, bitmaps8x8);
                    loadBitmaps(block2, tileFileName, bitmaps8x8);
                    if (wallsetCount == 3) loadBitmaps(block3, tileFileName, bitmaps8x8);
                }
            }
            bitmaps8x8[0] = bitmapZero();
            return bitmaps8x8;
        }

        List<Bitmap> aggregateWalls(List<byte[,]> wallSetData, List<Bitmap> bitmaps8x8)
        {
            List<Bitmap> wallSetBitmaps = new List<Bitmap>();
            // loop through walls
            foreach (var wallData in wallSetData)
            {
                
                Bitmap wallbm = new Bitmap(wallData.GetLength(1) * 8, wallData.GetLength(0) * 8);
                // create bitmap
                Draw8x8Tiles(wallbm, wallData, bitmaps8x8);
                // call 8x8draw on them
                // add to wallBitmaps
                wallSetBitmaps.Add(wallbm);
            }
            return wallSetBitmaps;
        }
        string getUniversalTileFileName(string fileName)
        {
            // the universal 8x8's always seem to be in the first 8x8d file

            string fn = "";
            fn = Path.GetDirectoryName(fileName) + "\\8x8d1.dax";
            if (getBitmapCount(203, fn) == 0)
            {
                fn = Path.GetDirectoryName(fileName) + "\\8x8d.dax";

            }

            return fn;

        }
        int getUniversalTileBlockId()
        {
            // assume the universal 8x8 tiles are found in block 203, for those games that have a universal block
            return 203;

        }
        List<byte[,]> loadWallDefs(DaxFileBlock daxFileBlock)
        {
            // rows, columns and offsets of the subarrays holding walldefs for various viewing angles and distances
            // taken from Simeon Pilgrim's curse of the azure bonds code in ovr031.cs
            byte[] idxOffset = { 0, 2, 6, 10, 22, 38, 54, 110, 132, 154, 1 };   // seg600:0ADA
            int[] colCount = { 1, 1, 1, 3, 2, 2, 7, 2, 2, 1 };                 // seg600:0AE4
            int[] rowCount = { 2, 4, 4, 4, 8, 8, 8, 11, 11, 2 };               // seg600:0AEE

            const int wallSliceSize = 156;
            byte[] data = daxFileBlock.Data;
            List<byte[,]> wallSetData = new List<byte[,]>();
            var wallCount = daxFileBlock.Data.Length / wallSliceSize;

            for (int wallNumber = 0; wallNumber < wallCount; wallNumber++)
            {
                for (int wallView = 0; wallView < 10; wallView++)
                {
                    var idx = idxOffset[wallView];
                    var rows = rowCount[wallView];
                    var cols = colCount[wallView];

                    var i = idx;
                    byte[,] wall = new byte[rows, cols];
                    for (var r = 0; r < rows; r++)
                    {
                        for (var c = 0; c < cols; c++)
                        {
                            wall[r, c] = data[(wallSliceSize * wallNumber) + i];
                            i++;
                        }
                    }
                    wallSetData.Add(wall);
                }
            }
            return wallSetData;
        }

        private int getBitmapCount(int daxBlockId, string fileName)
        {
            // load 8x8 bitmaps from daxBlockId in fileName into the list bitmaps8x8 to simulate the order the bitmaps would be in memory in the game

            if (File.Exists(fileName) == false) return 0;
            var file = new DaxImageFile(string.Format(fileName));
            foreach (DaxFileBlock block in file.Blocks)
            {

                if (block.Id == daxBlockId)
                {
                    var parameters = new FileBlockParameters
                    {
                        Data = block.Data,
                        Name = block.File,
                        Id = block.Id,
                    };

                    var render = new RenderBlockFactory().CreateUsing(parameters);

                    var x = render.GetBitmaps().Count();
                    return x;

                }
            }

            return 0;
        }

        private void loadBitmaps(int daxBlockId, string fileName, List<Bitmap> bitmaps8x8)
        {
            // load 8x8 bitmaps from daxBlockId in fileName into the list bitmaps8x8 to simulate the order the bitmaps would be in memory in the game
            var file = new DaxImageFile(string.Format(fileName));
            foreach (DaxFileBlock block in file.Blocks)
            {

                if (block.Id == daxBlockId)
                {
                    var parameters = new FileBlockParameters
                    {
                        Data = block.Data,
                        Name = block.File,
                        Id = block.Id,
                    };
                    var render = new RenderBlockFactory().CreateUsing(parameters);
                    foreach (var bitmap in render.GetBitmaps())
                    {
                        bitmaps8x8.Add(bitmap);
                    }
                }
            }
        }
    }
}

