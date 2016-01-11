using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaCharacter
    {
        public FruaCharacter()
        {
            Str = new Stat();
            Int = new Stat();
            Wis = new Stat();
            Dex = new Stat();
            Con = new Stat();
            Cha = new Stat();
            Extra = new Stat();
            Levels = new List<LevelDef>();
        }

        public byte SixtyMinusBaseAC { get; set; }
        public uint XpTotal { get; set; }
        public uint XpTotalBeforeDraining { get; set; }
        public ushort PlatinumPieces { get; set; }
        public ushort Gems { get; set; }
        public ushort Jewels { get; set; }
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

        public override string ToString()
        {
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

            ch.AppendFormat("Name: {0}\r\n", Name);
            ch.AppendFormat("Race: {0}\r\n", raceNames[Race]);
            ch.AppendFormat("Class: {0}\r\n", classNames[Class]);
            ch.AppendFormat("Gender: {0}\r\n", genderTypes[Gender]);
            ch.AppendFormat("Alignment: {0}\r\n", alignmentTypes[Alignment]);
            ch.AppendFormat("Age: {0}\r\n", Age);
            ch.AppendLine();

            ch.AppendFormat("Current Level (in highest active class): {0}\r\n", CurrentLevel);
            ch.AppendFormat("Level in Former Class: {0}\r\n", FormerLevel);
            ch.AppendFormat("Warrior Level: {0}\r\n", WarriorLevel);
            ch.AppendLine();

            var levelDesc = new[] {"Pre-Draining", "Current", "Before Changing Class; Human only"};
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

            ch.AppendFormat("XP Total (Current): {0}\r\n", XpTotal);
            ch.AppendFormat("XP Total (Before Draining): {0}\r\n", XpTotalBeforeDraining);
            ch.AppendFormat("XP for Killing: {0}\r\n", XpForKilling);
            ch.AppendLine();

            ch.AppendFormat("Platinum Pieces: {0}\r\n", PlatinumPieces);
            ch.AppendFormat("Gems: {0}\r\n", Gems);
            ch.AppendFormat("Jewels: {0}\r\n", Jewels);
            ch.AppendLine();

            ch.AppendFormat("Current Encumbrance in Coins: {0}\r\n", CurrentEncumbrance);
            ch.AppendFormat("Undead Type: {0}\r\n", undeadTypes[UndeadType]);
            ch.AppendFormat("Status: {0}\r\n", statusTypes[Status]);
            ch.AppendFormat("Combat Mode: {0}\r\n", combatModeTypes[CombatMode]);
            ch.AppendFormat("Size Indicator: {0}\r\n", SizeIndicator);
            ch.AppendFormat("Base Movement Rate: {0}\r\n", BaseMovementRate);
            ch.AppendFormat("Icon ID: {0}\r\n", IconId);
            // TODO see cchform.txt for details on breakdown of this bit
            ch.AppendFormat("March Order: {0}\r\n", MarchOrder);
            ch.AppendFormat("Number of Item Bundles Carried: {0}\r\n", NumberOfItemBundlesCarried);
            ch.AppendFormat("Number of Hands Full: {0}\r\n", NumberOfHandsFull);
            ch.AppendFormat("Train for Level?: {0}\r\n", TrainForLevel);
            ch.AppendLine();

            ch.AppendFormat("Strength: {0}\r\n", Str);
            ch.AppendFormat("Intelligence: {0}\r\n", Int);
            ch.AppendFormat("Wisdom: {0}\r\n", Wis);
            ch.AppendFormat("Dexterity: {0}\r\n", Dex);
            ch.AppendFormat("Constitution: {0}\r\n", Con);
            ch.AppendFormat("Charisma: {0}\r\n", Cha);
            ch.AppendFormat("Extraordinary Strength: {0}\r\n", Extra);
            ch.AppendLine();

            ch.AppendFormat("Pick Pocket: {0}\r\n", PickPocket);
            ch.AppendFormat("Open Lock: {0}\r\n", OpenLock);
            ch.AppendFormat("Find/Remove Traps: {0}\r\n", FindTraps);
            ch.AppendFormat("Move Silently: {0}\r\n", MoveSilently);
            ch.AppendFormat("Hide in Shadows: {0}\r\n", HideInShadows);
            ch.AppendFormat("Hear Noise: {0}\r\n", HearNoise);
            ch.AppendFormat("Climb Walls: {0}\r\n", ClimbWalls);
            ch.AppendFormat("Read Languages: {0}\r\n", ReadLanguages);
            ch.AppendLine();

            ch.AppendFormat("60 - Base THAC0: {0}\r\n", SixtyMinusBaseTHAC0);
            ch.AppendFormat("60 - Base AC: {0}\r\n", SixtyMinusBaseAC);
            ch.AppendFormat("Number of attacks per two rounds (1st/2nd attacks, unarmed): {0}\r\n",
                            NumberOfAttacksPerTwoRounds);
            ch.AppendFormat("Dice of Damage: {0}\r\n", DamageDice);
            ch.AppendFormat("Type of Damage Die Rolled: {0}\r\n", TypeOfDamageDiceRolled);
            ch.AppendFormat("Damage Bonus: {0}\r\n", DamageBonus);
            ch.AppendFormat("Total Magic Protective Bonus: {0}\r\n", TotalMagicProtectiveBonus);
            ch.AppendFormat("Magic Resistance: {0}\r\n", MagicResistance);
            ch.AppendLine();

            var spelllist = string.Empty;

            // TODO show breakdown of this
            ch.Append("Memorized Spells:\r\n");
            spelllist = MemorizedSpells.Where(spell => spell != 0).Aggregate(string.Empty, (current, spell) => current + string.Format("{0} ", spell));
            ch.Append(string.IsNullOrEmpty(spelllist.Trim()) ? "N/A" : spelllist);
            ch.AppendLine();
            ch.AppendLine();

            // TODO show breakdown of this
            ch.Append("Spell Book:\r\n");
            spelllist = SpellBook.Where(spell => spell != 0).Aggregate(string.Empty, (current, spell) => current + string.Format("{0} ", spell));
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

            ch.AppendFormat("Maximum HP: {0}\r\n", MaximumHP);
            ch.AppendFormat("Pre-Drain HP Total: {0}\r\n", PreDrainHPTotal);
            ch.AppendFormat("HP (as rolled): {0}\r\n", HPAsRolled);
            ch.AppendLine();

            ch.AppendFormat("Save vs. paralysis/poison/death magic: {0}\r\n", SaveVsParalysis);
            ch.AppendFormat("Save vs. petrification/polymorph: {0}\r\n", SaveVsPetrify);
            ch.AppendFormat("Save vs. rod/staff/wand: {0}\r\n", SaveVsRod);
            ch.AppendFormat("Save vs. breath weapon: {0}\r\n", SaveVsBreath);
            ch.AppendFormat("Save vs. spell: {0}\r\n", SaveVsSpell);

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