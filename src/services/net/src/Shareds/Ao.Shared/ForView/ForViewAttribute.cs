using System;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 这是指定属性使用哪一个view
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ForViewAttribute : Attribute
    {
        /// <summary>
        /// 初始化<see cref="ForViewAttribute"/>
        /// </summary>
        /// <param name="buildType"><inheritdoc cref="BuildType"/></param>
        public ForViewAttribute(Type buildType)
        {
            BuildType = buildType;
        }
        /// <summary>
        /// 建造类型
        /// </summary>
        public Type BuildType { get; }

    }
}
