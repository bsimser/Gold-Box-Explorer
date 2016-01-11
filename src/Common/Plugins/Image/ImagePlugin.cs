using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Image
{
    public class ImagePlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var fileExt = Path.GetExtension(path);
            var extensions = new[] { "PNG", "PCX", "JPG", "JPEG", "GIF", "TIF", "BMP", "TGA", };
            if (fileExt != null && fileExt != "") fileExt = fileExt.Substring(1).ToUpper();
            return extensions.Any(extension => fileExt != null && fileExt.Equals(extension));
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            Viewer = new ImageFileViewer(args.Filename, args.ContainerWidth);
            return this;
        }

        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return true; }
        public IEnumerable<Bitmap> GetBitmaps()
        {
            return null;
        }
        public IList<int> GetBitmapIds() { return null; }

    }
}