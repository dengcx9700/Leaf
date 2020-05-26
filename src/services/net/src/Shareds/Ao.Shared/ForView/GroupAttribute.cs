using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示一个组属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class GroupAttribute : Attribute, IValueProvider<string>
    {
        /// <summary>
        /// 从目标实例获取一个组名
        /// </summary>
        /// <param name="inst">目标实例</param>
        /// <returns></returns>
        public abstract string GetValue(object inst);
    }

}
