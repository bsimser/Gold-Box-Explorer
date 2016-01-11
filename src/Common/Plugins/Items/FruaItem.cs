namespace GoldBoxExplorer.Lib.Plugins.Items
{
    public class FruaItem
    {
        public static readonly string[] NameParts = {
                                                        "", "Battle Axe", "Hand Axe", "Club", "Dagger", "Dart", "Hammer", "Javelin", 
                                                        "Mace", "Morning Star", "Military Pick", "Awl Pike", "Bolt", "Scimitar", "Spear",
                                                        "Quarter Staff", "Bastard Sword", "Broad Sword", "Long Sword", "Short Sword", 
                                                        "Two-Handed Sword", "Trident", "Composite Long Bow", "Composite Short Bow",
                                                        "Long Bow", "Short Bow", "Fine", "Light Crossbow", "Sling", "Staff", "Arrow",
                                                        "Leather", "Ring", "Scale", "Chain", "Banded", "Plate", "Shield", "Cleric",
                                                        "Scroll", "Mage", "Helm", "Belt", "Robe", "Cloak", "Boots", "Ring", "Mail",
                                                        "Armor", "of Prot", "Bracers", "Wand", "Elixir", "Potion", "Youth", "Ruby",
                                                        "Boulder", "Dragon Breath", "Displacement", "Eyes", "Drow", "Elfin Chain",
                                                        "Ice Storm", "Sapphire", "Emerald", "Wizardry", "Hornet's Nest", "Fire Resistance",
                                                        "Stone", "Good Luck", "Flail", "Halberd", "Gauntlets", "Periapt", "Health",
                                                        "Cursed", "Blessed", "Bundle of", "Ogre Power", "Girdle", "Giant Strength",
                                                        "Mirror", "Necklace", "Dragon", "vs Giants", "vorpal", "cold resistance",
                                                        "Diamond", "Lightning", "Fireballs", "of", "Vulnerability", "Speed",
                                                        "Silver", "Extra", "Healing", "Charming", "Fear", "Magic Missiles", "Missiles",
                                                        "1 Spell", "2 Spells", "3 Spells", "Paralyzation", "Invisibility",
                                                        "Cute Yellow Canary", "AC 10", "AC 6", "AC 4", "AC 3", "AC 2", 
                                                        "+1", "+2", "+3", "+4", "+5", "-1", "-2", "-3",
                                                        "Electric Immunity", "Gaze Resistance", "Spiritual", "Gem", "Jewelry", "blinking", "from evil",
                                                        "Hoopak", "Dragonlance", "Striking", "Disruption", "Dragon Slayer", "Foethumper",
                                                        "Solamnic", "Petrification", "Free Action", "Footman's", "Red Mage", "White Mage",
                                                        "Quarrel", "Cler", "MU Scroll", "Amulet", "Bag", "Solamnic Plate", "Proof vs Poision",
                                                        "Elixir of Youth", "Eyes of Charming", "vs Reptiles", "Frost Brand", "Berserker", "Olin's",
                                                        "White", "Red", "Blank",
                                                    };

        public FruaItem(byte firstNameCode, byte secondNameCode, byte thirdNameCode)
        {
            _firstNameCode = firstNameCode;
            _secondNameCode = secondNameCode;
            _thirdNameCode = thirdNameCode;
        }

        public string Name
        {
            get
            {
                // TODO reveal name based on Identified property (see ITEM.TXT)
                var first = NameParts[_firstNameCode];
                var second = NameParts[_secondNameCode];
                var third = NameParts[_thirdNameCode];
                return string.Format("{0} {1} {2}", first, second, third);
            }
        }

        public byte Pointer { get; set; }
        public int Encumbrance { get; set; }
        public int Price { get; set; }
        public int MagicBonus { get; set; }
        public int NumberOfItemsInBundle { get; set; }
        public int Charges { get; set; }
        public int SecondaryCode { get; set; }
        public bool IsReady { get; set; }
        public int Identified { get; set; }
        public bool IsCursed { get; set; }
        public int MagicalEffectCode { get; set; }
        public string SpecialCode { get; set; }
        public int Location { get; set; }
        public int Hands { get; set; }
        public string MeleeClass { get; set; }
        public string WeaponClass { get; set; }
        public int Rate { get; set; }
        public int Protection { get; set; }
        public int MissileType { get; set; }
        public int Range { get; set; }
        public int ClassUsage { get; set; }

        public FruaItemDamageDice DamageVsLarge { get; set; }
        public FruaItemDamageDice DamageVsMedium { get; set; }

        private readonly byte _firstNameCode;
        private readonly byte _secondNameCode;
        private readonly byte _thirdNameCode;

        public string GetLocation()
        {
            string[] locations =
                {
                    "Weapon Hand",
                    "Shield Hand",
                    "Body",
                    "Hands",
                    "Head",
                    "Waist",
                    "Body",
                    "Back",
                    "Feet",
                    "Fingers",
                    "Ammo Quiver",
                    "Protection",
                    "Mage",
                    "Cleric",
                };
            return locations[Location];
        }
    }
}