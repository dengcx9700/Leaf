using System.IO;
using System;

namespace Ao.Resource
{
    /// <summary>
    /// 目录操作的结果
    /// </summary>
    public class FolderAccessResult : IOAccessResult
    {
        public FolderAccessResult(WatcherChangeTypes changeType, ResourceNode node, bool succeed, Exception exception) : base(changeType, node, succeed, exception)
        {
        }
    }
}
