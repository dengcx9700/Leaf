using System;
using System.Collections;
using System.Collections.Generic;
namespace Ao.Shared.ForView
{
    internal class ViewBuilderComparer<TView> : IComparer<IViewBuilder<TView>>, IComparer
    {
        public static ViewBuilderComparer<TView> Default = new ViewBuilderComparer<TView>();

        public int Compare(IViewBuilder<TView> x, IViewBuilder<TView> y)
        {
            if (x.Order < 0 && y.Order >= 0)
            {
                return 1;
            }
            return y.Order - x.Order;
        }

        public int Compare(object x, object y)
        {
            if (x is IViewBuilder<TView> vx&&y is IViewBuilder<TView> vy)
            {
                return Compare(vx, vy);
            }
            throw new InvalidOperationException("无法比较不同类型");
        }
    }
}
