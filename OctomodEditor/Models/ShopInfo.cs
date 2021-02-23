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
    }
    
    public enum ShopType { WEAPON, ITEM, GENERAL, INN, BAR }
}
