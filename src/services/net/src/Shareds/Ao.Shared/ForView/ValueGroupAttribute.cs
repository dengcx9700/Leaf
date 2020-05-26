using System;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示固定值的组
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ValueGroupAttribute:GroupAttribute
    {
        /// <summary>
        /// 初始化<see cref="ValueGroupAttribute"/>
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name"/></param>
        public ValueGroupAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 组名
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="inst"><inheritdoc/></param>
        /// <returns></returns>
        public override string GetValue(object inst)
        {
            return Name;
        }
    }
}
