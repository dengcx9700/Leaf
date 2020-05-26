using System.Threading.Tasks;

namespace Ao.Middleware
{
    /// <summary>
    /// 表示可以调用的
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IInvokable<TContext>
    {
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        Task InvokeAsync(MiddlewareContext<TContext> context);
    }
}
