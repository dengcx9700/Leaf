using Ao.Shared.ForView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Ao.Wpf
{
    public abstract class WpfBuilders
    {
        public static readonly IViewBuilder<UIElement>[] KnowsBuilders =
        {
            new ArrayTypeViewBuilder(),
            new BoolTypeViewBuilder(),
            new EnumTypeViewBuilder(),
            new ValueTypeViewBuilder()
        };
    }
}
