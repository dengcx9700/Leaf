using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;

namespace Ao.Core
{
    /// <summary>
    /// 包服务的定义
    /// </summary>
    /// <typeparam name="TPackage">包类型</typeparam>
    /// <typeparam name="TInheritType">包内容类型</typeparam>
    /// <typeparam name="TAddingType">添加的类型</typeparam>
    public interface IPackagingService<TPackage, TInheritType, TAddingType>
        where TPackage:PackageBase<TInheritType>
    {
        /// <summary>
        /// 同步根
        /// </summary>
        object SyncRoot { get; }
        /// <summary>
        /// view类型集合
        /// </summary>
        IReadOnlyCollection<TPackage> Packages { get; }
        /// <summary>
        /// 包含的所有<see cref="TInheritType"/>对象
        /// </summary>
        IReadOnlyCollection<TInheritType> Inherits { get; }
        /// <summary>
        /// 表示<see cref="Packages"/>集合改变了
        /// </summary>
        event Action<NotifyCollectionChangedAction, TPackage> PackagesChanged;
        /// <summary>
        /// 添加<see cref="View"/>类型
        /// </summary>
        /// <param name="views">类型集合</param>
        /// <returns>成功添加入的类型</returns>
        TInheritType[] Add(Assembly assembly, params TInheritType[] views);
        /// <summary>
        /// 类型条件
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <returns>是否合适</returns>
        bool Condition(TInheritType type);
        /// <summary>
        /// 移除整个视图包裹，根据程序集找
        /// </summary>
        /// <param name="assembly">目标程序集</param>
        /// <param name="removedPkg">被移除的包，如果找不到或者权限不够返回null</param>
        /// <returns></returns>
        bool Remove(Assembly assembly, TPackage removedPkg);
        /// <summary>
        /// 移除某一项视图项
        /// </summary>
        /// <param name="assembly">目标程序集</param>
        /// <param name="removes">需要移除的视图项目</param>
        /// <returns>移除成功的集合</returns>
        TInheritType[] Remove(Assembly assembly, params TInheritType[] removes);
        /// <summary>
        /// 清除使所有视图包
        /// </summary>
        /// <returns></returns>
        bool Clear();
    }

}
