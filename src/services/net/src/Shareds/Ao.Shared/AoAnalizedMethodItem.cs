using System.Reflection;

namespace Ao
{
    /// <summary>
    /// 表示正在分析中的方法项
    /// </summary>
    public class AoAnalizedMethodItem:AoAnalizedMethodItemBase
    {
        /// <summary>
        /// 初始化<see cref="AoAnalizedMethodItem"/>
        /// </summary>
        /// <param name="source">目标源</param>
        /// <param name="method">目标方法</param>
        public AoAnalizedMethodItem(object source, MethodInfo method)
            : base(source)
        {
            Method = method;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override AoMemberInvoker<object> InitInvoker()
        {
            invoker = ReflectionHelper.GetInvoker(Source, Method);
            return invoker;
        }
    }
}
