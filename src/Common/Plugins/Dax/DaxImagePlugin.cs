using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class DaxImagePlugin : IPlugin
    {
        private DaxImageFile _file;
        public bool IsSatisifedBy(string path)
        {
            var nonImageDaxFiles = new [] { "WALLDEF", "MON", "ITEM", "MUSIC", "SOUND", "ZOUNDS", "LUCK" };
            var fileName = Path.GetFileName(path);
            if (fileName == null) return false;
            fileName = fileName.ToUpper();
            return !nonImageDaxFiles.Any(prefix => fileName.StartsWith(prefix)) && fileName.ToUpper().EndsWith(".DAX");
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var filename = Path.GetFileName(args.Filename);
            var display35ImagesPerRow = filename != null && (filename.ToUpper().StartsWith("8X8D"));
            var displayBorder = filename != null && filename.ToUpper().StartsWith("SPRIT");

            _file = new DaxImageFile(args.Filename);
            Viewer = new DaxImageViewer(_file.GetBitmaps(), args.Zoom, args.ContainerWidth, display35ImagesPerRow, displayBorder, _file.GetBitmapIds());
            return this;
        }

        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return true; }
        public IEnumerable<Bitmap> GetBitmaps()
        {
            return _file.GetBitmaps();
        }
        public IList<int> GetBitmapIds() { return _file.GetBitmapIds(); }

    }
}