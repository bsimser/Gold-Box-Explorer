using System.Collections.Generic;
using System.IO;
using GoldBoxExplorer.Lib.Plugins.Dax;
using GoldBoxExplorer.Lib.Plugins;
using GoldBoxExplorer.Lib.Plugins.DaxEcl;
using System;
using System.Drawing;

namespace GoldBoxExplorer.Lib.Plugins.GeoDax
{
    public class GeoDaxFile : DaxFile
    {
        private readonly Dictionary<int, IDictionary<int, string>> _mapNames = new Dictionary<int, IDictionary<int, string>>();
        private IList<GeoMapRecord> _maps;
        public Dictionary<int, int[]> wallsetMapping = new Dictionary<int,int[]>(); // todo: rename this to mapping
        private string _filename;
        public Dictionary<int, List<Bitmap>> wallsetBitmaps = new Dictionary<int, List<Bitmap>>();
        public List<int> referencedByEcl = new List<int>();
        public Dictionary<int, int> highestEvent = new Dictionary<int,int>();
        public DaxEclFile _daxEclFile;
        public GeoDaxFile(string file) : base(file)
        {
            _filename = file;
            loadWallSets(file);
            PopulateMapNames();
            ProcessBlocks();
            //var path = Path.GetDirectoryName(file);
            
        }

        private void loadWallSets(string file)
        {
            var path = Path.GetDirectoryName(file);

            for (int n = 0; n < 8; n++)
            {
                var fn = path + "\\WALLDEF" + n + ".DAX";
                if (n == 0) { fn = path + "\\WALLDEF.DAX"; }
                if (System.IO.File.Exists(fn))
                {
                    var dwf = new DaxWallDefFile(fn);
                    int i = 0;
                    foreach (var b in dwf.wallsets)
                    {
                        var id = dwf._blockIds[i];
                        if (!wallsetBitmaps.ContainsKey(id))
                            wallsetBitmaps.Add(id, b);
                        i++;
                    }
                }
            }
        }
        private bool isValidWallSetId(int id) {
            return true;
            if (id == 127 || id == 255)
                return true;
            if (wallsetBitmaps.ContainsKey(id))
                return true;
            return false;
        }
        // scan the ecl files in the current directory for references to this map, return the filename and block id of the first reference
        private void scanECLFilesForMap(string file, int geoID)
        {
            var filename = Path.GetFileNameWithoutExtension(file);
            var path = Path.GetDirectoryName(file);
            var diskNumber = filename.Substring(3, 1);
            var eclFileName = "ECL" + diskNumber + ".DAX";
            _daxEclFile = new DaxEcl.DaxEclFile(path + "\\" + eclFileName);
           // _daxWallDefFiles.Add(new DaxWallDefFile(path + "\\WALLDEF" + diskNumber + ".DAX"));
            foreach (var block in _daxEclFile.Blocks)
            {
                // scan for the load file geo command, which consists of the pattern 0x21 a 0x00 where a is the geo id to load
                // todo: since we can decode the ecl's pretty well now, maybe use that instead?
                byte load_file = 0x21;
                for (int i = 0; i < block.Data.Length-4; i++)
                {
                    if (block.Data[i] == load_file && block.Data[i + 1] == 0 && block.Data[i + 3] == 0)
                    {
                        int a = block.Data[i + 2];
                        if (geoID == a) 
                        {
                            referencedByEcl.Add(block.Id);
                            scanECLFileForWallset(block, geoID, block.Id);
                        }
                    }
                }
            }
            

        }
        // scan the designated ecl file/block for wallset loading commands, return the first one
        private void scanECLFileForWallset(DaxFileBlock block, int geoID, int blockID)
        {
                // scan for the load pieces command, which consists of the pattern 0x37 a 0x00 b 0x00 c 0x00, where a, b and c are the wallsets to load
                byte load_pieces = 0x37;
                for (int i = 0; i < block.Data.Length-7; i++)
                {
                    if (block.Data[i] == load_pieces && block.Data[i + 1] == 0 && block.Data[i + 3] == 0 && block.Data[i + 5] == 0)
                    {
                        int a = block.Data[i + 2];
                        int b = block.Data[i + 4];
                        int c = block.Data[i + 6];
                        int[] ws = { a, b, c };
                        if (isValidWallSetId(a) && isValidWallSetId(b) && isValidWallSetId(c))
                        {
                            if (!wallsetMapping.ContainsKey(geoID))
                                wallsetMapping.Add(geoID, ws);
                        }
                    }
                }
        }

