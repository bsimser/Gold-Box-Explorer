using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    public class HexPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            return false;
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new BinaryFile(args.Filename);
            Viewer = new HexFileViewer(file, args);
            return this;
        }
        
        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return false; }
        public IList<int> GetBitmapIds() { return null; }
        public IEnumerable<Bitmap> GetBitmaps()
        {
            return null;
        }
    }
}