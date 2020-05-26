using Ao.Shared.ForView;
using Ao.Wpf.Xaml;
using System;
using System.Linq;
using System.Windows;

namespace Ao.Wpf
{
    public class ArrayTypeViewBuilder : IViewBuilder<UIElement>
    {
        public int Order { get; }

        public UIElement BuildView(ViewBuildContext<UIElement> context, AoAnalizedPropertyItemBase propertyItem)
        {
            var view = new AoListItem();
            view.Init(context, propertyItem);
            object vm = context.Vm;
            if (vm == null && propertyItem.CanGet)
            {
                vm = propertyItem.Getter();
            }
            view.DisplayName = context.GetString(propertyItem, propertyItem.ValueName);
            view.DataContext = vm;
            return view;
        }

        public bool Condition(Type type)
        {
            return KnowTypes.IsEmunerable(type);
        }
    }
}
