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
    /// AoItemsControl.xaml 的交互逻辑
    /// </summary>
    public partial class AoExpander : Expander, IInitable<UIElement>, IDisaplayable
    {
        public AoExpander()
        {
            InitializeComponent();
        }

        public AoAnalizedPropertyItemBase PropertyItem { get; private set; }

        public ViewBuildContext<UIElement> Context { get; private set; }

        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }

        public string DisplayName { get; set; }

        // Using a DependencyProperty as the backing store for TitleBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(AoExpander), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public void Init(ViewBuildContext<UIElement> context, AoAnalizedPropertyItemBase propertyItem)
        {
            Context = context;
            PropertyItem = propertyItem;
        }
    }
}
