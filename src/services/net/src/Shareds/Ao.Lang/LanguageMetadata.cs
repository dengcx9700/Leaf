using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Ao.Lang
{
    /// <summary>
    /// 默认的语言元数据
    /// </summary>
    public abstract class LanguageMetadata : ILanguageMetadata,ICultureIdentity
    {
        protected LanguageMetadata()
        {

        }
        public LanguageMetadata(CultureInfo culture)
        {
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual CultureInfo Culture { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IConfigurationSource[] LanguageSources => GetConfigurationSources();
        /// <summary>
        /// 获取配置源
        /// </summary>
        /// <returns></returns>
        protected abstract IConfigurationSource[] GetConfigurationSources();
    }
}
