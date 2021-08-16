using DataEditorUE4.Models;
using OctomodEditor.Canvases;
using OctomodEditor.Models;
using OctomodEditor.Parsers;
using OctomodEditor.Utilities;
using OctomodEditor.Windows;
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

namespace OctomodEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<string, GameText> MasterGameText { get; set; }
        public static Dictionary<string, GameText> ModGameText { get; set; }
        public static UEDataTable MasterGameTextTable { get; set; }
        public static UEDataTable ModGameTextTable { get; set; }
        public static Dictionary<string, Enemy> MasterEnemyList { get; set; }
        public static Dictionary<string, Enemy> ModEnemyList { get; set; }
        public static UEDataTable MasterEnemyTable { get; set; }
        public static UEDataTable ModEnemyTable { get; set; }
        public static Dictionary<string, Item> MasterItemList { get; set; }
        public static Dictionary<string, Item> ModItemList { get; set; }
        public static UEDataTable MasterItemTable { get; set; }
        public static UEDataTable ModItemTable { get; set; }
        public static Dictionary<string, PurchaseItem> MasterPurchaseItemList { get; set; }
        public static Dictionary<string, PurchaseItem> ModPurchaseItemList { get; set; }
        public static UEDataTable MasterPurchaseItemTable { get; set; }
        public static UEDataTable ModPurchaseItemTable { get; set; }
        public static Dictionary<string, ShopList> MasterShopListList { get; set; }
        public static Dictionary<string, ShopList> ModShopListList { get; set; }
        public static UEDataTable MasterShopListTable { get; set; }
        public static UEDataTable ModShopListTable { get; set; }
        public static Dictionary<string, ShopInfo> MasterShopInfoList { get; set; }
        public static Dictionary<string, ShopInfo> ModShopInfoList { get; set; }
        public static UEDataTable MasterShopInfoTable { get; set; }
        public static UEDataTable ModShopInfoTable { get; set; }
        public static Dictionary<string, Ability> MasterAbilityList { get; set; }
        public static Dictionary<string, Ability> ModAbilityList { get; set; }
        public static UEDataTable MasterAbilityTable { get; set; }
        public static UEDataTable ModAbilityTable { get; set; }
        public static Dictionary<string, AbilitySet> MasterAbilitySetList { get; set; }
        public static Dictionary<string, AbilitySet> ModAbilitySetList { get; set; }
        public static UEDataTable MasterAbilitySetTable { get; set; }
        public static UEDataTable ModAbilitySetTable { get; set; }
        public static MainWindow Instance { get; set; }
        public bool ConfigLoadedSuccessfully { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPaths();
            if (ConfigLoadedSuccessfully)
            {
                LoadTextLists("EN");
            }
        }

        public async Task LoadEnemyLists()
        {
            var enemyParser = new EnemyParser();

            await Task.Run(() =>
            {
                MasterEnemyTable = enemyParser.GetTableFromFile(true);
                MasterEnemyList = enemyParser.ParseTable(MasterEnemyTable);
                ModEnemyTable = enemyParser.GetTableFromFile();
                ModEnemyList = enemyParser.ParseTable(ModEnemyTable);
            });
        }

        public async Task LoadTextLists(string language)
        {
            var gameTextParser = new GameTextParser(language);

            await Task.Run(() =>
            {
                MasterGameTextTable = gameTextParser.GetTableFromFile(true);
                MasterGameText = gameTextParser.ParseTable(MasterGameTextTable);
                ModGameTextTable = gameTextParser.GetTableFromFile();
                ModGameText = gameTextParser.ParseTable(ModGameTextTable);
            });
        }

        public async Task LoadItemLists()
        {
            var itemParser = new ItemParser();

            await Task.Run(() =>
            {
                MasterItemTable = itemParser.GetTableFromFile(true);
                ModItemTable = itemParser.GetTableFromFile();
                MasterItemList = itemParser.ParseTable(MasterItemTable);
                ModItemList = itemParser.ParseTable(ModItemTable);
            });
        }

        public async Task LoadPurchaseItemLists()
        {
            var purchaseItemParser = new PurchaseItemParser();

            await Task.Run(() =>
            {
                MasterPurchaseItemTable = purchaseItemParser.GetTableFromFile(true);
                ModPurchaseItemTable = purchaseItemParser.GetTableFromFile();
                MasterPurchaseItemList = purchaseItemParser.ParseTable(MasterPurchaseItemTable);
                ModPurchaseItemList = purchaseItemParser.ParseTable(ModPurchaseItemTable);
            });
        }

        public async Task LoadShopInfoLists()
        {
            var shopInfoParser = new ShopInfoParser();

            await Task.Run(() =>
            {
                MasterShopInfoTable = shopInfoParser.GetTableFromFile(true);
                ModShopInfoTable = shopInfoParser.GetTableFromFile();
                MasterShopInfoList = shopInfoParser.ParseTable(MasterShopInfoTable);
                ModShopInfoList = shopInfoParser.ParseTable(ModShopInfoTable);
            });
        }

        public async Task LoadShopLists()
        {
            var shopListParser = new ShopListParser();

            await Task.Run(() =>
            {
                MasterShopListTable = shopListParser.GetTableFromFile(true);
                ModShopListTable = shopListParser.GetTableFromFile();
                MasterShopListList = shopListParser.ParseTable(MasterShopListTable);
                ModShopListList = shopListParser.ParseTable(ModShopListTable);
            });
        }

        public async Task LoadAbilityLists()
        {
            var abilityParser = new AbilityParser();

            await Task.Run(() =>
            {
                MasterAbilityTable = abilityParser.GetTableFromFile(true);
                ModAbilityTable = abilityParser.GetTableFromFile();
                MasterAbilityList = abilityParser.ParseTable(MasterAbilityTable);
                ModAbilityList = abilityParser.ParseTable(ModAbilityTable);
            });
        }

        public async Task LoadAbilitySetLists()
        {
            var abilitySetParser = new AbilitySetParser();

            await Task.Run(() =>
            {
                MasterAbilitySetTable = abilitySetParser.GetTableFromFile(true);
                ModAbilitySetTable = abilitySetParser.GetTableFromFile();
                MasterAbilitySetList = abilitySetParser.ParseTable(MasterAbilitySetTable);
                ModAbilitySetList = abilitySetParser.ParseTable(ModAbilitySetTable);
            });
        }

        public void StartLoading()
        {
            DataGrid.IsEnabled = false;
            LoadingScreen.Visibility = Visibility.Visible;
        }

        public void StopLoading()
        {
            LoadingScreen.Visibility = Visibility.Hidden;
            DataGrid.IsEnabled = true;
        }

        private void LoadPaths()
        {
            if (!File.Exists(string.Join("/", Directory.GetCurrentDirectory(), "config.octo")))
            {
                var stream = File.Create(string.Join("/", Directory.GetCurrentDirectory(), "config.octo"));
                stream.Close();
            }

            try
            {
                Dictionary<string, string> settings = new Dictionary<string, string>();
                string[] lines = File.ReadAllLines(string.Join("/", Directory.GetCurrentDirectory(), "config.octo"));
                foreach (string line in lines)
                {
                    settings.Add(line.Split('=')[0], line.Split('=')[1]);
                }

                bool canceled = false;

                if (!settings.ContainsKey("baseFilesLocation"))
                {
                    var result = MessageBox.Show("Please select the location of your Octopath unpacked files.", "", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        var openOctopathFolderDialogue = new System.Windows.Forms.FolderBrowserDialog();
                        var resultPath = openOctopathFolderDialogue.ShowDialog();
                        if (resultPath != System.Windows.Forms.DialogResult.Cancel)
                        {
                            string path = openOctopathFolderDialogue.SelectedPath;
                            CommonOctomodUtilities.AddSettingToConfig(new KeyValuePair<string, string>("baseFilesLocation", path));
                            CommonOctomodUtilities.BaseFilesLocation = path;
                        }
                        else
                        {
                            MessageBox.Show("Octomod Editor cannot function without unpacked Octopath files.", "", MessageBoxButton.OK);
                            canceled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Octomod Editor cannot function without unpacked Octopath files.", "", MessageBoxButton.OK);
                        canceled = true;
                    }
                }
                else
                {
                    CommonOctomodUtilities.BaseFilesLocation = settings["baseFilesLocation"];
                }

                if (!settings.ContainsKey("modLocation") && !canceled)
                {
                    var result = MessageBox.Show("Please select the location of your mod.", "", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        var openOctopathFolderDialogue = new System.Windows.Forms.FolderBrowserDialog();
                        var resultPath = openOctopathFolderDialogue.ShowDialog();
                        if (resultPath != System.Windows.Forms.DialogResult.Cancel)
                        {
                            string path = openOctopathFolderDialogue.SelectedPath;
                            CommonOctomodUtilities.AddSettingToConfig(new KeyValuePair<string, string>("modLocation", path));
                            CommonOctomodUtilities.ModLocation = path;
                        }
                        else
                        {
                            MessageBox.Show("Octomod Editor cannot function without a mod folder selected.", "", MessageBoxButton.OK);
                            canceled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Octomod Editor cannot function without a mod folder selected.", "", MessageBoxButton.OK);
                        canceled = true;
                    }
                }
                else if (!canceled)
                {
                    CommonOctomodUtilities.ModLocation = settings["modLocation"];
                }

                if (canceled)
                {
                    ConfigLoadedSuccessfully = false;
                }
                else
                {
                    ConfigLoadedSuccessfully = true;
                }
            }
            catch
            {
                MessageBox.Show("Your config file has been corrupted.");
                File.Delete(string.Join("/", Directory.GetCurrentDirectory(), "config.octo"));
                Application.Current.Shutdown();
            }
        }

        private void ClearCanvas()
        {
            foreach (var child in OptionStackPanel.Children)
            {
                ((Label)child).Background.Opacity = 0.5;
            }
            DataGrid.Children.Clear();
        }

        private void MenuItemPreferences_Click(object sender, RoutedEventArgs e)
        {
            PreferencesWindow prefWindow = new PreferencesWindow();
            prefWindow.Show();
            prefWindow.Focus();
            prefWindow.ResizeMode = ResizeMode.NoResize;
        }

        private async void EnemySelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                StartLoading();
                await LoadEnemyLists();
                await LoadItemLists();
                ClearCanvas();
                EnemySelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Add(new EnemyEditorCanvas());
                StopLoading();
            }
        }

        private async void ItemSelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                StartLoading();
                await LoadItemLists();
                ClearCanvas();
                ItemSelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Add(new ItemEditorCanvas());
                StopLoading();
            }
        }

        private async void ShopSelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                StartLoading();
                await LoadShopInfoLists();
                await LoadShopLists();
                await LoadPurchaseItemLists();
                await LoadItemLists();
                ClearCanvas();
                ShopSelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Add(new ShopEditorCanvas());
                StopLoading();
            }
        }

        private async void AbilitySelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                StartLoading();
                await LoadAbilityLists();
                await LoadAbilitySetLists();
                ClearCanvas();
                AbilitySelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Add(new AbilityEditorCanvas());
                StopLoading();
            }
        }
    }
}
