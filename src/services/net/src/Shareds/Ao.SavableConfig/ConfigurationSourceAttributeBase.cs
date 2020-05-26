using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示配置源特性的基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public abstract class ConfigurationSourceAttributeBase : Attribute
    {
        /// <summary>
        /// 初始化<see cref="ConfigurationSourceAttributeBase"/>
        /// </summary>
        /// <param name="bindType">绑定类型</param>
        protected ConfigurationSourceAttributeBase(Type bindType)
        {
            BindType = bindType ?? throw new ArgumentNullException(nameof(bindType));
            if (!bindType.IsClass||BindType.IsAbstract || BindType.GetConstructor(Array.Empty<Type>()) == null)
            {
                throw new ArgumentException($"类型:{bindType}不是类或者是抽象的或者没有一个无参公共构造函数");
            }
        }
        /// <summary>
        /// 初始化<see cref="ConfigurationSourceAttributeBase"/>
        /// </summary>
        protected ConfigurationSourceAttributeBase()
        {

        }
        private Assembly assembly;
        /// <summary>
        /// 此属性可以为空,配置绑定的类型,此类型必须是类并且不抽象，而且有一个无参公共构造函数
        /// </summary>
        public Type BindType { get; }
        /// <summary>
        /// 依附的程序集
        /// </summary>
        public Assembly Assembly => assembly;
        /// <summary>
        /// 生成一个配置源
        /// </summary>
        /// <returns></returns>
        public abstract IConfigurationSource GetConfigurationSource();
        /// <summary>
        /// 制作一个对象默认的字符串
        /// </summary>
        /// <returns></returns>
        public abstract string MakeDefault();
        /// <summary>
        /// 获取当前配置制作源的状态
        /// </summary>
        /// <returns></returns>
        public abstract MakeSourceStatus GetMakeSourceStatus();
        /// <summary>
        /// 依附程序集
        /// </summary>
        /// <param name="assembly"></param>
        internal void Attack(Assembly assembly)
        {
            this.assembly = assembly;
        }
    }
}
