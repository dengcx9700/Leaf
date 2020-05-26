using System;

namespace Ao.Middleware
{
    /// <summary>
    /// 表示一个服务框架的上下文接口
    /// </summary>
    public interface IServicesContext
    {
        /// <summary>
        /// 服务提供器
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}
