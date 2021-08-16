using OctomodEditor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OctomodEditor.UserControls
{
    /// <summary>
    /// Interaction logic for AilmentControl.xaml
    /// </summary>
    public partial class AilmentControl : UserControl
    {
        public string AilmentTitle { get; set; }
        public Ailment Ailment { get; set; }
        public List<string> AilmentNames { get; set; }
        public int AilmentIndex { get; set; }
        public AilmentControl(string title, Ailment ailment, List<string> ailmentNames, int ailmentIndex = 0)
        {
            InitializeComponent();
            this.DataContext = this;

            AilmentTitle = title;
            Ailment = ailment;
            AilmentNames = ailmentNames;
            AilmentIndex = ailmentIndex;
        }
    }
}
