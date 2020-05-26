using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ao.Middleware
{
    /// <summary>
    /// 实例服务集合
    /// </summary>
    public class InstancesServiceCollection : IEnumerable<KeyValuePair<Type, Func<object>>>
    {
        private readonly Dictionary<Type, Func<object>> services = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// 当前加入的服务
        /// </summary>
        private IReadOnlyDictionary<Type, Func<object>> Services => services;
        /// <summary>
        /// 添加一个服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="objMaker">对象实例</param>
        public void Add(Type type,Func<object> objMaker)
        {
            if (services.ContainsKey(type))
            {
                services[type] = objMaker;
            }
            else
            {
                services.Add(type, objMaker);
            }
        }
        /// <summary>
        /// 添加一个服务并在使用时创建此实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public void Add<T>(Type type)
            where T:new()
        {
            Add(type, () => new T());
        }
        /// <summary>
        /// 添加一个固定实例服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inst"></param>
        public void Add(Type type,object inst)
        {
            Add(type, () => inst);
        }
        public IEnumerator<KeyValuePair<Type, Func<object>>> GetEnumerator()
        {
            return services.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// 生成服务提供者
        /// </summary>
        /// <param name="threadSafe">是否要求线程安全</param>
        /// <returns></returns>
        public IServiceProvider Build(bool threadSafe = true)
        {
            return new InstancesServiceProvider(services.ToArray(), threadSafe);
        }
    }
}
