using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ao.DI.Lookup
{
    /// <summary>
    /// 服务生成者
    /// </summary>
    /// <returns></returns>
    public delegate object ServiceNewer();
    /// <summary>
    /// 服务解析者
    /// </summary>
    public class ServiceRealizer
    {
        private readonly ServicesInfo servicesInfo;
        public HashSet<ServiceDescriptor> ServicesDescriptors { get; }
        public Dictionary<Type, ServiceDescriptor> ServicesDescriptorsDic { get; }
        public Dictionary<Type, ServiceNewer> ServiceNewers { get; }
        public Dictionary<Type, CreateNewInfo> CreateNewInfos { get; }
        public Dictionary<Type,object> SingletonInstances { get; }
        public ServiceRealizer(ServiceDescriptor[] servicesDescriptors)
        {
            var implTypes = servicesDescriptors.Select(d => d.ImplementationType).ToArray();
            var serviceTypes = servicesDescriptors.Select(d => d.ServiceType).ToArray();
            ServicesDescriptors = new HashSet<ServiceDescriptor>(servicesDescriptors);
            ServicesDescriptorsDic = new Dictionary<Type, ServiceDescriptor>();
            CreateNewInfos = new Dictionary<Type, CreateNewInfo>();
            SingletonInstances = new Dictionary<Type, object>();
            foreach (var item in ServicesDescriptors)
            {
                ServicesDescriptorsDic.Add(item.ServiceType, item);
            }
            servicesInfo = new ServicesInfo(this,ServicesDescriptorsDic,SingletonInstances,
                servicesDescriptors, implTypes, serviceTypes);
            
            ServiceNewers = new Dictionary<Type, ServiceNewer>();
        }
        public void Build(IServiceCreator serviceCreator)
        {
            foreach (var item in ServicesDescriptors)
            {
                if (!ServiceNewers.ContainsKey(item.ServiceType))
                {
                    BuildNewer(item, serviceCreator, new Dictionary<Type, object>(),new HashSet<Type>());
                }
            }
        }
        public object Get(Type service)
        {
            if (!ServiceNewers.ContainsKey(service))
            {
                return null;
            }
            if (ServicesDescriptorsDic.TryGetValue(service,out var desc))
            {
                CreateNewInfos[desc.ServiceType].ScopeTable.Clear();
                return ServiceNewers[service]();
            }
            return null;
        }
        private ServiceNewer BuildNewer(ServiceDescriptor target,IServiceCreator serviceCreator, Dictionary<Type,object> scopeTable,HashSet<Type> analizingSet)
        {

            if (ServiceNewers.TryGetValue(target.ServiceType, out var n) && n != null) 
            {
                return ServiceNewers[target.ServiceType];
            }
            if (analizingSet.Contains(target.ServiceType))
            {
                throw new InvalidOperationException($"类型{target.ServiceType.FullName}循环引用");
            }
            //创建生成链
            var selectedConstructor = serviceCreator.SelectConstructor(target.ImplementationType, servicesInfo);
            if (selectedConstructor == null)
            {
                return null;
            }
            var paramters = selectedConstructor.GetParameters();
            //var parLists = new ServiceNewer[paramters.Length];
            for (int i = 0; i < paramters.Length; i++)
            {
                var item = paramters[i];
                //if (!ServicesDescriptorsDic.ContainsKey(item.ParameterType))
                //{
                //    throw new InvalidOperationException($"不能解析服务{item.ParameterType.FullName}");
                //}
                //if (!ServiceNewers.TryGetValue(item.ParameterType, out var sn))
                {
                    if (!ServicesDescriptorsDic.ContainsKey(item.ParameterType))
                    {
                        if (item.HasDefaultValue&&!ServiceNewers.ContainsKey(item.ParameterType))
                        {
                            ServiceNewers.Add(item.ParameterType, () => item.DefaultValue);
                            continue;
                        }
                    }
                    var parDesc = ServicesDescriptorsDic[item.ParameterType];
                    //if (target.Lifetime < parDesc.Lifetime) 
                    //{
                    //    throw new InvalidOperationException($"依赖校验失败：{target.Lifetime}不能依赖{parDesc.Lifetime}");
                    //}
                    analizingSet.Add(target.ServiceType);
                    BuildNewer(parDesc, serviceCreator, scopeTable, analizingSet);
                }
            }
            //创建目标newer
            var createNewInfo = new CreateNewInfo(target,selectedConstructor, ServiceNewers, servicesInfo,scopeTable);
            var newer = serviceCreator.CreateNewer(target.ImplementationType, createNewInfo);
            CreateNewInfos.Add(target.ServiceType, createNewInfo);
            ServiceNewers.Add(target.ServiceType, newer);
            return newer;
        }
    }

}
