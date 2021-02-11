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
        public EnemyEditorCanvas(EnemyViewModel enemies)
        {
            InitializeComponent();

            ViewModel = enemies;
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
            ViewModel.CurrentEnemyList = ViewModel.EnemyList.Where(x => x.Key.StartsWith("ENE_Mou")).Select(x => x.Value).ToList();
            EnemyComboBox.SelectedIndex = 0;
            //EnemyComboBox.SelectionChanged += EnemyComboBoxSelectionChanged;
            //EnemyComboBox.ItemsSource = Enemies.EnemyList.Values;
            //EnemyComboBox.SelectedIndex = 0;

            //Attribute1ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute2ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute3ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute4ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute5ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute6ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute7ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute8ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute9ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute10ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute11ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute12ComboBox.ItemsSource = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
        }

        private void EnemyComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentEnemy = (Enemy)EnemyComboBox.SelectedItem;
            IEnumerable<AttributeResistance> resistanceList = Enum.GetValues(typeof(AttributeResistance)).Cast<AttributeResistance>();
            //Attribute1ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[0]);
            //Attribute2ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[1]);
            //Attribute3ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[2]);
            //Attribute4ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[3]);
            //Attribute5ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[4]);
            //Attribute6ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[5]);
            //Attribute7ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[6]);
            //Attribute8ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[7]);
            //Attribute9ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[8]);
            //Attribute10ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[9]);
            //Attribute11ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[10]);
            //Attribute12ComboBox.SelectedItem = resistanceList.Single(x => x == ((Enemy)EnemyComboBox.SelectedItem).AttributeResistances[11]);
        }

        private void SaveEnemyButton_Click(object sender, RoutedEventArgs e)
        {
            //enemy.AttributeResistances[0] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[1] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[2] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[3] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[4] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[5] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[6] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[7] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[8] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[9] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[10] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //enemy.AttributeResistances[11] = (AttributeResistance)Attribute1ComboBox.SelectedItem;
            //EnemyDBParser.SaveEnemy(Enemies.CurrentEnemy, @"../../Test/", @"../../Test/New/"); ;
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
    }
}
