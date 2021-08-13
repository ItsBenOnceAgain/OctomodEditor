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
            if (!useBaseGame && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset"))
            {
                uassetPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp"))
            {
                uexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }
            else
            {
                uexpPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonOctomodUtilities.ParseUAssetFile(uassetPath);

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
            string key = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int itemId = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string nameId = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string detailTextId = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string iconLabel = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 41;
            ItemCategory category = CommonOctomodUtilities.ConvertStringToItemCategory(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 33;
            int sortCategory = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 37;
            ItemDisplayType displayType = CommonOctomodUtilities.ConvertStringToItemDisplayType(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 41;
            ItemUseType useType = CommonOctomodUtilities.ConvertStringToItemUseType(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 41;
            TargetType targetType = CommonOctomodUtilities.ConvertStringToTargetType(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 41;
            AttributeType attributeType = CommonOctomodUtilities.ConvertStringToItemAttributeType(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
            currentOffset += 119;
            Ailment[] ailments = new Ailment[4];
            for(int i = 0; i < ailments.Length; i++)
            {
                string ailmentName = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
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
            EquipmentCategory equipCategory = CommonOctomodUtilities.ConvertStringToItemEquipmentCategory(CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings));
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
            if (!Directory.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/"))
            {
                Directory.CreateDirectory($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/");
            }

            string uassetPath;
            if (File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset"))
            {
                uassetPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }
            else
            {
                uassetPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset";
            }
            string oldUexpPath;
            if (File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp"))
            {
                oldUexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }
            else
            {
                oldUexpPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";
            }

            string uexpPath;
            if (!File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp"))
            {
                var openStream = File.Create($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp");
                openStream.Close();
            }
            uexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uexp";

            Dictionary<int, string> uassetStrings = CommonOctomodUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(oldUexpPath);
            foreach (var item in items)
            {
                SaveItem(item, allBytes, uassetStrings, uassetPath, $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Item/Database/ItemDB.uasset");
            }
            File.WriteAllBytes(uexpPath, allBytes);
        }

        public static void SaveItem(Item item, byte[] allBytes, Dictionary<int, string> uassetStrings, string uassetPath, string modUassetPath)
        {
            int currentOffset = item.Offset + 95;
            byte[] detailTextIdData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(item.DetailTextID, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(detailTextIdData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] iconLabelData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(item.IconLabelID, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(iconLabelData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] categoryData = CommonOctomodUtilities.ConvertItemCategoryToBytes(item.Category, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(categoryData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] sortCategoryBytes = BitConverter.GetBytes(item.SortCategory);
            CommonOctomodUtilities.UpdateBytesAtOffset(sortCategoryBytes, allBytes, currentOffset);
            currentOffset += 37;
            byte[] displayTypeData = CommonOctomodUtilities.ConvertItemDisplayTypeToBytes(item.DisplayType, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(displayTypeData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] useTypeData = CommonOctomodUtilities.ConvertItemUseTypeToBytes(item.UseType, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(useTypeData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] targetTypeData = CommonOctomodUtilities.ConvertItemTargetTypeToBytes(item.TargetType, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(targetTypeData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] attributeTypeData = CommonOctomodUtilities.ConvertItemAttributeTypeToBytes(item.AttributeType, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(attributeTypeData, allBytes, currentOffset);
            currentOffset += 119;
            for (int i = 0; i < item.Ailments.Length; i++)
            {
                byte[] ailmentNameData = CommonOctomodUtilities.GetBytesFromStringWithPossibleSuffix(item.Ailments[i].AilmentName, uassetStrings, uassetPath, modUassetPath);
                CommonOctomodUtilities.UpdateBytesAtOffset(ailmentNameData, allBytes, currentOffset);
                currentOffset += 33;
                byte[] invocationValueData = BitConverter.GetBytes(item.Ailments[i].InvocationValue);
                CommonOctomodUtilities.UpdateBytesAtOffset(invocationValueData, allBytes, currentOffset);
                currentOffset += 29;
                byte[] invocationTurnData = BitConverter.GetBytes(item.Ailments[i].InvocationTurn);
                CommonOctomodUtilities.UpdateBytesAtOffset(invocationTurnData, allBytes, currentOffset);
                currentOffset += 29;
                byte[] diseaseRateData = BitConverter.GetBytes(item.Ailments[i].DiseaseRate);
                CommonOctomodUtilities.UpdateBytesAtOffset(diseaseRateData, allBytes, currentOffset);
                currentOffset += i == item.Ailments.Length - 1 ? 36 : 37;
            }
            byte[] isValuableData = BitConverter.GetBytes(item.IsValuable);
            CommonOctomodUtilities.UpdateBytesAtOffset(isValuableData, allBytes, currentOffset);
            currentOffset += 27;
            byte[] buyPriceData = BitConverter.GetBytes(item.BuyPrice);
            CommonOctomodUtilities.UpdateBytesAtOffset(buyPriceData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] sellPriceData = BitConverter.GetBytes(item.SellPrice);
            CommonOctomodUtilities.UpdateBytesAtOffset(sellPriceData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] equipCategoryData = CommonOctomodUtilities.ConvertItemEquipmentCategoryToBytes(item.EquipmentCategory, uassetStrings, uassetPath, modUassetPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(equipCategoryData, allBytes, currentOffset);
            currentOffset += 82;
            byte[] hpRevisionData = BitConverter.GetBytes(item.HPRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(hpRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mpRevisionData = BitConverter.GetBytes(item.MPRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(mpRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] bpRevisionData = BitConverter.GetBytes(item.BPRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(bpRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] spRevisionData = BitConverter.GetBytes(item.SPRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(spRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] pAttackRevisionData = BitConverter.GetBytes(item.PAttackRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(pAttackRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] pDefenseRevisionData = BitConverter.GetBytes(item.PDefenseRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(pDefenseRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mAttackRevisionData = BitConverter.GetBytes(item.MAttackRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(mAttackRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] mDefenseRevisionData = BitConverter.GetBytes(item.MDefenseRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(mDefenseRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] accuracyRevisionData = BitConverter.GetBytes(item.AccuracyRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(accuracyRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] evasionRevisionData = BitConverter.GetBytes(item.EvasionRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(evasionRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] criticalRevisionData = BitConverter.GetBytes(item.CriticalRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(criticalRevisionData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] speedRevisionData = BitConverter.GetBytes(item.SpeedRevision);
            CommonOctomodUtilities.UpdateBytesAtOffset(speedRevisionData, allBytes, currentOffset);
            currentOffset += 142;
            byte[] resistanceData = ConvertItemResistancesToBytes(item, uassetStrings);
            CommonOctomodUtilities.UpdateBytesAtOffset(resistanceData, allBytes, currentOffset);
            currentOffset += 37;
            byte[] commandEffecterData = GetEffecterBytesFromString(uassetStrings, item.CommandEffecterPath);
            CommonOctomodUtilities.UpdateBytesAtOffset(commandEffecterData, allBytes, currentOffset);
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
            resistBytes[10] = 0;
            resistBytes[11] = 0;

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
    }
}
