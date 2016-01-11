using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GoldBoxExplorer.Lib.Plugins.Items;

namespace GoldBoxExplorer.Lib.Plugins.Vault
{
    public class FruaVaultFile : GoldBoxFile
    {
        private uint _pps;
        private uint _gems;
        private uint _jewels;
        private short _itemsInVault;
        private readonly IList<FruaItem> _items = new List<FruaItem>();
 
        public FruaVaultFile(string fullPath)
        {
            Load(fullPath);
        }

        private void Load(string fullPath)
        {
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    _pps = reader.ReadUInt32();
                    _gems = reader.ReadUInt32();
                    _jewels = reader.ReadUInt32();
                    _itemsInVault = reader.ReadInt16();
                    var scratch = reader.ReadUInt16();

                    for (int i = 0; i < 50; i++)
                    {
                        var pointer = reader.ReadByte();
                        var first = reader.ReadByte();
                        var second = reader.ReadByte();
                        var third = reader.ReadByte();
                        var item = new FruaItem(first, second, third);
                        // TODO not reading full item here
                        var tmp = reader.ReadBytes(14);
                        if (pointer != 255 && _itemsInVault > i)
                        {
                            _items.Add(item);
                        }
                    }
                }
            }
        }

        public override IList<byte> GetBytes()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Platinum Pieces: {0}\r\n", _pps);
            sb.AppendFormat("Gems: {0}\r\n", _gems);
            sb.AppendFormat("Jewels: {0}\r\n", _jewels);
            sb.AppendFormat("Items: {0}\r\n", _itemsInVault == -1 ? "None" : _itemsInVault.ToString());
            foreach (var item in _items)
            {
                sb.AppendLine(item.Name);
            }
            return sb.ToString();
        }
    }
}