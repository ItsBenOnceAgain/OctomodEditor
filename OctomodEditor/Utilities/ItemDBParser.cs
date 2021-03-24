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

        public static void SaveItems(List<Item> items)
        {
            if (!Directory.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/"))
            {
                Directory.CreateDirectory($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/");
            }

            string uassetPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }
            string oldUexpPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp"))
            {
                oldUexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }
            else
            {
                oldUexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }

            string uexpPath;
            if (!File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp"))
            {
                var openStream = File.Create($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp");
                openStream.Close();
            }
            uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(oldUexpPath);
            foreach (var item in items)
            {
                SaveItem(item, allBytes, uassetStrings, uassetPath);
            }
            File.WriteAllBytes(uexpPath, allBytes);
        }

        public static void SaveItem(Item item, byte[] allBytes, Dictionary<int, string> uassetStrings, string uassetPath)
        {
            int currentOffset = item.Offset + 95;
            byte[] detailTextIdData = GetBytesFromStringWithPossibleSuffix(item.DetailTextID, uassetStrings, uassetPath);
            UpdateBytesAtOffset(detailTextIdData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] iconLabelData = GetBytesFromStringWithPossibleSuffix(item.IconLabelID, uassetStrings, uassetPath);
            UpdateBytesAtOffset(iconLabelData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] categoryData = ConvertItemCategoryToBytes(item.Category, uassetStrings);
            UpdateBytesAtOffset(categoryData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] sortCategoryBytes = BitConverter.GetBytes(item.SortCategory);
            UpdateBytesAtOffset(sortCategoryBytes, allBytes, currentOffset);
            currentOffset += 37;
            byte[] displayTypeData = ConvertItemDisplayTypeToBytes(item.DisplayType, uassetStrings);
            UpdateBytesAtOffset(displayTypeData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] useTypeData = ConvertItemUseTypeToBytes(item.UseType, uassetStrings);
            UpdateBytesAtOffset(useTypeData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] targetTypeData = ConvertItemTargetTypeToBytes(item.TargetType, uassetStrings);
            UpdateBytesAtOffset(targetTypeData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] attributeTypeData = ConvertItemAttributeTypeToBytes(item.AttributeType, uassetStrings);
            UpdateBytesAtOffset(attributeTypeData, allBytes, currentOffset);
            currentOffset += 119;
            for (int i = 0; i < item.Ailments.Length; i++)
            {
                byte[] ailmentNameData = GetBytesFromStringWithPossibleSuffix(item.Ailments[i].AilmentName, uassetStrings, uassetPath);
                UpdateBytesAtOffset(ailmentNameData, allBytes, currentOffset);
                currentOffset += 33;
                byte[] invocationValueData = BitConverter.GetBytes(item.Ailments[i].InvocationValue);
                UpdateBytesAtOffset(invocationValueData, allBytes, currentOffset);
                currentOffset += 29;
                byte[] invocationTurnData = BitConverter.GetBytes(item.Ailments[i].InvocationTurn);
                UpdateBytesAtOffset(invocationTurnData, allBytes, currentOffset);
                currentOffset += 29;
                byte[] diseaseRateData = BitConverter.GetBytes(item.Ailments[i].DiseaseRate);
                UpdateBytesAtOffset(diseaseRateData, allBytes, currentOffset);
                currentOffset += i == item.Ailments.Length - 1 ? 36 : 37;
            }
            byte[] isValuableData = BitConverter.GetBytes(item.IsValuable);
            UpdateBytesAtOffset(isValuableData, allBytes, currentOffset);
            currentOffset += 27;
            byte[] buyPriceData = BitConverter.GetBytes(item.BuyPrice);
            UpdateBytesAtOffset(buyPriceData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] sellPriceData = BitConverter.GetBytes(item.SellPrice);
            UpdateBytesAtOffset(sellPriceData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] equipCategoryData = ConvertItemEquipmentCategoryToBytes(item.EquipmentCategory, uassetStrings);
            UpdateBytesAtOffset(equipCategoryData, allBytes, currentOffset);
            currentOffset += 82;
            byte[] hpRevisionData = BitConverter.GetBytes(item.HPRevision);
            UpdateBytesAtOffset(hpRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mpRevisionData = BitConverter.GetBytes(item.MPRevision);
            UpdateBytesAtOffset(mpRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] bpRevisionData = BitConverter.GetBytes(item.BPRevision);
            UpdateBytesAtOffset(bpRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] spRevisionData = BitConverter.GetBytes(item.SPRevision);
            UpdateBytesAtOffset(spRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] pAttackRevisionData = BitConverter.GetBytes(item.PAttackRevision);
            UpdateBytesAtOffset(pAttackRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] pDefenseRevisionData = BitConverter.GetBytes(item.PDefenseRevision);
            UpdateBytesAtOffset(pDefenseRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mAttackRevisionData = BitConverter.GetBytes(item.MAttackRevision);
            UpdateBytesAtOffset(mAttackRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mDefenseRevisionData = BitConverter.GetBytes(item.MDefenseRevision);
            UpdateBytesAtOffset(mDefenseRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] accuracyRevisionData = BitConverter.GetBytes(item.AccuracyRevision);
            UpdateBytesAtOffset(accuracyRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] evasionRevisionData = BitConverter.GetBytes(item.EvasionRevision);
            UpdateBytesAtOffset(evasionRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] criticalRevisionData = BitConverter.GetBytes(item.CriticalRevision);
            UpdateBytesAtOffset(criticalRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] speedRevisionData = BitConverter.GetBytes(item.SpeedRevision);
            UpdateBytesAtOffset(speedRevisionData, allBytes, currentOffset);
            currentOffset += 142;
            byte[] resistanceData = ConvertItemResistancesToBytes(item, uassetStrings);
            UpdateBytesAtOffset(resistanceData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] commandEffecterData = GetEffecterBytesFromString(uassetStrings, item.CommandEffecterPath);
            UpdateBytesAtOffset(commandEffecterData, allBytes, currentOffset);
        }

        private static void UpdateBytesAtOffset(byte[] updateBytes, byte[] allBytes, int currentOffset)
        {
            for (int i = 0; i < updateBytes.Length; i++)
            {
                allBytes[currentOffset + i] = updateBytes[i];
            }
        }

        public static byte[] ConvertItemCategoryToBytes(ItemCategory category, Dictionary<int, string> uassetStrings)
        {
            string categoryString;
            switch (category)
            {
                case ItemCategory.CONSUMABLE:
                    categoryString = "EITEM_CATEGORY::NewEnumerator0";
                    break;
                case ItemCategory.MATERIAL_A:
                    categoryString = "EITEM_CATEGORY::NewEnumerator1";
                    break;
                case ItemCategory.TREASURE:
                case ItemCategory.TRADING:
                    categoryString = "EITEM_CATEGORY::NewEnumerator4";
                    break;
                case ItemCategory.EQUIPMENT:
                    categoryString = "EITEM_CATEGORY::NewEnumerator7";
                    break;
                case ItemCategory.INFORMATION:
                    categoryString = "EITEM_CATEGORY::NewEnumerator8";
                    break;
                case ItemCategory.MATERIAL_B:
                    categoryString = "EITEM_CATEGORY::NewEnumerator9";
                    break;
                default:
                    categoryString = "EITEM_CATEGORY::NewEnumerator0";
                    break;
            }

            int value = uassetStrings.Single(x => x.Value == categoryString).Key;
            return BitConverter.GetBytes(value);
        }

        public static byte[] ConvertItemDisplayTypeToBytes(ItemDisplayType displayType, Dictionary<int, string> uassetStrings)
        {
            string displayTypeString;
            switch (displayType)
            {
                case ItemDisplayType.ITEM_USE:
                    displayTypeString = "EITEM_DISPLAY_TYPE::NewEnumerator0";
                    break;
                case ItemDisplayType.ON_HIT:
                    displayTypeString = "EITEM_DISPLAY_TYPE::NewEnumerator1";
                    break;
                case ItemDisplayType.BATTLE_START:
                    displayTypeString = "EITEM_DISPLAY_TYPE::NewEnumerator2";
                    break;
                case ItemDisplayType.DISABLE:
                    displayTypeString = "EITEM_DISPLAY_TYPE::NewEnumerator3";
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            int value = uassetStrings.Single(x => x.Value == displayTypeString).Key;
            return BitConverter.GetBytes(value);
        }

        public static byte[] ConvertItemUseTypeToBytes(ItemUseType useType, Dictionary<int, string> uassetStrings)
        {
            string useTypeString;
            switch (useType)
            {
                case ItemUseType.DISABLE:
                    useTypeString = "EITEM_USE_TYPE::NewEnumerator0";
                    break;
                case ItemUseType.ALWAYS:
                    useTypeString = "EITEM_USE_TYPE::NewEnumerator1";
                    break;
                case ItemUseType.FIELD_ONLY:
                    useTypeString = "EITEM_USE_TYPE::NewEnumerator2";
                    break;
                case ItemUseType.BATTLE_ONLY:
                    useTypeString = "EITEM_USE_TYPE::NewEnumerator3";
                    break;
                default:
                    useTypeString = "EITEM_USE_TYPE::NewEnumerator0";
                    break;
            }

            int value = uassetStrings.Single(x => x.Value == useTypeString).Key;
            return BitConverter.GetBytes(value);
        }

        public static byte[] ConvertItemTargetTypeToBytes(ItemTargetType targetType, Dictionary<int, string> uassetStrings)
        {
            string targetTypeString;
            switch (targetType)
            {
                case ItemTargetType.SELF:
                    targetTypeString = "ETARGET_TYPE::NewEnumerator0";
                    break;
                case ItemTargetType.PARTY_SINGLE:
                    targetTypeString = "ETARGET_TYPE::NewEnumerator1";
                    break;
                case ItemTargetType.PARTY_ALL:
                    targetTypeString = "ETARGET_TYPE::NewEnumerator2";
                    break;
                case ItemTargetType.ENEMY_SINGLE:
                    targetTypeString = "ETARGET_TYPE::NewEnumerator3";
                    break;
                case ItemTargetType.ENEMY_ALL:
                    targetTypeString = "ETARGET_TYPE::NewEnumerator4";
                    break;
                case ItemTargetType.ALL:
                    targetTypeString = "ETARGET_TYPE::NewEnumerator5";
                    break;
                default:
                    targetTypeString = "ETARGET_TYPE::NewEnumerator0";
                    break;
            }

            int value = uassetStrings.Single(x => x.Value == targetTypeString).Key;
            return BitConverter.GetBytes(value);
        }

        public static byte[] ConvertItemAttributeTypeToBytes(ItemAttributeType attributeType, Dictionary<int, string> uassetStrings)
        {
            string attributeTypeString;
            switch (attributeType)
            {
                case ItemAttributeType.NONE:
                    attributeTypeString = "EATTRIBUTE_TYPE::NewEnumerator0";
                    break;
                case ItemAttributeType.FIRE:
                    attributeTypeString = "EATTRIBUTE_TYPE::NewEnumerator1";
                    break;
                case ItemAttributeType.ICE:
                    attributeTypeString = "EATTRIBUTE_TYPE::NewEnumerator2";
                    break;
                case ItemAttributeType.LIGHTNING:
                    attributeTypeString = "EATTRIBUTE_TYPE::NewEnumerator3";
                    break;
                case ItemAttributeType.WIND:
                    attributeTypeString = "EATTRIBUTE_TYPE::NewEnumerator4";
                    break;
                case ItemAttributeType.LIGHT:
                    attributeTypeString = "EATTRIBUTE_TYPE::NewEnumerator5";
                    break;
                case ItemAttributeType.DARK:
                    attributeTypeString = "EATTRIBUTE_TYPE::NewEnumerator6";
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            int value = uassetStrings.Single(x => x.Value == attributeTypeString).Key;
            return BitConverter.GetBytes(value);
        }

        public static byte[] ConvertItemEquipmentCategoryToBytes(ItemEquipmentCategory equipmentCategory, Dictionary<int, string> uassetStrings)
        {
            string equipmentCategoryString;
            switch (equipmentCategory)
            {
                case ItemEquipmentCategory.SWORD:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator0";
                    break;
                case ItemEquipmentCategory.LANCE:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator1";
                    break;
                case ItemEquipmentCategory.DAGGER:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator2";
                    break;
                case ItemEquipmentCategory.AXE:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator3";
                    break;
                case ItemEquipmentCategory.BOW:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator5";
                    break;
                case ItemEquipmentCategory.ROD:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator6";
                    break;
                case ItemEquipmentCategory.SHIELD:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator7";
                    break;
                case ItemEquipmentCategory.HEAVY_HELMET:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator10";
                    break;
                case ItemEquipmentCategory.HEAVY_ARMOR:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator11";
                    break;
                case ItemEquipmentCategory.ACCESSORY:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator14";
                    break;
                default:
                    equipmentCategoryString = "EEQUIPMENT_CATEGORY::NewEnumerator0";
                    break;
            }

            int value = uassetStrings.Single(x => x.Value == equipmentCategoryString).Key;
            return BitConverter.GetBytes(value);
        }

        public static byte[] ConvertItemResistancesToBytes(Item item, Dictionary<int, string> uassetStrings)
        {
            byte[] resistBytes = new byte[12];

            resistBytes[0] = BitConverter.GetBytes(item.ResistPoison).Single();
            resistBytes[1] = BitConverter.GetBytes(item.ResistSilence).Single();
            resistBytes[2] = BitConverter.GetBytes(item.ResistBlindness).Single();
            resistBytes[3] = BitConverter.GetBytes(item.ResistConfusion).Single();
            resistBytes[4] = BitConverter.GetBytes(item.ResistSleep).Single();
            resistBytes[5] = BitConverter.GetBytes(item.ResistTerror).Single();
            resistBytes[6] = BitConverter.GetBytes(item.ResistUnconciousness).Single();
            resistBytes[7] = BitConverter.GetBytes(item.ResistInstantDeath).Single();
            resistBytes[8] = BitConverter.GetBytes(item.ResistTransform).Single();
            resistBytes[9] = BitConverter.GetBytes(item.ResistDebuff).Single();
            resistBytes[0] = 0;
            resistBytes[0] = 0;

            return resistBytes;
        }

        public static byte[] GetEffecterBytesFromString(Dictionary<int, string> uassetStrings, string mainEffecterString)
        {
            List<string> baseStrings = uassetStrings.Where(x => x.Value.StartsWith(@"/Game/Battle/BP/BattleAction")).Select(x => x.Value).ToList();
            Dictionary<string, int> effecterValues = new Dictionary<string, int>();
            effecterValues.Add("None", 0);
            int currentValue = -2;
            foreach (var effecterString in baseStrings)
            {
                effecterValues.Add(effecterString, currentValue);
                currentValue--;
                if (effecterString.EndsWith("_D"))
                {
                    effecterValues.Add($"{effecterString}_2", currentValue);
                    effecterValues.Add($"{effecterString}_3", currentValue - 1);
                    currentValue -= 2;
                }
            }

            return BitConverter.GetBytes(effecterValues[mainEffecterString]);
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
            effecterValues.Add(0, "None");
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

        public static byte[] GetBytesFromStringWithPossibleSuffix(string stringWithPossibleSuffix, Dictionary<int, string> uassetStrings, string uassetPath)
        {
            string[] data = stringWithPossibleSuffix.Split('_');
            string prefix = string.Join("_", data.Where(y => y != data.Last()));

            byte[] byteData = new byte[8];
            if (!uassetStrings.ContainsValue(stringWithPossibleSuffix) && !uassetStrings.ContainsValue(stringWithPossibleSuffix))
            {
                CommonUtilities.AddStringToUasset(uassetPath, $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset", stringWithPossibleSuffix);
                uassetStrings = CommonUtilities.ParseUAssetFile($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset");
            }

            if (uassetStrings.ContainsValue(stringWithPossibleSuffix))
            {
                UpdateBytesAtOffset(BitConverter.GetBytes(uassetStrings.Single(x => x.Value == stringWithPossibleSuffix).Key), byteData, 0);
            }
            else
            {
                byte[] numericValue = BitConverter.GetBytes(int.Parse(data[data.Length - 1]) + 1);
                UpdateBytesAtOffset(BitConverter.GetBytes(uassetStrings.Single(x => x.Value == prefix).Key), byteData, 0);
                UpdateBytesAtOffset(numericValue, byteData, 4);
            }
            return byteData;
        }
    }
}
