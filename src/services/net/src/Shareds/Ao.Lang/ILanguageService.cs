using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Ao.Core;

namespace Ao.Lang
{
    /// <summary>
    /// 语言服务
    /// </summary>
    public interface ILanguageService : IPackagingService<DefaultPackage<ILanguageMetadata>, ILanguageMetadata, DefaultPackage<ILanguageMetadata>>, ICollectionChangeReproducible
    {
        /// <summary>
        /// 获取当前语言文化的字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string this[string key] { get; }
        /// <summary>
        /// 支持的语言文化
        /// </summary>
        IReadOnlyCollection<CultureInfo> SupportCultures { get; }
        /// <summary>
        /// 获取一个语言节点
        /// </summary>
        /// <param name="cultureInfo">语言文化</param>
        /// <returns></returns>
        ILanguageRoot GetRoot(CultureInfo cultureInfo);
        /// <summary>
        /// 获取该语言的语言节点是否已经建造了
        /// </summary>
        /// <param name="cultureInfo">语言文化</param>
        /// <returns></returns>
        bool IsBuilt(CultureInfo cultureInfo);
    }
}
