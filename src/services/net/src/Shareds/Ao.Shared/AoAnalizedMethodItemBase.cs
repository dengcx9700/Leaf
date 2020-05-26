using System.Reflection;

namespace Ao
{
    /// <summary>
    /// 表示正在分析中的方法项基类
    /// </summary>
    public abstract class AoAnalizedMethodItemBase : AoAnalizedItem
    {
        /// <summary>
        /// 调用器
        /// </summary>
        protected AoMemberInvoker<object> invoker;
        /// <summary>
        /// 初始化<see cref="AoAnalizedMethodItemBase"/>
        /// </summary>
        /// <param name="source">目标源</param>
        public AoAnalizedMethodItemBase(object source) : base(source)
        {
        }
        /// <summary>
        /// 引用的方法
        /// </summary>
        public MethodInfo Method { get; internal set; }
        /// <summary>
        /// 调用者
        /// </summary>
        public AoMemberInvoker<object> Invoker => invoker?? InitInvoker();
        /// <summary>
        /// 调用初始化
        /// </summary>
        /// <returns></returns>
        protected abstract AoMemberInvoker<object> InitInvoker();
    }
}
