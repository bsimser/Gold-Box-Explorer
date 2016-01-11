using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace GoldBoxExplorer.Lib.Plugins.Glb
{
    public class FruaGlbPlugin : IPlugin
    {
        public IList<int> GetBitmapIds() { return null; }
        public bool IsSatisifedBy(string path)
        {
            var filename = Path.GetFileName(path);

            if (filename == null) return false;

            filename = filename.ToUpper();

            if (!filename.EndsWith(".GLB")) return false;

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var headerText = new string(reader.ReadChars(4));
                    var fileSize = reader.ReadUInt32();
                    return headerText == "HLIB" && fileSize == new FileInfo(path).Length;
                }
            }
        }

        public IPlugin CreateUsing(PluginParameter args)
        {
            var file = new FruaGlbFile(args.Filename);
            Viewer = new FruaGlbFileViewer(file, args);
            return this;
        }

        public IGoldBoxViewer Viewer { get; set; }

        public bool IsImageFile() { return true; }
        public IEnumerable<Bitmap> GetBitmaps()
        {
            return null;
        }


    }
}