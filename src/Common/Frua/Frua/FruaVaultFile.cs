using System;
using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaVaultFile : GoldBoxFile
    {
        private int _pps;
        private int _gems;
        private int _jewels;
        private short _itemCount;

        public FruaVaultFile(string fullPath)
        {
            load(fullPath);
        }

        private void load(string fullPath)
        {
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    _pps = reader.ReadInt32();
                    _gems = reader.ReadInt32();
                    _jewels = reader.ReadInt32();
                    _itemCount = reader.ReadInt16();
                }
            }
        }

        public override IList<byte> GetBytes()
        {
            throw new NotImplementedException();
        }

        public override string GetStatusMessage()
        {
            return "FRUA Vault File";
        }

        public override string ToString()
        {
            return string.Format("{0} PPS, {1} Gems, {2} Jewels, {3} Items", _pps, _gems, _jewels, _itemCount);
        }
    }
}