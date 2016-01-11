using System;
using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Lib.Plugins.Items
{
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
            var filename = Path.GetFileName(fullPath);

            // always read ITEM.DAT first then ITEMS.DAT
            filename = Path.Combine(path, filename.Equals("ITEMS.DAT") ? "ITEM.DAT" : filename);

            var ckitFile = Path.Combine(path, "CKIT.CFG");
            var isFrua = File.Exists(ckitFile);

            using (var reader = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read)))
            {
                for (var i = 0; i < NumberOfItems; i++)
                {
                    var item = ReadItemRecord(reader, isFrua);
                    ReadItemDetails(path, item);
                    _items.Add(item);
                }
            }
        }

        private static void ReadItemDetails(string path, FruaItem item)
        {
            var itemsFile = Path.Combine(path, "ITEMS.DAT");

            using (var reader = new BinaryReader(File.Open(itemsFile, FileMode.Open, FileAccess.Read)))
            {
                reader.BaseStream.Seek(ItemRecordSize*item.Pointer, SeekOrigin.Begin);

                item.Location = reader.ReadByte();
                item.Hands = reader.ReadByte();
                item.DamageVsLarge = new FruaItemDamageDice
                {
                    Number = reader.ReadByte(),
                    Type = reader.ReadByte(),
                    Bonus = reader.ReadByte(),
                };
                item.Rate = reader.ReadByte();
                item.Protection = reader.ReadByte();
                item.WeaponClass = GetWeaponClass(reader.ReadByte());
                item.MeleeClass = GetMeleeType(reader.ReadByte());
                item.DamageVsMedium = new FruaItemDamageDice
                {
                    Number = reader.ReadByte(),
                    Type = reader.ReadByte(),
                    Bonus = reader.ReadByte(),
                };
                item.Range = reader.ReadByte();
                item.ClassUsage = reader.ReadByte();
                item.MissileType = reader.ReadByte();
                reader.ReadByte(); // unused byte
            }
        }

        private static string GetMeleeType(byte value)
        {
            return value == 0 ? "Not usable" : "Usable";
        }

        private static string GetWeaponClass(byte value)
        {
            var rc = "None";
            
            switch (value)
            {
                case 1:
                    rc = "Cutting";
                    break;
                case 128:
                    rc = "Blunt";
                    break;
            }

            return rc;
        }

        private static FruaItem ReadItemRecord(BinaryReader reader, bool isFrua)
        {
            var pointerToItemsRecord = reader.ReadByte();
            var thirdNameCode = reader.ReadByte();
            var secondNameCode = reader.ReadByte();
            var firstNameCode = reader.ReadByte();

            var item = new FruaItem(firstNameCode, secondNameCode, thirdNameCode)
                {
                    Pointer = pointerToItemsRecord,
                    Encumbrance = reader.ReadInt16(),
                    Price = reader.ReadInt16(),
                    MagicBonus = reader.ReadByte(),
                    SecondaryCode = reader.ReadByte(),
                    IsReady = reader.ReadByte() == 1,
                    Identified = reader.ReadByte(),
                    IsCursed = reader.ReadByte() == 1,
                    NumberOfItemsInBundle = reader.ReadByte(),
                    Charges = reader.ReadByte(),
                    MagicalEffectCode = reader.ReadByte(),
                    SpecialCode = GetSpecialCode(reader.ReadByte()),
                };

            if (isFrua)
            {
                reader.ReadByte(); // always 0
            }

            return item;
        }

        private static string GetSpecialCode(byte value)
        {
            var rc = "Magic Effect";

            if (value > 1 && value < 128)
            {
                rc = "Scroll";
            }
            else switch (value)
            {
                case 129:
                    rc = "Ring of Wizardry";
                    break;
                case 131:
                    rc = "Gauntlets of Ogre Power";
                    break;
                case 133:
                    rc = "Girdle of Giant Strength";
                    break;
            }

            return rc;
        }

        public override IList<byte> GetBytes()
        {
            throw new NotImplementedException();
        }

        public IList<FruaItem> GetItems()
        {
            return _items.AsReadOnly();
        }
    }
}