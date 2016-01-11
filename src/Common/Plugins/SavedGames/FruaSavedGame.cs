using System.Text;

namespace GoldBoxExplorer.Lib.Plugins.SavedGames
{
    public class FruaSavedGame
    {
        public byte NoMagicFlag { get; set; }
        public int Rounds { get; set; }
        public int Turns { get; set; }
        public int Hours { get; set; }
        public int Days { get; set; }
        public int Months { get; set; }
        public int Years { get; set; }
        public byte FacingDirection { get; set; }
        public byte GameSpeed { get; set; }
        public byte CurrentModule { get; set; }
        public byte InitiatedFlag { get; set; }
        public byte PreviousColumn { get; set; }
        public byte PreviousRow { get; set; }
        public byte SearchModeOn { get; set; }
        public byte OverlandFlag { get; set; }
        public byte PartyNeverDiesFlag { get; set; }
        public byte NoTreasureFlag { get; set; }
        public byte NoXpForCombatFlag { get; set; }
        public byte PartySize { get; set; }
        public byte DungeonFlag { get; set; }
        public byte SecondOverlandFlag { get; set; }
        public byte PartyColumnInOverland { get; set; }
        public byte PartyRowInOverland { get; set; }
        public byte CombatLevel { get; set; }
        public byte NoRestZoneFlag { get; set; }
        public byte AllowedTrainFlag { get; set; }
        public byte OpponentsDistanceAtStart { get; set; }
        public byte FriendlyOpponents { get; set; }
        public byte UnderwaterFlag { get; set; }
        public byte PartyColumnInDungeon { get; set; }
        public byte PartyRowInDungeon { get; set; }
        public byte[] Keys { get; set; }
        public byte[] Items { get; set; }
        public byte[] Quests { get; set; }

        public override string ToString()
        {
            var directions = new[]
                {
                    "North",
                    "Northeast",
                    "East",
                    "Southeast",
                    "South",
                    "Southwest",
                    "West",
                    "Northwest",
                };

            var sb = new StringBuilder();

            sb.AppendFormat("Rounds: {0}\r\n", Rounds);
            sb.AppendFormat("Turns: {0}\r\n", Turns);
            sb.AppendFormat("Hours: {0}\r\n", Hours);
            sb.AppendFormat("Days: {0}\r\n", Days);
            sb.AppendFormat("Months: {0}\r\n", Months);
            sb.AppendFormat("Years: {0}\r\n", Years);
            sb.AppendFormat("Facing Direction: {0}\r\n", directions[FacingDirection]);
            sb.AppendFormat("Game Speed: {0}\r\n", GameSpeed);
            sb.AppendFormat("Current Module: {0}\r\n", CurrentModule);
            sb.AppendFormat("Initiated Flag: {0}\r\n", InitiatedFlag);
            sb.AppendFormat("Previous Column: {0}\r\n", PreviousColumn);
            sb.AppendFormat("Previous Row: {0}\r\n", PreviousRow);
            sb.AppendFormat("Search Mode On?: {0}\r\n", SearchModeOn);
            sb.AppendFormat("Overland Flag: {0}\r\n", OverlandFlag);
            sb.AppendFormat("Party Never Dies Flag: {0}\r\n", PartyNeverDiesFlag);
            sb.AppendFormat("No Treasure Flag: {0}\r\n", NoTreasureFlag);
            sb.AppendFormat("No XP For Combat Flag: {0}\r\n", NoXpForCombatFlag);
            sb.AppendFormat("Party Size: {0}\r\n", PartySize);
            sb.AppendFormat("Dungeon Flag: {0}\r\n", DungeonFlag);
            sb.AppendFormat("Second Overland Flag: {0}\r\n", SecondOverlandFlag);
            sb.AppendFormat("Party Column (Overland): {0}\r\n", PartyColumnInOverland);
            sb.AppendFormat("Party Row (Overland): {0}\r\n", PartyRowInOverland);
            sb.AppendFormat("Combat Level: {0}\r\n", CombatLevel);
            sb.AppendFormat("No Rest Zone Flag: {0}\r\n", NoRestZoneFlag);
            sb.AppendFormat("Allowed Train Flag: {0}\r\n", AllowedTrainFlag);
            sb.AppendFormat("Opponents Distance at Start: {0}\r\n", OpponentsDistanceAtStart);
            sb.AppendFormat("Opponents Friendly?: {0}\r\n", FriendlyOpponents);
            sb.AppendFormat("Underwater Flag: {0}\r\n", UnderwaterFlag);
            sb.AppendFormat("Party Column (Dungeon): {0}\r\n", PartyColumnInDungeon);
            sb.AppendFormat("Party Row (Dungeon): {0}\r\n", PartyRowInDungeon);

            return sb.ToString();
        }
    }
}