using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Data
{
    public interface IApiConfiguration
    {
        /// <summary>
        /// api超时的时间
        /// </summary>
        long ApiOutTimeMillSec { get; }
        /// <summary>
        /// 分布式锁超时的时间
        /// </summary>
        long RedLockOuttimeMillSec { get; }

        long CommentCacheMillTime { get; }

        long ArticleFindCacheMillTime { get; }

        long NotExistCacheMillTime { get; }
    }
    public class ApiConfiguration: IApiConfiguration
    {
        public long ApiOutTimeMillSec { get; set; }

        public long RedLockOuttimeMillSec { get; set; }

        public long CommentCacheMillTime { get; set; }

        public long NotExistCacheMillTime { get; set; }

        public long ArticleFindCacheMillTime { get; set; }
    }
}
