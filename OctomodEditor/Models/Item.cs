using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class Item : OctopathModel
    {
        public int ItemID { get; set; }
        public string ItemNameID { get; set; }
        public string DetailTextID { get; set; }
        public string IconLabelID { get; set; }
        public ItemCategory Category { get; set; }
        public int SortCategory { get; set; }
        public ItemDisplayType DisplayType { get; set; }
        public ItemUseType UseType { get; set; }
        public TargetType TargetType { get; set; }
        public AttributeType AttributeType { get; set; }
        public Ailment[] Ailments { get; set; }
        public bool IsValuable { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public EquipmentCategory EquipmentCategory { get; set; }
        public int HPRevision { get; set; }
        public int MPRevision { get; set; }
        public int BPRevision { get; set; }
        public int SPRevision { get; set; }
        public int PAttackRevision { get; set; }
        public int PDefenseRevision { get; set; }
        public int MAttackRevision { get; set; }
        public int MDefenseRevision { get; set; }
        public int AccuracyRevision { get; set; }
        public int EvasionRevision { get; set; }
        public int CriticalRevision { get; set; }
        public int SpeedRevision { get; set; }
        public AttributeResistance[] AttributeResistances { get; set; }
        public bool ResistPoison { get; set; }
        public bool ResistSilence { get; set; }
        public bool ResistBlindness { get; set; }
        public bool ResistConfusion { get; set; }
        public bool ResistSleep { get; set; }
        public bool ResistTerror { get; set; }
        public bool ResistUnconciousness { get; set; }
        public bool ResistInstantDeath { get; set; }
        public bool ResistTransform { get; set; }
        public bool ResistDebuff { get; set; }
        public int CommandEffecterID { get; set; }

        public string DescriptionText
        {
            get
            {
                return MainWindow.ModGameText.ContainsKey(DetailTextID) ? MainWindow.ModGameText[DetailTextID].Text : DetailTextID;
            }
        }

        public bool IsDifferentFrom(Item item)
        {
            return !(Key == item.Key &&
                   ItemID == item.ItemID &&
                   ItemNameID == item.ItemNameID &&
                   DetailTextID == item.DetailTextID &&
                   IconLabelID == item.IconLabelID &&
                   Category == item.Category &&
                   SortCategory == item.SortCategory &&
                   DisplayType == item.DisplayType &&
                   UseType == item.UseType &&
                   TargetType == item.TargetType &&
                   AttributeType == item.AttributeType &&
                   Ailments.SequenceEqual(item.Ailments) &&
                   IsValuable == item.IsValuable &&
                   BuyPrice == item.BuyPrice &&
                   SellPrice == item.SellPrice &&
                   EquipmentCategory == item.EquipmentCategory &&
                   HPRevision == item.HPRevision &&
                   MPRevision == item.MPRevision &&
                   BPRevision == item.BPRevision &&
                   SPRevision == item.SPRevision &&
                   PAttackRevision == item.PAttackRevision &&
                   PDefenseRevision == item.PDefenseRevision &&
                   MAttackRevision == item.MAttackRevision &&
                   MDefenseRevision == item.MDefenseRevision &&
                   AccuracyRevision == item.AccuracyRevision &&
                   EvasionRevision == item.EvasionRevision &&
                   CriticalRevision == item.CriticalRevision &&
                   SpeedRevision == item.SpeedRevision &&
                   ResistPoison == item.ResistPoison &&
                   ResistSilence == item.ResistSilence &&
                   ResistBlindness == item.ResistBlindness &&
                   ResistConfusion == item.ResistConfusion &&
                   ResistSleep == item.ResistSleep &&
                   ResistTerror == item.ResistTerror &&
                   ResistUnconciousness == item.ResistUnconciousness &&
                   ResistInstantDeath == item.ResistInstantDeath &&
                   ResistTransform == item.ResistTransform &&
                   ResistDebuff == item.ResistDebuff &&
                   CommandEffecterID == item.CommandEffecterID &&
                   DescriptionText == item.DescriptionText);
        }

        public override string ToString()
        {
            return MainWindow.ModGameText.ContainsKey(ItemNameID) ? MainWindow.ModGameText[ItemNameID].ToString() : Key;
        }
    }
}
