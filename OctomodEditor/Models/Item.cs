using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.Models
{
    public class Item
    {
        public int Offset { get; set; }
        public string Key { get; set; }
        public int ItemID { get; set; }
        public string ItemNameID { get; set; }
        public string DetailTextID { get; set; }
        public string IconLabelID { get; set; }
        public ItemCategory Category { get; set; }
        public int SortCategory { get; set; }
        public ItemDisplayType DisplayType { get; set; }
        public ItemUseType UseType { get; set; }
        public ItemTargetType TargetType { get; set; }
        public ItemAttributeType AttributeType { get; set; }
        public Ailment[] Ailments { get; set; }
        public bool IsValuable { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public ItemEquipmentCategory EquipmentCategory { get; set; }
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
        public string CommandEffecterPath { get; set; }
    }

    public enum ItemCategory { CONSUMABLE, EQUIPMENT, INFORMATION, MATERIAL_A, MATERIAL_B, TRADING, TREASURE}
    public enum ItemDisplayType { DISABLE, ITEM_USE, ON_HIT, BATTLE_START, ON_TAKE_DAMAGE }
    public enum ItemUseType { DISABLE, ALWAYS, FIELD_ONLY, BATTLE_ONLY }
    public enum ItemTargetType { SELF, PARTY_SINGLE, PARTY_ALL, ENEMY_SINGLE, ENEMY_ALL, ALL, ALL_SINGLE }
    public enum ItemAttributeType { NONE, FIRE, ICE, LIGHTNING, WIND, LIGHT, DARK }
    public enum ItemEquipmentCategory { ACCESSORY, AXE, BEAST, BOW, CLOTH, DAGGER, HAT, HEAVY_ARMOR, HEAVY_HELMET, HUMAN, LANCE, LIGHT_ARMOR, LIGHT_HELMET, PLANT, ROD, SHIELD, SWORD, UNDEAD }
}
