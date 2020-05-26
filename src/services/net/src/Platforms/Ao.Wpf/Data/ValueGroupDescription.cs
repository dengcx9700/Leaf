using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Ao.Wpf.Data
{
    public class ValueGroupDescription : GroupDescription
    {
        public ValueGroupDescription(string value)
        {
            Value = value;
        }

        public string Value { get; }
        public override object GroupNameFromItem(object item, int level, CultureInfo culture)
        {
            return Value;
        }
    }
}
