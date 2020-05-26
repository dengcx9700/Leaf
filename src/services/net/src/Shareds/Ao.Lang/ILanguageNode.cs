using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Ao.Lang
{
    /// <summary>
    /// 标识语言节点
    /// </summary>
    internal interface ILanguageNode : ICollectionChangeReproducible, INotifyCollectionChanged, IList<ILanguageMetadata>, ICollection<ILanguageMetadata>, IEnumerable<ILanguageMetadata>, ICultureIdentity
    {
        /// <summary>
        /// 当前节点是否已被建立
        /// </summary>
        bool IsBuilt { get; }
        /// <summary>
        /// 当前节点的根节点
        /// </summary>
        ILanguageRoot Root { get; }
    }
}