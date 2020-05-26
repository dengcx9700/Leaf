using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.DI.InjectWay
{
    [AttributeUsage(AttributeTargets.Interface| AttributeTargets.Class,Inherited =false,AllowMultiple =false)]
    public sealed class AoService : Attribute
    {
        public AoService(ServiceLifetime serviceType)
        {
            ServiceType = serviceType;
        }
        public AoService(ServiceLifetime serviceType, Type implementType)
            :this(serviceType)
        {
            ImplementType = implementType;
        }
        public Type ImplementType { get; }
        public ServiceLifetime ServiceType { get; }
    }
}
