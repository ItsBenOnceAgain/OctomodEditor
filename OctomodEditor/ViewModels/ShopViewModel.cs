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
    public class ShopViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, ShopInfo> _allShopInfoRecords;
        private Dictionary<string, ShopList> _allShopListRecords;
        private Dictionary<string, PurchaseItem> _allPurchaseItemRecords;
        private ShopInfo _currentShop;
        private ShopList _currentShopList;
        private List<PurchaseItem> _currentPurchaseItems;
        private List<ShopInfo> _currentSelectableShopInfoRecords;

        public List<ShopType> ShopTypes { get; set; }
        public Dictionary<string, string> AllItems { get; set; }

        public Dictionary<string, ShopInfo> AllShopInfoRecords
        {
            get
            {
                return _allShopInfoRecords;
            }
            set
            {
                _allShopInfoRecords = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, ShopList> AllShopListRecords
        {
            get
            {
                return _allShopListRecords;
            }
            set
            {
                _allShopListRecords = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, PurchaseItem> AllPurchaseItemRecords
        {
            get
            {
                return _allPurchaseItemRecords;
            }
            set
            {
                _allPurchaseItemRecords = value;
                OnPropertyChanged();
            }
        }

        public ShopInfo CurrentShop
        {
            get
            {
                return _currentShop;
            }
            set
            {
                if (!(value is null))
                {
                    _currentShop = value;
                    OnPropertyChanged();
                }
            }
        }

        public ShopList CurrentShopList
        {
            get
            {
                return _currentShopList;
            }
            set
            {
                if (!(value is null))
                {
                    _currentShopList = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<PurchaseItem> CurrentPurchaseItems
        {
            get
            {
                return _currentPurchaseItems;
            }
            set
            {
                if (!(value is null))
                {
                    _currentPurchaseItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<ShopInfo> CurrentSelectableShopInfoRecords
        {
            get
            {
                return _currentSelectableShopInfoRecords;
            }
            set
            {
                _currentSelectableShopInfoRecords = value;
                OnPropertyChanged();
            }
        }

        public ShopViewModel(Dictionary<string, ShopInfo> shops, Dictionary<string, ShopList> shopLists, Dictionary<string, PurchaseItem> purchaseItems)
        {
            AllShopInfoRecords = shops;
            AllShopListRecords = shopLists;
            AllPurchaseItemRecords = purchaseItems;

            CurrentShop = AllShopInfoRecords.First().Value;
            if (AllShopListRecords.ContainsKey(CurrentShop.Key))
            {
                CurrentShopList = AllShopListRecords[CurrentShop.Key];
            }

            ShopTypes = Enum.GetValues(typeof(ShopType)).Cast<ShopType>().ToList();
            AllItems = GetItemNames();
        }

        private Dictionary<string, string> GetItemNames()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach (var item in MainWindow.ModItemList)
            {
                items.Add(item.Value.Key, MainWindow.ModGameText[item.Value.ItemNameID].ToString());
            }
            var finalDictionary = items.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            finalDictionary.Add("None", "None");
            return finalDictionary;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
