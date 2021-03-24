using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OctomodEditor.Canvases;
using OctomodEditor.Models;
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
        public static Dictionary<string, string> MasterGameText { get; set; }
        public static Dictionary<string, string> ModGameText { get; set; }
        public static Dictionary<string, Enemy> MasterEnemyList { get; set; }
        public static Dictionary<string, Enemy> ModEnemyList { get; set; }
        public static Dictionary<string, Item> MasterItemList { get; set; }
        public static Dictionary<string, Item> ModItemList { get; set; }
        public static Dictionary<string, PurchaseItem> MasterPurchaseItemList { get; set; }
        public static Dictionary<string, PurchaseItem> ModPurchaseItemList { get; set; }
        public static Dictionary<string, ShopList> MasterShopListList { get; set; }
        public static Dictionary<string, ShopList> ModShopListList { get; set; }
        public static Dictionary<string, ShopInfo> MasterShopInfoList { get; set; }
        public static Dictionary<string, ShopInfo> ModShopInfoList { get; set; }
        public bool ConfigLoadedSuccessfully { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPaths();
            if (ConfigLoadedSuccessfully)
            {
                LoadMasterFiles();
            }
        }

        public static void LoadMasterFiles()
        {
            MasterGameText = GameTextParser.ParseGameText("EN", true);
            ModGameText = GameTextParser.ParseGameText("EN");
            MasterEnemyList = EnemyDBParser.ParseEnemyObjects(true);
            ModEnemyList = EnemyDBParser.ParseEnemyObjects();
            MasterItemList = ItemDBParser.ParseItemObjects(true);
            ModItemList = ItemDBParser.ParseItemObjects();
            MasterPurchaseItemList = PurchaseItemTableParser.ParsePurchaseItemObjects(true);
            ModPurchaseItemList = PurchaseItemTableParser.ParsePurchaseItemObjects();
            MasterShopListList = ShopListParser.ParseShopListObjects(true);
            ModShopListList = ShopListParser.ParseShopListObjects();
            MasterShopInfoList = ShopInfoParser.ParseShopInfoObjects(true);
            ModShopInfoList = ShopInfoParser.ParseShopInfoObjects();
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
                            CommonUtilities.AddSettingToConfig(new KeyValuePair<string, string>("baseFilesLocation", path));
                            CommonUtilities.BaseFilesLocation = path;
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
                    CommonUtilities.BaseFilesLocation = settings["baseFilesLocation"];
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
                            CommonUtilities.AddSettingToConfig(new KeyValuePair<string, string>("modLocation", path));
                            CommonUtilities.ModLocation = path;
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
                    CommonUtilities.ModLocation = settings["modLocation"];
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

        private void EnemySelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                ClearCanvas();
                EnemySelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Add(new EnemyEditorCanvas());
            }
        }

        private void ItemSelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                ClearCanvas();
                ItemSelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Add(new ItemEditorCanvas());
            }
        }

        private void ShopSelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                ClearCanvas();
                ShopSelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Add(new ShopEditorCanvas());
            }
        }
    }
}
