using System.Threading.Tasks;

namespace Ao.Middleware
{
    /// <summary>
    /// 直接的中间件
    /// </summary>
    /// <typeparam name="TContext">上下文</typeparam>
    internal class DirectMiddleware<TContext> : IInvokable<TContext>
    {
        private readonly Handler<TContext> next;
        private readonly Handler<TContext> worker;

        public DirectMiddleware(Handler<TContext> worker, Handler<TContext> next)
        {
            this.worker = worker;
            this.next = next;
        }

        public async Task InvokeAsync(MiddlewareContext<TContext> context)
        {
            await worker(context);
            await next(context);
        }
    }
}
