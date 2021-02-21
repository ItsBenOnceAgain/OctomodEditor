using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OctomodEditor.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, Item> _itemList;
        private Item _currentItem;
        private List<Item> _currentItemList;

        public List<ItemCategory> Categories { get; set; }
        public List<ItemDisplayType> DisplayTypes { get; set; }
        public List<ItemUseType> UseTypes { get; set; }
        public List<ItemTargetType> TargetTypes { get; set; }
        public List<ItemAttributeType> AttributeTypes { get; set; }
        public List<ItemEquipmentCategory> EquipmentCategories { get; set; }
        public List<string> EffectNames { get; set; }
        public List<string> DetailIDs { get; set; }
        public List<string> IconLabels { get; set; }

        public Dictionary<string, Item> ItemList
        {
            get
            {
                return _itemList;
            }
            set
            {
                _itemList = value;
                OnPropertyChanged();
            }
        }

        public Item CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                if (value != null)
                {
                    _currentItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Item> CurrentItemList
        {
            get
            {
                return _currentItemList;
            }
            set
            {
                _currentItemList = value;
                OnPropertyChanged();
            }
        }

        public ItemViewModel(Dictionary<string, Item> items)
        {
            ItemList = items;
            CurrentItem = items.First().Value;

            Categories = Enum.GetValues(typeof(ItemCategory)).Cast<ItemCategory>().ToList();
            DisplayTypes = Enum.GetValues(typeof(ItemDisplayType)).Cast<ItemDisplayType>().ToList();
            UseTypes = Enum.GetValues(typeof(ItemUseType)).Cast<ItemUseType>().ToList();
            TargetTypes = Enum.GetValues(typeof(ItemTargetType)).Cast<ItemTargetType>().ToList();
            AttributeTypes = Enum.GetValues(typeof(ItemAttributeType)).Cast<ItemAttributeType>().ToList();
            EquipmentCategories = Enum.GetValues(typeof(ItemEquipmentCategory)).Cast<ItemEquipmentCategory>().ToList();

            EffectNames = GetEffectNames();
            DetailIDs = GetDetailIDs();
            IconLabels = GetIconLabels();
        }

        private List<string> GetEffectNames()
        {
            List<string> effects = new List<string>();

            foreach (var item in ItemList.Select(x => x.Value))
            {
                foreach (var ailment in item.Ailments)
                {
                    if (!effects.Contains(ailment.AilmentName))
                    {
                        effects.Add(ailment.AilmentName);
                    }
                }
            }
            effects.Sort();
            return effects;
        }

        private List<string> GetDetailIDs()
        {
            List<string> ids = new List<string>();

            foreach (var item in ItemList.Select(x => x.Value))
            {
                if (!ids.Contains(item.DetailTextID))
                {
                    ids.Add(item.DetailTextID);
                }
            }
            ids.Sort();
            return ids;
        }

        private List<string> GetIconLabels()
        {
            List<string> ids = new List<string>();

            foreach (var item in ItemList.Select(x => x.Value))
            {
                if (!ids.Contains(item.IconLabelID))
                {
                    ids.Add(item.IconLabelID);
                }
            }
            ids.Sort();
            return ids;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
