using System;

namespace Ao
{
    /// <summary>
    /// 表示正在分析中的数据项
    /// </summary>
    public abstract class AoAnalizedItem
    {
        /// <summary>
        /// 初始化<see cref="AoAnalizedItem"/>
        /// </summary>
        /// <param name="source">目标源</param>
        protected AoAnalizedItem(object source)
        {
            Source = source;
            SourceType = source?.GetType();
        }
        /// <summary>
        /// 目标源
        /// </summary>
        public object Source { get;  }
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type SourceType { get; }
    }

}
