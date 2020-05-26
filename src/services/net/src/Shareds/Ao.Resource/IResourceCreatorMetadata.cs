using Ao.Core;
using System;

namespace Ao.Resource
{
    /// <summary>
    /// 表示资源创建器的元数据
    /// </summary>
    public interface IResourceCreatorMetadata : IIdentitiyable
    {
        /// <summary>
        /// 表示资源扩展名字，如png
        /// </summary>
        string Extension { get; }
        /// <summary>
        /// 创建场景器
        /// </summary>
        /// <returns></returns>
        IResourceCreator CreateCreator();
    }
}
