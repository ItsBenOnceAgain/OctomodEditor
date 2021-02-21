using OctomodEditor.ViewModels;
using OctomodEditor.Models;
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
using OctomodEditor.Utilities;

namespace OctomodEditor.Canvases
{
    /// <summary>
    /// Interaction logic for ItemEditorCanvas.xaml
    /// </summary>
    public partial class ItemEditorCanvas : UserControl
    {
        public ItemViewModel ViewModel { get; private set; }
        public List<Item> ItemsToSave { get; set; }
        public ItemEditorCanvas()
        {
            InitializeComponent();

            ItemsToSave = new List<Item>();

            ViewModel = new ItemViewModel(ItemDBParser.ParseItemObjects());
            this.DataContext = ViewModel;

            ViewModel.CurrentItemList = ViewModel.ItemList.Where(x => x.Value.Category == ItemCategory.CONSUMABLE).Select(x => x.Value).OrderBy(x => MainWindow.MasterGameText[x.ItemNameID]).ToList();
            ItemComboBox.SelectedItem = ViewModel.CurrentItem;

            UpdateComboBoxes();
        }

        private void UpdateComboBoxes()
        {
            ItemCategoryComboBox.SelectedItem = ViewModel.Categories.Single(x => x == ViewModel.CurrentItem.Category);
            DisplayTypeComboBox.SelectedItem = ViewModel.DisplayTypes.Single(x => x == ViewModel.CurrentItem.DisplayType);
            UseTypeComboBox.SelectedItem = ViewModel.UseTypes.Single(x => x == ViewModel.CurrentItem.UseType);
            TargetTypeComboBox.SelectedItem = ViewModel.TargetTypes.Single(x => x == ViewModel.CurrentItem.TargetType);
            AttributeTypeComboBox.SelectedItem = ViewModel.AttributeTypes.Single(x => x == ViewModel.CurrentItem.AttributeType);
            EquipmentCategoryComboBox.SelectedItem = ViewModel.EquipmentCategories.Single(x => x == ViewModel.CurrentItem.EquipmentCategory);
            Effect1NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[0].AilmentName);
            Effect2NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[1].AilmentName);
            Effect3NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[2].AilmentName);
            Effect4NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[3].AilmentName);
            DetailTextIDComboBox.SelectedItem = ViewModel.DetailIDs.Single(x => x == ViewModel.CurrentItem.DetailTextID);
            IconLabelComboBox.SelectedItem = ViewModel.IconLabels.Single(x => x == ViewModel.CurrentItem.IconLabelID);
        }

        private void UpdateItemsToSave()
        {
            if (ItemsToSave.Select(x => x.Key).Contains(ViewModel.CurrentItem.Key))
            {
                ItemsToSave.Remove(ItemsToSave.Single(x => x.Key == ViewModel.CurrentItem.Key));
            }
            ItemsToSave.Add(ViewModel.CurrentItem);
            SaveItemButton.IsEnabled = true;
            DiscardChangesButton.IsEnabled = true;
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

        private void ItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem = (Item)ItemComboBox.SelectedItem;
            UpdateComboBoxes();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((string)CategoryComboBox.SelectedValue)
            {
                case "Standard":
                    ViewModel.CurrentItemList = ViewModel.ItemList.Where(x => x.Value.Category == ItemCategory.CONSUMABLE).Select(x => x.Value).OrderBy(x => MainWindow.MasterGameText[x.ItemNameID]).ToList();
                    break;
                case "Equipment":
                    ViewModel.CurrentItemList = ViewModel.ItemList.Where(x => x.Value.Category == ItemCategory.EQUIPMENT).Select(x => x.Value).OrderBy(x => MainWindow.MasterGameText[x.ItemNameID]).ToList();
                    break;
                case "Information":
                    ViewModel.CurrentItemList = ViewModel.ItemList.Where(x => x.Value.Category == ItemCategory.INFORMATION).Select(x => x.Value).OrderBy(x => MainWindow.MasterGameText[x.ItemNameID]).ToList();
                    break;
                case "Concoct Materials":
                    ViewModel.CurrentItemList = ViewModel.ItemList.Where(x => x.Value.Category == ItemCategory.MATERIAL_A || x.Value.Category == ItemCategory.MATERIAL_B).Select(x => x.Value).OrderBy(x => MainWindow.MasterGameText[x.ItemNameID]).ToList();
                    break;
                case "Key Items":
                    ViewModel.CurrentItemList = ViewModel.ItemList.Where(x => x.Value.Category == ItemCategory.TREASURE).Select(x => x.Value).OrderBy(x => MainWindow.MasterGameText[x.ItemNameID]).ToList();
                    break;
            }
        }

        private void ItemCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.Category = (ItemCategory)ItemCategoryComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void DisplayTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.DisplayType = (ItemDisplayType)DisplayTypeComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void UseTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.UseType = (ItemUseType)UseTypeComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void TargetTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.TargetType = (ItemTargetType)TargetTypeComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void AttributeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.AttributeType = (ItemAttributeType)AttributeTypeComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void EquipmentCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.EquipmentCategory = (ItemEquipmentCategory)EquipmentCategoryComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void IsValuableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void BuyPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].BuyPrice);
            UpdateItemsToSave();
        }

        private void SellPriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].SellPrice);
            UpdateItemsToSave();
        }

        private void Effect1NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.Ailments[0].AilmentName = (string)Effect1NameComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void Effect1ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[0].InvocationValue);
            UpdateItemsToSave();
        }

        private void Effect1TurnsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[0].InvocationTurn);
            UpdateItemsToSave();
        }

        private void Effect1InflictPercentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[0].DiseaseRate);
            UpdateItemsToSave();
        }

        private void Effect2NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.Ailments[1].AilmentName = (string)Effect2NameComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void Effect2ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[1].InvocationValue);
            UpdateItemsToSave();
        }

        private void Effect2TurnsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[1].InvocationTurn);
            UpdateItemsToSave();
        }

        private void Effect2InflictPercentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[1].DiseaseRate);
            UpdateItemsToSave();
        }

        private void Effect3NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.Ailments[2].AilmentName = (string)Effect3NameComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void Effect3ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[2].InvocationValue);
            UpdateItemsToSave();
        }

        private void Effect3TurnsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[2].InvocationTurn);
            UpdateItemsToSave();
        }

        private void Effect3InflictPercentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[2].DiseaseRate);
            UpdateItemsToSave();
        }

        private void Effect4NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.Ailments[3].AilmentName = (string)Effect4NameComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void Effect4ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[3].InvocationValue);
            UpdateItemsToSave();
        }

        private void Effect4TurnsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[3].InvocationTurn);
            UpdateItemsToSave();
        }

        private void Effect4InflictPercentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].Ailments[3].DiseaseRate);
            UpdateItemsToSave();
        }

        private void HPRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].HPRevision);
            UpdateItemsToSave();
        }

        private void MPRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].MPRevision);
            UpdateItemsToSave();
        }

        private void BPRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].BPRevision);
            UpdateItemsToSave();
        }

        private void PAttackRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].PAttackRevision);
            UpdateItemsToSave();
        }

        private void PDefenseRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].PDefenseRevision);
            UpdateItemsToSave();
        }

        private void MAttackRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].MAttackRevision);
            UpdateItemsToSave();
        }

        private void MDefenseRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].MDefenseRevision);
            UpdateItemsToSave();
        }

        private void AccuracyRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].AccuracyRevision);
            UpdateItemsToSave();
        }

        private void EvasionRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].EvasionRevision);
            UpdateItemsToSave();
        }

        private void CriticalRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].CriticalRevision);
            UpdateItemsToSave();
        }

        private void SpeedRevisionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].SpeedRevision);
            UpdateItemsToSave();
        }

        private void PoisonResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void SilenceResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void BlindnessResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void ConfusionResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void SleepResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void TerrorResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void UnconciousnessResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void InstantDeathResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void TransformResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void DebuffResistanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateItemsToSave();
        }

        private void SaveItemButton_Click(object sender, RoutedEventArgs e)
        {
            string itemList = "";
            foreach (var item in ItemsToSave)
            {
                itemList += item.ToString() + "\n";
            }
            var result = MessageBox.Show($"The following items will be saved. Is that OK?\n\n{itemList}", "", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                //ItemDBParser.SaveEnemies(ItemsToSave);
                ItemsToSave.Clear();
                SaveItemButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
            }
        }

        private void DiscardChangesButton_Click(object sender, RoutedEventArgs e)
        {
            // Save Enemy Here
            string itemList = "";
            foreach (var item in ItemsToSave)
            {
                itemList += item.ToString() + "\n";
            }
            var result = MessageBox.Show($"Changes to the following items will be discarded. Is that OK?\n\n{itemList}", "", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                ItemsToSave.Clear();
                ViewModel.ItemList = ItemDBParser.ParseItemObjects();
                ViewModel.CurrentItem = ViewModel.ItemList.Single(x => x.Key == ViewModel.CurrentItem.Key).Value;
                SaveItemButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ItemsToSave.Clear();
            SaveItemButton.IsEnabled = false;
            DiscardChangesButton.IsEnabled = false;
        }

        private void DetailTextIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.DetailTextID = (string)DetailTextIDComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void IconLabelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentItem.IconLabelID = (string)IconLabelComboBox.SelectedItem;
            UpdateItemsToSave();
        }

        private void SortCategoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterItemList[ViewModel.CurrentItem.Key].SortCategory);
            UpdateItemsToSave();
        }
    }
}
