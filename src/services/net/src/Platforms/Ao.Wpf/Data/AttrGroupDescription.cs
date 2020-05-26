using Ao.Shared.ForView;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Ao.Wpf.Data
{
    public class AttrGroupDescription<TView> : GroupDescription
    {
        public override object GroupNameFromItem(object item, int level, CultureInfo culture)
        {
            if (item is BuildResualItem<TView> bki)
            {
                return bki.GetGroupName();
            }
            throw new NotSupportedException(item.GetType().FullName);
        }
    }
}
