using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.DaxEcl
{
    public class DaxEclPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var fileName = Path.GetFileName(path);
            if (fileName == null) return false;
            fileName = fileName.ToUpper();
            return (fileName.StartsWith("ECL") && fileName.EndsWith(".DAX"));
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new DaxEclFile(args.Filename);
            Viewer = new EclFileViewer(file);
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