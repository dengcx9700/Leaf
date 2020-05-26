using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
using Ao.Shared;
using Ao.Shared.ForView;
using Ao.Wpf.Data;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;

namespace Ao.Wpf.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var analizer = new AoAnalizer();
            var forView = new ForViewBuilder<UIElement>();
            forView.AddRange(WpfBuilders.KnowsBuilders);
            lv.ItemsSource = analizer.Analize(this, false)
                                .Build(null,forView)
                                .GetAllView();
        }
    }
}


