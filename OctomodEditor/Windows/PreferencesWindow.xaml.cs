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
            MainLocationLabel.Content = CommonOctomodUtilities.BaseFilesLocation;
            ModLocationLabel.Content = CommonOctomodUtilities.ModLocation;
        }

        private void MainFilesLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var openOctopathFolderDialogue = new System.Windows.Forms.FolderBrowserDialog();
            var resultPath = openOctopathFolderDialogue.ShowDialog();
            if (resultPath != System.Windows.Forms.DialogResult.Cancel)
            {
                string path = openOctopathFolderDialogue.SelectedPath;
                CommonOctomodUtilities.AddSettingToConfig(new KeyValuePair<string, string>("baseFilesLocation", path));
                CommonOctomodUtilities.BaseFilesLocation = path;
                MainLocationLabel.Content = CommonOctomodUtilities.BaseFilesLocation;
            }
        }

        private void ModFilesLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var openOctopathFolderDialogue = new System.Windows.Forms.FolderBrowserDialog();
            var resultPath = openOctopathFolderDialogue.ShowDialog();
            if (resultPath != System.Windows.Forms.DialogResult.Cancel)
            {
                string path = openOctopathFolderDialogue.SelectedPath;
                CommonOctomodUtilities.AddSettingToConfig(new KeyValuePair<string, string>("modLocation", path));
                CommonOctomodUtilities.ModLocation = path;
                ModLocationLabel.Content = CommonOctomodUtilities.ModLocation;
            }
        }
    }
}
