using Ao.Shared.ForView;
using Ao.Shared.ForView.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// AoDrapdownItem.xaml 的交互逻辑
    /// </summary>
    public partial class AoDrapdownItem : UserControl, IInitable<UIElement>, IDisaplayable
    {
        public AoDrapdownItem()
        {
            InitializeComponent();
            Loaded += AoDrapdownItem_Loaded;
            DropDatas = new ObservableCollection<object>();
        }
        private object dropItem;

        public object DropItem
        {
            get { return dropItem; }
            set
            {
                dropItem = value;
                if (PropertyItem.CanSet)
                {
                    PropertyItem.Setter(EnumValues[value]);
                }
            }
        }
        private readonly Dictionary<object, object> EnumValues=new Dictionary<object, object>();
        public ObservableCollection<object> DropDatas { get; }
        
        public string DisplayName { get; set; }

        public AoAnalizedPropertyItemBase PropertyItem { get; private set; }

        public ViewBuildContext<UIElement> Context { get; private set; }

        public void Init(ViewBuildContext<UIElement> context, AoAnalizedPropertyItemBase propertyItem)
        {
            Context = context;
            PropertyItem = propertyItem;
        }

        private void AoDrapdownItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (EnumValues.Count==0)
            {
                if (!PropertyItem.CanSet)
                {
                    Cb.IsEnabled= false;
                }
                MainGrid.DataContext = this;
                var now = PropertyItem.Getter();
                var names = Enum.GetNames(PropertyItem.ValueType);
                var values = Enum.GetValues(PropertyItem.ValueType);
                for (int i = 0; i < names.Length; i++)
                {
                    var n = names[i];
                    n = Context.ViewBuilder.StringProvider?.GetString($"{PropertyItem.ValueType.FullName}.{n}") ??
                        Context.ViewBuilder.StringProvider?.GetString(n) ??
                        n;
                    var val = values.GetValue(i);
                    EnumValues.Add(n,val);
                    DropDatas.Add(n);
                    if (val.Equals(now))
                    {
                        Cb.SelectedItem= n;
                    }
                }
            }
        }
    }
}
