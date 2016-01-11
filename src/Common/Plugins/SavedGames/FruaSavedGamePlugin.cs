using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.SavedGames
{
    public class FruaSavedGamePlugin : IPlugin
    {
        public bool IsSatisifedBy(string path)
        {
            var filename = Path.GetFileName(path);
            return filename != null && (filename.ToUpper().StartsWith("SAVGAM") && filename.EndsWith(".CSV"));
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new FruaSavedGameFile(args);
            Viewer = new FruaSavedGameFileViewer(file, args);
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