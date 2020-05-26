using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Command.Attributes
{
    /// <summary>
    /// 别名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// 初始化<see cref="AliasAttribute"/>
        /// </summary>
        /// <param name="alias"><inheritdoc cref="Alias"/></param>
        public AliasAttribute(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentException("message", nameof(alias));
            }

            Alias = alias;
        }
        /// <summary>
        /// 别名，不能为空
        /// </summary>
        public string Alias { get; }
    }
}
