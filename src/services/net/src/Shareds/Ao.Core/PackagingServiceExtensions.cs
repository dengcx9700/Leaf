using System.Reflection;

namespace Ao.Core
{
    /// <summary>
    /// 对<see cref="IPackagingService{TPackage, TInheritType, TAddingType}"/>的扩展
    /// </summary>
    public static class PackagingServiceExtensions
    {
        /// <summary>
        /// 添加当前程序集的包内容
        /// </summary>
        /// <typeparam name="TPackage">包类型</typeparam>
        /// <typeparam name="TInheritType">包内容类型</typeparam>
        /// <typeparam name="TAddingType">添加的类型</typeparam>
        /// <param name="service">目标范围</param>
        /// <param name="metadatas">内容元数据集合</param>
        public static void Add<TPackage, TInheritType, TAddingType>(this IPackagingService<TPackage, TInheritType, TAddingType> service, params TInheritType[] metadatas)
                where TPackage : PackageBase<TInheritType>
        {
            var assembly = Assembly.GetCallingAssembly();
            service.Add(assembly, metadatas);
        }
        /// <summary>
        /// 移除一个包
        /// </summary>
        /// <typeparam name="TPackage">包类型</typeparam>
        /// <typeparam name="TInheritType">包内容类型</typeparam>
        /// <typeparam name="TAddingType">添加的类型</typeparam>
        /// <param name="service">目标范围</param>
        /// <param name="pkg">目标包</param>
        /// <returns></returns>
        public static bool Remove<TPackage, TInheritType, TAddingType>(this IPackagingService<TPackage, TInheritType, TAddingType> service, TPackage pkg)
                where TPackage : PackageBase<TInheritType>
        {
            var assembly = Assembly.GetCallingAssembly();
            return service.Remove(assembly, pkg);
        }
        /// <summary>
        /// 移除一些包内容
        /// </summary>
        /// <typeparam name="TPackage">包类型</typeparam>
        /// <typeparam name="TInheritType">包内容类型</typeparam>
        /// <typeparam name="TAddingType">添加的类型</typeparam>
        /// <param name="service">目标范围</param>
        /// <param name="inherits">包内容</param>
        /// <returns></returns>
        public static TInheritType[] Remove<TPackage, TInheritType, TAddingType>(this IPackagingService<TPackage, TInheritType, TAddingType> service, params TInheritType[] inherits)
                where TPackage : PackageBase<TInheritType>
        {
            var assembly = Assembly.GetCallingAssembly();
            return service.Remove(assembly, inherits);
        }
    }

}
