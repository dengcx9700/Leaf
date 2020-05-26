using Ao.Middleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Flow
{
    /// <summary>
    /// 流式宿主建造者
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IFlowHostBuilder<TContext> : IMiddlewareBuilder<TContext>
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }

}
