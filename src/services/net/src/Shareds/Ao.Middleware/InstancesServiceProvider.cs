using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Middleware
{
    /// <summary>
    /// 实例服务提供者
    /// </summary>
    public class InstancesServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, Lazy<object>> typeToObjects;
        /// <summary>
        /// 获取服务时是否要求线程安全
        /// </summary>
        public bool ThreadSafe { get; }

        public InstancesServiceProvider(KeyValuePair<Type,Func<object>>[] objectFactory,bool threadSafe)
        {
            ThreadSafe = threadSafe;
            typeToObjects = objectFactory
                .ToDictionary(t => t.Key,t=>new Lazy<object>(t.Value,threadSafe));
        }

        public object GetService(Type serviceType)
        {
            if (typeToObjects.TryGetValue(serviceType,out var factory))
            {
                return factory.Value;
            }
            return null;
        }
    }
}
