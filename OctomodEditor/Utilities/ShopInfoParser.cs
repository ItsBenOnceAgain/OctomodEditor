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
            ShopType shopType = ConvertStringToShopType(uassetStrings[BitConverter.ToInt32(allBytes, currentOffset)]);
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

        public static ShopType ConvertStringToShopType(string shopTypeString)
        {
            ShopType shopType;
            switch (shopTypeString)
            {
                case "ESHOP_TYPE::NewEnumerator0":
                    shopType = ShopType.WEAPON;
                    break;
                case "ESHOP_TYPE::NewEnumerator1":
                    shopType = ShopType.ITEM;
                    break;
                case "ESHOP_TYPE::NewEnumerator2":
                    shopType = ShopType.GENERAL;
                    break;
                case "ESHOP_TYPE::NewEnumerator3":
                    shopType = ShopType.INN;
                    break;
                case "ESHOP_TYPE::NewEnumerator4":
                    shopType = ShopType.BAR;
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            return shopType;
        }
    }
}
