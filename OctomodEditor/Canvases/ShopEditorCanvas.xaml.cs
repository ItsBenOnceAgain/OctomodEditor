using OctomodEditor.Models;
using OctomodEditor.Utilities;
using OctomodEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OctomodEditor.Canvases
{
    /// <summary>
    /// Interaction logic for ShopEditorCanvas.xaml
    /// </summary>
    public partial class ShopEditorCanvas : UserControl
    {
        public ShopViewModel ViewModel { get; private set; }
        public List<ShopInfo> ShopsToSave { get; set; }
        public List<PurchaseItem> PurchaseItemsToSave { get; set; }

        public ShopEditorCanvas()
        {
            InitializeComponent();

            ShopsToSave = new List<ShopInfo>();
            PurchaseItemsToSave = new List<PurchaseItem>();

            ViewModel = new ShopViewModel(ShopInfoParser.ParseShopInfoObjects(), ShopListParser.ParseShopListObjects(), PurchaseItemTableParser.ParsePurchaseItemObjects());
            this.DataContext = ViewModel;

            UpdateCurrentSelectableShopList((string)CategoryComboBox.SelectedValue);
            ShopComboBox.SelectedItem = ViewModel.CurrentShop;

            UpdateComboBoxes();
        }

        private void LoadPurchaseItemRows()
        {
            PurchaseItemPanel.Children.Clear();
            if(ViewModel.CurrentPurchaseItems != null)
            {
                foreach (var purchaseItem in ViewModel.CurrentPurchaseItems)
                {
                    var parentGrid = new Grid();
                    parentGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    parentGrid.ColumnDefinitions.Add(new ColumnDefinition());

                    var baseItemComboBox = new ComboBox();
                    baseItemComboBox.ItemsSource = ViewModel.AllItems;
                    baseItemComboBox.SelectedItem = ViewModel.AllItems.Single(x => x.Key == purchaseItem.ItemLabel);
                    baseItemComboBox.Margin = new Thickness(5, 5, 5, 5);
                    baseItemComboBox.DisplayMemberPath = "Value";
                    baseItemComboBox.SelectionChanged += PurchaseItemBaseComboBox_SelectionChanged;
                    Grid.SetColumn(baseItemComboBox, 0);
                    parentGrid.Children.Add(baseItemComboBox);

                    var necessaryItemComboBox = new ComboBox();
                    necessaryItemComboBox.ItemsSource = ViewModel.AllItems;
                    necessaryItemComboBox.SelectedItem = ViewModel.AllItems.Single(x => x.Key == purchaseItem.PossibleItemLabel);
                    necessaryItemComboBox.Margin = new Thickness(5, 5, 5, 5);
                    necessaryItemComboBox.SelectionChanged += PurchaseItemNecessaryComboBox_SelectionChanged;
                    Grid.SetColumn(necessaryItemComboBox, 1);
                    parentGrid.Children.Add(necessaryItemComboBox);

                    PurchaseItemPanel.Children.Add(parentGrid);
                }
            }
        }

        private void UpdateComboBoxes()
        {
            ShopTypeComboBox.SelectedItem = ViewModel.ShopTypes.Single(x => x == ViewModel.CurrentShop.ShopType);
            InnDiscountItemComboBox.SelectedItem = ViewModel.AllItems.Single(x => x.Key == ViewModel.CurrentShop.InnDiscountItem);

            LoadPurchaseItemRows();
        }

        private void UpdateCurrentSelectableShopList(string s)
        {
            switch (s)
            {
                case "Frostlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("Snow")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Flatlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("Plain")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Coastlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("Sea")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Highlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("Mount")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Sunlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("Desert")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Riverlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("River")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Cliftlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("Cliff")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Woodlands":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("Forest")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "NPCs":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("NPC")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
                case "Unused":
                    ViewModel.CurrentSelectableShopInfoRecords = ViewModel.AllShopInfoRecords.Where(x => x.Key.StartsWith("MINATO")).Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                    break;
            }
        }

        private void ShopComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentPurchaseItems = new List<PurchaseItem>();
            ViewModel.CurrentShop = (ShopInfo)ShopComboBox.SelectedItem;
            if (ViewModel.AllShopListRecords.ContainsKey(ViewModel.CurrentShop.Key))
            {
                ViewModel.CurrentShopList = ViewModel.AllShopListRecords[ViewModel.CurrentShop.Key];
                
                foreach(string itemKey in ViewModel.CurrentShopList.PurchaseItemIDs)
                {
                    ViewModel.CurrentPurchaseItems.Add(ViewModel.AllPurchaseItemRecords[itemKey]);
                }
            }
            else
            {
                ViewModel.CurrentShopList = null;
                ViewModel.CurrentPurchaseItems = null;
            }
            UpdateComboBoxes();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCurrentSelectableShopList((string)CategoryComboBox.SelectedValue);
        }

        private void ShopTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentShop.ShopType = (ShopType)ShopTypeComboBox.SelectedItem;
            UpdateShopsToSave();
        }

        private void InnBaseTypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterShopInfoList[ViewModel.CurrentShop.Key].InnBasePrice);
            AnyTextBox_TextChanged(sender, e);
        }

        private void InnDiscountPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterShopInfoList[ViewModel.CurrentShop.Key].InnDiscountBasePrice);
            AnyTextBox_TextChanged(sender, e);
        }

        private void InnDiscountItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentShop.InnDiscountItem = ((KeyValuePair<string, string>)InnDiscountItemComboBox.SelectedItem).Key;
            UpdateShopsToSave();
        }

        private void PurchaseItemBaseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            int index = Grid.GetRow(comboBox);

            ViewModel.CurrentPurchaseItems[index].ItemLabel = ((KeyValuePair<string, string>)comboBox.SelectedItem).Key;
            UpdateShopsToSave();
        }

        private void PurchaseItemNecessaryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            int index = Grid.GetRow(comboBox);

            ViewModel.CurrentPurchaseItems[index].PossibleItemLabel = ((KeyValuePair<string, string>)comboBox.SelectedItem).Key;
            UpdateShopsToSave();
        }

        private void AnyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateShopsToSave();
        }

        private void UpdateShopsToSave()
        {
            if (ShopsToSave.Select(x => x.Key).Contains(ViewModel.CurrentShop.Key))
            {
                ShopsToSave.Remove(ShopsToSave.Single(x => x.Key == ViewModel.CurrentShop.Key));
            }
            if (ViewModel.CurrentShop.IsDifferentFrom(MainWindow.ModShopInfoList[ViewModel.CurrentShop.Key]))
            {
                ShopsToSave.Add(ViewModel.CurrentShop);
                SaveShopButton.IsEnabled = true;
                DiscardChangesButton.IsEnabled = true;
            }

            foreach(var purchaseItem in ViewModel.CurrentPurchaseItems)
            {
                if (PurchaseItemsToSave.Select(x => x.Key).Contains(purchaseItem.Key))
                {
                    PurchaseItemsToSave.Remove(PurchaseItemsToSave.Single(x => x.Key == purchaseItem.Key));
                }
                if (purchaseItem.IsDifferentFrom(MainWindow.ModPurchaseItemList[purchaseItem.Key]))
                {
                    PurchaseItemsToSave.Add(purchaseItem);
                    SaveShopButton.IsEnabled = true;
                    DiscardChangesButton.IsEnabled = true;
                }
            }

            if (ShopsToSave.Count == 0 && PurchaseItemsToSave.Count == 0)
            {
                SaveShopButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
            }
        }

        private void ChangeTextBoxColor(TextBox sender, int valueToCompare)
        {
            bool parsed = int.TryParse(sender.Text, out int newValue);
            if (parsed)
            {
                if (newValue != valueToCompare)
                {
                    sender.Foreground = new SolidColorBrush(Color.FromRgb(0, 150, 150));
                }
                else
                {
                    sender.Foreground = Brushes.LightGray;
                }
            }
        }

        private void SaveShopButton_Click(object sender, RoutedEventArgs e)
        {
            string shopList = "";
            foreach (var shop in ShopsToSave)
            {
                shopList += shop.ToString() + "\n";
            }

            string purchaseItemList = "";
            foreach (var item in PurchaseItemsToSave)
            {
                purchaseItemList += item.ToString() + "\n";
            }
            var messageString = "";
            if(shopList.Length > 0)
            {
                messageString += $"The following shops have been modified: \n\n{shopList}\n\n";
            }
            if(purchaseItemList.Length > 0)
            {
                messageString += $"The following items in shops have been modified: \n\n{purchaseItemList}\n\n";
            }
            messageString += "These changes will be saved. Is that OK?";
            var result = MessageBox.Show(messageString, "", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                ShopInfoParser.SaveShopInfoObjects(ShopsToSave);
                ShopsToSave.Clear();
                PurchaseItemTableParser.SavePurchaseItemObjects(PurchaseItemsToSave);
                PurchaseItemsToSave.Clear();
                SaveShopButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
                MainWindow.ModShopInfoList = ShopInfoParser.ParseShopInfoObjects();
                MainWindow.ModShopListList = ShopListParser.ParseShopListObjects();
                MainWindow.ModPurchaseItemList = PurchaseItemTableParser.ParsePurchaseItemObjects();
            }
        }

        private void DiscardChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string shopList = "";
            foreach (var shop in ShopsToSave)
            {
                shopList += shop.ToString() + "\n";
            }

            string purchaseItemList = "";
            foreach (var item in PurchaseItemsToSave)
            {
                purchaseItemList += item.ToString() + "\n";
            }
            var messageString = "";
            if (shopList.Length > 0)
            {
                messageString += $"The following shops have been modified: \n\n{shopList}\n\n";
            }
            if (purchaseItemList.Length > 0)
            {
                messageString += $"The following items in shops have been modified: \n\n{purchaseItemList}\n\n";
            }
            messageString += "These changes will be discarded. Is that OK?";
            var result = MessageBox.Show(messageString, "", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                ShopsToSave.Clear();
                PurchaseItemsToSave.Clear();
                ViewModel.AllShopInfoRecords = ShopInfoParser.ParseShopInfoObjects();
                ViewModel.AllShopListRecords = ShopListParser.ParseShopListObjects();
                ViewModel.AllPurchaseItemRecords = PurchaseItemTableParser.ParsePurchaseItemObjects();
                ViewModel.CurrentShop = ViewModel.AllShopInfoRecords.Single(x => x.Key == ViewModel.CurrentShop.Key).Value;
                UpdateCurrentSelectableShopList((string)CategoryComboBox.SelectedValue);
                ShopComboBox.SelectedItem = ViewModel.CurrentShop;
                SaveShopButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
            }
        }
    }
}
