using Microsoft.Extensions.Configuration;

namespace Ao.Lang
{
    /// <summary>
    /// 语言元数据
    /// </summary>
    public interface ILanguageMetadata:ICultureIdentity
    {
        /// <summary>
        /// 语言源
        /// </summary>
        IConfigurationSource[] LanguageSources { get; }
    }
}
