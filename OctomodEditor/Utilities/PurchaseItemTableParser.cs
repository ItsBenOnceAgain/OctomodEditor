using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public static class PurchaseItemTableParser
    {
        public const int ITEM_ENTRY_COUNT_OFFSET = 41;

        public static Dictionary<string, PurchaseItem> ParsePurchaseItemObjects(bool useBaseGame = false)
        {
            string uassetPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp"))
            {
                uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp";
            }
            else
            {
                uexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(uexpPath);
            int numOfEntries = BitConverter.ToInt32(allBytes, ITEM_ENTRY_COUNT_OFFSET);
            int currentOffset = ITEM_ENTRY_COUNT_OFFSET + 4;
            Dictionary<string, PurchaseItem> itemObjects = new Dictionary<string, PurchaseItem>();

            for (int i = 0; i < numOfEntries; i++)
            {
                var item = ParseSinglePurchaseItem(uassetStrings, allBytes, ref currentOffset);
                itemObjects.Add(item.Key, item);
            }

            return itemObjects;
        }

        public static PurchaseItem ParseSinglePurchaseItem(Dictionary<int, string> uassetStrings, byte[] allBytes, ref int currentOffset)
        {
            int itemOffset = currentOffset;
            string key = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string itemLabel = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int fcPrice = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int possibleFlag = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string possibleItemLabel = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int arrivalStatus = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int obtainFlag = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            int properSteal = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 12;

            return new PurchaseItem()
            {
                Offset = itemOffset,
                Key = key,
                ItemLabel = itemLabel,
                FCPrice = fcPrice,
                PossibleFlag = possibleFlag,
                PossibleItemLabel = possibleItemLabel,
                ArrivalStatus = arrivalStatus,
                ObtainFlag = obtainFlag,
                ProperSteal = properSteal
            };
        }
    }
}
