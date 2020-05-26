using Ao.Shared.ForView;
using Ao.Shared.ForView.Input;
using Ao.Wpf.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
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
    /// AoListItem.xaml 的交互逻辑
    /// </summary>
    public partial class AoListItem : UserControl, IInitable<UIElement>, IDisaplayable
    {
        private static readonly Brush DefaultSpliteBrush = new SolidColorBrush(Colors.Blue);
        
        public AoListItem()
        {
            InitializeComponent();
            Loaded += AoListItem_Loaded;
        }

        private void AoListItem_Loaded(object sender, RoutedEventArgs e)
        {
            ItemTemplate = Resources["DefDt"] as DataTemplate;
        }
        private ArrayViewModel viewModel;
        public string DisplayName { get; set; }

        public void Init(ViewBuildContext<UIElement> context, AoAnalizedPropertyItemBase item)
        {
            MainGrid.DataContext = this;

            Context = context;
            PropertyItem= item;
            viewModel = ArrayViewModel.FromDefaultViewAdapter(context, item);
            Ic.ItemsSource = viewModel.Views;
            BtnAdd.DataContext= Cm.DataContext = viewModel;
        }



        public Brush SpliteBrush
        {
            get { return (Brush)GetValue(SpliteBrushProperty); }
            set { SetValue(SpliteBrushProperty, value); }
        }


        public Thickness SpliteThinkness
        {
            get { return (Thickness)GetValue(SpliteThinknessProperty); }
            set { SetValue(SpliteThinknessProperty, value); }
        }


        public double HeaderMinHeight
        {
            get { return (double)GetValue(HeaderMinHeightProperty); }
            set { SetValue(HeaderMinHeightProperty, value); }
        }




        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public AoAnalizedPropertyItemBase PropertyItem { get; private set; }

        public ViewBuildContext<UIElement> Context { get; private set; }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AoListItem), new PropertyMetadata(null));


        // Using a DependencyProperty as the backing store for HeaderMinHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderMinHeightProperty =
            DependencyProperty.Register("HeaderMinHeight", typeof(double), typeof(AoListItem), new PropertyMetadata(32d));


        // Using a DependencyProperty as the backing store for SpliteThinkness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpliteThinknessProperty =
            DependencyProperty.Register("SpliteThinkness", typeof(Thickness), typeof(AoListItem), new PropertyMetadata(new Thickness(0,0,0,1)));


        // Using a DependencyProperty as the backing store for SpliteBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpliteBrushProperty =
            DependencyProperty.Register("SpliteBrush", typeof(Brush), typeof(AoListItem), new PropertyMetadata(DefaultSpliteBrush));


    }
}
