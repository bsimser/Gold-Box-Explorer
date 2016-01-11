using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoldBoxExplorer.Lib.Plugins.Text
{
    public class TextFile : GoldBoxFile
    {
        private readonly IList<byte> _bytes;

        public TextFile(string fullPath)
        {
            var bytes = File.ReadAllText(fullPath);
            _bytes = new List<byte>(bytes.Length);
            foreach (var item in bytes.Select(b => (byte) b))
            {
                _bytes.Add(item);
            }
        }

        public override IList<byte> GetBytes()
        {
            return _bytes;
        }
    }
}