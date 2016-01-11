using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Geo
{
    public class FruaGeoPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var filename = Path.GetFileName(path);
            return filename != null && (filename.ToUpper().StartsWith("GEO") && filename.EndsWith(".DAT"));
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new FruaGeoFile(args.Filename);
            Viewer = new FruaGeoFileViewer(file);
            return this;
        }

        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return false; }
        public IEnumerable<Bitmap> GetBitmaps()
        {
            return null;
        }
        public IList<int> GetBitmapIds() { return null; }
    }
}