        private void PopulateMapNames()
        {
            var maps = new Dictionary<int, string>
                {
                    {0, "Civilized Area, New Phlan"},
                    {1, "Buccaneer Base"},
                    {2, "Cadorna Textile House"},
                    {3, "Valjevo Castle, North West"},
                    {4, "Valjevo Castle, North East"},
                    {5, "Valjevo Castle, South East"},
                    {6, "Valjevo Castle, South West"},
                    {7, "Valjevo Castle, Inner Tower"},
                    {9, "Stojanow Gate"},
                    {10, "Valhingen Graveyard"},
                    {13, "Kobold Caves"},
                    {14, "Kovel Mansion"},
                    {15, "Mendor's Library"},
                    {16, "Lizard Men Keep"},
                    {17, "Nomad Camp"},
                    {18, "Podal Plaza"},
                    {20, "Slums"},
                    {21, "Sokal Keep"},
                    {22, "Sorcerer's Island, Level 1"},
                    {23, "Sorcerer's Island, Level 2 and 3"},
                    {24, "Temple of Bane"},
                    {25, "Unknown Lair"},
                    {26, "Unknown Zone"},
                    {27, "Unknown Lair"},
                    {28, "Outpost of Zhentil Keep"},
                    {29, "Kuto's Well"},
                    {30, "Lizard Men Catacombs"},
                    {31, "Wealthy Area"},
                    {32, "Kuto's Well Catacombs"}
                };
            _mapNames.Add((int) FileHelper.GameList.PoolOfRadiance, maps);

            maps = new Dictionary<int, string>
                {
                    {1, "Tilverton City, Thieves' Guild"},
                    {3, "Tilverton Sewers"},
                    {4, "The Fire Knift Hideout"},
                    {16, "Yulash"},
                    {17, "The Pit of Moander, Levels 1-2"},
                    {32, "Zhentil Keep, The Shrine of Bane"},
                    {33, "The Cave of the Beholder"},
                    {37, "Oxam's Tower, Dungeon, Cavern"},
                    {50, "Village of Haptooth, Cave of the Dracolich"},
                    {51, "The Wizard's Tower"},
                    {64, "The Burial Glen"},
                    {66, "The Ruins of Myth Drannor"},
                    {67, "The Grand Ruined Temple, Levels 2-1"}
                };
            _mapNames.Add((int)FileHelper.GameList.CurseOfTheAzureBonds, maps);

            maps = new Dictionary<int, string>
                {
                    {16, "New Verdigris"},
                    {32, "Old Verdigris Ruins"},
                    {33, "Well of Knowledge"},
                    {34, "Black Circle Headquarters"},
                    {48, "The Mines, Level 1"},
                    {49, "The Mines, Level 2"},
                    {50, "Temple of Tyr"},
                    {80, "Crevasses, Level 1"},
                    {81, "Frost Giant Village"},
                    {82, "Crevasses, Level 2"},
                    {96, "Castle Entrance"},
                    {97, "Middle Level of the Castle"},
                    {98, "Dreadlord's Sanctum"},
                    {64, "Dungeon Levels 10-7"},
                    {65, "Dungeon Levels 4-3"},
                    {66, "Dungeon Levels 2-1"},
                    {68, "Drider Base"}
                };
            _mapNames.Add((int)FileHelper.GameList.SecretOfTheSilverBlades, maps);
        
            maps = new Dictionary<int, string>
                {
                    {1, "Playtester Town"},
                    {16, "Phlan (East)"},
                    {17, "Phlan (West)"},
                    {18, "Limbo"},
                    {19, "Mulmaster, Zhentil Keep"},
                    {20, "Overland Minis"},
                    {21, "Hill Giant Steading"},
                    {22, "Sasha and the Lands of Thar"},
                    {32, "Fire Giant Cave"},
                    {33, "Dragons' Aerie"},
                    {34, "Thorne's Cave"},
                    {35, "Descent To The Depths"},
                    {36, "Cave of the Beholder"},
                    {37, "Vala vs. Vaasa"},
                    {39, "Dave's Challenge"},
                    {40, "Manshoon's Tower"},
                    {48, "Manshoon's Tower"},
                    {49, "Dragon's Aerie"},
                    {50, "Temple of Tyr"},
                    {52, "Moander Minis"},
                    {53, "Moander's Heart"},
                    {64, "Dark Phlan"},
                    {65, "The Palace of Gothmenes"},
                    {66, "Nacacia and Myth Drannor"},
                    {67, "Tower of Marcus Levels 9-11"},
                    {68, "Silk and the Slaves of the Drow"},
                    {69, "Kalistes' Temple"},
                    {70, "Tower of Marcus Levels 0-5"},
                    {71, "Tower of Marcus Levels 6-8"},
                    {81, "Drow Testing Ground"},
                    {82, "Kalistes' Parlor, Upper Level"},
                    {83, "Web Dimension Mines"},
                    {84, "Kalistes' Parlor, Lower Level"}
                };
            _mapNames.Add((int)FileHelper.GameList.PoolsOfDarkness, maps);
            
            maps = new Dictionary<int, string>();
            _mapNames.Add((int)FileHelper.GameList.ForgottenRealmsUnlimitedAdventures, maps);
            
            maps = new Dictionary<int, string>
                {
                    {1, "Yartar"},
                    {2, "Kraken Complex"},
                    {3, "Nesme"},
                    {4, "Silverymoon"},
                    {5, "Everlund"},
                    {6, "Neverwinter"},
                    {7, "Port Llast and Gallant Prince"},
                    {8, "Luskan"},
                    {9, "Hosttower of the Arcane"},
                    {10, "Tuern"},
                    {11, "Gundarlun"},
                    {12, "Purple Rocks"},
                    {13, "Kraken Headquarters"},
                    {14, "Secomber"},
                    {15, "Loudwater"},
                    {16, "Llorkh"},
                    {17, "The Arena Beneath Llorkh"},
                    {18, "The Star Mounts"},
                    {19, "Sundabar"},
                    {20, "Ascore"},
                    {21, "Outdoors"},
                    {27, "Vaalgamon Taunts At Ascore Maze"},
                    {28, "Victory at Ascore"},
                    {99, "Playtester Town"}
                };
            _mapNames.Add((int)FileHelper.GameList.GatewayToTheSavageFrontier, maps);
            
            maps = new Dictionary<int, string>
                {
                    {16, "Llorkh"},
                    {17, "Lord Geildarr's Keep"},
                    {18, "Loudwater"},
                    {19, "Secomber"},
                    {20, "Leilon"},
                    {21, "Smuggler's Dock"},
                    {22, "Waterdeep and the Caverns"},
                    {23, "Yartar"},
                    {24, "Triboar"},
                    {25, "Longsaddle"},
                    {26, "Mintarn"},
                    {27, "Orlumbor"},
                    {28, "Neverwinter"},
                    {29, "Port Llast"},
                    {30, "Mirabar"},
                    {31, "Luskan"},
                    {32, "Ruathym"},
                    {33, "Fireshear"},
                    {34, "Fireshear Mines"},
                    {35, "Aurilssbaarg"},
                    {36, "Bjorn's Hold"},
                    {37, "Icewolf"},
                    {38, "Freezefire's Lair"},
                    {39, "Daggerford"},
                    {40, "Way Inn"},
                    {47, "Far Windward"},
                    {48, "Trackless Sea Tours"},
                    {49, "Tower of Twilight"},
                    {50, "The Crossroads"},
                    {51, "Luskan, Port Llast, Neverwinter Area"},
                    {52, "Mirabar, Longsaddle Area"},
                    {53, "Neverinter, Leilon Area"},
                    {54, "Triboar, Yartar Area"},
                    {55, "Waterdeep Area"},
                    {56, "Secomber Area"},
                    {57, "Loudwater, Llorkh Area"},
                    {58, "Fireshear"},
                    {59, "Freezefire's Lair"},
                    {60, "Orlumbor"},
                    {61, "Luskan"},
                    {62, "Luscan"}
                };
            _mapNames.Add((int)FileHelper.GameList.TreasuresOfTheSavageFrontier, maps);
            
            maps = new Dictionary<int, string>
                {
                    {1, "Neverwinter"},
                    {2, "Warehouse"},
                    {3, "Wharves"},
                    {4, "Southwall"},
                    {5, "Windy Cliffs"},
                    {6, "Port Llast"},
                    {7, "Gallant Prince"},
                    {8, "Luskan"},
                    {9, "Vilnask"},
                    {10, "Crossergate"},
                    {11, "Lost Hills"},
                    {12, "Triboar"},
                    {13, "Chargen"},
                    {14, "Triboar Lower"},
                    {15, "Berun's Hill"},
                    {16, "Longsaddle"},
                    {17, "Longsaddle Lower"},
                    {18, "NW Woods"},
                    {19, "NE Woods"},
                    {20, "W Woods"},
                    {21, "E Woods"},
                    {22, "SW Woods"},
                    {23, "SE Woods"},
                    {24, "Floodblest"},
                    {25, "Nightsedge"},
                    {26, "Nightsedge Caverns"},
                    {27, "Sewers"},
                    {28, "Utheraal"},
                    {29, "Trisk"},
                    {30, "Port Llast"}
                };
            _mapNames.Add((int)FileHelper.GameList.NeverwinterNights, maps);
            
            maps = new Dictionary<int, string>
                {
                    {32, "Throtl, Throtl Temple"},
                    {34, "Throtl Catacombs"},
                    {48, "Gargath"},
                    {49, "Gargath Keep"},
                    {50, "Jalek"},
                    {64, "Nereka City and Base"},
                    {66, "Nereka Prison"},
                    {67, "Tomb of Sir Dargaard"},
                    {68, "Southern Outpost"},
                    {80, "Sanction Docks"},
                    {81, "Temple of Huerzyd"},
                    {82, "Temple of Duerghast"},
                    {96, "Citadels"},
                    {97, "Kernen Square"},
                    {99, "Ogre Base"}
                };
            _mapNames.Add((int)FileHelper.GameList.ChampionsOfKrynn, maps);
            
            maps = new Dictionary<int, string>
                {
                    {32, "Kalaman"},
                    {33, "Vingaard Keep"},
                    {34, "Cekos"},
                    {36, "Gargath Outpost"},
                    {49, "High Clerist's Tower"},
                    {50, "Dragon Pit"},
                    {51, "Throtl Keep"},
                    {64, "Graveyard"},
                    {65, "Kuo-Toa Slave Ship"},
                    {66, "Gnome Village (Quazle)"},
                    {67, "Turef"},
                    {80, "Cerberus, Dulcimer"},
                    {82, "Voice Wood"},
                    {83, "Shipwreck, Cursed Village, Fun House, Father of Trees"},
                    {96, "Dargaard Keep, First Floor"},
                    {97, "Dargaard Keep, Second Floor"},
                    {98, "Dargaard Keep, Third Floor"},
                    {99, "Challenge"}
                };
            _mapNames.Add((int)FileHelper.GameList.DeathKnightsOfKrynn, maps);
            
            maps = new Dictionary<int, string>();
            _mapNames.Add((int)FileHelper.GameList.DarkQueenOfKyrnn, maps);
            
            maps = new Dictionary<int, string>
                {
                    {16, "Chicagorg"},
                    {32, "Spy Ship"},
                    {35, "Asteroid Base"},
                    {48, "Asteroid Base, Level 1"},
                    {49, "Asteroid Base, Level 2"},
                    {50, "Pirate Ship, Levels 1-5"},
                    {51, "Pirate Ship, Levels 6-10"},
                    {52, "Pirate Ship, Levels 11-15"},
                    {65, "Desert Runner Village"},
                    {66, "Mars Base Gradiuvs Mons"},
                    {67, "More Asteroid Bases"},
                    {81, "Lowlander Village, Venusian Space Elevator Ruins"},
                    {82, "Venus RAM Base"},
                    {96, "Mercury Merchants Area"},
                    {97, "Mariposa Core"},
                    {98, "Mercurian Finale, Weapons Control Level"},
                    {99, "Enemy Ships"}
                };
            _mapNames.Add((int)FileHelper.GameList.CountdownToDoomsday, maps);
            
            maps = new Dictionary<int, string>
                {
                    {1, "Luna Base"},
                    {17, "Caloris Space Port"},
                    {18, "Asteroid"},
                    {33, "Losangelorg Sprawls"},
                    {34, "Historical Museum, Levels 2-1"},
                    {35, "Asteroid Base"},
                    {38, "Losangelorg Sprawls"},
                    {49, "Lowlander Village"},
                    {50, "Venus Laboratory, Level 1"},
                    {51, "Venus Laboratory, Level 2"},
                    {52, "Lowlander Mines"},
                    {64, "Luna Base"},
                    {65, "Tsai Weaponry Labs"},
                    {66, "RAM Battler, Deimos Level 19-41"},
                    {80, "Mars Prison, Level 1"},
                    {81, "PURGE Headquarters, Floors Ground-Upper"},
                    {82, "NEO Installation"},
                    {84, "Mars Prison, Level 2"},
                    {96, "Living Ship"},
                    {97, "Living Ship"},
                    {112, "Stormrider University"},
                    {113, "Genetics Foundation Building, Levels 1-4"},
                    {114, "Jupiter Finale"},
                    {115, "Jupiter Aircar"}
                };
            _mapNames.Add((int)FileHelper.GameList.MatrixCubed, maps);
        }

