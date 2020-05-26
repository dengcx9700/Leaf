using System.Collections.Generic;

namespace Ao.Resource
{
    /// <summary>
    /// 表示一个节点
    /// </summary>
    public interface INodeble
    {
        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 所有的节点
        /// </summary>
        IReadOnlyCollection<INodeble> AllNexts { get; }
    }
}
