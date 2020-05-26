using System.Threading.Tasks;

namespace Ao.Middleware
{
    /// <summary>
    /// 空终结点
    /// </summary>
    /// <typeparam name="TContxt"></typeparam>
    public class NullEndPoint<TContxt> : IMiddleware<TContxt>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context"><inheritdoc/></param>
        /// <param name="next"><inheritdoc/></param>
        /// <returns></returns>
        public Task InvokeAsync(MiddlewareContext<TContxt> context, Handler<TContxt> next)
        {
#if NET452
            return new Task(()=>{});
#else
            return Task.CompletedTask;
#endif
        }
    }
}
