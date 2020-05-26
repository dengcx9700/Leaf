using System;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示设置信息的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =false)]
    public abstract class SettingInfoAttribute : Attribute
    {
        /// <summary>
        /// 初始化<see cref="SettingInfoAttribute"/>
        /// </summary>
        /// <param name="stringKey"><inheritdoc cref="StringKey"/></param>
        protected SettingInfoAttribute(string stringKey)
            :this(stringKey,null)
        {
        }
        /// <summary>
        /// <inheritdoc cref="SettingInfoAttribute(string)"/>
        /// </summary>
        /// <param name="stringKey"><inheritdoc cref="StringKey"/></param>
        /// <param name="default"><inheritdoc cref="Default"/></param>
        protected SettingInfoAttribute(string stringKey, string @default)
        {
            if (string.IsNullOrEmpty(stringKey))
            {
                throw new ArgumentException("message", nameof(stringKey));
            }

            StringKey = stringKey;
            Default = @default;
        }

        /// <summary>
        /// 字符串键
        /// </summary>
        public string StringKey { get; }
        /// <summary>
        /// 默认字符串
        /// </summary>
        public string Default { get; }
    }
}
