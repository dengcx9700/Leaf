using System;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示设置名字的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SettingNameAttribute : SettingInfoAttribute
    {
        /// <summary>
        /// 初始化<inheritdoc cref="SettingNameAttribute"/>
        /// </summary>
        /// <param name="stringKey"><inheritdoc cref="SettingInfoAttribute.StringKey"/></param>
        public SettingNameAttribute(string stringKey) : base(stringKey)
        {
        }
        /// <summary>
        /// 初始化<inheritdoc cref="SettingNameAttribute"/>
        /// </summary>
        /// <param name="stringKey"><inheritdoc cref="SettingInfoAttribute.StringKey"/></param>
        /// <param name="default"><inheritdoc cref="SettingInfoAttribute.Default"/></param>
        public SettingNameAttribute(string stringKey, string @default)
            : base(stringKey, @default)
        {
        }
    }
}
