using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ao.Resource
{
    internal static class ThrowHelper
    {
        public static void ThrowServiceNotFound(Type service)
        {
            throw new InvalidOperationException($"没能发现服务{service.FullName}");
        }
        public static void ThrowServiceNotFound<T>()
        {
            ThrowServiceNotFound(typeof(T));
        }
        public static void ThrowNotImplType(Type type,Type destType)
        {
            throw new NotSupportedException($"类型{destType.FullName}没实现类型{type.FullName}");
        }
        public static void ThrowNotImplType<T>(Type destType)
        {
            ThrowNotImplType(typeof(T), destType);
        }
        public static void ThrowCanNotDoThis(string msg)
        {
            throw new NotSupportedException(msg);
        }
        public static void ThrowProjectNotLoad()
        {
            throw new InvalidOperationException("工程为加载");
        }
        public static void ThrowResourceNotFound(string resourcePath)
        {
            throw new Exception($"资源 {resourcePath} 未找到");
        }
        public static void ThrowObjectDisponsed([CallerMemberName] string name=null)
        {
            throw new ObjectDisposedException(name);
        }
    }
}
