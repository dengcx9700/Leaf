using Ao.DI.InjectWay;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AssemblyLookUp
    {
        public static void LookUpAssembly(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            var types = assembly.GetTypes();
            foreach (var item in types)
            {
                var attr = item.GetCustomAttribute<AoService>();
                if (attr!=null)
                {
                    services.Add(new ServiceDescriptor(item, attr.ImplementType ?? item, attr.ServiceType));
                }
            }
        }
    }
}
