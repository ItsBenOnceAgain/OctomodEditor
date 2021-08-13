using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public static class ShopListParser
    {
        public const int LIST_ENTRY_COUNT_OFFSET = 41;

        public static Dictionary<string, ShopList> ParseShopListObjects(bool useBaseGame = false)
        {
            string uassetPath;
            if (!useBaseGame && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopList.uasset"))
            {
                uassetPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopList.uasset";
            }
            else
            {
                uassetPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/ShopList.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopList.uexp"))
            {
                uexpPath = $"{CommonOctomodUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopList.uexp";
            }
            else
            {
                uexpPath = $"{CommonOctomodUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/ShopList.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonOctomodUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(uexpPath);
            int numOfEntries = BitConverter.ToInt32(allBytes, LIST_ENTRY_COUNT_OFFSET);
            int currentOffset = LIST_ENTRY_COUNT_OFFSET + 4;
            Dictionary<string, ShopList> shopLists = new Dictionary<string, ShopList>();

            for (int i = 0; i < numOfEntries; i++)
            {
                var list = ParseSingleShopList(uassetStrings, allBytes, ref currentOffset);
                shopLists.Add(list.Key, list);
            }

            return shopLists;
        }

        public static ShopList ParseSingleShopList(Dictionary<int, string> uassetStrings, byte[] allBytes, ref int currentOffset)
        {
            int itemOffset = currentOffset;
            string key = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 41;
            int numOfShopItems = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 4;
            string[] purchaseItemIDs = new string[numOfShopItems];
            for(int i = 0; i < numOfShopItems; i++)
            {
                purchaseItemIDs[i] = CommonOctomodUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
                currentOffset += 8;
            }
            currentOffset += 8;
            return new ShopList()
            {
                Offset = itemOffset,
                Key = key,
                PurchaseItemIDs = purchaseItemIDs
            };
        }
    }
}
