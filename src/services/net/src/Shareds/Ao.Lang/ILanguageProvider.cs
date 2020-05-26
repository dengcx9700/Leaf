using Microsoft.Extensions.Configuration;

namespace Ao.Lang
{
    /// <summary>
    /// 语言提供者
    /// </summary>
    public interface ILanguageProvider : IConfigurationProvider, ICultureIdentity
    {
    }
}
