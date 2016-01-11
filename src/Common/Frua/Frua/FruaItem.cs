namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaItem
    {
        public static readonly string[] ItemNames = {
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

        public FruaItem(string firstName, string secondName, string thirdName)
        {
            _firstName = firstName;
            _secondName = secondName;
            _thirdName = thirdName;
        }

        public string Name
        {
            get { return string.Format("{0} {1} {2}", _firstName, _secondName, _thirdName); }
        }

        public override string ToString()
        {
            return Name;
        }

        public int Encumbrance { get; set; }
        public int Price { get; set; }
        public int MagicBonus { get; set; }
        public int Items { private get; set; }
        public int Charges { get; set; }
        public int SecondaryCode { get; set; }
        public bool IsReady { get; set; }
        public int Identified { get; set; }
        public bool IsCursed { get; set; }
        public int MagicalEffectCode { get; set; }
        public int SpecialCode { get; set; }
        public int Location { get; set; }
        public int Hands { get; set; }
        public int MeleeClass { get; set; }
        public int WeaponClass { get; set; }
        public int Rate { get; set; }
        public int Protection { get; set; }
        public int MissileType { get; set; }
        public int Range { get; set; }
        public int ClassUsage { get; set; }

        public FruaItemDamageDice DamageVsLarge { get; set; }
        public FruaItemDamageDice DamageVsMedium { get; set; }

        private string _firstName { get; set; }
        private string _secondName { get; set; }
        private string _thirdName { get; set; }

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

        public string GetDamageDice(FruaItemDamageDice dice)
        {
            if (dice.Number > 0)
            {
                var format = string.Format("{0}d{1}", dice.Number, dice.Type);
                if (dice.Bonus > 0)
                    format += string.Format("+{0}", dice.Bonus);
                return format;
            }
            
            return "N/A";
        }
    }
}