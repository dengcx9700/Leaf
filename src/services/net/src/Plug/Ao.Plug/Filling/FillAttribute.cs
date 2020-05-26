using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;

namespace Ao.Plug.Filling
{
    /// <summary>
    /// 表示填充类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public abstract class FillAttribute : Attribute
    {
        /// <summary>
        /// 初始化<see cref="FillAttribute"/>
        /// </summary>
        /// <param name="refType"><inheritdoc cref="RefType"/></param>
        public FillAttribute(Type refType)
        {
            RefType = refType ?? throw new ArgumentNullException(nameof(refType));
            if (!refType.IsInterface&&!refType.IsClass)
            {
                throw new NotSupportedException("引用类型只能是接口或者类");
            }
        }
        /// <summary>
        /// 引用的类型，此类型只能是接口或类
        /// </summary>
        public Type RefType { get; }
        /// <summary>
        /// 放进集合
        /// </summary>
        /// <param name="context">填充上下文</param>
        public abstract void PutIn(FillContext context);
    }
}
