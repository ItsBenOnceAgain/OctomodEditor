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

        public static void SavePurchaseItemObjects(List<PurchaseItem> purchaseItems)
        {
            if (!Directory.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/"))
            {
                Directory.CreateDirectory($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/");
            }

            string uassetPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset";
            }
            string oldUexpPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp"))
            {
                oldUexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp";
            }
            else
            {
                oldUexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp";
            }

            string uexpPath;
            if (!File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp"))
            {
                var openStream = File.Create($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp");
                openStream.Close();
            }
            uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uexp";

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(oldUexpPath);
            foreach (var purchaseItem in purchaseItems)
            {
                SaveSinglePurchaseItem(purchaseItem, allBytes, uassetStrings, uassetPath);
            }
            File.WriteAllBytes(uexpPath, allBytes);
        }

        public static void SaveSinglePurchaseItem(PurchaseItem item, byte[] allBytes, Dictionary<int, string> uassetStrings, string uassetPath)
        {
            int currentOffset = item.Offset + 33;
            byte[] itemLabelData = GetBytesFromStringWithPossibleSuffix(item.ItemLabel, uassetStrings, uassetPath);
            UpdateBytesAtOffset(itemLabelData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] fcPriceData = BitConverter.GetBytes(item.FCPrice);
            UpdateBytesAtOffset(fcPriceData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] possibleFlagData = BitConverter.GetBytes(item.PossibleFlag);
            UpdateBytesAtOffset(possibleFlagData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] possibleItemLabelData = GetBytesFromStringWithPossibleSuffix(item.PossibleItemLabel, uassetStrings, uassetPath);
            UpdateBytesAtOffset(possibleItemLabelData, allBytes, currentOffset);
            currentOffset += 33;
            byte[] arrivalStatusData = BitConverter.GetBytes(item.ArrivalStatus);
            UpdateBytesAtOffset(arrivalStatusData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] obtainFlagData = BitConverter.GetBytes(item.ObtainFlag);
            UpdateBytesAtOffset(obtainFlagData, allBytes, currentOffset);
            currentOffset += 29;
            byte[] properStealData = BitConverter.GetBytes(item.ProperSteal);
            UpdateBytesAtOffset(properStealData, allBytes, currentOffset);
        }

        private static void UpdateBytesAtOffset(byte[] updateBytes, byte[] allBytes, int currentOffset)
        {
            for (int i = 0; i < updateBytes.Length; i++)
            {
                allBytes[currentOffset + i] = updateBytes[i];
            }
        }

        public static byte[] GetBytesFromStringWithPossibleSuffix(string stringWithPossibleSuffix, Dictionary<int, string> uassetStrings, string uassetPath)
        {
            string[] data = stringWithPossibleSuffix.Split('_');
            string prefix = string.Join("_", data.Where(y => y != data.Last()));

            byte[] byteData = new byte[8];
            if(!uassetStrings.ContainsValue(stringWithPossibleSuffix) && !uassetStrings.ContainsValue(prefix))
            {
                CommonUtilities.AddStringToUasset(uassetPath, $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset", stringWithPossibleSuffix);
                uassetStrings = CommonUtilities.ParseUAssetFile($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/PurchaseItemTable.uasset");
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
