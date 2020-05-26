using System.IO;
using System;

namespace Ao.Resource
{
    /// <summary>
    /// 文件操作的结果
    /// </summary>
    public class FileAccessResult : IOAccessResult
    {
        public FileAccessResult(WatcherChangeTypes changeType, ResourceNode node, bool succeed, Exception exception) : base(changeType, node, succeed, exception)
        {
        }
    }
}
