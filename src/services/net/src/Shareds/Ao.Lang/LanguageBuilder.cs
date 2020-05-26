using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Linq;

namespace Ao.Lang
{
    /// <summary>
    /// 语言建造器的实现
    /// </summary>
    public class LanguageBuilder : ConfigurationBuilder, ILanguageBuilder,IConfigurationBuilder
    {
        public LanguageBuilder(CultureInfo culture)
        {
            Culture = culture;
        }

        /// <summary>
        /// 语言
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        IConfigurationRoot IConfigurationBuilder.Build()
        {
            var providers = Sources.Select(s => s.Build(this)).ToArray();
            return new LanguageRoot(Culture, providers);
        }
        /// <summary>
        /// 建造一个语言根节点
        /// </summary>
        /// <returns></returns>
        public new ILanguageRoot Build()
        {
            var providers = Sources.Select(s => s.Build(this)).ToArray();
            return new LanguageRoot(Culture, providers);
        }
    }
}
