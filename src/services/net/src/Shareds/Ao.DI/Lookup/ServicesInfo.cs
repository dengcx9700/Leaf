using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
namespace Ao.DI.Lookup
{
    public class ServicesInfo
    {
        public ServicesInfo(ServiceRealizer serviceRealizer,Dictionary<Type, ServiceDescriptor> servicesDescriptorsDic, Dictionary<Type, object> singletonInstances, ServiceDescriptor[] servicesDescriptors, IEnumerable<Type> implTypes, IEnumerable<Type> serviceTypes)
        {
            ServicesDescriptorsDic = servicesDescriptorsDic;
            SingletonInstances = singletonInstances;
            ServicesDescriptors = servicesDescriptors;
            ServiceRealizer = serviceRealizer;
            ImplTypes = new HashSet<Type>(implTypes);
            ServiceTypes = new HashSet<Type>(serviceTypes);
        }
        public ServiceRealizer ServiceRealizer { get; }
        public Dictionary<Type, ServiceDescriptor> ServicesDescriptorsDic{ get; }
        public Dictionary<Type, object> SingletonInstances{ get; }
        public ServiceDescriptor[] ServicesDescriptors{ get; }
        public HashSet<Type> ImplTypes { get; }
        public HashSet<Type> ServiceTypes { get; }
    }
}
