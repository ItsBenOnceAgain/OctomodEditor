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
    /// Interaction logic for AbilityEditorCanvas.xaml
    /// </summary>
    public partial class AbilityEditorCanvas : UserControl
    {
        public AbilityViewModel ViewModel { get; private set; }
        public List<Ability> AbilitiesToSave { get; set; }

        public AbilityEditorCanvas()
        {
            InitializeComponent();

            AbilitiesToSave = new List<Ability>();

            ViewModel = new AbilityViewModel(AbilityDataParser.ParseAbilityObjects());
            this.DataContext = ViewModel;

            UpdateCurrentAbilityList((string)CategoryComboBox.SelectedValue);
            AbilityComboBox.SelectedItem = ViewModel.CurrentAbility;

            UpdateComboBoxes();
        }

        private void UpdateComboBoxes()
        {
            //ItemCategoryComboBox.SelectedItem = ViewModel.Categories.Single(x => x == ViewModel.CurrentItem.Category);
            //DisplayTypeComboBox.SelectedItem = ViewModel.DisplayTypes.Single(x => x == ViewModel.CurrentItem.DisplayType);
            //UseTypeComboBox.SelectedItem = ViewModel.UseTypes.Single(x => x == ViewModel.CurrentItem.UseType);
            //TargetTypeComboBox.SelectedItem = ViewModel.TargetTypes.Single(x => x == ViewModel.CurrentItem.TargetType);
            //AttributeTypeComboBox.SelectedItem = ViewModel.AttributeTypes.Single(x => x == ViewModel.CurrentItem.AttributeType);
            //EquipmentCategoryComboBox.SelectedItem = ViewModel.EquipmentCategories.Single(x => x == ViewModel.CurrentItem.EquipmentCategory);
            //Effect1NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[0].AilmentName);
            //Effect2NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[1].AilmentName);
            //Effect3NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[2].AilmentName);
            //Effect4NameComboBox.SelectedItem = ViewModel.EffectNames.Single(x => x == ViewModel.CurrentItem.Ailments[3].AilmentName);
            //DetailTextIDComboBox.SelectedItem = ViewModel.DetailIDs.Single(x => x == ViewModel.CurrentItem.DetailTextID);
            //IconLabelComboBox.SelectedItem = ViewModel.IconLabels.Single(x => x == ViewModel.CurrentItem.IconLabelID);
        }

        private void UpdateCurrentAbilityList(string s)
        {
            //switch (s)
            //{
            //    case "Player":
            //        ViewModel.CurrentAbilityList = ViewModel.AbilityList.Where(x => x.Value. == ItemCategory.CONSUMABLE).Select(x => x.Value).OrderBy(x => MainWindow.ModGameText[x.DisplayName]).ToList();
            //        break;
            //}
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AbilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
