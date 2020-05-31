using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Leaf.Data
{
    /// <summary>
    /// 热点寻找器
    /// </summary>
    public class HotFinder
    {
        /// <summary>
        /// 命中策略集合
        /// </summary>
        public HitTrigger[] HitTriggers { get; set; }
        /// <summary>
        /// 一开始命中的缓存时间
        /// </summary>
        public int StartMillSec { get; set; }
        /// <summary>
        /// 全命中增加的缓存时间
        /// </summary>
        public int AllHitIncMillSec { get; set; }
    }
}
