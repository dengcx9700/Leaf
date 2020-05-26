using Ao.Middleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Flow
{
    /// <summary>
    /// 表示一个启动服务
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IStartUp<TContext>
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services">服务容器</param>
        void ConfigureServices(IServiceCollection services);
        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="builder">中间件建造器</param>
        void Configure(IMiddlewareBuilder<TContext> builder);
    }
}
