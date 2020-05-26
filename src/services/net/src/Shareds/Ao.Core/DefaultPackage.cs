using System.Reflection;

namespace Ao.Core
{
    /// <summary>
    /// 默认的包
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultPackage<T> : PackageBase<T>
    {
        public DefaultPackage(Assembly assembly)
            : base(assembly)
        {
        }
    }
}
