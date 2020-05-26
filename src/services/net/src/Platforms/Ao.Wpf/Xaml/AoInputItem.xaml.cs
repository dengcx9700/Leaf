using Ao.Shared.ForView;
using Ao.Shared.ForView.Input;
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

namespace Ao.Wpf.Xaml
{
    /// <summary>
    /// AoInputItem.xaml 的交互逻辑
    /// </summary>
    public partial class AoInputItem : UserControl, IInitable<UIElement>, IDisaplayable
    {
        public string DisplayName { get; set; }

        public AoAnalizedPropertyItemBase PropertyItem { get; private set; }

        public ViewBuildContext<UIElement> Context { get; private set; }

        public AoInputItem()
        {
            InitializeComponent();
            Loaded += AoInputItem_Loaded;
        }

        public void Init(ViewBuildContext<UIElement> context, AoAnalizedPropertyItemBase propertyItem)
        {
            Context = context;
            PropertyItem = propertyItem;

        }

        private void AoInputItem_Loaded(object sender, RoutedEventArgs e)
        {
            MainGrid.DataContext = this;
            var bd = new Binding(PropertyItem.ValueName) { Source = PropertyItem.Source};
            if (PropertyItem.CanSet)
            {
                bd.Mode = BindingMode.TwoWay;
            }
            else
            {
                bd.Mode = BindingMode.OneWay;
                Tb.IsEnabled = false;
            }
            Tb.SetBinding(TextBox.TextProperty, bd);
        }
    }
}