        public IList<GeoMapRecord> GetMaps()
        {
            return _maps;
        }

        private void recordHighestEvent(int id, int eventNumber) {
            eventNumber &= 127;
            if (highestEvent.ContainsKey(id))
            {
                if (eventNumber > highestEvent[id])
                    highestEvent[id] = eventNumber;
            }
            else
            {
                highestEvent.Add(id, eventNumber);
            }
        }
        protected override sealed void ProcessBlocks()
        {
            _maps = new List<GeoMapRecord>();

            foreach (var block in Blocks)
            {
                scanECLFilesForMap(_filename, block.Id);
                var walls = new List<GeoWallRecord>();
                
                for (var i = 0; i < 256; i++)
                {
                    var row = i >> 4;
                    var col = i & 0x0f;

                    var neWallType = block.Data[i + 2];
                    var north = (byte) (neWallType >> 4) & 0x0f;
                    var east = (byte) (neWallType & 0x0f);

                    var swWallType = block.Data[i + 258];
                    var south = (byte)(swWallType >> 4) & 0x0f;
                    var west = (byte)(swWallType & 0x0f);
                    
                    var eventdata = block.Data[i + 514];
                    var doorinfo = block.Data[i + 770];
                    recordHighestEvent(block.Id, eventdata & 127);
                    walls.Add(new GeoWallRecord
                                   {
                                       Row = row,
                                       Column = col,
                                       North = north,
                                       East = east,
                                       South = south,
                                       West = west,
                                       Event = eventdata,
                                       Door = doorinfo,
                                   });
                }

                _maps.Add(new GeoMapRecord(GetMapNameFrom(block), walls, block.Id));
            }
        }

        private string GetMapNameFrom(DaxFileBlock block)
        {
            var mapname = block.Id.ToString();

            var game = FileHelper.DetermineGameFrom(block.File);
            if (game != FileHelper.GameList.Unknown)
            {
                // do we have a map with that id?
                if(_mapNames.ContainsKey((int) game))
                {
                    var names = _mapNames[(int) game];
                    if(names.ContainsKey(block.Id))
                    {
                        mapname = names[block.Id];
                    }
                }
            }

            return mapname;
        }

 
 
    }
}