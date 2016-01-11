using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Hex
{
    public class BinaryFile : GoldBoxFile
    {
        private readonly string _filename;
        private readonly byte[] _data;

        public BinaryFile(string filename)
        {
            _filename = Path.GetFileName(filename).ToUpper();
            _data = File.ReadAllBytes(filename);
        }

        public override IList<byte> GetBytes()
        {
            return _data;
        }
    }
}