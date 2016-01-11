using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Items
{
    public class FruaItemPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var filename = Path.GetFileName(path);
            return filename != null && (filename.ToUpper().Equals("ITEM.DAT") || filename.ToUpper().Equals("ITEMS.DAT"));
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new FruaItemFile(args.Filename);
            Viewer = new FruaItemViewer(file);
            return this;
        }

        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return true; }
        public IList<int> GetBitmapIds() { return null; }
        public IEnumerable<Bitmap> GetBitmaps()
        {
            return null;
        }

    }
}