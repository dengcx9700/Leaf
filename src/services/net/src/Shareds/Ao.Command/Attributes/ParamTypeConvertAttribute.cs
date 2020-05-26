using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ao.Command.Attributes
{
    /// <summary>
    /// 参数转换特性,最终会调用<see cref="TypeConverter.ConvertTo(object, Type)"/>，并且传入的参数为<see cref="StringValue"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class ParamTypeConvertAttribute : Attribute
    {
        /// <summary>
        /// 初始化<see cref="ParamTypeConvertAttribute"/>
        /// </summary>
        /// <param name="convertType"></param>
        public ParamTypeConvertAttribute(Type convertType)
        {
            ConvertType = convertType ?? throw new ArgumentNullException(nameof(convertType));
            if (typeof(TypeConverter).IsAssignableFrom(convertType))
            {
                throw new ArgumentException("转换类型必须继承TypeConverter");
            }
        }

        /// <summary>
        /// 转换的类型，此类型必须是继承自<see cref="TypeConverter"/>
        /// </summary>
        public Type ConvertType { get; }
    }
}
