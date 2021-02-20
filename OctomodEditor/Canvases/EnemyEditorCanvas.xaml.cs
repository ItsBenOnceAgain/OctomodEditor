using OctomodEditor.Models;
using OctomodEditor.ViewModels;
using OctomodEditor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
using WpfAnimatedGif;

namespace OctomodEditor.Canvases
{
    /// <summary>
    /// Interaction logic for EnemyEditorCanvas.xaml
    /// </summary>
    public partial class EnemyEditorCanvas : UserControl
    {
        public EnemyViewModel ViewModel { get; private set; }
        public List<Enemy> EnemiesToSave { get; set; }
        public EnemyEditorCanvas()
        {
            InitializeComponent();

            EnemiesToSave = new List<Enemy>();

            ViewModel = new EnemyViewModel(EnemyDBParser.ParseEnemyObjects());

            this.DataContext = ViewModel;
            InnerUnusedGrid.DataContext = ViewModel;
            InnerAdvancedGrid.DataContext = ViewModel;
            InnerEventsGrid.DataContext = ViewModel;

            UnusedStatsGrid.DataContext = ShowUnusedCheckBox;
            AdvancedStatsGrid.DataContext = ShowAdvancedCheckBox;
            BattleEventsGrid.DataContext = ShowAdvancedCheckBox;

            ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith("ENE_Mou")).Select(x => x.Value).ToList();
            EnemyComboBox.SelectedIndex = 0;

            UpdateComboBoxes();
            LoadImagePreview();
        }

        private void LoadImagePreview()
        {
            //var image = new BitmapImage();
            //image.BeginInit();
            //var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "/Preview/preview.png");
            //var uri = new Uri(path, UriKind.Absolute);
            //image.UriSource = uri;
            //image.EndInit();
            //ImageBehavior.SetAnimatedSource(ImagePreview, image);
        }

        private void LoadAbilityComboBoxes()
        {
            AbilitiesPanel.Children.Clear();
            foreach (var ability in ViewModel.CurrentEnemy.AbilityList)
            {
                var comboBox = new ComboBox();
                comboBox.ItemsSource = ViewModel.AllAbilities;
                comboBox.SelectedItem = ViewModel.AllAbilities.Single(x => x.Key == ability);
                comboBox.Margin = new Thickness(5, 5, 5, 5);
                comboBox.DisplayMemberPath = "Value";
                comboBox.SelectionChanged += AbilityComboBox_SelectionChanged;
                AbilitiesPanel.Children.Add(comboBox);
            }
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
            EnemyRaceComboBox.SelectedItem = ViewModel.Races.Single(x => x == ViewModel.CurrentEnemy.RaceType);
            EnemyFlipbookComboBox.SelectedItem = ViewModel.AllFlipbookPaths.Single(x => x.Key == ViewModel.CurrentEnemy.FlipbookPath);
            EnemyTextureComboBox.SelectedItem = ViewModel.AllTexturePaths.Single(x => x.Key == ViewModel.CurrentEnemy.TexturePath);
            EnemyAIComboBox.SelectedItem = ViewModel.AllAIPaths.Single(x => x.Key == ViewModel.CurrentEnemy.AIPath);
            EnemyBeastLoreComboBox.SelectedItem = ViewModel.AllBeastLoreIDs.Single(x => x == ViewModel.CurrentEnemy.CapturedEnemyID);
            EnemyDeadTypeComboBox.SelectedItem = ViewModel.DeadTypes.Single(x => x == ViewModel.CurrentEnemy.DeadType);
            EnemyEvent1ComboBox.SelectedItem = ViewModel.AllEvents.Single(x => x == ViewModel.CurrentEnemy.BattleEvents[0]);
            EnemyEvent2ComboBox.SelectedItem = ViewModel.AllEvents.Single(x => x == ViewModel.CurrentEnemy.BattleEvents[1]);
            EnemyEvent3ComboBox.SelectedItem = ViewModel.AllEvents.Single(x => x == ViewModel.CurrentEnemy.BattleEvents[2]);
            LoadAbilityComboBoxes();
        }

