using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Ao.Wpf.Converters
{
    public class ValueTypeConvert : IValueConverter
    {
        private readonly Type targetType;

        public ValueTypeConvert(Type targetType)
        {
            this.targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ChangeType(value, targetType);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ChangeType(value, targetType);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
