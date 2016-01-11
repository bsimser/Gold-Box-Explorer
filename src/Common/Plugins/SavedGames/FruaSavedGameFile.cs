using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.SavedGames
{
    public class FruaSavedGameFile : GoldBoxFile
    {
        private readonly string _path;

        public FruaSavedGameFile(PluginParameter parameters)
        {
            _path = parameters.Filename;
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_path);
        }

        /// <summary>
        /// TODO not complete yet (see SAVGAM.TXT)
        /// </summary>
        /// <returns></returns>
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
                    reader.ReadBytes(5); // not defined
                    game.FacingDirection = reader.ReadByte();
                    game.GameSpeed = reader.ReadByte();
                    game.CurrentModule = reader.ReadByte();
                    reader.ReadBytes(2); // not defined
                    game.InitiatedFlag = reader.ReadByte();
                    game.PreviousColumn = reader.ReadByte();
                    game.PreviousRow = reader.ReadByte();
                    game.SearchModeOn = reader.ReadByte();
                    game.OverlandFlag = reader.ReadByte();
                    reader.ReadBytes(2); // not defined
                    game.PartyNeverDiesFlag = reader.ReadByte();
                    game.NoTreasureFlag = reader.ReadByte();
                    game.NoXpForCombatFlag = reader.ReadByte();
                    game.PartySize = reader.ReadByte();
                    reader.ReadByte(); // not defined
                    game.DungeonFlag = reader.ReadByte();
                    reader.ReadByte(); // not defined
                    game.SecondOverlandFlag = reader.ReadByte();
                    game.PartyColumnInOverland = reader.ReadByte();
                    game.PartyRowInOverland = reader.ReadByte();
                    game.CombatLevel = reader.ReadByte();
                    reader.ReadBytes(4); // not defined
                    game.NoRestZoneFlag = reader.ReadByte();
                    reader.ReadBytes(3); // not defined
                    game.AllowedTrainFlag = reader.ReadByte();
                    reader.ReadBytes(6); // not defined
                    game.OpponentsDistanceAtStart = reader.ReadByte();
                    reader.ReadBytes(2); // not defined
                    game.FriendlyOpponents = reader.ReadByte();
                    reader.ReadByte(); // not defined
                    game.UnderwaterFlag = reader.ReadByte();
                    reader.ReadBytes(5); // not defined
                    game.PartyColumnInDungeon = reader.ReadByte();
                    game.PartyRowInDungeon = reader.ReadByte();
                    game.Keys = reader.ReadBytes(8);
                    game.Items = reader.ReadBytes(12);
                    game.Quests = reader.ReadBytes(44);
                }
            }

            return game;
        }
    }
}