using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class ShopInfo
    {
        public int Offset { get; set; }
        public string Key { get; set; }
        public string ShopName { get; set; }
        public ShopType ShopType { get; set; }
        public string ShopBGM { get; set; }
        public int InnBasePrice { get; set; }
        public string InnDiscountItem { get; set; }
        public int InnDiscountBasePrice { get; set; }

        public override string ToString()
        {
            string returnString = Key;
            string[] shopData = Key.Split('_');
            if(shopData[0] == "MINATO")
            {
                returnString = $"Unused Shop - {MainWindow.ModGameText[ShopName].Text}";
            }
            else if(shopData[0] == "NPC")
            {
                if (shopData[1].StartsWith("T"))
                {
                    returnString = MainWindow.ModGameText[ShopName].Text;
                }
                else
                {
                    returnString = $"{MainWindow.ModGameText[ShopName].Text} - {GetNPCShopLocation(shopData[1])}";
                }
            }
            else
            {
                returnString = $"{GetMainShopLocation(shopData[0], shopData[1])} - {ShopType}";
            }
            return returnString;
        }

        public bool IsDifferentFrom(ShopInfo info)
        {
            return !(Key == info.Key &&
                   ShopName == info.ShopName &&
                   ShopType == info.ShopType &&
                   ShopBGM == info.ShopBGM &&
                   InnBasePrice == info.InnBasePrice &&
                   InnDiscountItem == info.InnDiscountItem &&
                   InnDiscountBasePrice == info.InnDiscountBasePrice);
        }

        public static string GetNPCShopLocation(string identifier)
        {
            switch (identifier)
            {
                case "DClf11":
                    return "Cliftlands";
                case "DDst11":
                    return "Sunlands";
                case "DFrt11":
                    return "Woodlands";
                case "DMnt11":
                    return "Highlands";
                case "DPln11":
                    return "Flatlands";
                case "DRiv11":
                    return "Riverlands";
                case "DSea11":
                    return "Coastlands";
                case "DSnow11":
                    return "Frostlands";
                default:
                    return "BAD_LOCATION";
            }
        }

        public static string GetMainShopLocation(string area, string size)
        {
            if(size == "S")
            {
                switch (area)
                {
                    case "Cliff":
                        return "Orewell";
                    case "Desert":
                        return "Wellspring";
                    case "Forest":
                        return "S'warkii";
                    case "Mount":
                        return "Cobbleston";
                    case "Plain":
                        return "Wispermill";
                    case "River":
                        return "Clearbrook";
                    case "Sea":
                        return "Rippletide";
                    case "Snow":
                        return "Stillsnow";
                    default:
                        return "BAD_LOCATION";
                }
            }
            else if(size == "M")
            {
                switch (area)
                {
                    case "Cliff":
                        return "Quarrycrest";
                    case "Desert":
                        return "Sunshade";
                    case "Forest":
                        return "Duskbarrow";
                    case "Mount":
                        return "Everhold";
                    case "Plain":
                        return "Noblecourt";
                    case "River":
                        return "Riverford";
                    case "Sea":
                        return "Goldshore";
                    case "Snow":
                        return "Northreach";
                    default:
                        return "BAD_LOCATION";
                }
            }
            else if(size == "L")
            {
                switch (area)
                {
                    case "Cliff":
                        return "Bolderfall";
                    case "Desert":
                        return "Marsalim";
                    case "Forest":
                        return "Victor's Hollow";
                    case "Mount":
                        return "Stonegard";
                    case "Plain":
                        return "Atlasdam";
                    case "River":
                        return "Saintsbridge";
                    case "Sea":
                        return "Grandport";
                    case "Snow":
                        return "Flamesgrace";
                    default:
                        return "BAD_LOCATION";
                }
            }
            else
            {
                return "BAD_LOCATION";
            }
        }
    }
}
