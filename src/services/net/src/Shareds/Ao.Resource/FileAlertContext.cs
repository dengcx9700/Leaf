using System.IO;

namespace Ao.Resource
{
    /// <summary>
    /// 表示文件更改的消息上下文
    /// </summary>
    public struct FileAlertContext
    {
        public FileAlertContext(bool isInnerChange, FileSystemEventArgs fileSystemEvent)
        {
            IsInnerChange = isInnerChange;
            FileSystemEvent = fileSystemEvent;
            RenamedEvent = null;
        }

        public FileAlertContext(bool isInnerChange, RenamedEventArgs renamedEvent)
        {
            IsInnerChange = isInnerChange;
            RenamedEvent = renamedEvent;
            FileSystemEvent = renamedEvent;
        }

        /// <summary>
        /// 是否内部节点更改的
        /// </summary>
        public bool IsInnerChange { get; }
        /// <summary>
        /// 文件被操作的事件信息
        /// </summary>
        public FileSystemEventArgs FileSystemEvent { get; }
        /// <summary>
        /// 如果是重命名，这属性指示重命名的信息
        /// </summary>
        public RenamedEventArgs RenamedEvent { get; }

    }
}
