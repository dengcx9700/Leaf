using System;

namespace Ao.Shared.ForView.Input
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Property,AllowMultiple =false,Inherited =false)]
    public sealed class StringKeyAttribute : Attribute
    {
        /// <summary>
        /// 初始化<see cref="StringKeyAttribute"/>
        /// </summary>
        /// <param name="key"></param>
        public StringKeyAttribute(string key)
        {
            Key = key;
        }
        /// <summary>
        /// 字符串键
        /// </summary>
        public string Key { get; }
    }
}
