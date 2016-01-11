using System.Collections.Generic;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins

{

    public interface IPlugin
    {
        bool IsSatisifedBy(string path);
        IPlugin CreateUsing(PluginParameter args);
        IGoldBoxViewer Viewer { get; set; }
        IEnumerable<Bitmap> GetBitmaps();
        IList<int> GetBitmapIds();
        bool IsImageFile();
        
    }
}