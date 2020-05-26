using System.Collections.Generic;
using System;
using Ao.Core;

namespace Ao.Resource
{
    /// <summary>
    /// 表示文件资源节点
    /// </summary>
    public class FileNode : NotifyableObject, INodeble
    {
        private static readonly INodeble[] Empty =
#if NET452
            new INodeble[0];
#else
            Array.Empty<INodeble>();
#endif
        public FileNode(IResourceMetadata resourceMetadata, ResourceNode node)
        {
            ResourceMetadata = resourceMetadata;
            Node = node;
            Name = ResourceMetadata.Name;
            AllNexts = Empty;
        }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 资源元数据
        /// </summary>
        public IResourceMetadata ResourceMetadata { get; }
        /// <summary>
        /// 所属节点
        /// </summary>
        public ResourceNode Node { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<INodeble> AllNexts { get; }
    }
}
