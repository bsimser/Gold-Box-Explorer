using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaSavedGameFile : GoldBoxFile
    {
        private readonly string _path;

        public FruaSavedGameFile(GoldBoxFileParameters parameters)
        {
            _path = parameters.FullPath;
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_path);
        }

        public override string GetStatusMessage()
        {
            return "Ready";
        }

        public FruaSavedGame Load()
        {
            var game = new FruaSavedGame();

            using (var file = new FileStream(_path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    reader.ReadByte(); // not defined
                    game.NoMagicFlag = reader.ReadByte();
                    reader.ReadBytes(3);  // not defined
                    game.Rounds = reader.ReadByte();
                    game.Turns = reader.ReadByte();
                    game.Hours = reader.ReadByte();
                    game.Days = reader.ReadByte();
                    game.Months = reader.ReadByte();
                    game.Years = reader.ReadByte();
                }
            }

            return game;
        }
    }
}