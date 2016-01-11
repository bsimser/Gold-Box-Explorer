using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GoldBoxExplorer.Lib.Plugins.Geo
{
    public class FruaGeoFile : GoldBoxFile
    {
        private const int NumEntryPoints = 8;
        private const int NumRestEvents = 8;
        private const int NumStepEvents = 8;
        private const int NumZones = 8;
        private const int NumOverlandWildernessSlots = 8;
        private const int NumOverlandTextStringSlots = 8;
        private const int NumMapDataSlots = 576;
        private const int NumEvents = 100;
        private readonly List<Event> _events = new List<Event>();
        private readonly List<RestEvent> _restEvents = new List<RestEvent>();
        private readonly List<StepEvent> _stepEvents = new List<StepEvent>();
        private readonly List<EntryPoint> _entryPoints = new List<EntryPoint>();
        private readonly List<MapGrid> _map = new List<MapGrid>(); 
        private readonly string[] _zones = new string[NumZones];
        private byte _areaViewFlag;
        private byte[] _backdropSlots;
        private byte _dungeonCombatArtSlot;
        private byte _wildernessCombatArtSlot;
        private byte[] _overlandWildernessSlots;
        private byte _summonedMonster;
        private readonly Dictionary<int, Event> _eventStrategies = new Dictionary<int, Event>();
        private StringBuilder _moduleSummary;

        public FruaGeoFile(string fullPath)
        {
            LoadModule(fullPath);
        }

        public bool IsOverlandModule
        {
            get { return WallSlots[0] == 255 && WallSlots[1] == 255 && WallSlots[2] == 255; }
        }

        public IList<byte> WallSlots { get; private set; }

        public string ModuleName { get; private set; }

        public int ModuleHeight { get; set; }

        public int ModuleWidth { get; set; }

        public string SummonedMonster
        {
            get { return _summonedMonster.ToString(CultureInfo.InvariantCulture); }
        }

        public bool AllowAreaView
        {
            get { return _areaViewFlag == 0; }
        }

        public int UndeadTurnDifficulty { get; set; }

        public int CampPicture { get; set; }

        public int TreasurePicture { get; set; }

        public string IndoorCombatArt
        {
            get { return _dungeonCombatArtSlot.ToString(CultureInfo.InvariantCulture); }
        }

        public string OutdoorCombatArt
        {
            get { return _wildernessCombatArtSlot.ToString(CultureInfo.InvariantCulture); }
        }

        public string GetZoneName(int zone)
        {
            return _zones[zone];
        }

        private void LoadModule(string fullPath)
        {
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    ReadHeader(reader);
                    ReadEntryPoints(reader);
                    ReadRestEvents(reader);
                    ReadStepEvents(reader);
                    var scratch = reader.ReadBytes(8); // blank in dungeon, zeroed out in overland
                    ModuleName = new string(reader.ReadChars(16)).Trim('\0');
                    ReadZoneNames(reader);
                    _summonedMonster = reader.ReadByte();
                    UndeadTurnDifficulty = reader.ReadByte();
                    _overlandWildernessSlots = reader.ReadBytes(NumOverlandWildernessSlots);
                    for (var i = 0; i < NumOverlandTextStringSlots; i++)
                    {
                        scratch = reader.ReadBytes(2);
                    }
                    CampPicture = reader.ReadByte();
                    TreasurePicture = reader.ReadByte();
                    scratch = reader.ReadBytes(3); // "map"
                    scratch = reader.ReadBytes(5); // unknown?
                    ReadMap(reader);
                    scratch = reader.ReadBytes(4); // "encr"
                    scratch = reader.ReadBytes(4); // ??
                    ReadEvents(reader);
                    scratch = reader.ReadBytes(4); // "strg"
                    reader.ReadByte(); // num text boxes
                    scratch = reader.ReadBytes(400); // string lengths
                    scratch = reader.ReadBytes(6760); // text strings
                }
            }
        }

        private void ReadMap(BinaryReader reader)
        {
            for (int i = 0; i < NumMapDataSlots; i++)
            {
                var northWall = reader.ReadByte();
                var eastWall = reader.ReadByte();
                var southWall = reader.ReadByte();
                var westWall = reader.ReadByte();
                var eventNumber = reader.ReadByte();
                var backdrop = reader.ReadByte();

                _map.Add(new MapGrid
                    {
                        North = northWall,
                        South = southWall,
                        East = eastWall,
                        West = westWall,
                        Event = eventNumber,
                        Backdrop = backdrop,
                    });
            }
        }

        private void ReadEvents(BinaryReader reader)
        {
            _eventStrategies.Add(0, new NullEvent());
            _eventStrategies.Add(1, new CombatEvent());
            _eventStrategies.Add(2, new TextStatementEvent());
            _eventStrategies.Add(3, new GiveTreasureEvent());
            _eventStrategies.Add(4, new DamageEvent());
            _eventStrategies.Add(5, new StairsEvent());
            _eventStrategies.Add(6, new TrainingHallEvent());
            _eventStrategies.Add(7, new TavernEvent());
            _eventStrategies.Add(8, new ShopEvent());
            _eventStrategies.Add(9, new TempleEvent());
            _eventStrategies.Add(10, new QuestionButtonEvent());
            _eventStrategies.Add(11, new TransferModuleEvent());
            _eventStrategies.Add(12, new GuidedTourEvent());
            _eventStrategies.Add(13, new AddNpcEvent());
            _eventStrategies.Add(14, new NpcSaysEvent());
            _eventStrategies.Add(15, new EncounterEvent());
            _eventStrategies.Add(16, new UtilityEvent());
            _eventStrategies.Add(17, new SoundsEvent());
            _eventStrategies.Add(18, new WhoTriesEvent());
            _eventStrategies.Add(19, new WhoPaysEvent());
            _eventStrategies.Add(20, new EnterPasswordEvent());
            _eventStrategies.Add(21, new QuestionListEvent());
            _eventStrategies.Add(22, new SmallTownEvent());
            _eventStrategies.Add(23, new ChainEvent());
            _eventStrategies.Add(24, new VaultEvent());
            _eventStrategies.Add(25, new CombatTreasureEvent());
            _eventStrategies.Add(26, new GainExperienceEvent());
            _eventStrategies.Add(27, new PassTimeEvent());
            _eventStrategies.Add(29, new CampEvent());
            _eventStrategies.Add(32, new RemoveNpcEvent());
            _eventStrategies.Add(33, new PickOneCombatEvent());
            _eventStrategies.Add(34, new TeleporterEvent());
            _eventStrategies.Add(35, new QuestStageEvent());
            _eventStrategies.Add(36, new QuestionYesNoEvent());
            _eventStrategies.Add(37, new TavernTalesEvent());
            _eventStrategies.Add(38, new SpecialItemEvent());

            for (var i = 0; i < NumEvents; i++)
            {
                var eventType = reader.ReadByte();
                reader.ReadByte(); // do flag
                reader.ReadByte(); // event condition
                reader.ReadByte(); // event chain
                _events.Add(_eventStrategies[eventType].LoadEvent(reader));
            }
        }

        private void ReadZoneNames(BinaryReader reader)
        {
            for (var i = 0; i < NumZones; i++)
            {
                var zoneName = "";
                for (var j = 0; j < 16; j++)
                {
                    var b = reader.ReadByte();
                    if (b != 0)
                        zoneName += (char) b;
                }
                _zones[i] = zoneName;
            }
        }

        private void ReadStepEvents(BinaryReader reader)
        {
            for (var i = 0; i < NumStepEvents; i++)
            {
                var numberOfSteps = reader.ReadByte();
                reader.ReadByte(); // unused
                var eventIndex = reader.ReadByte();
                var zoneFlags = reader.ReadByte();

                _stepEvents.Add(
                    new StepEvent
                        {
                            EventIndex = eventIndex,
                            StepNumber = numberOfSteps,
                            ZoneFlags = zoneFlags,
                        });
            }
        }

        private void ReadRestEvents(BinaryReader reader)
        {
            for (var i = 0; i < NumRestEvents; i++)
            {
                var zoneMinutes = reader.ReadByte();
                reader.ReadByte(); // unused
                var eventIndex = reader.ReadByte();
                var percentChance = reader.ReadByte();

                _restEvents.Add(
                    new RestEvent
                        {
                            EventIndex = eventIndex,
                            Chance = percentChance,
                            Minutes = zoneMinutes,
                        });
            }
        }

        private void ReadHeader(BinaryReader reader)
        {
            reader.ReadBytes(26);
            ModuleHeight = reader.ReadByte();
            ModuleWidth = reader.ReadByte();
            WallSlots = reader.ReadBytes(3);
            _areaViewFlag = reader.ReadByte();
            _backdropSlots = reader.ReadBytes(4);
            _dungeonCombatArtSlot = reader.ReadByte();
            _wildernessCombatArtSlot = reader.ReadByte();
        }

        private void ReadEntryPoints(BinaryReader reader)
        {
            for (var i = 0; i < NumEntryPoints; i++)
            {
                var entryRow = reader.ReadByte();
                var entryCol = reader.ReadByte();
                var entryDir = reader.ReadByte(); //0=N, 2=E, 4=S, 6=W
                reader.ReadByte(); // unused
                _entryPoints.Add(new EntryPoint{Column = entryCol, Row = entryRow, Direction = entryDir});
            }
        }

        public override IList<byte> GetBytes()
        {
            throw new NotImplementedException();
        }

        public string GetOverlandMapArt()
        {
            return _backdropSlots[0].ToString();
        }

        public string GetZoneArt(int zone)
        {
            return _overlandWildernessSlots[zone].ToString();
        }

        public string GetStepEventType(int stepEvent)
        {
            var eventIndex = _stepEvents[stepEvent].EventIndex;
            var eventData = _events[eventIndex];
            return eventData.Name;
        }

        public int GetStepEventSteps(int stepEvent)
        {
            return _stepEvents[stepEvent].StepNumber;
        }

        public string GetRestEventType(int zoneEvent)
        {
            var eventIndex = _restEvents[zoneEvent].EventIndex;
            var eventData = _events[eventIndex];
            return eventData.Name;
        }

        public string GetRestChance(int eventIndex)
        {
            return _restEvents[eventIndex].Chance.ToString();
        }

        public string GetRestEventMinutes(int eventIndex)
        {
            return _restEvents[eventIndex].Minutes.ToString();
        }

        public string GetBackdropSlot(int slot)
        {
            return _backdropSlots[slot].ToString();
        }

        public int GetEntryPointX(int point)
        {
            return _entryPoints[point].Column;
        }

        public int GetEntryPointY(int point)
        {
            return _entryPoints[point].Row;
        }

        private void BuildModuleSummary()
        {
            _moduleSummary = new StringBuilder();

            if (IsOverlandModule)
            {
                BuildOverlandSummary();
            }
            else
            {
                BuildDungeonSummary();
            }
        }
        private void BuildDungeonSummary()
        {
            _moduleSummary.AppendLine("Dungeon Global Information");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Adventure Module Name:");
            _moduleSummary.AppendLine(ModuleName);
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine(
                string.Format("Height: {0} Width: {1}",
                              ModuleHeight, ModuleWidth));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Allow Area View? {0}",
                              AllowAreaView ? "Yes" : "No"));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine(
                string.Format("Summoned Monster: {0}",
                SummonedMonster));
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("Zone Name {0}: {1}", i + 1, GetZoneName(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Wall Art Sets");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 3; i++)
            {
                _moduleSummary.AppendLine(string.Format("Wall Art Slot {0}: {1}", i + 1, WallSlots[i]));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Floor and Ceiling Art");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 4; i++)
            {
                _moduleSummary.AppendLine(string.Format("Floor Art Slot {0}: {1}", i + 1, GetBackdropSlot(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Combat Art");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(string.Format("Indoor Combat Art: {0}", IndoorCombatArt));
            _moduleSummary.AppendLine(string.Format("Outdoor Combat Art: {0}", OutdoorCombatArt));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Step Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("{0}) A {1} Event",
                                  i + 1, GetStepEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Happens on Step {0} in Zones:",
                                  GetStepEventSteps(i)));
                _moduleSummary.AppendLine();
            }

            _moduleSummary.AppendLine("More Module Options");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Undead are more difficult to turn by {0}",
                UndeadTurnDifficulty));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Camp Picture: {0}", CampPicture));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Treasure Picture: {0}", TreasurePicture));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Rest Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("In Zone {0} a {1} Event",
                    i + 1, GetRestEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Has a {0} % Chance of Occurring",
                    GetRestChance(i)));
                _moduleSummary.AppendLine(
                    string.Format("Every {0} Minutes.",
                    GetRestEventMinutes(i)));
                _moduleSummary.AppendLine();
            }
        }

        private void BuildOverlandSummary()
        {
            _moduleSummary.AppendLine("Overland Global Information");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Adventure Module Name:");
            _moduleSummary.AppendLine(ModuleName);

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Height: {0} Width: {1}",
                ModuleHeight, ModuleWidth));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Entry Points");
            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("{0} ({1},{2})",
                                  i + 1, GetEntryPointX(i), GetEntryPointY(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Summoned Monster: {0}",
                SummonedMonster));
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("Zone Name {0}: {1}", i + 1, GetZoneName(i)));
            }

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Map Art");
            _moduleSummary.AppendLine(
                string.Format("{0}", GetOverlandMapArt()));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Outdoor Combat Art for Each Zone");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("Zone {0}: {1}", i + 1, GetZoneArt(i)));
            }

            // TODO zone messages. where do they come from??

            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine("Step Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("{0}) A {1} Event",
                                  i + 1, GetStepEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Happens on Step {0} in Zones:",
                                  GetStepEventSteps(i)));
                _moduleSummary.AppendLine();
            }

            _moduleSummary.AppendLine("More Module Options");
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Undead are more difficult to turn by {0}",
                UndeadTurnDifficulty));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Camp Picture: {0}", CampPicture));
            _moduleSummary.AppendLine();
            _moduleSummary.AppendLine(
                string.Format("Treasure Picture: {0}", TreasurePicture));
            _moduleSummary.AppendLine();

            _moduleSummary.AppendLine("Rest Events");
            _moduleSummary.AppendLine();

            for (var i = 0; i < 8; i++)
            {
                _moduleSummary.AppendLine(
                    string.Format("In Zone {0} a {1} Event",
                    i + 1, GetRestEventType(i)));
                _moduleSummary.AppendLine(
                    string.Format("Has a {0} % Chance of Occurring",
                    GetRestChance(i)));
                _moduleSummary.AppendLine(
                    string.Format("Every {0} Minutes.",
                    GetRestEventMinutes(i)));
                _moduleSummary.AppendLine();
            }
        }

        public string GetSummary()
        {
            BuildModuleSummary();
            return _moduleSummary.ToString();
        }

        public string GetMap()
        {
            var mapSummary = new StringBuilder();

            foreach (var grid in _map)
            {
                mapSummary.AppendLine(
                    string.Format("N:{0} E:{1} S:{2} W:{3} EVT:{4} BACK:{5}",
                                          grid.North, grid.East, grid.South, grid.West, grid.Event,
                                          grid.Backdrop));
            }

            return mapSummary.ToString();
        }
    }
}