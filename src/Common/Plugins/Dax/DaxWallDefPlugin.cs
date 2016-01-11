using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class DaxWallDefPlugin : IPlugin
    {
        DaxWallDefFile _file;

        public bool IsSatisifedBy(string path)
        {
            var fileName = Path.GetFileName(path);
            if (fileName == null) return false;
            fileName = fileName.ToUpper();
            return fileName.ToUpper().StartsWith("WALL") && fileName.ToUpper().EndsWith(".DAX");
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var filename = Path.GetFileName(args.Filename);
            _file = new DaxWallDefFile(args.Filename);
            Viewer = new DaxWallDefViewer(_file.wallsets, _file._blockIds, args.Zoom, args.ContainerWidth);


            return this;
        }

        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return true; }

        public IEnumerable<Bitmap> GetBitmaps() { return _file.GetBitmaps(); }
        public IList<int> GetBitmapIds() { return _file._bitmapIds; }
    }
}