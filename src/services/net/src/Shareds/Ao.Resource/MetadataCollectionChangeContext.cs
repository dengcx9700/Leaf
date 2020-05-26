using System.IO;

namespace Ao.Resource
{
    /// <summary>
    /// 表示元数据集合更改的上下文
    /// </summary>
    public struct MetadataCollectionChangeContext
    {
        public MetadataCollectionChangeContext(bool isInnerChange, WatcherChangeTypes changeType, FileSystemEventArgs fileSystemEvent, IResourceMetadata oldMedata, IResourceMetadata newMedata)
        {
            IsInnerChange = isInnerChange;
            ChangeType = changeType;
            FileSystemEvent = fileSystemEvent;
            OldMedata = oldMedata;
            NewMedata = newMedata;
        }

        /// <summary>
        /// 是否内部节点更改的
        /// </summary>
        public bool IsInnerChange { get; }
        /// <summary>
        /// 更改的类型
        /// </summary>
        public WatcherChangeTypes ChangeType { get; }
        /// <summary>
        /// 文件信息
        /// </summary>
        public FileSystemEventArgs FileSystemEvent{ get; }
        /// <summary>
        /// 就的资源元数据
        /// </summary>
        public IResourceMetadata OldMedata { get; }
        /// <summary>
        /// 新的资源元数据
        /// </summary>
        public IResourceMetadata NewMedata { get; }
    }
}
