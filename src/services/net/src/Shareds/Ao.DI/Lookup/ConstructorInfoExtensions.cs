using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class ConstructorInfoExtensions
    {
        public static IEnumerable<ConstructorInfo> SelectUseableConstructors(this ConstructorInfo[] cis, IEnumerable<Type> serviceTypes)
        {
            if (cis==null)
            {
                return null;
            }
            return cis.Where(c => c.GetParameters().All(p => serviceTypes.Contains(p.ParameterType)||p.HasDefaultValue));
        }
        public static ConstructorInfo SelectUseableConstructor(this ConstructorInfo[] cis, IEnumerable<Type> serviceTypes)
        {
            return cis.SelectUseableConstructors(serviceTypes).FirstOrDefault();
        }
    }

}
