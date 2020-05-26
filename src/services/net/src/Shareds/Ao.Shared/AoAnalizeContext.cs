using System.Collections.Generic;

namespace Ao
{
    /// <summary>
    /// 表示一个解析中的上下文
    /// </summary>
    public class AoAnalizeContext : AoAnalizedItem
    {
        /// <summary>
        /// 初始化<see cref="AoAnalizeContext"/>
        /// </summary>
        /// <param name="source">目标源</param>
        internal AoAnalizeContext(object source)
            : base(source)
        {
        }

        /// <summary>
        /// 对于循环引用的对象，此实例为了防止此情况
        /// </summary>
        internal HashSet<object> AnalizedObject { get;set; }
    }

}
