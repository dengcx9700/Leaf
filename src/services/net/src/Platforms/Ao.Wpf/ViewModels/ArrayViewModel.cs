using Ao.Shared.ForView;
using Ao.Shared.ForView.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Ao.Wpf.ViewModels
{
    public class ArrayViewModel
    {
        public static readonly IArrayViewAdapter DefaultViewAdapter = new ClassArrayViewAdapter();
        public static readonly string IEnumerableName = typeof(IEnumerable).FullName;

        private IArrayViewAdapter viewAdapter;
        /// <summary>
        /// 转换的枚举类型
        /// </summary>
        public IEnumerable Value { get; }
        public IList ListValue { get; }
        public AoAnalizedPropertyItemBase PropertyItem { get; }
        public ViewBuildContext<UIElement> Context { get; }
        /// <summary>
        /// 列表泛型类型
        /// </summary>
        public Type GenericType { get; }
        public ObservableCollection<UIElement> Views { get; }
        /// <summary>
        /// 集合是否可以修改
        /// </summary>
        public bool CanModify => ListValue != null;
        public IArrayViewAdapter ViewAdapter { get; }

        public ArrayViewModel(ViewBuildContext<UIElement> context, 
            AoAnalizedPropertyItemBase propertyItem,
            IArrayViewAdapter viewAdapter)
        {

            Views = new ObservableCollection<UIElement>();

            PropertyItem = propertyItem;
            Context = context;
            ViewAdapter = viewAdapter;
            if (propertyItem.ValueType.GetInterface(IEnumerableName)==null
                &&propertyItem.ValueType.GenericTypeArguments.Length==0)
            {
                throw new ArgumentException("接受的数组类型必须是泛型并且实现了IEnumerable接口");
            }
            if (propertyItem.ValueType.GenericTypeArguments.Length>0)
            {
                GenericType = propertyItem.ValueType.GenericTypeArguments[0];
            }
            Value = propertyItem.Getter() as IEnumerable;
            if (Value==null)
            {
                throw new NotSupportedException("仅支持实现了IEnumerable接口的列表");
            }
            ListValue = Value as IList;
            InitView();
        }

        private void InitView()
        {
            foreach (var item in Value)
            {
                var view = MakeView(context => item);
                Views.Add(view);
            }
        }
        protected UIElement MakeView(Func<ArrayViewAdapterContext,object> objGetter)
        {
            var context = new ArrayViewAdapterContext(Value, GenericType, PropertyItem, Context);
            return ViewAdapter.GetView(objGetter(context), context);

        }
        protected UIElement MakeView()
        {
            return MakeView(context=> ViewAdapter.MakeObject(context));
        }
        public static ArrayViewModel FromDefaultViewAdapter(ViewBuildContext<UIElement> context,
            AoAnalizedPropertyItemBase propertyItem)
        {
            return new ArrayViewModel(context, propertyItem, DefaultViewAdapter);
        }
    }
}
