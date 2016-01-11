using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.GameData
{
    public class FruaGameDataPlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var fileName = Path.GetFileName(path);
            return fileName != null && fileName.ToUpper().Equals("GAME001.DAT");
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new FruaGameDataFile(args.Filename);
            Viewer = new FruaGameDataFileViewer(file.GetGameData());
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