using System;
using System.Collections.Generic;
using OctomodEditor.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OctomodEditor.Utilities
{
    public static class ItemDBParser
    {
        public const int ITEM_ENTRY_COUNT_OFFSET = 41;
        public static Dictionary<string, Item> ParseItemObjects(bool useBaseGame = false)
        {
            string uassetPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp"))
            {
                uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }
            else
            {
                uexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(uexpPath);
            int numOfEntries = BitConverter.ToInt32(allBytes, ITEM_ENTRY_COUNT_OFFSET);
            int currentOffset = ITEM_ENTRY_COUNT_OFFSET + 4;
            Dictionary<string, Item> itemObjects = new Dictionary<string, Item>();

            for (int i = 0; i < numOfEntries; i++)
            {
                var item = ParseSingleItem(uassetStrings, allBytes, ref currentOffset);
                itemObjects.Add(item.Key, item);
            }

            return itemObjects;
        }

        public static Item ParseSingleItem(Dictionary<int, string> uassetStrings, byte[] allBytes, ref int currentOffset)
        {
            int itemOffset = currentOffset;
            string key = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int itemId = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string nameId = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string detailTextId = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string iconLabel = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
            currentOffset += 41;
            ItemCategory category = ConvertStringToItemCategory(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 33;
            int sortCategory = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            ItemDisplayType displayType = ConvertStringToItemDisplayType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 41;
            ItemUseType useType = ConvertStringToItemUseType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 41;
            ItemTargetType targetType = ConvertStringToItemTargetType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 41;
            ItemAttributeType attributeType = ConvertStringToItemAttributeType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 119;
            Ailment[] ailments = new Ailment[4];
            for(int i = 0; i < ailments.Length; i++)
            {
                string ailmentName = uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)];
                currentOffset += 33;
                int invocationValue = BitConverter.ToInt32(allBytes, currentOffset);
                currentOffset += 29;
                int invocationTurn = BitConverter.ToInt32(allBytes, currentOffset);
                currentOffset += 29;
                int diseaseRate = BitConverter.ToInt32(allBytes, currentOffset);
                ailments[i] = new Ailment() { AilmentName = ailmentName, InvocationValue = invocationValue, InvocationTurn = invocationTurn, DiseaseRate = diseaseRate };
                currentOffset += i == ailments.Length - 1 ? 36 : 37;
            }
            bool isValuable = BitConverter.ToBoolean(allBytes, currentOffset);
            currentOffset += 27;
            int buyPrice = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int sellPrice = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            ItemEquipmentCategory equipCategory = ConvertStringToItemEquipmentCategory(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 82;
            int hpRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int mpRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int bpRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int spRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int pAttackRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int pDefenseRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int mAttackRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int mDefenseRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int accuracyRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int evasionRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int criticalRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int speedRevision = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 142;
            bool[] resistances = new bool[12];
            for(int i = 0; i < resistances.Length; i++)
            {
                resistances[i] = BitConverter.ToBoolean(allBytes, currentOffset);
                currentOffset += 1;
            }
            currentOffset += 25;

            string commandEffecter = GetEffecterStringFromValue(uassetStrings, BitConverter.ToInt32(allBytes, currentOffset));
            currentOffset += 12;

            return new Item()
            {
                Offset = itemOffset,
                Key = key,
                ItemID = itemId,
                ItemNameID = nameId,
                DetailTextID = detailTextId,
                IconLabelID = iconLabel,
                Category = category,
                SortCategory = sortCategory,
                DisplayType = displayType,
                UseType = useType,
                TargetType = targetType,
                AttributeType = attributeType,
                Ailments = ailments,
                IsValuable = isValuable,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                EquipmentCategory = equipCategory,
                HPRevision = hpRevision,
                MPRevision = mpRevision,
                BPRevision = bpRevision,
                SPRevision = spRevision,
                PAttackRevision = pAttackRevision,
                PDefenseRevision = pDefenseRevision,
                MAttackRevision = mAttackRevision,
                MDefenseRevision = mDefenseRevision,
                AccuracyRevision = accuracyRevision,
                EvasionRevision = evasionRevision,
                CriticalRevision = criticalRevision,
                SpeedRevision = speedRevision,
                ResistPoison = resistances[0],
                ResistSilence = resistances[1],
                ResistBlindness = resistances[2],
                ResistConfusion = resistances[3],
                ResistSleep = resistances[4],
                ResistTerror = resistances[5],
                ResistUnconciousness = resistances[6],
                ResistInstantDeath = resistances[7],
                ResistTransform = resistances[8],
                ResistDebuff = resistances[9],
                CommandEffecterPath = commandEffecter
            };
        }

        public static ItemCategory ConvertStringToItemCategory(string categoryString)
        {
            ItemCategory category;
            switch (categoryString)
            {
                case "EITEM_CATEGORY::NewEnumerator0":
                    category = ItemCategory.CONSUMABLE;
                    break;
                case "EITEM_CATEGORY::NewEnumerator1":
                    category = ItemCategory.MATERIAL_A;
                    break;
                case "EITEM_CATEGORY::NewEnumerator4":
                    category = ItemCategory.TREASURE;
                    break;
                case "EITEM_CATEGORY::NewEnumerator7":
                    category = ItemCategory.EQUIPMENT;
                    break;
                case "EITEM_CATEGORY::NewEnumerator8":
                    category = ItemCategory.INFORMATION;
                    break;
                case "EITEM_CATEGORY::NewEnumerator9":
                    category = ItemCategory.MATERIAL_B;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return category;
        }

        public static ItemDisplayType ConvertStringToItemDisplayType(string displayTypeString)
        {
            ItemDisplayType displayType;
            switch (displayTypeString)
            {
                case "EITEM_DISPLAY_TYPE::NewEnumerator0":
                    displayType = ItemDisplayType.ITEM_USE;
                    break;
                case "EITEM_DISPLAY_TYPE::NewEnumerator1":
                    displayType = ItemDisplayType.ON_HIT;
                    break;
                case "EITEM_DISPLAY_TYPE::NewEnumerator2":
                    displayType = ItemDisplayType.BATTLE_START;
                    break;
                case "EITEM_DISPLAY_TYPE::NewEnumerator3":
                    displayType = ItemDisplayType.DISABLE;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return displayType;
        }

        public static ItemUseType ConvertStringToItemUseType(string useTypeString)
        {
            ItemUseType useType;
            switch (useTypeString)
            {
                case "EITEM_USE_TYPE::NewEnumerator0":
                    useType = ItemUseType.DISABLE;
                    break;
                case "EITEM_USE_TYPE::NewEnumerator1":
                    useType = ItemUseType.ALWAYS;
                    break;
                case "EITEM_USE_TYPE::NewEnumerator2":
                    useType = ItemUseType.FIELD_ONLY;
                    break;
                case "EITEM_USE_TYPE::NewEnumerator3":
                    useType = ItemUseType.BATTLE_ONLY;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return useType;
        }

        public static ItemTargetType ConvertStringToItemTargetType(string targetTypeString)
        {
            ItemTargetType targetType;
            switch (targetTypeString)
            {
                case "ETARGET_TYPE::NewEnumerator0":
                    targetType = ItemTargetType.SELF;
                    break;
                case "ETARGET_TYPE::NewEnumerator1":
                    targetType = ItemTargetType.PARTY_SINGLE;
                    break;
                case "ETARGET_TYPE::NewEnumerator2":
                    targetType = ItemTargetType.PARTY_ALL;
                    break;
                case "ETARGET_TYPE::NewEnumerator3":
                    targetType = ItemTargetType.ENEMY_SINGLE;
                    break;
                case "ETARGET_TYPE::NewEnumerator4":
                    targetType = ItemTargetType.ENEMY_ALL;
                    break;
                case "ETARGET_TYPE::NewEnumerator5":
                    targetType = ItemTargetType.ALL;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return targetType;
        }

        public static ItemAttributeType ConvertStringToItemAttributeType(string targetTypeString)
        {
            ItemAttributeType targetType;
            switch (targetTypeString)
            {
                case "EATTRIBUTE_TYPE::NewEnumerator0":
                    targetType = ItemAttributeType.NONE;
                    break;
                case "EATTRIBUTE_TYPE::NewEnumerator1":
                    targetType = ItemAttributeType.FIRE;
                    break;
                case "EATTRIBUTE_TYPE::NewEnumerator2":
                    targetType = ItemAttributeType.ICE;
                    break;
                case "EATTRIBUTE_TYPE::NewEnumerator3":
                    targetType = ItemAttributeType.LIGHTNING;
                    break;
                case "EATTRIBUTE_TYPE::NewEnumerator4":
                    targetType = ItemAttributeType.WIND;
                    break;
                case "EATTRIBUTE_TYPE::NewEnumerator5":
                    targetType = ItemAttributeType.LIGHT;
                    break;
                case "EATTRIBUTE_TYPE::NewEnumerator6":
                    targetType = ItemAttributeType.DARK;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return targetType;
        }

        public static ItemEquipmentCategory ConvertStringToItemEquipmentCategory(string targetTypeString)
        {
            ItemEquipmentCategory targetType;
            switch (targetTypeString)
            {
                case "EEQUIPMENT_CATEGORY::NewEnumerator0":
                    targetType = ItemEquipmentCategory.SWORD;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator1":
                    targetType = ItemEquipmentCategory.LANCE;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator2":
                    targetType = ItemEquipmentCategory.DAGGER;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator3":
                    targetType = ItemEquipmentCategory.AXE;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator5":
                    targetType = ItemEquipmentCategory.BOW;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator6":
                    targetType = ItemEquipmentCategory.ROD;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator7":
                    targetType = ItemEquipmentCategory.SHIELD;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator10":
                    targetType = ItemEquipmentCategory.HEAVY_HELMET;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator11":
                    targetType = ItemEquipmentCategory.HEAVY_ARMOR;
                    break;
                case "EEQUIPMENT_CATEGORY::NewEnumerator14":
                    targetType = ItemEquipmentCategory.ACCESSORY;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return targetType;
        }

        public static string GetEffecterStringFromValue(Dictionary<int, string> uassetStrings, int value)
        {
            List<string> baseStrings = uassetStrings.Where(x => x.Value.StartsWith(@"/Game/Battle/BP/BattleAction")).Select(x => x.Value).ToList();
            Dictionary<int, string> effecterValues = new Dictionary<int, string>();
            effecterValues.Add(0, null);
            int currentValue = -2;
            foreach(var effecterString in baseStrings)
            {
                effecterValues.Add(currentValue, effecterString);
                currentValue--;
                if (effecterString.EndsWith("_D"))
                {
                    effecterValues.Add(currentValue, $"{effecterString}_2");
                    effecterValues.Add(currentValue - 1, $"{effecterString}_3");
                    currentValue -= 2;
                }
            }

            return effecterValues[value];
        }
    }
}
