using System;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示设置描述的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SettingDescriptAttribute : SettingInfoAttribute
    {
        /// <summary>
        /// 初始化<see cref="SettingDescriptAttribute"/>
        /// </summary>
        /// <param name="stringKey"><inheritdoc cref="SettingInfoAttribute.StringKey"/></param>
        public SettingDescriptAttribute(string stringKey) : base(stringKey)
        {
        }
        /// <summary>
        /// <inheritdoc cref="SettingDescriptAttribute(string)"/>
        /// </summary>
        /// <param name="stringKey"><inheritdoc cref="SettingInfoAttribute.StringKey"/></param>
        /// <param name="default"><inheritdoc cref="SettingInfoAttribute.Default"/></param>
        public SettingDescriptAttribute(string stringKey, string @default)
            : base(stringKey, @default)
        {
        }
    }
}
