using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.GeoDax
{
    public class GeoDaxPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var fileName = Path.GetFileName(path);
            return fileName != null && (fileName.ToUpper().StartsWith("GEO") && fileName.EndsWith(".DAX"));
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new GeoDaxFile(args.Filename);
            Viewer = new GeoDaxFileViewer(file, args.ContainerWidth, args.Zoom);
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