using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldBoxExplorer.Lib.Plugins.Character
{
    public class Annotation
    {
        public bool isItem = false;
        public bool duplicated { get; set; }
        public bool firstInRecord { get; set; }
        public int len { get; set; }
        public int addr { get; set; }
        public string description { get; set; }
        public string fieldType { get; set; }
        public string[] races = {"Monster", "Dwarf", "Elf", "Gnome", "Half-Elf", "Halfling", "Half-Orc", "Human"};
        public string[] classes = {"Cleric", "Druid", "Fighter", "Paladin", "Ranger", "Magic-User", "Thief", "Monk", "Cleric/Fighter", "Cleric/Fighter/Magic-User",
                                      "Cleric/Ranger", "Cleric/Magic-User", "Cleric/Thief", "Fighter/Magic-User", "Fighter/Thief", "Fighter/Magic-User/Thief",
                                      "Magic-User/Thief" };
        public string[] alignments = {"Lawful Good", "Lawful Neutral", "Lawful Evil", "Neutral Good", "True Neutral", "Neutral Evil", "Chaotic Good", 
                                         "Chaotic Neutral", "Chaotic Evil" };
        public string[] spells = {"Bless", "Curse", "Cure Light Wounds", "Cause Light Wounds", "Detect Magic", "Protection From Evil", "Protection From Good", 
                                     "Resist Cold", "Burning Hands", "Charm Person", "Detect Magic", "Enlarge", "Reduce", "Friends", "Magic Missile", 
                                     "Protection From Evil", "Protection From Good", "Read Magic", "Shield", "Shocking Grasp", "Sleep", "Find Traps", "Hold Person", 
                                     "Resist Fire", "Silence 15' Radius", "Slow Poison", "Snake Charm", "Spritual Hammer", "Detect Invisibility", "Invisibility", 
                                     "Knock", "Mirror Image", "Ray of Enfeeblement", "Stinking Cloud", "Strength", "Animate Dead", "Cure Blindness", 
                                     "Cause Blindness", "Cure Disease", "Cause Disease", "Dispel Magic", "Prayer", "Remove Curse", "Bestow Curse", "Blink", 
                                     "Dispel Magic", "Fireball", "Haste", "Hold Person", "Invisbility 10' Radius", "Lightning Bolt", 
                                     "Protection From Evil 10' Radius", "Protection From Good 10' Radius", "Protection From Normal Missiles", "Slow", "Restoration" };
        public string[] itemTable = { "", "Battle Axe", "Hand Axe", "Bardiche", "Bec De Corbin", "Bill-Guisarme", "Bo Stick", "Club", "Dagger", "Dart", "Fauchard", 
                                        "Fauchard-Fork", "Flail", "Military Fork", "Glaive", "Glaive-Guisarme", "Guisarme", "Guisarme-Voulge", "Halberd", 
                                        "Lucern Hammer", "Hammer", "Javelin", "Jo Stick", "Mace", "Morning Star", "Partisan", "Military Pick", "Awl Pike", "Quarrel",
                                        "Ranseur", "Scimitar", "Spear", "Spetum", "Quarter Staff", "Bastard Sword", "Broad Sword", "Long Sword", "Short Sword", 
                                        "Two-Handed Sword", "Trident", "Voulge", "Composite Long Bow", "Composite Short Bow", "Long Bow", "Short Bow",
                                        "Heavy Crossbow", "Light Crossbow", "Sling", "Mail", "Armor", "Leather", "Padded", "Studded", "Ring", "Scale", "Chain", 
                                        "Splint", "Banded", "Plate", "Shield", "Woods", "Arrow", "", "", "Potion", "Scroll", "Ring", "Rod", "Stave", "Wand", "Jug", 
                                        "Amulet", "Apparatus", "Bag", "Beaker", "Boat", "Book", "Boots", "Bowl", "Bracers", "Brazier", "Brooch", "Broom", "Purse",
                                        "Candle", "Carpet", "Censer", "Chime", "Cloak", "Crystal", "Cube", "Cubic", "Fortress", "Decanter", "Deck", "Drums", "Dust",
                                        "Eyes", "Figurine", "Flask", "Gauntlets", "Gem", "Girdle", "Helm", "Horn", "Horseshoes", "Incense", "Stone", "Instrument",
                                        "Javelin", "Jewel", "Ointment", "Libram", "Lyre", "Manual", "Mattock", "Maul", "Medallion", "Mirror", "Necklace", "Net",
                                        "Pigment", "Pearl", "Periapt", "Phylactery", "Pipes", "Hole", "Token", "Robe", "Rope", "Rug", "Saw", "Scarab", "Spade", 
                                        "Sphere", "", "Talisman", "Tome", "Trident", "Grimoire", "Well", "Wings", "Vial", "Lantern", "", "Oil", "10 ft. Pole",
                                        "50 ft. Rope", "Iron", "Thf Prickly Tools", "Iron Rations", "Standard Rations", "Holy Symbol", "Holy Water vial", 
                                        "Unholy Water vial", "Barding", "Dragon", "Lightning", "Saddle", "Small Raft", "Cart", "Wagon", "+1", "+2", "+3", "+4", "+5",
                                        "of", "", "Cloak", "Displacement", "Torches", "Oil", "Speed", "Tapestry", "Bodily Health", "Copper", "Silver", "Electrum", 
                                        "Gold", "Platinum", "Ointment", "Keoghtum's", "Sheet", "Strength", "Healing", "Holding", "Extra", "Gaseous Form", 
                                        "Slipperiness", "Jewelled", "Flying", "Treasure Finding", "Fear", "Disappearance", "Statuette", "Fungus", "Chain", "Pendant",
                                        "Broach", "Of Seeking", "-1", "-2", "-3", "Lightning Bolt", "Fire Resistance", "Magic Missiles", "Save", "Clerical Scroll",
                                        "Magic User Scroll", "With 1 Spell", "With 2 Spells", "With 3 Spells", "Protection Scroll", "Jewelry", "Fine", "Huge", "Bone",
                                        "Brass", "Key", "AC 2", "AC 6", "AC 4", "AC 3", "Of Protection", "Paralyzation", "Ogre Power", "Invisibility", "Missiles",
                                        "Elvenkind", "Rotting", "Covered", "Efreeti", "Bottle", "Missile Attractor", "Of Maglubiyet", "Secr Door & Trap Det", 
                                        "Gd Dragon Control", "Feather Falling", "Giant Strength", "Restoring Level(s)", "Flame Tongue", "Fireballs", "Spiritual", 
                                        "Boulder", "Diamond", "Emerald", "Opal", "Saphire", "Of Tyr", "Of Tempus", "Of Sune", "Wooden", "+3 vs Undead", "Pass", 
                                        "Cursed" };
        public Annotation(int vlen, int vaddr, string vdescr, string vfieldType = "number", bool dup = false, bool first = false)
        {
            len = vlen;
            addr = vaddr;
            description = vdescr;
            fieldType = vfieldType;
            duplicated = dup;
            firstInRecord = first;
        }

        public static string getSpecialAbilityName(int specAb)
        {
            Dictionary<int, string> effects = new Dictionary<int, string> {// from Simeon Pilgrim's curse of the azure bonds remake; adjusted for pool of radiance
                {0 , "none"}, {0x1 , "bless"}, {0x2 , "cursed"}, {0x3 , "healing"}, {0x4 , "dispel_evil"}, {0x5 , "detect_magic"}, {0x6 , "affect_06"}, 
                {0x7 , "faerie_fire"}, {0x8 , "protection_from_evil"}, {0x9 , "protection_from_good"}, {0xa , "resist_cold"}, {0xb , "charm_person"},
                {0xc , "enlarge"}, {0xd , "reduce"}, {0xe , "friends"}, {0xf , "poison_damage"}, {0x10 , "read_magic"}, {0x11 , "shield"}, 
                {0x12 , "gnome_vs_man_sized_giant"}, {0x13 , "find_traps"}, {0x14 , "resist_fire"}, {0x15 , "silence_15_radius"}, {0x16 , "slow_poison"}, {0x17 , "spiritual_hammer"}, 
                {0x18 , "detect_invisibility"},
                {0x19 , "invisibility"}, {0x1a , "dwarf_vs_orc"}, {0x1b , "fumbling"}, {0x1c , "mirror_image"}, {0x1d , "ray_of_enfeeblement"}, {0x1e , "stinking_cloud"},
                {0x1f , "helpless"}, {0x20 , "animate_dead"}, {0x21 , "blinded"}, {0x22 , "cause_disease_1"}, {0x23 , "confuse"}, {0x24 , "bestow_curse"}, {0x25 , "blink"},
                {0x26 , "strength"}, {0x27 , "haste"}, {0x28 , "affect_28"}, {0x29 , "prot_from_normal_missiles"}, {0x2a , "slow"}, {0x2b , "weaken"}, {0x2c , "cause_disease_2"}, 
                {0x2d , "prot_from_evil_10_radius"}, 
                {0x2e , "prot_from_good_10_radius"}, {0x2f , "dwarf_and_gnome_vs_giants"}, {0x30 , "affect_30"}, {0x31 , "prayer"}, {0x32 , "hot_fire_shield"}, 
                {0x33 , "lightning"}, {0x34 , "paralyze"}, {0x35 , "sleep"}, {0x36 , "cold_fire_shield"}, {0x37 , "poisoned"}, {0x38 , "item_invisibility"}, {0x39 , "affect_39"},
                {0x3a , "clear_movement"}, {0x3b , "regenerate"}, {0x3c , "resist_normal_weapons"}, {0x3d , "fire_resist"}, {0x3e , "highConRegen"}, {0x3f , "minor_globe_of_invulnerability"}, 
                {0x40 , "poison_plus_0"}, {0x41 , "poison_plus_4"}, {0x42 , "poison_plus_2"}, {0x43 , "thri_kreen_paralyze"}, {0x44 , "ghoul_paralyze"}, {0x45 , "drider_paralyzation_poison"}, 
                {0x46 , "poison_neg_2"}, {0x47 , "invisible"}, {0x48 , "unknown_thief_power_backstab?"}, {0x49 , "tiger_claw_rake"}, {0x4a , "affect_4a"}, {0x4b , "weap_dragon_slayer"},
                {0x4c , "stirge_blood_suck"}, {0x4d , "giant_mantis_bite_and_hold"}, {0x4e , "affect_4e"}, {0x4f , "tyranthraxus_unknown_power1"}, {0x50 , "ankheg_acid_attack"},
                {0x51 , "tyranthraxus_unknown_power_half_damage?"}, {0x52 , "mummy_only_power_cold_immunity?"}, {0x53 , "lightning"}, {0x54 , "unknown_vampire_only_power_charm_gaze?"},
                {0x55 , "unknown_undead_power0"}, {0x56 , "undead_power_double_level_drain?"}, {0x57 , "mummy_only_power_half_damage_from_weapons?"}, {0x58 , "magic missile attack"},
                {0x59 , "displace"}, {0x5a , "dwarf/halfling resist magic/poison?"}, {0x5b , "juju_zombie_power_half_damage_from_piercing_and_blunt_weapons?"}, {0x5c , "affect_5c"}, {0x5d , "unknown_juju_zombie_power1"}, 
                {0x5e , "unknown_juju_zombie_power2"}, {0x5f , "affect_5F"}, {0x60 , "owlbear_hug_check"}, {0x61 , "con_saving_bonus"}, {0x62 , "regen_3_hp"}, {0x63 , "wild_boar_fight_until_-10hp"},
                {0x64 , "troll_fire_or_acid"}, {0x65 , "troll_regen"}, {0x66 , "TrollRegen"}, {0x67 , "unknown_vampire_power"}, {0x68 , "thri_kreen_dodge_missile"}, {0x69 , "resist_magic_50_percent"},
                {0x6a , "resist_magic_15_percent"}, {0x6b , "elf_resist_sleep"}, {0x6c , "protect_charm_sleep"}, {0x6d , "resist_paralyze"}, {0x6e , "unknown_undead_power1"}, 
                {0x6f , "unknown_undead_power2"}, {0x70 , "immune_to_fire"}, {0x71 , "efreet_fire_resistance"}, {0x72 , "unknown_vampire_resistance?"}, 
                {0x73 , "unknown_skeleton_resistance_half_damage_from_edged_weapons?"}, {0x74 , "unknown_mummy_power_fire_vulnerability?"}, {0x75 , "unknown_undead_power_3?"}, 
                {0x76 , "unknown_vampire_only_power"}, {0x77 , "unknown_undead_power4"}, {0x78 , "giant_rock_hurling"}, {0x79 , "ankheg_acid_attack?"}, {0x7a , "dracolich_paralysis"},
                {0x7b , "affect_7b"}, {0x7c , "halfelf_resistance"}, {0x7d , "unknown_undead_power5"}, {0x7e , "unknown_vampire_power"}, {0x7f , "gaze_attack_turn_to_stone"}, {0x80 , "affect_80"},
                {0x81 , "protect_magic"}, {0x82 , "affect_82"}, {0x83 , "cast_breath_fire"}, {0x84 , "cast_throw_lightening"}, {0x85 , "affect_85"}, 
                {0x86 , "ranger_vs_giant"}, {0x87 , "protect_elec"}, {0x88 , "entangle"}, {0x89 , "affect_89"}, {0x8a , "affect_8a"}, {0x8b , "affect_8b"}, 
                {0x8c , "paladinDailyHealCast"}, {0x8d , "paladinDailyCureRefresh"}, {0x8e , "fear"}, {0x8f , "affect_8f"}, {0x90 , "owlbear_hug_round_attack"}, {0x91 , "sp_dispel_evil"},
                {0x92 , "strength_spell"}, {0x93 , "do_items_affect"}
                };
            if (specAb > effects.Count)
                return "unknown effect";
            return effects[specAb];
        }

        
        public static string GetItemTemplateName(int itemType) {
            string[] itemTypes = { "", "Battle Axe", "Hand Axe", "Bardiche", "Bec De Corbin", "Bill-Guisarme", "Bo Stick", "Club", "Dagger", "Dart", "Fauchard", 
                                        "Fauchard-Fork", "Flail", "Military Fork", "Glaive", "Glaive-Guisarme", "Guisarme", "Guisarme-Voulge", "Halberd", 
                                        "Lucern Hammer", "Hammer", "Javelin", "Jo Stick", "Mace", "Morning Star", "Partisan", "Military Pick", "Awl Pike", "Quarrel",
                                        "Ranseur", "Scimitar", "Spear", "Spetum", "Quarter Staff", "Bastard Sword", "Broad Sword", "Long Sword", "Short Sword", 
                                        "Two-Handed Sword", "Trident", "Voulge", "Composite Long Bow", "Composite Short Bow", "Long Bow", "Short Bow",
                                        "Heavy Crossbow", "Light Crossbow", "Sling", "Mail", "Armor", "Leather Armor", "Padded Armor", "Studded Armor", "Ring Armor", "Scale Armor", "Chain Armor", 
                                        "Splint Armor", "Banded Armor", "Plate Armor", "Shield", "Woods", "Magic User Scroll", "Cleric Scroll", "", "Miscelleanous", "Scroll", "Ring", "Rod",
                                        "Stave", "Miscelleanous Item", "Miscelleanous Item", 
                                        "Miscelleanous Item", "Miscelleanous Item", "Arrows", "Beaker", "Key", "Book", "Clothing", "Wand", "Wand", "Brazier", "Brooch", "Broom", "Purse",
                                        "Candle", "Vial", "Censer", "Chime", "Cloak", "Crystal", "Ring", "Pass", "Cloak", "Ring", "Deck", "Drums", "Dust",
                                        "Eyes", "Figurine", "Flask", "Gauntlets", "Gem", "Girdle", "Helm", "Horn", "Horseshoes", "Incense", "Stone", "Instrument",
                                        "Javelin", "Jewel", "Ointment", "Libram", "Lyre", "Manual", "Mattock", "Maul", "Medallion", "Mirror", "Necklace", "Net",
                                        "Pigment", "Pearl", "Periapt", "Phylactery", "Pipes", "Hole", "Token"}; //, "Robe", "Rope", "Rug", "Saw", "Scarab", "Spade"};
                                    //    "Sphere", "", "Talisman", "Tome", "Trident", "Grimoire", "Well", "Wings", "Vial", "Lantern", "", "Oil", "10 ft. Pole"};
                                    //    "50 ft. Rope", "Iron", "Thf Prickly Tools", "Iron Rations", "Standard Rations", "Holy Symbol", "Holy Water vial"}; 
                                    //    "Unholy Water vial", "Barding", "Dragon", "Lightning", "Saddle", "Small Raft", "Cart", "Wagon"};
            return itemTypes[itemType];
        }
        public string interpret(IList<byte> data)
        {
            StringBuilder a = new StringBuilder();
            
            if (addr > data.Count) { return "file format error"; }
        //    a.Append(" : ");
            if (fieldType == "string")
            {
                var v = new ASCIIEncoding();
                int nameLen = data[addr - 1];
                var str = v.GetString(data.Skip(addr).Take(nameLen).ToArray());
                a.AppendFormat("{0}", str);
            }
            if (fieldType == "name concealed")
            {
                if (data[addr] == 0) a.Append("name fully revealed");
                if ((data[addr] & 1) == 1) a.Append("component 3 hidden. ");
                if ((data[addr] & 2) == 2) a.Append("component 2 hidden. ");
                if ((data[addr] & 4) == 4) a.Append("component 1 hidden. ");

            }

            if (fieldType == "special effect")
            {
                const int clericalScroll = 61;
                const int MUScroll = 62;
                const int Wand = 78;
                if (isItem)
                {
                    const int itemTypeAddr = 0x2E;
                    if (data[itemTypeAddr] == clericalScroll || data[itemTypeAddr] == MUScroll)
                    {
                        if (data[addr] > 0)
                        {
                            if (data[addr] > 56)
                                a.AppendFormat("[{0}]: {1}", data[addr], "unknown spell");
                            else
                                a.AppendFormat("[{0}]: {1}",data[addr], spells[data[addr]-1]);
                            return a.ToString();
                        }
                    }
                    
                }
                if (data[addr] > 0)
                {
                    if (description == "effect 3" && data[addr] > 127)
                    {
                        a.AppendFormat("[{0}]: Activated by readying", data[addr]);
                        return a.ToString();
                    }
                    if (description == "effect 1" && data[0x3E] == 0)
                    {
                        // charges
                            a.AppendFormat("Charges: {0}", data[addr]);
                    }
                    else
                        a.AppendFormat("[{0}]: {1}", data[addr], getSpecialAbilityName(data[addr]));
                }
                else
                    a.Append("0");
            }
            if (fieldType == "name component")
            {
                a.AppendFormat("[{0}]: {1}", data[addr], itemTable[data[addr]]);
            }
            if (fieldType == "item type")
            {
                a.AppendFormat("[{0}]: {1}", data[addr], GetItemTemplateName(data[addr]));
            }
            if (fieldType == "number")
            {
                if (len == 1)
                {
                    a.AppendFormat("{0}", data[addr]);
                }
                if (len == 2)
                {
                    a.AppendFormat("{0}", data[addr] + (data[addr+1] << 8));
                }
            }
            if (fieldType == "60-number")
            {
                a.AppendFormat("{0}", 60-data[addr]);
            }
            if (fieldType == "class")
            {
                a.AppendFormat("{0}", classes[data[addr]]);
            }
            if (fieldType == "race")
            {
                a.AppendFormat("{0}", races[data[addr]]);
            }
            if (fieldType == "alignment")
            {
                a.AppendFormat("{0}", alignments[data[addr]]);
            }
            if (fieldType == "memorized spells")
            {
                for (int i = 0; i < len; i++)
                {
                    if (data[addr+i] >0)
                        a.AppendFormat("{0}, ", spells[data[addr+i]-1]);
                }
            }
            return a.ToString();
        }
    }
    public class FruaCharacter
    {
        private const int RACE_MONSTER = 6;
        public List<Annotation> annotations;
        public int recordLength;
  //      public List<byte> filedata;

        public FruaCharacter(string formatType = "poolrad")
        {
            Str = new Stat();
            Int = new Stat();
            Wis = new Stat();
            Dex = new Stat();
            Con = new Stat();
            Cha = new Stat();
            Extra = new Stat();
            Levels = new List<LevelDef>();
            if (formatType == "poolrad")
            {
                annotations = PoolradFormat();
                recordLength = 0x11d;
            }
            if (formatType == "item" || formatType == "daxitem")
            {
                annotations = ItemFormat();
                recordLength = 63;
            }
            if (formatType == "item template" )
            {
                annotations = ItemTemplateFormat();
                recordLength = 16;
            }

            if (formatType == "spc" || formatType == "daxspc")
            {
                annotations = EffectFormat();
                recordLength = 9;
            }

        }

        public List<Annotation> getAnnotations()
        {
            return annotations;
        }
        private List<Annotation> ItemTemplateFormat()
        {
            var l = new List<Annotation>();
            l.Add(new Annotation(1, 0x00, "location where worn"));
            l.Add(new Annotation(1, 0x01, "hands required to wield"));
            l.Add(new Annotation(1, 0x02, "number of damage dice vs Large sized enemies"));
            l.Add(new Annotation(1, 0x03, "sides of damage dice vs Large sized enemies"));
            l.Add(new Annotation(1, 0x04, "dice damage bonus vs Large sized enemies"));
            l.Add(new Annotation(1, 0x05, "rate of fire"));
            l.Add(new Annotation(1, 0x06, "protective value"));
            l.Add(new Annotation(1, 0x07, "damage type"));
            l.Add(new Annotation(1, 0x08, "melee weapon flag"));
            l.Add(new Annotation(1, 0x09, "number of damage dice vs Small/Medium sized enemies"));
            l.Add(new Annotation(1, 0x0A, "sides of damage dice vs Small/Medium sized enemies"));
            l.Add(new Annotation(1, 0x0B, "dice damage bonus vs Small/Medium sized enemies"));
            l.Add(new Annotation(1, 0x0C, "range"));
            l.Add(new Annotation(1, 0x0D, "class restrictions"));
            l.Add(new Annotation(1, 0x0E, "ammo type used"));
            l.Add(new Annotation(1, 0x0F, "unknown byte"));
            return l;
        }
            private List<Annotation> ItemFormat()
        {
            var l = new List<Annotation>();
            l.Add(new Annotation(45, 0x01, "item name", "string", false, true));
            l.Add(new Annotation(1, 0x2E, "item type", "item type"));
            l.Add(new Annotation(1, 0x2F, "name component 1", "name component"));
            l.Add(new Annotation(1, 0x30, "name component 2", "name component"));
            l.Add(new Annotation(1, 0x31, "name component 3", "name component"));
            l.Add(new Annotation(1, 0x32, "item bonus"));
            l.Add(new Annotation(1, 0x33, "save bonus"));
            l.Add(new Annotation(1, 0x34, "item is readied"));
            l.Add(new Annotation(1, 0x35, "name components revealed", "name concealed"));
            l.Add(new Annotation(1, 0x36, "cursed"));
            l.Add(new Annotation(2, 0x37, "weight"));
            l.Add(new Annotation(1, 0x39, "stack size"));
            l.Add(new Annotation(2, 0x3A, "item value"));
            l.Add(new Annotation(1, 0x3C, "effect 1", "special effect"));
            l.Add(new Annotation(1, 0x3D, "effect 2", "special effect"));
            l.Add(new Annotation(1, 0x3E, "effect 3", "special effect"));
            foreach (var a in l) { a.isItem = true; }
            return l;
        }
        private List<Annotation> EffectFormat()
        {
            var l = new List<Annotation>();
            l.Add(new Annotation(1, 0x00, "effect code", "special effect"));
            l.Add(new Annotation(2, 0x01, "effect duration (0=permament)"));
            l.Add(new Annotation(1, 0x03, "effect data"));
            l.Add(new Annotation(1, 0x04, "effect table flag"));
            l.Add(new Annotation(4, 0x05, "next effect ptr"));
            return l;

        }
        private List<Annotation> PoolradFormat()
        {
            var l = new List<Annotation>();
            l.Add(new Annotation(15,0x01, "character name", "string", false, true));
            l.Add(new Annotation(1, 0x10 , "strength"));
            l.Add(new Annotation(1, 0x11, "intelligence"));
            l.Add(new Annotation(1, 0x12, "wisdom"));
            l.Add(new Annotation(1, 0x13, "dexterity"));
            l.Add(new Annotation(1, 0x14, "constitution"));
            l.Add(new Annotation(1, 0x15, "charisma"));
            l.Add(new Annotation(1, 0x16, "extraordinary strength"));

            l.Add(new Annotation(22, 0x17, "memorized spells", "memorized spells"));
            l.Add(new Annotation(1, 0x2D, "THAC0", "60-number"));
            l.Add(new Annotation(1, 0x2E, "race", "race"));
            l.Add(new Annotation(1, 0x2F, "class", "class"));
            l.Add(new Annotation(2, 0x30, "age"));
            l.Add(new Annotation(1, 0x32, "maximum hp"));
            l.Add(new Annotation(1, 0x6B, "highest character level"));
            l.Add(new Annotation(1, 0x6C, "creature size"));

            l.Add(new Annotation(1, 0x6D, "save vs paralyzation, poison or death"));
            l.Add(new Annotation(1, 0x6E, "save vs petrification or polymorph"));
            l.Add(new Annotation(1, 0x6F, "save vs rod staff or wand"));
            l.Add(new Annotation(1, 0x70, "save vs breath weapon"));
            l.Add(new Annotation(1, 0x71, "save vs spell"));

            l.Add(new Annotation(1, 0x72, "base movement speed"));
            l.Add(new Annotation(1, 0x73, "hit dice"));
            l.Add(new Annotation(1, 0x74, "drained levels"));
            l.Add(new Annotation(1, 0x75, "drained hp"));
            l.Add(new Annotation(1, 0x76, "undead resistance vs turning"));
            l.Add(new Annotation(1, 0x77, "thief skills: pick pockets"));
            l.Add(new Annotation(1, 0x78, "thief skills: open locks"));
            l.Add(new Annotation(1, 0x79, "thief skills: find/remove traps"));
            l.Add(new Annotation(1, 0x7A, "thief skills: move silently"));
            l.Add(new Annotation(1, 0x7B, "thief skills: hide in shadows"));
            l.Add(new Annotation(1, 0x7C, "thief skills: hear noise"));
            l.Add(new Annotation(1, 0x7D, "thief skills: climb walls"));
            l.Add(new Annotation(1, 0x7E, "thief skills: read languages"));
            l.Add(new Annotation(4, 0x7F, "effects ptr"));

            l.Add(new Annotation(1, 0x83, "unknown byte 1"));
            l.Add(new Annotation(1, 0x84, "NPC Status"));
            l.Add(new Annotation(1, 0x85, "NPC treasure share"));
            l.Add(new Annotation(1, 0x86, "unknown byte 2"));
            l.Add(new Annotation(1, 0x87, "unknown byte 3"));
            l.Add(new Annotation(1, 0x88, "unknown byte 4"));
            l.Add(new Annotation(2, 0x89, "copper"));
            l.Add(new Annotation(2, 0x8B, "silver"));
            l.Add(new Annotation(2, 0x8D, "electrum"));
            l.Add(new Annotation(2, 0x8E, "gold"));
            l.Add(new Annotation(2, 0x90, "platinum"));
            l.Add(new Annotation(2, 0x92, "gems"));
            l.Add(new Annotation(2, 0x94, "jewels"));
            l.Add(new Annotation(1, 0x96, "cleric levels"));
            l.Add(new Annotation(1, 0x97, "druid levels"));
            l.Add(new Annotation(1, 0x98, "fighter levels"));
            l.Add(new Annotation(1, 0x99, "paladin levels"));
            l.Add(new Annotation(1, 0x9A, "ranger levels"));
            l.Add(new Annotation(1, 0x9B, "magic user levels"));
            l.Add(new Annotation(1, 0x9C, "thief levels"));
            l.Add(new Annotation(1, 0x9D, "monk levels"));
            l.Add(new Annotation(1, 0x9E, "gender"));
            l.Add(new Annotation(1, 0x9F, "monster type"));
            l.Add(new Annotation(1, 0xA0, "alignment", "alignment"));
            l.Add(new Annotation(1, 0xA1, "number of primary attacks *2"));
            l.Add(new Annotation(1, 0xA2, "number of secondary attacks *2"));
            l.Add(new Annotation(1, 0xA3, "primary attack damage dice number"));
            l.Add(new Annotation(1, 0xA4, "secondary attack damage dice number"));
            l.Add(new Annotation(1, 0xA5, "primary attack damage dice sides"));
            l.Add(new Annotation(1, 0xA6, "secondary attack damage dice sides"));
            l.Add(new Annotation(1, 0xA7, "primary attack damage dice modifier"));
            l.Add(new Annotation(1, 0xA8, "secondary attack damage dice modifier"));
            l.Add(new Annotation(1, 0xA9, "armor class", "60-number"));
        l.Add(new Annotation(1, 0xAA, "strength bonus allowed"));
        l.Add(new Annotation(1, 0xAB, "combat icon"));
        l.Add(new Annotation(4, 0xAC, "experience points"));
        l.Add(new Annotation(1, 0xB0, "class item usage flags"));
        l.Add(new Annotation(1, 0xB1, "hit points rolled"));
        l.Add(new Annotation(1, 0xB2, "cleric 1st level spell slots"));
        l.Add(new Annotation(1, 0xB3, "cleric 2nd level spell slots"));
        l.Add(new Annotation(1, 0xB4, "cleric 3rd level spell slots"));
        l.Add(new Annotation(1, 0xB5, "magic user 1st level spell slots"));
        l.Add(new Annotation(1, 0xB6, "magic user 2nd level spell slots"));
        l.Add(new Annotation(1, 0xB7, "magic user 3rd level spell slots"));
        l.Add(new Annotation(2, 0xB8, "base XP for defeating this monster"));
        l.Add(new Annotation(1, 0xBA, "bonus XP per monster HP for defeating"));
        l.Add(new Annotation(1, 0xBB, "head portrait"));
        l.Add(new Annotation(1, 0xBC, "body portrait"));
        l.Add(new Annotation(1, 0xBD, "head icon"));
        l.Add(new Annotation(1, 0xBE, "weapon icon"));
        l.Add(new Annotation(1, 0xBF, "unknown byte 5"));
        l.Add(new Annotation(1, 0xC0, "icon size"));
        l.Add(new Annotation(1, 0xC1, "body color"));
        l.Add(new Annotation(1, 0xC2, "arm color"));
        l.Add(new Annotation(1, 0xC3, "leg color"));
        l.Add(new Annotation(1, 0xC4, "head color"));
        l.Add(new Annotation(1, 0xC5, "shield color"));
        l.Add(new Annotation(1, 0xC6, "weapon color"));
        l.Add(new Annotation(1, 0xC7, "special vulnerabilty flags"));
        l.Add(new Annotation(56, 0xC8, "items"));
        l.Add(new Annotation(1, 0x100, "hands used"));
        l.Add(new Annotation(1, 0x101, "unknown byte 6" ));
        l.Add(new Annotation(2, 0x102, "encumbrance"));
        l.Add(new Annotation(1, 0x104, "unknown byte 7"));
        l.Add(new Annotation(1, 0x105, "unknown byte 8"));
        l.Add(new Annotation(1, 0x106, "unknown byte 9"));
        l.Add(new Annotation(4, 0x107, "actions"));
        l.Add(new Annotation(1, 0x10B, "unknown byte 10" ));
        l.Add(new Annotation(1, 0x10C, "health status" ));
        l.Add(new Annotation(1, 0x10D, "in combat"  ));
        l.Add(new Annotation(1, 0x10E, "side in combat" ));
        l.Add(new Annotation(1, 0x10F, "quick fight flag" ));
        l.Add(new Annotation(1, 0x110, "current THAC0", "60-number"));
        l.Add(new Annotation(1, 0x111, "current AC", "60-number"));
        l.Add(new Annotation(1, 0x112, "AC for rear attacks", "60-number"));
        l.Add(new Annotation(1, 0x113, "primary attacks left"));
        l.Add(new Annotation(1, 0x114, "secondary attacks left"));
        l.Add(new Annotation(1, 0x115, "current primary attack damage dice number"));
        l.Add(new Annotation(1, 0x116, "current secondary attack damage dice number"));
        l.Add(new Annotation(1, 0x117, "current primary attack damage dice sides"));
        l.Add(new Annotation(1, 0x118, "current secondary attack dice sides"));
        l.Add(new Annotation(1, 0x119, "current primary attack bonus"));
        l.Add(new Annotation(1, 0x11A, "current secondary attack bonus"));
        l.Add(new Annotation(1, 0x11B, "current HP"));
        l.Add(new Annotation(1, 0x11C, "current movement"));
            return l;
        }
        public byte SixtyMinusBaseAC { get; set; }
        public uint XpTotal { get; set; }
        public uint XpTotalBeforeDraining { get; set; }
        public ushort Platinum { get; set; }
        public ushort Gems { get; set; }
        public ushort Jewelry { get; set; }
        public ushort Age { get; set; }
        public ushort XpForKilling { get; set; }
        public ushort CurrentEncumbrance { get; set; }
        public byte Race { get; set; }
        public byte Class { get; set; }
        public byte UndeadType { get; set; }
        public byte Gender { get; set; }
        public byte Alignment { get; set; }
        public byte Status { get; set; }
        public byte CombatMode { get; set; }
        public string Name { get; set; }
        public Stat Str { get; set; }
        public Stat Int { get; set; }
        public Stat Wis { get; set; }
        public Stat Dex { get; set; }
        public Stat Con { get; set; }
        public Stat Cha { get; set; }
        public Stat Extra { get; set; }
        public byte SixtyMinusBaseTHAC0 { get; set; }
        public byte CureDiseaseFlag { get; set; }
        public byte MaximumHP { get; set; }
        public byte SizeIndicator { get; set; }
        public byte SaveVsParalysis { get; set; }
        public byte SaveVsPetrify { get; set; }
        public byte SaveVsRod { get; set; }
        public byte SaveVsBreath { get; set; }
        public byte SaveVsSpell { get; set; }
        public byte BaseMovementRate { get; set; }
        public byte CurrentLevel { get; set; }
        public byte FormerLevel { get; set; }
        public byte PickPocket { get; set; }
        public byte OpenLock { get; set; }
        public byte FindTraps { get; set; }
        public byte MoveSilently { get; set; }
        public byte HearNoise { get; set; }
        public byte ClimbWalls { get; set; }
        public byte ReadLanguages { get; set; }
        public byte HideInShadows { get; set; }
        public byte WarriorLevel { get; set; }
        public IList<LevelDef> Levels { get; private set; }
        public ushort NumberOfAttacksPerTwoRounds { get; set; }
        public ushort DamageDice { get; set; }
        public ushort TypeOfDamageDiceRolled { get; set; }
        public ushort DamageBonus { get; set; }
        public byte PreDrainHPTotal { get; set; }
        public byte ItemUseIndicator { get; set; }
        public byte HPAsRolled { get; set; }
        public byte IconId { get; set; }
        public byte MarchOrder { get; set; }
        public byte SpecialAbilityFlags { get; set; }
        public byte MoreSpecialAbilityFlags { get; set; }
        public byte NumberOfItemBundlesCarried { get; set; }
        public byte NumberOfHandsFull { get; set; }
        public byte TotalMagicProtectiveBonus { get; set; }
        public byte MagicResistance { get; set; }
        public byte TrainForLevel { get; set; }
        public byte[] MemorizedSpells { get; set; }
        public byte[] SpellBook { get; set; }
        public byte[] ClericSpells { get; set; }
        public byte[] DruidSpells { get; set; }
        public byte[] MageSpells { get; set; }
        public byte ActiveFlag { get; set; }
        public byte AdjustedTHAC0 { get; set; }
        public byte ArmorClass { get; set; }
        public ushort ArmedDamageBonus { get; set; }
        public byte CurrentHp { get; set; }
        public byte MovementRate { get; set; }
        public byte MonsterSlot { get; set; }
        public byte[] ItemsCarriedByMonster { get; set; }
        public byte[] QualityCodeForEachItemCarried { get; set; }
        public byte[] SpecialAbilityCodes { get; set; }

        public override string ToString()
        {

            // from SPELLLIST.TXT TODO move to config file
            var spellList = new[]
                {
                    "",
                    "C: Bless",
                    "C: Curse",
                    "C: Cure Light Wounds",
                    "C: Cause Light Wounds",
                    "C: Detect Magic",
                    "C: Protection from Evil",
                    "C: Protection from Good",
                    "C: Resist Cold",
                    "Burning Hands",
                    "Charm Person",
                    "Detect Magic",
                    "Enlarge",
                    "Reduce",
                    "Friends",
                    "Magic Missile",
                    "Prot. from Evil",
                    "Prot. from Good",
                    "Read Magic",
                    "Shield",
                    "Shocking Grasp",
                    "Sleep",
                    "C: Find Traps",
                    "C: Hold Person",
                    "C: Resist Fire",
                    "C: Silence 15' Radius",
                    "C: Slow Poison",
                    "C: Snake Charm",
                    "C: Spiritual Hammer",
                    "Detect Invisibility",
                    "Invisibility",
                    "Knock",
                    "Mirror Image",
                    "Ray of Enfeeblement",
                    "Stinking Cloud",
                    "Strength",
                    "C: Heal",
                    "C: Cure Blindness",
                    "C: Cause Blindness",
                    "C: Cure Disease",
                    "C: Cause Disease",
                    "C: Dispel Magic",
                    "C: Prayer",
                    "C: Remove Curse",
                    "C: Bestow Curse",
                    "Blink",
                    "Dispel Magic",
                    "Fireball",
                    "Haste",
                    "Hold Person",
                    "Invisibility 10' Radius",
                    "Lightning Bolt",
                    "Prot. from Evil",
                    "Prot. from Good",
                    "Prot. from Norm Miss",
                    "Slow",
                    "C: Harm",
                    "(Potion of Speed)",
                    "C: Cure Serious Wounds",
                    "(Potion of Gt Strength)",
                    "(Javelin of Lightning)",
                    "(Wand of Paralyzation)",
                    "(Potion of Healing)",
                    "(Elixir of Youth)",
                    "(Necklace of Missiles)",
                    "(Wand of Magic Miss)",
                    "C: Cause Serious Wounds",
                    "C: Neutralize Poison",
                    "C: Poison",
                    "C: Pro Evil 10' Rad",
                    "C: Sticks to Snakes",
                    "C: Cure Crit Wounds",
                    "C: Cause Crit Wounds",
                    "C: Dispel Evil",
                    "C: Flame Strike",
                    "C: Raise Dead",
                    "C: Slay Living",
                    "D: Detect Magic",
                    "D: Entangle",
                    "D: Faerie Fire",
                    "D: Invis to Animals",
                    "Charm Monsters",
                    "Confusion",
                    "Dimension Door",
                    "Fear",
                    "Fire Shield",
                    "Fumble",
                    "Ice Storm",
                    "Min Globe of Invuln",
                    "Remove Curse",
                    "D: Barkskin",
                    "Cloudkill",
                    "Cone of Cold",
                    "Feeblemind",
                    "Hold Monsters",
                    "(Scroll Pro from Drag)",
                    "D: Charm Person or Mam",
                    "(Potion of Invis)",
                    "D: Cure Light Wounds",
                    "(Potion Extra Healing)",
                    "Bestow Curse",
                    "C: Blade Barrier",
                    "C: Restoration",
                    "C: Energy Drain",
                    "C: Destruction",
                    "C: Resurrection",
                    "D: Cure Disease",
                    "D: Neutralize Poison",
                    "D: Hold Animal",
                    "D: Pro from Fire",
                    "Death",
                    "Disintegrate",
                    "Globe of Invuln",
                    "Stone to Flesh",
                    "Flesh to Stone",
                    "Delayed Blast Fireball",
                    "Mass Invisibility",
                    "Power Word Stun",
                    "Fire Touch",
                    "Iron Skin",
                    "Mass Charm",
                    "Otto's Irr Dance",
                    "Mind Blank",
                    "Power Word Blind",
                    "Meteor Swarm",
                    "Power Word Kill",
                    "Monster Summoning",
                };
            var raceNames = new[]
                {
                    "Elf",
                    "Half-Elf",
                    "Dwarf",
                    "Gnome",
                    "Halfling",
                    "Human",
                    "Monster"
                };

            var classNames = new[]
                {
                    "Cleric",
                    "Knight",
                    "Fighter",
                    "Paladin",
                    "Ranger",
                    "Magic-User",
                    "Thief",
                    "Monk",
                    "Cleric/Fighter",
                    "Cleric/Fighter/Magic-User",
                    "Cleric/Ranger",
                    "Cleric/Magic-User",
                    "Cleric/Thief",
                    "Fighter/Magic-User",
                    "Fighter/Thief",
                    "Fighter/Magic-User/Thief",
                    "Magic-User/Theif"
                };
            var undeadTypes = new[]
                {
                    "Not Undead",
                    "Skeleton",
                    "Zombie",
                    "Ghoul",
                    "Shadow",
                    "Wight",
                    "Ghast",
                    "Wraith",
                    "Mummy",
                    "Spectre",
                    "Vampire",
                    "Ghost",
                    "Lich",
                    "Special"
                };
            var genderTypes = new[] {"Male", "Female"};
            var alignmentTypes = new[]
                {
                    "Lawful Good",
                    "Lawful Neutral",
                    "Lawful Evil",
                    "Neutral Good",
                    "True Neutral",
                    "Neutral Evil",
                    "Chaotic Good",
                    "Chaotic Neutral",
                    "Chaotic Evil"
                };
            var statusTypes = new[]
                {
                    "Okay",
                    "Animated",
                    "Temp Gone",
                    "Running",
                    "Unconcious",
                    "Dying",
                    "Dead",
                    "Petrified",
                    "Gone",
                    "Awaiting Summons"
                };
            var combatModeTypes = new[]
                {
                    "PC/NPC",
                    "Monster"
                };
            var ch = new StringBuilder();
          /*  foreach (var a in annotations)
            {
                ch.AppendLine(a.interpret(filedata));
            }*/
            ch.AppendLine("oh hai");
            return ch.ToString();

            ch.AppendFormat("Name: {0}\r\n", Name);
            ch.AppendFormat("Race: {0}\r\n", raceNames[Race]);
            ch.AppendFormat("Gender: {0}\r\n", genderTypes[Gender]);
            ch.AppendFormat("Alignment: {0}\r\n", alignmentTypes[Alignment]);
            ch.AppendFormat("Class: {0}\r\n", classNames[Class]);
            ch.AppendFormat("Hit Points: {0}\r\n", CurrentHp);
            ch.AppendFormat("Age: {0}\r\n", Age);
            ch.AppendFormat("Magic Resistance: {0}\r\n", MagicResistance);
            ch.AppendFormat("Level: {0}\r\n", CurrentLevel);
            ch.AppendLine();

            // stats
            if (Extra.Raw > 0)
            {
                ch.AppendFormat("Strength: {0} % {1}\r\n", Str.Raw, Extra.Raw);
            }
            else
            {
                ch.AppendFormat("Strength: {0}\r\n", Str.Raw);
            }
            ch.AppendFormat("Intelligence: {0}\r\n", Int.Raw);
            ch.AppendFormat("Wisdom: {0}\r\n", Wis.Raw);
            ch.AppendFormat("Dexterity: {0}\r\n", Dex.Raw);
            ch.AppendFormat("Constitution: {0}\r\n", Con.Raw);
            ch.AppendFormat("Charisma: {0}\r\n", Cha.Raw);
            ch.AppendLine();

            // levels
            ch.AppendFormat("Cleric Level: {0}\r\n", Levels[1].Cleric);
            ch.AppendFormat("Fighter Level: {0}\r\n", Levels[1].Fighter);
            ch.AppendFormat("Paladin Level: {0}\r\n", Levels[1].Paladin);
            ch.AppendFormat("Ranger Level: {0}\r\n", Levels[1].Ranger);
            ch.AppendFormat("Mage Level: {0}\r\n", Levels[1].Mage);
            ch.AppendFormat("Thief Level: {0}\r\n", Levels[1].Thief);
            ch.AppendLine();

            ch.AppendFormat("The party gets {0} experience points when the monster is killed.\r\n", XpForKilling);
            ch.AppendLine();

            ch.AppendFormat("Undead Type: {0}\r\n", undeadTypes[UndeadType]);
            ch.AppendLine();

            // more combat options
            ch.AppendFormat("Base THAC0: {0}\r\n", 60 - SixtyMinusBaseTHAC0);
            ch.AppendFormat("Base AC: {0}\r\n", 60 - SixtyMinusBaseAC);
            ch.AppendFormat("Base Movement: {0}\r\n", BaseMovementRate);
            ch.AppendLine();

            if (Race == RACE_MONSTER)
            {
                ch.AppendLine("The Monster Has");
            }
            ch.AppendFormat("Platinum: {0}\r\n", Platinum);
            ch.AppendFormat("Gems: {0}\r\n", Gems);
            ch.AppendFormat("Jewelry: {0}\r\n", Jewelry);

            if (ItemsCarriedByMonster != null)
            {
                ch.AppendLine("Items Carried by Monster:");
                foreach (var b in ItemsCarriedByMonster)
                {
                    ch.AppendFormat("{0} ", b);
                }
                ch.AppendLine();
            }
            ch.AppendLine();

            ch.Append("Memorized Spells:\r\n");
            var spellCount = MemorizedSpells.Where(x => x != 0).Count();
            if (spellCount == 0)
            {
                ch.AppendLine("N/A");
            }
            else
            {
                foreach (var spell in MemorizedSpells.Where(spell => spell != 0))
                {
                    ch.AppendLine(spellList[spell]);
                }
            }
            ch.AppendLine();

            // TODO show breakdown of this
            ch.Append("Spell Book:\r\n");
            var spelllist = SpellBook.Where(spell => spell != 0).Aggregate(string.Empty, (current, spell) => current + string.Format("{0} ", spell));
            ch.Append(string.IsNullOrEmpty(spelllist.Trim()) ? "N/A" : spelllist);
            ch.AppendLine();
            ch.AppendLine();

            // TODO show breakdown of this
            ch.Append("Number of Cleric Spells:\r\n");
            foreach (byte spell in ClericSpells)
            {
                ch.AppendFormat("{0} ", spell);
            }
            ch.AppendLine();
            ch.AppendLine();

            // TODO show breakdown of this
            ch.Append("Number of Druid Spells:\r\n");
            foreach (byte spell in DruidSpells)
            {
                ch.AppendFormat("{0} ", spell);
            }
            ch.AppendLine();
            ch.AppendLine();

            // TODO show breakdown of this
            ch.Append("Number of Mage Spells:\r\n");
            foreach (byte spell in MageSpells)
            {
                ch.AppendFormat("{0} ", spell);
            }
            ch.AppendLine();
            ch.AppendLine();

            // everything else
            ch.AppendFormat("XP Total (Current): {0}\r\n", XpTotal);
            ch.AppendFormat("XP Total (Before Draining): {0}\r\n", XpTotalBeforeDraining);
            ch.AppendFormat("Maximum Hit Points: {0}\r\n", MaximumHP);
            ch.AppendFormat("Pre-Drain Hit Point Total: {0}\r\n", PreDrainHPTotal);
            ch.AppendFormat("Hit Points (as rolled): {0}\r\n", HPAsRolled);
            ch.AppendFormat("Level in Former Class: {0}\r\n", FormerLevel);
            ch.AppendFormat("Warrior Level: {0}\r\n", WarriorLevel);
            ch.AppendFormat("Current Encumbrance in Coins: {0}\r\n", CurrentEncumbrance);
            ch.AppendFormat("Status: {0}\r\n", statusTypes[Status]);
            ch.AppendFormat("Combat Mode: {0}\r\n", combatModeTypes[CombatMode]);
            ch.AppendFormat("Size Indicator: {0}\r\n", SizeIndicator);
            ch.AppendFormat("Movement Rate: {0}\r\n", MovementRate);
            ch.AppendFormat("Icon ID: {0}\r\n", IconId);
            // TODO see cchform.txt for details on breakdown of this bit
            ch.AppendFormat("March Order: {0}\r\n", MarchOrder);
            ch.AppendFormat("Number of Item Bundles Carried: {0}\r\n", NumberOfItemBundlesCarried);
            ch.AppendFormat("Number of Hands Full: {0}\r\n", NumberOfHandsFull);
            ch.AppendFormat("Train for Level?: {0}\r\n", TrainForLevel);
            ch.AppendFormat("Pick Pocket: {0}\r\n", PickPocket);
            ch.AppendFormat("Open Lock: {0}\r\n", OpenLock);
            ch.AppendFormat("Find/Remove Traps: {0}\r\n", FindTraps);
            ch.AppendFormat("Move Silently: {0}\r\n", MoveSilently);
            ch.AppendFormat("Hide in Shadows: {0}\r\n", HideInShadows);
            ch.AppendFormat("Hear Noise: {0}\r\n", HearNoise);
            ch.AppendFormat("Climb Walls: {0}\r\n", ClimbWalls);
            ch.AppendFormat("Read Languages: {0}\r\n", ReadLanguages);
            ch.AppendFormat("Adjusted THAC0: {0}\r\n", AdjustedTHAC0);
            ch.AppendFormat("AC: {0}\r\n", ArmorClass);
            ch.AppendFormat("Number of attacks per two rounds (1st/2nd attacks, unarmed): {0}\r\n",
                            NumberOfAttacksPerTwoRounds);
            ch.AppendFormat("Dice of Damage: {0}\r\n", DamageDice);
            ch.AppendFormat("Type of Damage Die Rolled: {0}\r\n", TypeOfDamageDiceRolled);
            ch.AppendFormat("Damage Bonus: {0}\r\n", DamageBonus);
            ch.AppendFormat("Armed Damage Bonus: {0}\r\n", ArmedDamageBonus);
            ch.AppendFormat("Total Magic Protective Bonus: {0}\r\n", TotalMagicProtectiveBonus);
            ch.AppendFormat("Save vs. paralysis/poison/death magic: {0}\r\n", SaveVsParalysis);
            ch.AppendFormat("Save vs. petrification/polymorph: {0}\r\n", SaveVsPetrify);
            ch.AppendFormat("Save vs. rod/staff/wand: {0}\r\n", SaveVsRod);
            ch.AppendFormat("Save vs. breath weapon: {0}\r\n", SaveVsBreath);
            ch.AppendFormat("Save vs. spell: {0}\r\n", SaveVsSpell);
            ch.AppendFormat("Monster Slot: {0}\r\n", MonsterSlot);
            // TODO see cchform.txt for details on breakdown of this bit
            ch.AppendFormat("Cure Disease Flag: {0}\r\n", CureDiseaseFlag);
            // TODO see cchform.txt for details on breakdown of this bit
            ch.AppendFormat("Item Use Indicator: {0}\r\n", ItemUseIndicator);
            // TODO see cchform.txt for details on breakdown of this bit
            ch.AppendFormat("Special Ability Flags: {0}\r\n", SpecialAbilityFlags);
            // TODO see cchform.txt for details on breakdown of this bit
            ch.AppendFormat("More Special Ability Flags: {0}\r\n", MoreSpecialAbilityFlags);
            // TODO see cchform.txt for details on breakdown of this bit
            ch.AppendFormat("Active Flag: {0}\r\n", ActiveFlag);
            ch.AppendLine();

            var levelDesc = new[] { "Pre-Draining", "Current", "Before Changing Class; Human only" };
            for (int lev = 0; lev < 3; lev++)
            {
                ch.AppendFormat("Cleric Level ({1}): {0}\r\n", Levels[lev].Cleric, levelDesc[lev]);
                ch.AppendFormat("Knight Level ({1}): {0}\r\n", Levels[lev].Knight, levelDesc[lev]);
                ch.AppendFormat("Fighter Level ({1}): {0}\r\n", Levels[lev].Fighter, levelDesc[lev]);
                ch.AppendFormat("Paladin Level ({1}): {0}\r\n", Levels[lev].Paladin, levelDesc[lev]);
                ch.AppendFormat("Ranger Level ({1}): {0}\r\n", Levels[lev].Ranger, levelDesc[lev]);
                ch.AppendFormat("Mage Level ({1}): {0}\r\n", Levels[lev].Mage, levelDesc[lev]);
                ch.AppendFormat("Thief Level ({1}): {0}\r\n", Levels[lev].Thief, levelDesc[lev]);
            }
            ch.AppendLine();

            if (QualityCodeForEachItemCarried != null)
            {
                ch.AppendLine("Quality Code for each item carried:");
                foreach (var b in QualityCodeForEachItemCarried)
                {
                    ch.AppendFormat("{0} ", b);
                }
                ch.AppendLine();
                ch.AppendLine();
            }

            if (SpecialAbilityCodes != null)
            {
                ch.AppendLine("Special ability codes:");
                foreach (var b in SpecialAbilityCodes)
                {
                    ch.AppendFormat("{0} ", b);
                }
                ch.AppendLine();
                ch.AppendLine();
            }

            return ch.ToString();
        }

        public class LevelDef
        {
            public byte Cleric;
            public byte Fighter;
            public byte Knight;
            public byte Mage;
            public byte Paladin;
            public byte Ranger;
            public byte Thief;
        }

        public class Stat
        {
            public byte Modified;
            public byte Raw;

            public override string ToString()
            {
                return string.Format("{0}/{1}", Raw, Modified);
            }
        }
    }
}