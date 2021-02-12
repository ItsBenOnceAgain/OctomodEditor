using OctomodEditor.Models;
using OctomodEditor.Utilities;
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
    /// Interaction logic for EnemyEditorCanvas.xaml
    /// </summary>
    public partial class EnemyEditorCanvas : UserControl
    {
        public EnemyViewModel ViewModel { get; private set; }
        public List<Enemy> EnemiesToSave { get; set; }
        public bool ValueChanged { get; set; }
        public EnemyEditorCanvas(EnemyViewModel enemies)
        {
            InitializeComponent();

            ValueChanged = false;
            EnemiesToSave = new List<Enemy>();

            ViewModel = enemies;
            ViewModel.AllItems = GetItemNames();

            EnemyNameLabel.DataContext = ViewModel;
            EnemyIdLabel.DataContext = ViewModel;
            EnemyComboBox.DataContext = ViewModel;
            EnemyHPTextBox.DataContext = ViewModel;
            EnemyShieldTextBox.DataContext = ViewModel;
            EnemyPAttackTextBox.DataContext = ViewModel;
            EnemyPDefenseTextBox.DataContext = ViewModel;
            EnemyMAttackTextBox.DataContext = ViewModel;
            EnemyMDefenseTextBox.DataContext = ViewModel;
            EnemyAccuracyTextBox.DataContext = ViewModel;
            EnemyEvasionTextBox.DataContext = ViewModel;
            EnemyCriticalTextBox.DataContext = ViewModel;
            EnemySpeedTextBox.DataContext = ViewModel;
            EnemyExperienceTextBox.DataContext = ViewModel;
            EnemyJobPointsTextBox.DataContext = ViewModel;
            EnemyLeavesTextBox.DataContext = ViewModel;
            EnemyCollectTextBox.DataContext = ViewModel;
            EnemyItemPercentageTextBox.DataContext = ViewModel;
            EnemyTameRateTextBox.DataContext = ViewModel;
            EnemyLevelTextBox.DataContext = ViewModel;
            EnemyDamageRatioTextBox.DataContext = ViewModel;
            EnemyItemComboBox.DataContext = ViewModel;
            EnemySizeComboBox.DataContext = ViewModel;
            EnemySwordWeakCheckBox.DataContext = ViewModel;
            EnemySpearWeakCheckBox.DataContext = ViewModel;
            EnemyDaggerWeakCheckBox.DataContext = ViewModel;
            EnemyAxeWeakCheckBox.DataContext = ViewModel;
            EnemyBowWeakCheckBox.DataContext = ViewModel;
            EnemyStaffWeakCheckBox.DataContext = ViewModel;
            EnemyFireWeakCheckBox.DataContext = ViewModel;
            EnemyIceWeakCheckBox.DataContext = ViewModel;
            EnemyLightningWeakCheckBox.DataContext = ViewModel;
            EnemyWindWeakCheckBox.DataContext = ViewModel;
            EnemyLightWeakCheckBox.DataContext = ViewModel;
            EnemyDarkWeakCheckBox.DataContext = ViewModel;
            EnemyAttribute1ComboBox.DataContext = ViewModel;
            EnemyAttribute2ComboBox.DataContext = ViewModel;
            EnemyAttribute3ComboBox.DataContext = ViewModel;
            EnemyAttribute4ComboBox.DataContext = ViewModel;
            EnemyAttribute5ComboBox.DataContext = ViewModel;
            EnemyAttribute6ComboBox.DataContext = ViewModel;
            EnemyAttribute7ComboBox.DataContext = ViewModel;
            EnemyAttribute8ComboBox.DataContext = ViewModel;
            EnemyAttribute9ComboBox.DataContext = ViewModel;
            EnemyAttribute10ComboBox.DataContext = ViewModel;
            EnemyAttribute11ComboBox.DataContext = ViewModel;
            EnemyAttribute12ComboBox.DataContext = ViewModel;
            EnemyIsNPCCheckBox.DataContext = ViewModel;
            EnemySlowDeathCheckBox.DataContext = ViewModel;
            EnemyIsMainEnemyCheckBox.DataContext = ViewModel;
            EnemyBattleExemptCheckBox.DataContext = ViewModel;
            EnemyCatDamageCheckBox.DataContext = ViewModel;
            EnemyKnockbackAnimationCheckBox.DataContext = ViewModel;
            EnemyCapturableCheckBox.DataContext = ViewModel;
            EnemyRobbableCheckBox.DataContext = ViewModel;

            UnusedStatsGrid.DataContext = ShowUnusedCheckBox;
            AdvancedStatsGrid.DataContext = ShowAdvancedCheckBox;
            BattleEventsGrid.DataContext = ShowAdvancedCheckBox;

            ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith("ENE_Mou")).Select(x => x.Value).ToList();
            EnemyComboBox.SelectedIndex = 0;
            UpdateComboBoxes();
        }

        private void EnemyComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ValueChanged)
            {
                if (!EnemiesToSave.Any(x => x.Key == ViewModel.CurrentEnemy.Key))
                {
                    EnemiesToSave.Add(ViewModel.CurrentEnemy);
                }
                ValueChanged = false;
            }
            
            ViewModel.CurrentEnemy = (Enemy)EnemyComboBox.SelectedItem;
            UpdateComboBoxes();
        }

        private void SaveEnemyButton_Click(object sender, RoutedEventArgs e) {

        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] prefixes = { "ENE_Sno", "ENE_Pla", "ENE_Sea", "ENE_Mou", "ENE_Des", "ENE_Riv", "ENE_Cri", "ENE_For", "BOS_", "EVE_", "BT_ENE_NPC" };
            switch ((string)CategoryComboBox.SelectedValue)
            {
                case "Frostlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[0])).Select(x => x.Value).ToList();
                    break;
                case "Flatlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[1])).Select(x => x.Value).ToList();
                    break;
                case "Coastlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[2])).Select(x => x.Value).ToList();
                    break;
                case "Highlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[3])).Select(x => x.Value).ToList();
                    break;
                case "Sunlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[4])).Select(x => x.Value).ToList();
                    break;
                case "Riverlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[5])).Select(x => x.Value).ToList();
                    break;
                case "Cliftlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[6])).Select(x => x.Value).ToList();
                    break;
                case "Woodlands":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[7])).Select(x => x.Value).ToList();
                    break;
                case "Bosses":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[8])).Select(x => x.Value).ToList();
                    break;
                case "Events":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[9])).Select(x => x.Value).ToList();
                    break;
                case "NPCs":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith(prefixes[10])).Select(x => x.Value).ToList();
                    break;
                case "Other":
                    ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => !prefixes.Any(y => x.Key.StartsWith(y))).Select(x => x.Value).ToList();
                    break;
            }
        }

        private Dictionary<string, string> GetItemNames()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            items.Add("None", "None");
            foreach (var itemId in ViewModel.EnemyList.Where(x => x.Value.ItemID != "None").Select(x => x.Value.ItemID).Distinct())
            {
                if (MainWindow.MasterGameText.ContainsKey($"TX_NA_{itemId}"))
                {
                    items.Add(itemId, MainWindow.MasterGameText[$"TX_NA_{itemId}"]);
                }
                else
                {
                    string[] idInfo = itemId.Split('_');
                    int identifier = int.Parse(idInfo[2]);
                    if (idInfo[1] == "MB")
                    {
                        identifier += 16;
                    }
                    items.Add(itemId, MainWindow.MasterGameText[$"MIX_ITM_NA_{identifier:D3}"]);
                }
            }
            return items;
        }

        private void UpdateComboBoxes()
        {
            EnemyItemComboBox.SelectedItem = ViewModel.AllItems.Single(x => x.Key == ViewModel.CurrentEnemy.ItemID);
            EnemySizeComboBox.SelectedItem = ViewModel.Sizes.Single(x => x == ViewModel.CurrentEnemy.Size);
            EnemyAttribute1ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[0]);
            EnemyAttribute2ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[1]);
            EnemyAttribute3ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[2]);
            EnemyAttribute4ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[3]);
            EnemyAttribute5ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[4]);
            EnemyAttribute6ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[5]);
            EnemyAttribute7ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[6]);
            EnemyAttribute8ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[7]);
            EnemyAttribute9ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[8]);
            EnemyAttribute10ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[9]);
            EnemyAttribute11ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[10]);
            EnemyAttribute12ComboBox.SelectedItem = ViewModel.Resistances.Single(x => x == ViewModel.CurrentEnemy.AttributeResistances[11]);
        }

        private void EnemyItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.ItemID = ((KeyValuePair<string, string>)EnemyItemComboBox.SelectedItem).Key;
            ValueChanged = true;
        }

        private void EnemySizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.Size = (CharacterSize)EnemySizeComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[0] = (AttributeResistance)EnemyAttribute1ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[1] = (AttributeResistance)EnemyAttribute2ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute3ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[2] = (AttributeResistance)EnemyAttribute3ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute4ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[3] = (AttributeResistance)EnemyAttribute4ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute5ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[4] = (AttributeResistance)EnemyAttribute5ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute6ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[5] = (AttributeResistance)EnemyAttribute6ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute7ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[6] = (AttributeResistance)EnemyAttribute7ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute8ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[7] = (AttributeResistance)EnemyAttribute8ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute9ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[8] = (AttributeResistance)EnemyAttribute9ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute10ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[9] = (AttributeResistance)EnemyAttribute10ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute11ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[10] = (AttributeResistance)EnemyAttribute11ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void EnemyAttribute12ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[11] = (AttributeResistance)EnemyAttribute12ComboBox.SelectedItem;
            ValueChanged = true;
        }

        private void DiscardChangesButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
