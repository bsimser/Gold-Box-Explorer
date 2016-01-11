using System.Text;
using System.Windows.Forms;
using DaxFileLibrary.Frua;
using GoldBoxExplorer.Common.Viewers;

namespace GoldBoxExplorer.Common.Frua
{
    public class FruaGeoFileViewer : IGoldBoxViewer
    {
        private StringBuilder _moduleSummary;

        public FruaGeoFileViewer(FruaGeoFile file)
        {
            BuildModuleSummary(file);
        }

        private void BuildModuleSummary(FruaGeoFile file)
        {
            _moduleSummary = new StringBuilder();

            if(file.IsOverlandModule)
            {
                buildOverlandSummary(file);
            }
            else
            {
                buildDungeonSummary(file);
            }
        }

        private void buildDungeonSummary(FruaGeoFile file)
        {
            _moduleSummary.AppendLine("Dungeon Global Information");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Adventure Module Name:");
            _moduleSummary.AppendLine(file.ModuleName);
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine(
                string.Format("Height: {0} Width: {1}",
                              file.ModuleHeight, file.ModuleWidth));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Allow Area View? {0}",
                              file.AllowAreaView ? "Yes" : "No"));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine(
                string.Format("Summoned Monster: {0}",
                file.SummonedMonster));
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("Zone Name {0}: {1}", i + 1, file.GetZoneName(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Wall Art Sets");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 3; i++)
            {
                _moduleSummary.AppendLine(string.Format("Wall Art Slot {0}: {1}", i + 1, file.WallSlots[i]));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Floor and Ceiling Art");
            _moduleSummary.AppendLine();
            
            for (var i = 0; i < 4; i++)
            {
                _moduleSummary.AppendLine(string.Format("Floor Art Slot {0}: {1}", i + 1, file.GetBackdropSlot(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Combat Art");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(string.Format("Indoor Combat Art: {0}", file.IndoorCombatArt));
            _moduleSummary.AppendLine(string.Format("Outdoor Combat Art: {0}", file.OutdoorCombatArt));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Step Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("{0}) A {1} Event",
                                  i + 1, file.GetStepEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Happens on Step {0} in Zones:",
                                  file.GetStepEventSteps(i)));
                _moduleSummary.AppendLine();
            }

            _moduleSummary.AppendLine("More Module Options");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Undead are more difficult to turn by {0}",
                file.UndeadTurnDifficulty));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Camp Picture: {0}", file.CampPicture));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Treasure Picture: {0}", file.TreasurePicture));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Rest Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("In Zone {0} a {1} Event",
                    i + 1, file.GetRestEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Has a {0} % Chance of Occurring",
                    file.GetRestChance(i)));
                _moduleSummary.AppendLine(
                    string.Format("Every {0} Minutes.",
                    file.GetRestEventMinutes(i)));
                _moduleSummary.AppendLine();
            }
        }

        private void buildOverlandSummary(FruaGeoFile file)
        {
            _moduleSummary.AppendLine("Overland Global Information");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Adventure Module Name:");
            _moduleSummary.AppendLine(file.ModuleName);

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Height: {0} Width: {1}",
                file.ModuleHeight, file.ModuleWidth));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Entry Points");
            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("{0} ({1},{2})",
                                  i + 1, file.GetEntryPointX(i), file.GetEntryPointY(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Summoned Monster: {0}", 
                file.SummonedMonster));
            _moduleSummary.AppendLine();
            
            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("Zone Name {0}: {1}", i+1, file.GetZoneName(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Map Art");
            _moduleSummary.AppendLine(
                string.Format("{0}", file.GetOverlandMapArt()));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Outdoor Combat Art for Each Zone");
            _moduleSummary.AppendLine();
            
            for (var i = 0; i <8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("Zone {0}: {1}", i+1, file.GetZoneArt(i)));
            }

            // TODO zone messages. where do they come from??

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Step Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("{0}) A {1} Event",
                                  i+1, file.GetStepEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Happens on Step {0} in Zones:",
                                  file.GetStepEventSteps(i)));
                _moduleSummary.AppendLine();
            }

            _moduleSummary.AppendLine("More Module Options");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Undead are more difficult to turn by {0}",
                file.UndeadTurnDifficulty));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Camp Picture: {0}", file.CampPicture));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Treasure Picture: {0}", file.TreasurePicture));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Rest Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("In Zone {0} a {1} Event",
                    i+1, file.GetRestEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Has a {0} % Chance of Occurring",
                    file.GetRestChance(i)));
                _moduleSummary.AppendLine(
                    string.Format("Every {0} Minutes.",
                    file.GetRestEventMinutes(i)));
                _moduleSummary.AppendLine();
            }
        }

        public Control GetControl()
        {
            return new RichTextBox
                       {
                           Text = _moduleSummary.ToString(), 
                           Dock = DockStyle.Fill,
                       };
        }

        public float Zoom { get; set; }

        public int ContainerWidth { get; set; }
        
        public string GetMode()
        {
            return "FRUA GEO File";
        }
    }
}