        private void UpdateEnemiesToSave()
        {
            if(EnemiesToSave.Select(x => x.Key).Contains(ViewModel.CurrentEnemy.Key))
            {
                EnemiesToSave.Remove(EnemiesToSave.Single(x => x.Key == ViewModel.CurrentEnemy.Key));
            }
            EnemiesToSave.Add(ViewModel.CurrentEnemy);
            SaveEnemyButton.IsEnabled = true;
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

        private void ChangeTextBoxColor(TextBox sender, double valueToCompare)
        {
            bool parsed = double.TryParse(sender.Text, out double newValue);
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

        private void SaveEnemyButton_Click(object sender, RoutedEventArgs e)
        {
            // Save Enemy Here
            string enemyList = "";
            foreach(var enemy in EnemiesToSave)
            {
                enemyList += enemy.ToString() + "\n";
            }
            var result = MessageBox.Show($"The following enemies will be saved. Is that OK?\n\n{enemyList}", "", MessageBoxButton.OKCancel);

            if(result == MessageBoxResult.OK)
            {
                EnemyDBParser.SaveEnemies(EnemiesToSave);
                EnemiesToSave.Clear();
                SaveEnemyButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
            }
        }

        private void DiscardChangesButton_Click(object sender, RoutedEventArgs e)
        {
            // Save Enemy Here
            string enemyList = "";
            foreach (var enemy in EnemiesToSave)
            {
                enemyList += enemy.ToString() + "\n";
            }
            var result = MessageBox.Show($"Changes to the following enemies will be discarded. Is that OK?\n\n{enemyList}", "", MessageBoxButton.OKCancel);

            if(result == MessageBoxResult.OK)
            {
                EnemiesToSave.Clear();
                ViewModel.EnemyList = EnemyDBParser.ParseEnemyObjects();
                ViewModel.CurrentEnemy = ViewModel.EnemyList.Single(x => x.Key == ViewModel.CurrentEnemy.Key).Value;
                SaveEnemyButton.IsEnabled = false;
                DiscardChangesButton.IsEnabled = false;
            }
        }

        private void EnemyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy = (Enemy)EnemyComboBox.SelectedItem;
            UpdateComboBoxes();
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

        private void AbilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            int index = AbilitiesPanel.Children.IndexOf(comboBox);

            ViewModel.CurrentEnemy.AbilityList[index] = ((KeyValuePair<string, string>)comboBox.SelectedItem).Key;
        }

        private void EnemyItemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.ItemID = ((KeyValuePair<string, string>)EnemyItemComboBox.SelectedItem).Key;
            UpdateEnemiesToSave();
        }

        private void EnemySizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.Size = (CharacterSize)EnemySizeComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[0] = (AttributeResistance)EnemyAttribute1ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[1] = (AttributeResistance)EnemyAttribute2ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute3ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[2] = (AttributeResistance)EnemyAttribute3ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute4ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[3] = (AttributeResistance)EnemyAttribute4ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute5ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[4] = (AttributeResistance)EnemyAttribute5ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute6ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[5] = (AttributeResistance)EnemyAttribute6ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute7ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[6] = (AttributeResistance)EnemyAttribute7ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute8ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[7] = (AttributeResistance)EnemyAttribute8ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute9ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[8] = (AttributeResistance)EnemyAttribute9ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute10ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[9] = (AttributeResistance)EnemyAttribute10ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute11ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[10] = (AttributeResistance)EnemyAttribute11ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyAttribute12ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AttributeResistances[11] = (AttributeResistance)EnemyAttribute12ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyRaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.RaceType = (CharacterRace)EnemyRaceComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyFlipbookComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.FlipbookPath = ((KeyValuePair<string, string>)EnemyFlipbookComboBox.SelectedItem).Key;
            UpdateEnemiesToSave();
        }

        private void EnemyTextureComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.TexturePath = ((KeyValuePair<string, string>)EnemyTextureComboBox.SelectedItem).Key;
            UpdateEnemiesToSave();
        }

        private void EnemyAIComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.AIPath = ((KeyValuePair<string, string>)EnemyAIComboBox.SelectedItem).Key;
            UpdateEnemiesToSave();
        }

        private void EnemyBeastLoreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.CapturedEnemyID = (string)EnemyBeastLoreComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyDeadTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.DeadType = (EnemyDeadType)EnemyDeadTypeComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyEvent1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.BattleEvents[0] = (string)EnemyEvent1ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyEvent2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.BattleEvents[1] = (string)EnemyEvent2ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void EnemyEvent3ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy.BattleEvents[2] = (string)EnemyEvent3ComboBox.SelectedItem;
            UpdateEnemiesToSave();
        }

        private void AnyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEnemiesToSave();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EnemiesToSave.Clear();
            SaveEnemyButton.IsEnabled = false;
            DiscardChangesButton.IsEnabled = false;
        }

        private void AnyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateEnemiesToSave();
        }

        private void EnemyHPTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].HP);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyShieldTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].Shields);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyPAttackTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].PhysicalAttack);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyPDefenseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].PhysicalDefense);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyMAttackTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].ElementalAttack);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyMDefenseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].ElementalDefense);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyAccuracyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].Accuracy);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyEvasionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].Evasion);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyCriticalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].Critical);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemySpeedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].Speed);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyExperienceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].ExperiencePoints);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyJobPointsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].JobPoints);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyLeavesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].Money);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyCollectTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].MoneyFromCollecting);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyItemPercentageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].ItemDropPercentage);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyTameRateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].DefaultTameRate);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyLevelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].EnemyLevel);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyDamageRatioTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].DamageRatio);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemySPTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].MP);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyBPTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].BP);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyInvocationTurnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].InvocationTurn);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyInvocationValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].InvocationValue);
            AnyTextBox_TextChanged(sender, e);
        }

        private void EnemyFirstBPTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeTextBoxColor((TextBox)sender, MainWindow.MasterEnemyList[ViewModel.CurrentEnemy.Key].FirstBP);
            AnyTextBox_TextChanged(sender, e);
        }
    }
}
