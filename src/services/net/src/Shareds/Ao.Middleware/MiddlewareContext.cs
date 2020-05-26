using System;

namespace Ao.Middleware
{
    /// <summary>
    /// 中间件上下文
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class MiddlewareContext<TContext>
    {
        public MiddlewareContext(IServiceProvider serviceProvider, TContext context)
        {
            ServiceProvider = serviceProvider;
            Context = context;
        }

        /// <summary>
        /// 服务提供者
        /// </summary>
        public IServiceProvider ServiceProvider { get; }
        /// <summary>
        /// 上下文
        /// </summary>
        public TContext Context { get; }
    }
}
