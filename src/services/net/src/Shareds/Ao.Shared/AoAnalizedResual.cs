namespace Ao
{
    /// <summary>
    /// 表示分析的结果
    /// </summary>
    public class AoAnalizedResual : AoAnalizedItem
    {
        /// <summary>
        /// 初始化<see cref="AoAnalizedResual"/>
        /// </summary>
        /// <param name="source">目标源</param>
        public AoAnalizedResual(object source) 
            : base(source)
        {
        }
        /// <summary>
        /// 分析的这一层的属性项
        /// </summary>
        public AoAnalizedPropertyItemBase[] MemberItems { get; internal set; }
        /// <summary>
        /// 分析的这一层的方法项
        /// </summary>
        public AoAnalizedMethodItemBase[] MethodItems { get; internal set; }
        /// <summary>
        /// 下一些层
        /// </summary>
        public AoAnalizedResual[] Nexts { get; internal set; }
    }

}
