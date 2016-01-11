using System.Collections.Generic;
using System.Text;

namespace GoldBoxExplorer.Lib.Plugins.GameData
{
    public class FruaGameData
    {
        private readonly string[] _equipmentNames = {"None", "Poor", "Modest", "Average", "Prosperous", "+1", "+2", "+3", "+4"};
        private readonly IList<string> _moduleNames; 

        public FruaGameData()
        {
            Keys = new List<string>();
            Items = new List<string>();
            _moduleNames = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                _moduleNames.Add(string.Format("Overland {0}", i));
            }
            for (int i = 0; i < 36; i++)
            {
                _moduleNames.Add(string.Format("Dungeon {0}", i));
            }
        }

        public string Name { get; set; }

        public int StartingXp { get; set; }

        public int StartingPp { get; set; }

        public int StartingGems { get; set; }

        public int StartingJewelry { get; set; }

        public byte StartingModule { get; set; }

        public byte StartingTown { get; set; }

        public byte StartingEquipment { get; set; }

        public IList<string> Keys { get; private set; }

        public IList<string> Items { get; private set; }

        public string Password { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Adventure Design Name");
            sb.AppendLine(Name);
            sb.AppendLine();

            sb.AppendLine("The Adventure Begins in");
            sb.AppendFormat("{0} at entry point {1}\r\n", _moduleNames[StartingModule], StartingTown);
            sb.AppendLine();

            sb.AppendLine("Each Party Member Starts With");
            sb.AppendFormat("{0} Experience Points\r\n", StartingXp);
            sb.AppendFormat("Starting Equipment {0}\r\n", _equipmentNames[StartingEquipment]);
            sb.AppendFormat("Platinum: {0}\r\n", StartingPp);
            sb.AppendFormat("Gems: {0}\r\n", StartingGems);
            sb.AppendFormat("Jewelry: {0}\r\n", StartingJewelry);
            sb.AppendLine();

            sb.AppendLine("Protection");
            sb.AppendFormat("Password: {0}\r\n", Password);
            sb.AppendLine();

            sb.AppendLine("Keys in Special Inventory");
            for (int i = 0; i < Keys.Count; i++)
            {
                var key = Keys[i];
                sb.AppendFormat("Key {0}: {1}\r\n", i+1, key);
            }
            sb.AppendLine();

            sb.AppendLine("Other Special Inventory Items");
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                sb.AppendFormat("Item {0}: {1}\r\n", i+1, item);
            }
            sb.AppendLine();

            return sb.ToString();
        }
    }
}