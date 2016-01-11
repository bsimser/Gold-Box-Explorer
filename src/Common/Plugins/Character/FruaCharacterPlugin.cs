using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Character
{
    public class FruaCharacterPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
//            return filename != null && (filename.EndsWith(".CCH") || (filename.StartsWith("MONST") && filename.EndsWith(".DAT")));
            var filename = Path.GetFileName(path.ToUpper());
            if (FileHelper.DetermineGameFrom(path) == FileHelper.GameList.PoolOfRadiance)
            {
                return filename != null && (filename.EndsWith(".SAV") || filename.StartsWith("ITEMS") || filename.EndsWith(".ITM") || filename.EndsWith(".SPC") ||
                    (filename.EndsWith(".DAX") && (filename.Contains("MON") || filename.StartsWith("ITEM"))));
            }
            return false;

        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new FruaCharacterFile(args.Filename);
            Viewer = new FruaCharacterViewer(file, args);
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