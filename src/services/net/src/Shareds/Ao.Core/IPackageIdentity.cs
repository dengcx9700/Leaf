using System.Reflection;

namespace Ao.Core
{
    /// <summary>
    /// 表示包裹的标识
    /// </summary>
    public interface IPackageIdentity
    {
        /// <summary>
        /// 标识程序集
        /// </summary>
        Assembly Assembly { get; }
    }
}