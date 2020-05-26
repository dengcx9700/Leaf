using Microsoft.Extensions.Configuration;

namespace Ao.Lang
{
    /// <summary>
    /// 语言建造器
    /// </summary>
    public interface ILanguageBuilder : IConfigurationBuilder, ICultureIdentity
    {

    }
}
