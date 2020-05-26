using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

namespace Ao.Middleware
{
    /// <summary>
    /// 中间件建造器
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class MiddlewareBuilder<TContext> : IMiddlewareBuilder<TContext>
    {
        private readonly Handler<TContext> EmptyHandler = _ =>
#if NET452
        Task.FromResult(0)
#else
         Task.CompletedTask
#endif
        ;
        public MiddlewareBuilder()
        {
            Handlers = new List<Func<Handler<TContext>, Handler<TContext>>>();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IList<Func<Handler<TContext>, Handler<TContext>>> Handlers { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public virtual Handler<TContext> Build()
        {
            var part = CreateEndPoint();

            foreach (var item in Handlers.Reverse())
            {
                part = item(part);
            }
            return part;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public Handler<TContext> CreateEndPoint()
        {
            return context => new NullEndPoint<TContext>().InvokeAsync(context, EmptyHandler);
        }
    }
}
