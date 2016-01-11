using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Vault
{
    public class FruaVaultPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var filename = Path.GetFileName(path);
            return filename != null && (filename.ToUpper().StartsWith("VAULT") && filename.EndsWith(".DAT"));
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new FruaVaultFile(args.Filename);
            Viewer = new FruaVaultFileViewer(file);
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