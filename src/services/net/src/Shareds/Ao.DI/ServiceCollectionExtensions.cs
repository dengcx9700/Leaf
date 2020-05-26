using Ao.DI;
using Ao.DI.Lookup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services,IServiceCreator serviceCreator)
        {
            return new ServiceProvider(services, serviceCreator);
        }
    }
}
