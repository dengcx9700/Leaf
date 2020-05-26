using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Ao.DI.Lookup
{
    public class ExpressionServiceCreator : IServiceCreator
    {
        private static readonly Type[] EmptyTypes = Array.Empty<Type>();
        private static readonly MethodInfo GetServiceMethod = typeof(ServiceRealizer).GetMethod(nameof(ServiceRealizer.Get), new Type[] { typeof(Type) });
        private static readonly MethodInfo DictionaryContainsKeyMethod = typeof(Dictionary<Type, object>).GetMethod("ContainsKey");
        private static readonly MethodInfo DictionaryAddKeyMethod = typeof(Dictionary<Type, object>).GetMethod("Add", new Type[] { typeof(Type), typeof(object) });
        private static readonly MethodInfo InvokeMethod = typeof(ServiceNewer).GetMethod(nameof(ServiceNewer.Invoke), EmptyTypes);
        private static readonly MethodInfo BeginInvokeMethod = typeof(ServiceNewer).GetMethod(nameof(ServiceNewer.BeginInvoke));
        private static readonly MethodInfo EndInvokeMethod = typeof(ServiceNewer).GetMethod(nameof(ServiceNewer.EndInvoke));
        public ServiceNewer CreateNewer(Type targetType, CreateNewInfo createNewInfo)
        {
            var pars = createNewInfo.TargetConstructor.GetParameters();
            var parExps = new Expression[pars.Length];
            for (int i = 0; i < pars.Length; i++)
            {
                var item = pars[i];
                ServiceNewer parNewer = null;
                if (createNewInfo.ServicesInfo.ServicesDescriptorsDic.TryGetValue(item.ParameterType, out var parDesc))
                {
                    if (createNewInfo.TargetServiceDescript.Lifetime < parDesc.Lifetime)
                    {
                        throw new InvalidOperationException($"依赖校验失败：{createNewInfo.TargetServiceDescript.Lifetime}不能依赖{parDesc.Lifetime}");
                    }
                    var parServiceDescript = createNewInfo.ServicesInfo.ServicesDescriptorsDic[item.ParameterType];
                    switch (parServiceDescript.Lifetime)
                    {
                        case ServiceLifetime.Singleton:

                            parNewer = () =>
                            {
                                if (!createNewInfo.ServicesInfo.SingletonInstances.TryGetValue(item.ParameterType, out var singletonInst))
                                {
                                    singletonInst = createNewInfo.ServiceNewers[item.ParameterType]();
                                    //createNewInfo.ServicesInfo.SingletonInstances.Add(item.ParameterType, singletonInst);
                                }
                                return singletonInst;
                            };
                            break;
                        case ServiceLifetime.Scoped:

                            parNewer = () =>
                            {
                                if (!createNewInfo.ScopeTable.TryGetValue(item.ParameterType, out var scopeInst))
                                {
                                    scopeInst = createNewInfo.ServiceNewers[item.ParameterType]();
                                    createNewInfo.ScopeTable.Add(item.ParameterType, scopeInst);
                                }
                                return scopeInst;
                            };
                            break;
                        case ServiceLifetime.Transient:
                            parNewer = createNewInfo.ServiceNewers[item.ParameterType];
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                else if (item.HasDefaultValue)
                {
                    parNewer = () => item.DefaultValue;
                }
                else
                {
                    throw new InvalidOperationException($"不能解析服务{item.ParameterType.FullName}");
                }
                parExps[i] = Expression.Convert(Expression.Call(Expression.Constant(parNewer), InvokeMethod), item.ParameterType);
            };
            ServiceNewer serviceNewer = null;
            var ret = Expression.Label(typeof(object));
            var call = Expression.New(createNewInfo.TargetConstructor, parExps);
            var n = Expression.Lambda<ServiceNewer>(call).Compile();
            var serviceType = createNewInfo.TargetServiceDescript.ServiceType;
            //createNewInfo.ServiceNewers.Add(createNewInfo.TargetServiceDescript.ServiceType, n);
            switch (createNewInfo.TargetServiceDescript.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    serviceNewer = () =>
                      {
                          if (!createNewInfo.ServicesInfo.SingletonInstances.TryGetValue(serviceType, out var value))
                          {
                              value = n();
                              createNewInfo.ServicesInfo.SingletonInstances.Add(serviceType, value);
                          }
                          return value;
                      };
                    break;
                case ServiceLifetime.Scoped:
                    serviceNewer = () =>
                    {
                        if (!createNewInfo.ScopeTable.TryGetValue(serviceType, out var res))
                        {
                            res = n();
                            //createNewInfo.ScopeTable.Add(serviceType, res);
                        }
                        return res;
                    };
                    break;
                case ServiceLifetime.Transient:
                    serviceNewer = n;
                    break;
                default:
                    break;
            }
            return serviceNewer;
        }
        public ConstructorInfo SelectConstructor(Type target, ServicesInfo servicesInfo)
        {
            return target.GetConstructors().SelectUseableConstructor(servicesInfo.ServiceTypes);
        }
    }
    //public class ExpressionServiceCreator : ServiceCreatorBase
    //{
    //    private readonly object LockerObject = new object();
    //    private static readonly Type[] EmptyTypes = Array.Empty<Type>();
    //    private static readonly MethodInfo InvokeMethod = typeof(ServiceNewer).GetMethod(nameof(ServiceNewer.Invoke), EmptyTypes);

    //    protected override ServiceNewer BuildScopeNewer(ServiceNewer targetNewer, Type targetType, CreateNewInfo createNewInfo)
    //    {
    //        return () =>
    //        {
    //            var serviceType = createNewInfo.TargetServiceDescript.ServiceType;
    //            if (!createNewInfo.ScopeTable.TryGetValue(serviceType, out var res))
    //            {
    //                res = targetNewer();
    //                createNewInfo.ScopeTable.TryAdd(serviceType, res);
    //            }
    //            return res;
    //        };
    //    }

    //    protected override ServiceNewer BuildSingletonNewer(ServiceNewer targetNewer, Type targetType, CreateNewInfo createNewInfo)
    //    {
    //        return () =>
    //        {
    //            var serviceType = createNewInfo.TargetServiceDescript.ServiceType;
    //            if (!createNewInfo.ServicesInfo.SingletonInstances.TryGetValue(serviceType, out var value))
    //            {
    //                value = targetNewer();
    //                createNewInfo.ServicesInfo.SingletonInstances.Add(serviceType, value);
    //            }
    //            return value;
    //        };
    //    }

    //    protected override ServiceNewer BuildTransientNewer(ServiceNewer targetNewer, Type targetType, CreateNewInfo createNewInfo)
    //    {
    //        return targetNewer;
    //    }

    //    protected override ServiceNewer CreateScopeNewer(ParameterInfo parameterInfo, Type targetType, CreateNewInfo createNewInfo)
    //    {
    //        return () =>
    //        {
    //            if (!createNewInfo.ScopeTable.TryGetValue(parameterInfo.ParameterType, out var scopeInst))
    //            {
    //                scopeInst = createNewInfo.ServiceNewers[parameterInfo.ParameterType]();
    //                //createNewInfo.ScopeTable.Add(parameterInfo.ParameterType, scopeInst);
    //            }
    //            return scopeInst;
    //        };
    //    }

    //    protected override ServiceNewer CreateSingletonNewer(ParameterInfo parameterInfo, Type targetType, CreateNewInfo createNewInfo)
    //    {
    //        return () =>
    //        {
    //            if (!createNewInfo.ServicesInfo.SingletonInstances.TryGetValue(parameterInfo.ParameterType, out var singletonInst))
    //            {
    //                singletonInst = createNewInfo.ServiceNewers[parameterInfo.ParameterType]();
    //                //createNewInfo.ServicesInfo.SingletonInstances.Add(item.ParameterType, singletonInst);
    //            }
    //            return singletonInst;
    //        };
    //    }

    //    protected override ServiceNewer CreateTransientNewer(ParameterInfo parameterInfo, Type targetType, CreateNewInfo createNewInfo)
    //    {
    //        return createNewInfo.ServiceNewers[parameterInfo.ParameterType];
    //    }

    //    protected override ServiceNewer GenerateTargetNewer(KeyValuePair<ParameterInfo, ServiceNewer>[] paramterNewersInfo, Type targetType, CreateNewInfo createNewInfo)
    //    {
    //        var parExps =new Expression[paramterNewersInfo.Length];
    //        for (int i = 0; i < parExps.Length; i++)
    //        {
    //            var item = paramterNewersInfo[i];
    //            parExps[i]= Expression.Convert(Expression.Call(Expression.Constant(item.Value), InvokeMethod), 
    //                item.Key.ParameterType);
    //        }
    //        var call = Expression.New(createNewInfo.TargetConstructor, parExps);
    //        return Expression.Lambda<ServiceNewer>(call).Compile();

    //    }
    //}
}
