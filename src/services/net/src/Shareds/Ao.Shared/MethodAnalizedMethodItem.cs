namespace Ao
{
    /// <summary>
    /// 表示一个正在分析的方法项
    /// </summary>
    public class MethodAnalizedMethodItem : AoAnalizedMethodItemBase
    {
        /// <summary>
        /// 初始化<see cref="MethodAnalizedMethodItem"/>
        /// </summary>
        /// <param name="source">目标源</param>
        /// <param name="invoker">方法调用器</param>
        public MethodAnalizedMethodItem(object source, AoMemberInvoker<object> invoker)
            : base(source)
        {
            this.invoker = invoker;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override AoMemberInvoker<object> InitInvoker()
        {
            return invoker;
        }
    }
}
