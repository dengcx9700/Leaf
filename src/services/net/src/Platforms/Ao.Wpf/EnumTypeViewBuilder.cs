﻿using Ao.Shared.ForView;
using Ao.Wpf.Xaml;
using System;
using System.Windows;

namespace Ao.Wpf
{
    public class EnumTypeViewBuilder : IViewBuilder<UIElement>
    {
        public int Order { get; } = 0;

        public UIElement BuildView(ViewBuildContext<UIElement> context, AoAnalizedPropertyItemBase propertyItem)
        {
            var view = new AoDrapdownItem();
            view.Init(context, propertyItem);
            var vm = context.Vm;
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
            return type.IsEnum;
        }
    }
}
