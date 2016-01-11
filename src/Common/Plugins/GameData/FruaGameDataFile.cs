using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.GameData
{
    public class FruaGameDataFile : GoldBoxFile
    {
        private readonly string _fullPath;

        public FruaGameDataFile(string fullPath)
        {
            _fullPath = fullPath;
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_fullPath);
        }

        public FruaGameData GetGameData()
        {
            var data = new FruaGameData();

            using (var stream = new FileStream(_fullPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    data.Name = new string(reader.ReadChars(32)).RemoveNulls();
                    data.StartingXp = reader.ReadInt32();
                    data.StartingPp = reader.ReadInt32();
                    data.StartingGems = reader.ReadInt32();
                    data.StartingJewelry = reader.ReadInt32();
                    data.StartingModule = reader.ReadByte();
                    data.StartingTown = reader.ReadByte();
                    data.StartingEquipment = reader.ReadByte();

                    reader.ReadByte(); // unused or unknown byte

                    for (int i = 0; i < 8; i++)
                    {
                        data.Keys.Add(new string(reader.ReadChars(16)).RemoveNulls());
                    }

                    for (int i = 0; i < 12; i++)
                    {
                        data.Items.Add(new string(reader.ReadChars(16)).RemoveNulls());
                    }

                    data.Password = new string(reader.ReadChars(15)).RemoveNulls();
                }
            }

            return data;
        }
    }
}