using System.IO;
using System;

namespace Ao.Resource
{
    /// <summary>
    /// 表示IO操作的结果
    /// </summary>
    public abstract class IOAccessResult
    {
        public IOAccessResult(WatcherChangeTypes changeType, ResourceNode node, bool succeed, Exception exception)
        {
            ChangeType = changeType;
            Node = node;
            Succeed = succeed;
            Exception = exception;
        }

        /// <summary>
        /// 操作的类型
        /// </summary>
        public WatcherChangeTypes ChangeType { get; }
        /// <summary>
        /// 操作的结点
        /// </summary>
        public ResourceNode Node { get; }
        /// <summary>
        /// 是否成功了
        /// </summary>
        public bool Succeed { get; }
        /// <summary>
        /// 操作出现的异常
        /// </summary>
        public Exception Exception { get; }

    }
}
