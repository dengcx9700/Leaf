using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Ao.DI.Lookup
{
    public class CreateNewInfo
    {
        public CreateNewInfo(ServiceDescriptor targetServiceDescript, 
            ConstructorInfo targetConstructor, Dictionary<Type, ServiceNewer> serviceNewers, 
            ServicesInfo servicesInfo, Dictionary<Type, object> scopeTable)
        {
            TargetServiceDescript = targetServiceDescript;
            TargetConstructor = targetConstructor;
            ServiceNewers = serviceNewers;
            ServicesInfo = servicesInfo;
            ScopeTable = scopeTable;
        }

        public ServiceDescriptor TargetServiceDescript { get; }
        public ConstructorInfo TargetConstructor { get;  }
        public Dictionary<Type, ServiceNewer> ServiceNewers{ get; }
        public ServicesInfo ServicesInfo { get; }
        public Dictionary<Type, object> ScopeTable{ get; }
    }
}
