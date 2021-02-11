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
        public MainWindow()
        {
            InitializeComponent();
            MasterGameText = GameTextParser.ParseGameText("EN");
        }

        private void EnemySelectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach(var child in OptionStackPanel.Children)
            {
                ((Label)child).Background.Opacity = 0.5;
            }
            EnemySelectorLabel.Background.Opacity = 0.8;
            Dictionary<string, Enemy> enemyData = EnemyDBParser.ParseEnemyObjects(@"../../Test/EnemyDB.uasset", @"../../Test/EnemyDB.uexp");
            DataGrid.Children.Clear();
            DataGrid.Children.Add(new EnemyEditorCanvas(new EnemyViewModel(enemyData)));
        }
    }
}
