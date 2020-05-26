using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Ao.Core
{
    /// <summary>
    /// 表示包的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PackageBase<T> : IEnumerable<T>, IPackageIdentity
    {
        internal readonly List<T> medatas;
        /// <summary>
        /// 提供方的程序集
        /// </summary>
        public Assembly Assembly { get; }

        public PackageBase(Assembly assembly)
        {
            Assembly = assembly;
            medatas = new List<T>();
        }


        /// <summary>
        /// 提供
        /// </summary>
        public IReadOnlyCollection<T> Medatas => medatas;

        public IEnumerator<T> GetEnumerator()
        {
            return medatas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
