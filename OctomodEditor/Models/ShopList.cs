using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class ShopList
    {
        public int Offset { get; set; }
        public string Key { get; set; }
        public string[] PurchaseItemIDs { get; set; }
    }
}
