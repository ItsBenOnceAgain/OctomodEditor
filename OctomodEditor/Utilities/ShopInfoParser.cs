using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Utilities
{
    public static class ShopInfoParser
    {
        public const int LIST_ENTRY_COUNT_OFFSET = 41;

        public static Dictionary<string, ShopInfo> ParseShopInfoObjects(bool useBaseGame = false)
        {
            string uassetPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset";
            }

            string uexpPath;
            if (!useBaseGame && File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp"))
            {
                uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp";
            }
            else
            {
                uexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp";
            }

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(uexpPath);
            int numOfEntries = BitConverter.ToInt32(allBytes, LIST_ENTRY_COUNT_OFFSET);
            int currentOffset = LIST_ENTRY_COUNT_OFFSET + 4;
            Dictionary<string, ShopInfo> shopInfoList = new Dictionary<string, ShopInfo>();

            for (int i = 0; i < numOfEntries; i++)
            {
                var list = ParseSingleShopInfo(uassetStrings, allBytes, ref currentOffset);
                shopInfoList.Add(list.Key, list);
            }

            return shopInfoList;
        }

        public static ShopInfo ParseSingleShopInfo(Dictionary<int, string> uassetStrings, byte[] allBytes, ref int currentOffset)
        {
            int itemOffset = currentOffset;
            string key = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            string shopName = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 41;
            ShopType shopType = CommonUtilities.ConvertStringToShopType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
            currentOffset += 33;
            string shopBGM = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int innBasePrice = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 29;
            string innDiscountItem = CommonUtilities.ParseUAssetStringWithPossibleSuffix(allBytes, currentOffset, uassetStrings);
            currentOffset += 33;
            int innDiscountPrice = BitConverter.ToInt32(allBytes, currentOffset);
            currentOffset += 12;

            return new ShopInfo()
            {
                Offset = itemOffset,
                Key = key,
                ShopName = shopName,
                ShopType = shopType,
                ShopBGM = shopBGM,
                InnBasePrice = innBasePrice,
                InnDiscountItem = innDiscountItem,
                InnDiscountBasePrice = innDiscountPrice
            };
        }

        public static void SaveShopInfoObjects(List<ShopInfo> shopInfos)
        {
            if (!Directory.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/"))
            {
                Directory.CreateDirectory($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/");
            }

            string uassetPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset"))
            {
                uassetPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset";
            }
            else
            {
                uassetPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset";
            }
            string oldUexpPath;
            if (File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp"))
            {
                oldUexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp";
            }
            else
            {
                oldUexpPath = $"{CommonUtilities.BaseFilesLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp";
            }

            string uexpPath;
            if (!File.Exists($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp"))
            {
                var openStream = File.Create($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp");
                openStream.Close();
            }
            uexpPath = $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uexp";

            Dictionary<int, string> uassetStrings = CommonUtilities.ParseUAssetFile(uassetPath);

            byte[] allBytes = File.ReadAllBytes(oldUexpPath);
            foreach (var shopInfo in shopInfos)
            {
                SaveShopInfo(shopInfo, allBytes, uassetStrings, uassetPath, $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset");
            }
            File.WriteAllBytes(uexpPath, allBytes);
        }

        public static void SaveShopInfo(ShopInfo shop, byte[] allBytes, Dictionary<int, string> uassetStrings, string uassetPath, string modUassetPath)
        {
            int currentOffset = shop.Offset + 33;
            byte[] shopNameData = CommonUtilities.GetBytesFromStringWithPossibleSuffix(shop.ShopName, uassetStrings, uassetPath, modUassetPath);
            CommonUtilities.UpdateBytesAtOffset(shopNameData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] shopTypeBytes = CommonUtilities.ConvertShopTypeToBytes(shop.ShopType, uassetStrings, uassetPath, modUassetPath);
            CommonUtilities.UpdateBytesAtOffset(shopTypeBytes, allBytes, currentOffset);
            currentOffset += 33;
            byte[] shopBGMBytes = CommonUtilities.GetBytesFromStringWithPossibleSuffix(shop.ShopBGM, uassetStrings, uassetPath, modUassetPath);
            CommonUtilities.UpdateBytesAtOffset(shopBGMBytes, allBytes, currentOffset);
            currentOffset += 33;
            byte[] innBasePriceBytes = BitConverter.GetBytes(shop.InnBasePrice);
            CommonUtilities.UpdateBytesAtOffset(innBasePriceBytes, allBytes, currentOffset);
            currentOffset += 29;
            byte[] innDiscountItemBytes = CommonUtilities.GetBytesFromStringWithPossibleSuffix(shop.InnDiscountItem, uassetStrings, uassetPath, modUassetPath);
            CommonUtilities.UpdateBytesAtOffset(innDiscountItemBytes, allBytes, currentOffset);
            currentOffset += 33;
            byte[] innDiscountPriceBytes = BitConverter.GetBytes(shop.InnDiscountBasePrice);
            CommonUtilities.UpdateBytesAtOffset(innDiscountPriceBytes, allBytes, currentOffset);
        }
    }
}
