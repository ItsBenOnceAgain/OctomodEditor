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
using System.Windows.Shapes;

namespace OctomodEditor.Windows
{
    /// <summary>
    /// Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        public PreferencesWindow()
        {
            InitializeComponent();
            MainLocationLabel.Content = CommonUtilities.BaseFilesLocation;
            ModLocationLabel.Content = CommonUtilities.ModLocation;
        }

        private void MainFilesLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var openOctopathFolderDialogue = new System.Windows.Forms.FolderBrowserDialog();
            var resultPath = openOctopathFolderDialogue.ShowDialog();
            if (resultPath != System.Windows.Forms.DialogResult.Cancel)
            {
                string path = openOctopathFolderDialogue.SelectedPath;
                CommonUtilities.AddSettingToConfig(new KeyValuePair<string, string>("baseFilesLocation", path));
                CommonUtilities.BaseFilesLocation = path;
                MainLocationLabel.Content = CommonUtilities.BaseFilesLocation;
            }
        }

        private void ModFilesLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var openOctopathFolderDialogue = new System.Windows.Forms.FolderBrowserDialog();
            var resultPath = openOctopathFolderDialogue.ShowDialog();
            if (resultPath != System.Windows.Forms.DialogResult.Cancel)
            {
                string path = openOctopathFolderDialogue.SelectedPath;
                CommonUtilities.AddSettingToConfig(new KeyValuePair<string, string>("modFilesLocation", path));
                CommonUtilities.ModLocation = path;
                ModLocationLabel.Content = CommonUtilities.ModLocation;
            }
        }
    }
}
