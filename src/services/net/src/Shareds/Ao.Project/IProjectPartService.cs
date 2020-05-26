using Ao.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Project
{
    /// <summary>
    /// 表示工程部分的服务
    /// </summary>
    public interface IProjectPartService:IPackagingService<ProjectPartPackage, IProjectPartProvider, ProjectPartPackage>
    {
        /// <summary>
        /// 类型实现了<see cref="IItemGroupPart"/>的元数据
        /// </summary>
        IReadOnlyCollection<IProjectPartProvider> GroupParts { get; }
        /// <summary>
        /// 类型实现了<see cref="IPropertyGroupItem"/>的元数据
        /// </summary>
        IReadOnlyCollection<IProjectPartProvider> PropertyParts { get; }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type">目标对象</param>
        /// <returns></returns>
        object Create(Type type);
    }
}
