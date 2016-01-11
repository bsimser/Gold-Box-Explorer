using System;
using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    public class FruaCharacterFile : GoldBoxFile
    {
        private readonly string _path;

        public FruaCharacterFile(GoldBoxFileParameters parameters)
        {
            _path = parameters.FullPath;
        }

        public override IList<byte> GetBytes()
        {
            return File.ReadAllBytes(_path);
        }

        public override string GetStatusMessage()
        {
            var filename = Path.GetFileName(_path);
            return filename.EndsWith(".CCH") ? "Player Character File" : "Monster File";
        }

        public FruaCharacter LoadCharacter()
        {
            var ch = new FruaCharacter();

            // read in file byte by byte and set properties on character object
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    // pointer to next character
                    reader.ReadBytes(4);
                    // special abilities
                    reader.ReadBytes(4);
                    // inventory
                    reader.ReadBytes(4);
                    // weapon hand
                    reader.ReadBytes(4);
                    // shield hand
                    reader.ReadBytes(4);
                    // armor worn
                    reader.ReadBytes(4);
                    // gauntlets
                    reader.ReadBytes(4);
                    // helmet
                    reader.ReadBytes(4);
                    // belt
                    reader.ReadBytes(4);
                    // robe
                    reader.ReadBytes(4);
                    // cloak
                    reader.ReadBytes(4);
                    // boots
                    reader.ReadBytes(4);
                    // ring
                    reader.ReadBytes(4);
                    // 2nd ring
                    reader.ReadBytes(4);
                    // arrows
                    reader.ReadBytes(4);
                    // bolts
                    reader.ReadBytes(4);
                    // reserved but not used
                    reader.ReadBytes(4);
                    ch.XpTotal = reader.ReadUInt32();
                    ch.XpTotalBeforeDraining = reader.ReadUInt32();
                    ch.PlatinumPieces = reader.ReadUInt16();
                    ch.Gems = reader.ReadUInt16();
                    ch.Jewels = reader.ReadUInt16();
                    ch.Age = reader.ReadUInt16();
                    ch.XpForKilling = reader.ReadUInt16();
                    ch.CurrentEncumbrance = reader.ReadUInt16();
                    ch.Race = reader.ReadByte();
                    ch.Class = reader.ReadByte();
                    ch.UndeadType = reader.ReadByte();
                    // ??? not needed for new characters
                    reader.ReadByte();
                    ch.Gender = reader.ReadByte();
                    ch.Alignment = reader.ReadByte();
                    ch.Status = reader.ReadByte();
                    ch.CombatMode = reader.ReadByte();
                    ch.Name = new string(reader.ReadChars(15));
                    var nullIndex = ch.Name.IndexOf(Convert.ToChar(0x0));
                    if (nullIndex != -1)
                    {
                        ch.Name = ch.Name.Substring(0, nullIndex);
                    }
                    // null for name
                    reader.ReadByte();
                    ch.Str.Raw = reader.ReadByte();
                    ch.Str.Modified = reader.ReadByte();
                    ch.Int.Raw = reader.ReadByte();
                    ch.Int.Modified = reader.ReadByte();
                    ch.Wis.Raw = reader.ReadByte();
                    ch.Wis.Modified = reader.ReadByte();
                    ch.Dex.Raw = reader.ReadByte();
                    ch.Dex.Modified = reader.ReadByte();
                    ch.Con.Raw = reader.ReadByte();
                    ch.Con.Modified = reader.ReadByte();
                    ch.Cha.Raw = reader.ReadByte();
                    ch.Cha.Modified = reader.ReadByte();
                    ch.Extra.Raw = reader.ReadByte();
                    ch.Extra.Modified = reader.ReadByte();
                    // ??? zero for new characters
                    reader.ReadByte();
                    ch.SixtyMinusBaseTHAC0 = reader.ReadByte();
                    ch.CureDiseaseFlag = reader.ReadByte();
                    ch.MaximumHP = reader.ReadByte();
                    ch.SizeIndicator = reader.ReadByte();
                    ch.SaveVsParalysis = reader.ReadByte();
                    ch.SaveVsPetrify = reader.ReadByte();
                    ch.SaveVsRod = reader.ReadByte();
                    ch.SaveVsBreath = reader.ReadByte();
                    ch.SaveVsSpell = reader.ReadByte();
                    ch.BaseMovementRate = reader.ReadByte();
                    ch.CurrentLevel = reader.ReadByte();
                    ch.FormerLevel = reader.ReadByte();
                    ch.PickPocket = reader.ReadByte();
                    ch.OpenLock = reader.ReadByte();
                    ch.FindTraps = reader.ReadByte();
                    ch.MoveSilently = reader.ReadByte();
                    ch.HideInShadows = reader.ReadByte();
                    ch.HearNoise = reader.ReadByte();
                    ch.ClimbWalls = reader.ReadByte();
                    ch.ReadLanguages = reader.ReadByte();
                    // set to zero (reserved for 128 + morale)
                    reader.ReadByte();
                    ch.WarriorLevel = reader.ReadByte();
                    // ??? set to zero for new character
                    reader.ReadByte();
                    // level defs
                    // 1st is pre-draining
                    // 2nd is current
                    // 3rd is before changing class (human only)
                    for (var i = 0; i < 3; i++)
                    {
                        ch.Levels.Add(new FruaCharacter.LevelDef
                        {
                            Cleric = reader.ReadByte(),
                            Knight = reader.ReadByte(),
                            Fighter = reader.ReadByte(),
                            Paladin = reader.ReadByte(),
                            Ranger = reader.ReadByte(),
                            Mage = reader.ReadByte(),
                            Thief = reader.ReadByte(),
                        });
                    }
                    ch.NumberOfAttacksPerTwoRounds = reader.ReadUInt16();
                    ch.DamageDice = reader.ReadUInt16();
                    ch.TypeOfDamageDiceRolled = reader.ReadUInt16();
                    ch.DamageBonus = reader.ReadUInt16();
                    ch.SixtyMinusBaseAC = reader.ReadByte();
                    // ??? set to zero for new character
                    reader.ReadByte();
                    ch.PreDrainHPTotal = reader.ReadByte();
                    ch.ItemUseIndicator = reader.ReadByte();
                    ch.HPAsRolled = reader.ReadByte();
                    // ??? set to zero for new character
                    reader.ReadBytes(3);
                    ch.IconId = reader.ReadByte();
                    ch.MarchOrder = reader.ReadByte();
                    // ??? set to zero for new character
                    reader.ReadByte();
                    ch.SpecialAbilityFlags = reader.ReadByte();
                    ch.MoreSpecialAbilityFlags = reader.ReadByte();
                    ch.NumberOfItemBundlesCarried = reader.ReadByte();
                    ch.NumberOfHandsFull = reader.ReadByte();
                    ch.TotalMagicProtectiveBonus = reader.ReadByte();
                    ch.MagicResistance = reader.ReadByte();
                    ch.TrainForLevel = reader.ReadByte();
                    ch.MemorizedSpells = reader.ReadBytes(140);
                    ch.SpellBook = reader.ReadBytes(15);
                    ch.ClericSpells = reader.ReadBytes(7);
                    // not used
                    reader.ReadBytes(2);
                    ch.DruidSpells = reader.ReadBytes(3);
                    // not used
                    reader.ReadBytes(6);
                    ch.MageSpells = reader.ReadBytes(9);
                    ch.ActiveFlag = reader.ReadByte();
                    // ??? set to zero for new character
                    reader.ReadByte();
                    // TODO complete more from line 243 in CCHFORM.TXT
                }
            }

            return ch;
        }
    }
}