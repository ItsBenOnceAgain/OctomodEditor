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
                SaveShopInfo(shopInfo, allBytes, uassetStrings, uassetPath);
            }
            File.WriteAllBytes(uexpPath, allBytes);
        }

        public static void SaveShopInfo(ShopInfo shop, byte[] allBytes, Dictionary<int, string> uassetStrings, string uassetPath)
        {
            int currentOffset = shop.Offset + 33;
            byte[] shopNameData = GetBytesFromStringWithPossibleSuffix(shop.ShopName, uassetStrings, uassetPath);
            UpdateBytesAtOffset(shopNameData, allBytes, currentOffset);
            currentOffset += 41;
            byte[] shopTypeBytes = ConvertShopTypeToBytes(shop.ShopType, uassetStrings);
            UpdateBytesAtOffset(shopTypeBytes, allBytes, currentOffset);
            currentOffset += 33;
            byte[] shopBGMBytes = GetBytesFromStringWithPossibleSuffix(shop.ShopBGM, uassetStrings, uassetPath);
            UpdateBytesAtOffset(shopBGMBytes, allBytes, currentOffset);
            currentOffset += 33;
            byte[] innBasePriceBytes = BitConverter.GetBytes(shop.InnBasePrice);
            UpdateBytesAtOffset(innBasePriceBytes, allBytes, currentOffset);
            currentOffset += 29;
            byte[] innDiscountItemBytes = GetBytesFromStringWithPossibleSuffix(shop.InnDiscountItem, uassetStrings, uassetPath);
            UpdateBytesAtOffset(innDiscountItemBytes, allBytes, currentOffset);
            currentOffset += 33;
            byte[] innDiscountPriceBytes = BitConverter.GetBytes(shop.InnDiscountBasePrice);
            UpdateBytesAtOffset(innDiscountPriceBytes, allBytes, currentOffset);
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

        public static byte[] ConvertShopTypeToBytes(ShopType shopType, Dictionary<int, string> uassetStrings)
        {
            string shopTypeString;
            switch (shopType)
            {
                case ShopType.WEAPON:
                    shopTypeString = "ESHOP_TYPE::NewEnumerator0";
                    break;
                case ShopType.ITEM:
                    shopTypeString = "ESHOP_TYPE::NewEnumerator1";
                    break;
                case ShopType.GENERAL:
                    shopTypeString = "ESHOP_TYPE::NewEnumerator2";
                    break;
                case ShopType.INN:
                    shopTypeString = "ESHOP_TYPE::NewEnumerator3";
                    break;
                case ShopType.BAR:
                    shopTypeString = "ESHOP_TYPE::NewEnumerator4";
                    break;
                default:
                    throw new ArgumentException("String was not in expected format.");
            }

            int value;
            try
            {
                value = uassetStrings.Single(x => x.Value == shopTypeString).Key;
            }
            catch (KeyNotFoundException)
            {
                value = uassetStrings.Single(x => x.Value == "ESHOP_TYPE::NewEnumerator0").Key;
            }

            return BitConverter.GetBytes(value);
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
            if (!uassetStrings.ContainsValue(stringWithPossibleSuffix) && !uassetStrings.ContainsValue(prefix))
            {
                CommonUtilities.AddStringToUasset(uassetPath, $"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset", stringWithPossibleSuffix);
                uassetStrings = CommonUtilities.ParseUAssetFile($"{CommonUtilities.ModLocation}/Octopath_Traveler/Content/Shop/Database/ShopInfo.uasset");
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
