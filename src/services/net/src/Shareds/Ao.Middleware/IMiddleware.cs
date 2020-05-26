using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Middleware
{
    /// <summary>
    /// 表示一个中间件
    /// </summary>
    public interface IMiddleware<TContext>
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="next">下一个中间件</param>
        /// <returns></returns>
        Task InvokeAsync(MiddlewareContext<TContext> context, Handler<TContext> next);
    }
}
