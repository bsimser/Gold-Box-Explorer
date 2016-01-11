using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class DaxImageFile : DaxFile
    {
        private readonly List<Bitmap> _bitmaps = new List<Bitmap>();
        private readonly List<int> _bitmapIds = new List<int>();
        public DaxImageFile(string file) : base(file)
        {
            ProcessBlocks();
        }

        protected override sealed void ProcessBlocks()
        {
            foreach (var renderBlock in Blocks.Select(
                fileBlock => new FileBlockParameters

                                 {
                                     Data = fileBlock.Data,
                                     Name = fileBlock.File,
                                     Id = fileBlock.Id,
                                 }).Select(parameters => new RenderBlockFactory().CreateUsing(parameters)))
            {

  
                foreach (var bitmap in renderBlock.GetBitmaps())
                {
                    _bitmaps.Add(bitmap);
                    _bitmapIds.Add(renderBlock.getBlockId());
                }
            }
        }


   

        public IList<Bitmap> GetBitmaps()
        {
            return _bitmaps.AsReadOnly();
        }
        public IList<int> GetBitmapIds()
        {
            return _bitmapIds.AsReadOnly();
        }

    }
}