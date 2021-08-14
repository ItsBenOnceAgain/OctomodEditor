using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class PurchaseItem : OctopathModel
    {
        public string ItemLabel { get; set; }
        public int FCPrice { get; set; }
        public int PossibleFlag { get; set; }
        public string PossibleItemLabel { get; set; }
        public int ArrivalStatus { get; set; }
        public int ObtainFlag { get; set; }
        public int ProperSteal { get; set; }

        public override string ToString()
        {
            string returnString = Key;
            if (MainWindow.ModItemList.ContainsKey(ItemLabel))
            {
                Item item = MainWindow.ModItemList[ItemLabel];
                if (MainWindow.ModGameText.ContainsKey(item.ItemNameID))
                {
                    returnString = MainWindow.ModGameText[item.ItemNameID].Text;
                }
            }
            return returnString;
        }

        public bool IsDifferentFrom(PurchaseItem item)
        {
            return !(Key == item.Key &&
                   ItemLabel == item.ItemLabel &&
                   FCPrice == item.FCPrice &&
                   PossibleFlag == item.PossibleFlag &&
                   PossibleItemLabel == item.PossibleItemLabel &&
                   ArrivalStatus == item.ArrivalStatus &&
                   ObtainFlag == item.ObtainFlag && 
                   ProperSteal == item.ProperSteal);
        }
    }
}
