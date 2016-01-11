using System;
using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Common.Frua.Frua
{
    // TODO this has problems with dark queen of krynn files
    // DQ - ItemRecordSize = 16 && don't read extra byte
    // FRUA - ItemRecordSize = 16 && read extra byte
    public class FruaItemFile : GoldBoxFile
    {
        const int NumberOfItems = 254;
        const int ItemRecordSize = 16;

        private readonly List<FruaItem> _items = new List<FruaItem>(NumberOfItems);

        public FruaItemFile(string fullPath)
        {
            LoadItems(fullPath);
        }

        private void LoadItems(string fullPath)
        {
            var path = Path.GetDirectoryName(fullPath);
            var filename = Path.GetFileName(fullPath).ToUpper();

            filename = Path.Combine(path, filename.Equals("ITEMS.DAT") ? "ITEM.DAT" : filename);

            using (var reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read)))
            {
                for (var i = 0; i < NumberOfItems; i++)
                {
                    var pointerToItemsRecord = reader.ReadByte();
                    var thirdNameCode = reader.ReadByte();
                    var secondNameCode = reader.ReadByte();
                    var firstNameCode = reader.ReadByte();
                    var encumbrance = reader.ReadInt16();
                    var priceInPp = reader.ReadInt16();
                    var magicBonus = reader.ReadByte();
                    var secondaryCode = reader.ReadByte();
                    var isReady = reader.ReadByte();
                    var isIdentified = reader.ReadByte();
                    var isCursed = reader.ReadByte();
                    var numberOfItemsInBundle = reader.ReadByte();
                    var numberOfCharges = reader.ReadByte();
                    var codeForMagicalEffect = reader.ReadByte();
                    var specialCode = reader.ReadByte();

                    //reader.ReadByte(); // unused byte

                    var item = new FruaItem(
                        firstNameCode != 0 && firstNameCode < FruaItem.ItemNames.Length ? FruaItem.ItemNames[firstNameCode] : string.Empty,
                        secondNameCode != 0 && secondNameCode < FruaItem.ItemNames.Length ? FruaItem.ItemNames[secondNameCode] : string.Empty,
                        thirdNameCode != 0 && thirdNameCode < FruaItem.ItemNames.Length ? FruaItem.ItemNames[thirdNameCode] : string.Empty
                        )
                    {
                        Encumbrance = encumbrance,
                        Price = priceInPp,
                        MagicBonus = magicBonus,
                        Items = numberOfItemsInBundle,
                        Charges = numberOfCharges,
                        SecondaryCode = secondaryCode,
                        IsReady = isReady == 1,
                        Identified = isIdentified,
                        IsCursed = isCursed == 1,
                        MagicalEffectCode = codeForMagicalEffect,
                        SpecialCode = specialCode
                    };

                    /*
                    var itemsFile = Path.Combine(path, "ITEMS.DAT");
                    // fetch additional data for item from items.dat using pointerToItemsRecord
                    using (var brItem = new BinaryReader(File.Open(itemsFile, FileMode.Open, FileAccess.Read)))
                    {
                        brItem.BaseStream.Seek(ItemRecordSize * pointerToItemsRecord, SeekOrigin.Begin);

                        var locationWhereItemCarried = brItem.ReadByte();
                        var handsRequiredToUse = brItem.ReadByte();
                        var numberOfDamageDiceVersusSizeL = brItem.ReadByte();
                        var typeOfDamageDiceVersusSizeL = brItem.ReadByte();
                        var damageBonusVerusSizeL = brItem.ReadByte();
                        var rateOfFirePerTwoRounds = brItem.ReadByte();
                        var protectionValue = brItem.ReadByte();
                        var weaponType = brItem.ReadByte();
                        var meleeClass = brItem.ReadByte();
                        var numberOfDamageDiceVersusSizeM = brItem.ReadByte();
                        var typeOfDamageDiceVersusSizeM = brItem.ReadByte();
                        var damageBonusVerusSizeM = brItem.ReadByte();
                        var range = brItem.ReadByte();
                        var classThatCanUseThisItem = brItem.ReadByte();
                        var missileType = brItem.ReadByte();

                        brItem.ReadByte(); // unused byte

                        item.Location = locationWhereItemCarried;
                        item.Hands = handsRequiredToUse;
                        item.WeaponClass = weaponType;
                        item.MeleeClass = meleeClass;
                        item.Rate = rateOfFirePerTwoRounds;
                        item.Protection = protectionValue;
                        item.MissileType = missileType;
                        item.Range = range;
                        item.ClassUsage = classThatCanUseThisItem;

                        item.DamageVsLarge = new FruaItemDamageDice
                        {
                            Number = numberOfDamageDiceVersusSizeL,
                            Bonus = damageBonusVerusSizeL,
                            Type = typeOfDamageDiceVersusSizeL
                        };

                        item.DamageVsMedium = new FruaItemDamageDice
                        {
                            Number = numberOfDamageDiceVersusSizeM,
                            Bonus = damageBonusVerusSizeM,
                            Type = typeOfDamageDiceVersusSizeM
                        };
                    }
                    */

                    _items.Add(item);
                }
            }
        }

        public override IList<byte> GetBytes()
        {
            throw new NotImplementedException();
        }

        public override string GetStatusMessage()
        {
            return "FRUA Item File";
        }

        public IList<FruaItem> GetItems()
        {
            return _items.AsReadOnly();
        }
    }
}