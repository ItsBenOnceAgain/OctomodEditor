using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class PurchaseItem
    {
        public int Offset { get; set; }
        public string Key { get; set; }
        public string ItemLabel { get; set; }
        public int FCPrice { get; set; }
        public int PossibleFlag { get; set; }
        public string PossibleItemLabel { get; set; }
        public int ArrivalStatus { get; set; }
        public int ObtainFlag { get; set; }
        public int ProperSteal { get; set; }
    }
}
