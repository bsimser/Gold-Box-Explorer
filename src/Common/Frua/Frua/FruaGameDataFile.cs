using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaGameDataFile : GoldBoxFile
    {
        private readonly string _fullPath;
        private readonly List<string> _gamedata = new List<string>();

        public FruaGameDataFile(string fullPath)
        {
            _fullPath = fullPath;
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_fullPath);
        }

        public override string GetStatusMessage()
        {
            return "FRUA Game Data File";
        }

        public IEnumerable<string> GetGameData()
        {
            using(var fs = new FileStream(_fullPath, FileMode.Open, FileAccess.Read))
            {
                using(var reader = new BinaryReader(fs))
                {
                    string[] equipment = { "None", "Poor", "Modest", "Average", "Prosperous", "+1", "+2", "+3", "+4"};
                    var bytes = reader.ReadBytes(32);
                    var text = bytes.Aggregate(string.Empty, (current, b) => current + (char)b);
                    _gamedata.Add(string.Format("Title: {0}", text));
                    _gamedata.Add(string.Format("Staring XP: {0}", reader.ReadInt32()));
                    _gamedata.Add(string.Format("Staring PP: {0}", reader.ReadInt32()));
                    _gamedata.Add(string.Format("Staring Gems: {0}", reader.ReadInt32()));
                    _gamedata.Add(string.Format("Staring Jewelry: {0}", reader.ReadInt32()));
                    _gamedata.Add(string.Format("Staring Module: {0}", reader.ReadByte()));
                    _gamedata.Add(string.Format("Staring Town: {0}", reader.ReadByte()));
                    _gamedata.Add(string.Format("Equipment: {0}", equipment[reader.ReadByte()]));
                    reader.ReadByte(); // unused or unknown byte

                    for (var i = 0; i < 8; i++)
                    {
                        bytes = reader.ReadBytes(16);
                        text = bytes.Aggregate(string.Empty, (current, b) => current + (char)b);
                        _gamedata.Add(string.Format("Key {0}: {1}", i+1, text));
                    }

                    for (var i = 0; i < 12; i++)
                    {
                        bytes = reader.ReadBytes(16);
                        text = bytes.Aggregate(string.Empty, (current, b) => current + (char)b);
                        _gamedata.Add(string.Format("Item {0}: {1}", i + 1, text));
                    }

                    bytes = reader.ReadBytes(15);
                    text = bytes.Aggregate(string.Empty, (current, b) => current + (char)b);
                    _gamedata.Add(string.Format("Password: {0}", text));
                }
            }

            return _gamedata;
        }
    }
}
