using OctomodEditor.Models;
using OctomodEditor.Parsers;
using OctomodEditor.UserControls;
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
        public AbilityParser Parser { get; set; }

        public AbilityEditorCanvas()
        {
            InitializeComponent();

            AbilitiesToSave = new List<Ability>();
            Parser = new AbilityParser();

            ViewModel = new AbilityViewModel(MainWindow.ModAbilityList);
            this.DataContext = ViewModel;

            UpdateCurrentAbilityList((string)CategoryComboBox.SelectedValue);
            AbilityComboBox.SelectedItem = ViewModel.CurrentAbility;

            UpdateAilmentList();
            UpdateComboBoxes();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AbilitiesToSave.Clear();
            SaveAbilityButton.IsEnabled = false;
            DiscardChangesButton.IsEnabled = false;
        }

        private void UpdateComboBoxes()
        {
            AbilityTypeComboBox.SelectedItem = ViewModel.AbilityTypes.Single(x => x == ViewModel.CurrentAbility.AbilityType);
            AbilityUseTypeComboBox.SelectedItem = ViewModel.AbilityUseTypes.Single(x => x == ViewModel.CurrentAbility.AbilityUseType);
            AttributeComboBox.SelectedItem = ViewModel.AttributeTypes.Single(x => x == ViewModel.CurrentAbility.AbilityAttributeType);
            WeaponRestrictionComboBox.SelectedItem = ViewModel.WeaponCategories.Single(x => x == ViewModel.CurrentAbility.RestrictWeapon);
            TargetTypeComboBox.SelectedItem = ViewModel.TargetTypes.Single(x => x == ViewModel.CurrentAbility.TargetType);
            CostTypeComboBox.SelectedItem = ViewModel.AbilityCostTypes.Single(x => x == ViewModel.CurrentAbility.CostType);
            OrderChangeComboBox.SelectedItem = ViewModel.AbilityOrderChangeTypes.Single(x => x == ViewModel.CurrentAbility.OrderChange);
            SupportTypeComboBox.SelectedItem = ViewModel.SupportAilmentTypes.Single(x => x == ViewModel.CurrentAbility.SupportAilment);
        }

        private void UpdateAilmentList()
        {
            AilmentPanel.Children.Clear();
            for(int i = 0; i < ViewModel.CurrentAbility.Ailments.Length; i++)
            {
                var ailment = ViewModel.CurrentAbility.Ailments[i];
                var control = new AilmentControl($"Effect {i + 1}", ailment, ViewModel.EffectNames, i);

                control.EffectNameComboBox.SelectedItem = control.AilmentNames.Where(x => x == ailment.AilmentName).FirstOrDefault();
                control.EffectNameComboBox.SelectionChanged += AilmentEffectComboBox_SelectionChanged;
                control.EffectValueTextBox.TextChanged += AilmentValueTextBox_TextChanged;
                control.EffectTurnsTextBox.TextChanged += AilmentTurnTextBox_TextChanged;
                control.EffectInflictPercentTextBox.TextChanged += AilmentDiseaseRateTextBox_TextChanged;

                AilmentPanel.Children.Add(control);
            }
        }

        private void UpdateAbilitiesToSave()
        {
            if (!AbilitiesToSave.Select(x => x.Key).Contains(ViewModel.CurrentAbility.Key))
            {
                AbilitiesToSave.Add(ViewModel.CurrentAbility);
                SaveAbilityButton.IsEnabled = true;
                DiscardChangesButton.IsEnabled = true;
            }
        }

        private void UpdateCurrentAbilityList(string s)
        {
            if(ViewModel != null)
            {
                Dictionary<string, List<string>> masterJobAbilityDictionary = MainWindow.ModAbilitySetList.ToDictionary(x => x.Key, x => new List<string>()
                                                                                                                        {
                                                                                                                            x.Value.AbilityBoost0ID,
                                                                                                                            x.Value.AbilityBoost1ID,
                                                                                                                            x.Value.AbilityBoost2ID,
                                                                                                                            x.Value.AbilityBoost3ID
                                                                                                                        });
                List<Ability> allPlayerAbilities = ViewModel.AbilityList.Where(x => masterJobAbilityDictionary.Count(y => y.Value.Contains(x.Key)) > 0)
                                                                             .Select(x => x.Value).OrderBy(x => x.ToString()).ToList();

                List<Ability> playerAbilities = allPlayerAbilities.Where(x => !masterJobAbilityDictionary.Where(y => y.Value.Contains(x.Key)).First().Key.StartsWith("INVADE")
                                                                         && x.AbilityUseType != AbilityUseType.SUPPORT)
                                                                         .OrderBy(x => x.ToString()).ToList();
                List<Ability> supportAbilities = allPlayerAbilities.Where(x => !masterJobAbilityDictionary.Where(y => y.Value.Contains(x.Key)).First().Key.StartsWith("INVADE")
                                                                         && x.AbilityUseType == AbilityUseType.SUPPORT)
                                                                         .OrderBy(x => x.ToString()).ToList();
                List<Ability> invadeAbilities = allPlayerAbilities.Where(x => masterJobAbilityDictionary.Where(y => y.Value.Contains(x.Key)).First().Key.StartsWith("INVADE"))
                                                                          .OrderBy(x => x.ToString()).ToList();
                List<Ability> basicAbilities = ViewModel.AbilityList.Where(x => !allPlayerAbilities.Contains(x.Value) && !x.Key.StartsWith("BT_ABI_BOS"))
                                                                                .Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                List<Ability> bossAbilities = ViewModel.AbilityList.Where(x => !allPlayerAbilities.Contains(x.Value) && x.Key.StartsWith("BT_ABI_BOS"))
                                                                                .Select(x => x.Value).OrderBy(x => x.ToString()).ToList();
                switch (s)
                {
                    case "Player":
                        ViewModel.CurrentAbilityList = playerAbilities;
                        break;
                    case "Support":
                        ViewModel.CurrentAbilityList = supportAbilities;
                        break;
                    case "Basic":
                        ViewModel.CurrentAbilityList = basicAbilities;
                        break;
                    case "Boss":
                        ViewModel.CurrentAbilityList = bossAbilities;
                        break;
                    case "Invade":
                        ViewModel.CurrentAbilityList = invadeAbilities;
                        break;
                }
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

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCurrentAbilityList((string)CategoryComboBox.SelectedValue);
        }

        private void AbilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility = (Ability)AbilityComboBox.SelectedItem;
            UpdateComboBoxes();
            UpdateAilmentList();
        }

        private void AlwaysDisableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableRepeatCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableConvergenceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableSkillAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableItemAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableHideOutCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableCoverCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableCounterCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableChaseAttackCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableEnchantCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableDiffusionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableReflectionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void EnableDisableMagicCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void DependOnWeaponCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void ExceptEnforcerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void KeepBoostEffectCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void CommandActorIDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].CommandActorID);
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void CostValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].CostValue);
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void CriticalRatioTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].CriticalRatio);
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void AbilityRatioTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].AbilityRatio);
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void CommandEffecterIDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].CommandEffecterID);
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void AbilityTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.AbilityType = (AbilityType)AbilityTypeComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void AbilityUseTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.AbilityUseType = (AbilityUseType)AbilityUseTypeComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void AttributeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.AbilityAttributeType = (AttributeType)AttributeComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void WeaponRestrictionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.RestrictWeapon = (WeaponCategory)WeaponRestrictionComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void TargetTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.TargetType = (TargetType)TargetTypeComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void CostTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.CostType = (AbilityCostType)CostTypeComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void SupportTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.SupportAilment = (SupportAilmentType)SupportTypeComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void OrderChangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentAbility.OrderChange = (AbilityOrderChangeType)OrderChangeComboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void AilmentEffectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            int index = ((AilmentControl)comboBox.DataContext).AilmentIndex;

            ViewModel.CurrentAbility.Ailments[index].AilmentName = (string)comboBox.SelectedItem;
            UpdateAbilitiesToSave();
        }

        private void AilmentValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            int index = ((AilmentControl)textBox.DataContext).AilmentIndex;

            ChangeTextBoxColor(textBox, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].Ailments[index].InvocationValue);
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void AilmentTurnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            int index = ((AilmentControl)textBox.DataContext).AilmentIndex;

            ChangeTextBoxColor(textBox, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].Ailments[index].InvocationTurn);
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private void AilmentDiseaseRateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            int index = ((AilmentControl)textBox.DataContext).AilmentIndex;

            ChangeTextBoxColor(textBox, MainWindow.MasterAbilityList[ViewModel.CurrentAbility.Key].Ailments[index].DiseaseRate);
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            UpdateAbilitiesToSave();
        }

        private async void SaveItemButton_Click(object sender, RoutedEventArgs e)
        {
            string abilityList = "";
            foreach (var ability in AbilitiesToSave)
            {
                abilityList += ability.ToString() + "\n";
            }
            var result = MessageBox.Show($"The following abilities will be saved. Is that OK?\n\n{abilityList}", "", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                MainWindow.Instance.StartLoading();
                await Task.Run(() =>
                {
                    Parser.SaveTable(MainWindow.ModAbilityTable, AbilitiesToSave);
                });
                AbilitiesToSave.Clear();
                SaveAbilityButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;

                await MainWindow.Instance.LoadAbilityLists();
                await MainWindow.Instance.LoadAbilitySetLists();
                MainWindow.Instance.StopLoading();

                ViewModel = new AbilityViewModel(MainWindow.ModAbilityList);
            }
        }

        private async void DiscardChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string abilityList = "";
            foreach (var ability in AbilitiesToSave)
            {
                abilityList += ability.ToString() + "\n";
            }
            var result = MessageBox.Show($"Changes to the following abilities will be discarded. Is that OK?\n\n{abilityList}", "", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                AbilitiesToSave.Clear();

                MainWindow.Instance.StartLoading();
                await MainWindow.Instance.LoadAbilityLists();
                await MainWindow.Instance.LoadAbilitySetLists();
                MainWindow.Instance.StopLoading();

                ViewModel.AbilityList = MainWindow.ModAbilityList;
                ViewModel.CurrentAbility = ViewModel.AbilityList.Single(x => x.Key == ViewModel.CurrentAbility.Key).Value;
                UpdateCurrentAbilityList((string)CategoryComboBox.SelectedValue);
                AbilityComboBox.SelectedItem = ViewModel.CurrentAbility;
                SaveAbilityButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
            }
        }
    }
}
