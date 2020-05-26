using Ao.DI.Lookup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ao.DI
{
    internal class ServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection services;
        private readonly IServiceCreator serviceCreator;
        private readonly ServiceRealizer serviceRealizer;
        private readonly ServiceDescriptor[] servicesDescriptors;
        public ServiceProvider(IServiceCollection services, IServiceCreator serviceCreator)
        {
            this.services = services;
            this.serviceCreator = serviceCreator;
            servicesDescriptors = services.ToArray();
            serviceRealizer = new ServiceRealizer(servicesDescriptors);
            BuildNewers();
        }
        private void BuildNewers()
        {
            serviceRealizer.Build(serviceCreator);
        }
        //编译Translation->Scope->Singleton
        //让他们可以直接调用方法创建
        public object GetService(Type serviceType)
        {
            return serviceRealizer.Get(serviceType);
        }
    }
}
