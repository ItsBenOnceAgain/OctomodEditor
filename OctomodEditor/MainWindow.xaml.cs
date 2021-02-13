using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OctomodEditor.Canvases;
using OctomodEditor.Models;
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

namespace OctomodEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<string, string> MasterGameText { get; set; }
        public bool ConfigLoadedSuccessfully { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EnemySelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigLoadedSuccessfully)
            {
                foreach (var child in OptionStackPanel.Children)
                {
                    ((Label)child).Background.Opacity = 0.5;
                }
                EnemySelectorLabel.Background.Opacity = 0.8;
                DataGrid.Children.Clear();

                DataGrid.Children.Add(new EnemyEditorCanvas());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPaths();
            MasterGameText = GameTextParser.ParseGameText("EN");
        }

        private void LoadPaths()
        {
            if (!File.Exists(@"/config.octo"))
            {
                var stream = File.Create(@"/config.octo");
                stream.Close();
            }

            try
            {
                Dictionary<string, string> settings = new Dictionary<string, string>();
                string[] lines = File.ReadAllLines(@"/config.octo");
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
                File.Delete(@"/config.octo");
                Application.Current.Shutdown();
            }
        }
    }
}
