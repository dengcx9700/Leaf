using Microsoft.Extensions.Configuration;

namespace Ao.Lang
{
    /// <summary>
    /// 语言根节点
    /// </summary>
    public interface ILanguageRoot : IConfigurationRoot, ICultureIdentity
    {

    }
}
