using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Ao.Command
{
    /// <summary>
    /// 默认的类型转换器
    /// </summary>
    public class DefaultTypeConverter : TypeConverter
    {

        #region 类型
        protected static readonly HashSet<Type> numberTypes = new HashSet<Type>
        {
            typeof(byte),typeof(short),typeof(int),typeof(long),
            typeof(uint),typeof(ulong)
        };
        protected static readonly HashSet<Type> singleTypes = new HashSet<Type>
        {
            typeof(float),typeof(double),typeof(decimal)
        };

        #endregion

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is StringValue v)
            {
                if (numberTypes.Contains(destinationType))
                {
                    return Convert.ChangeType(v.Long, destinationType);
                }
                else if (singleTypes.Contains(destinationType))
                {
                    return Convert.ChangeType(v.Decimal, destinationType);
                }
                return v.Origin;
            }
            return value;
        }
    }
}
