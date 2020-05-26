using System;
using System.Collections.Generic;

namespace Ao.Middleware
{
    /// <summary>
    /// 表示中间件建造器
    /// </summary>
    /// <typeparam name="TContext">中间件上下文</typeparam>
    public interface IMiddlewareBuilder<TContext>
    {
        /// <summary>
        /// 中间件的只读集合
        /// </summary>
        IList<Func<Handler<TContext>, Handler<TContext>>> Handlers { get; }
        /// <summary>
        /// 创建中间件调用链
        /// </summary>
        /// <returns></returns>
        Handler<TContext> Build();
        /// <summary>
        /// 创建终结点
        /// </summary>
        /// <returns></returns>
        Handler<TContext> CreateEndPoint();
    }
}
