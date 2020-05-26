using Ao.Core;
using System;

namespace Ao.Project
{
    /// <summary>
    /// 工程部分提供者
    /// </summary>
    public interface IProjectPartProvider : IIdentitiyable
    {
        /// <summary>
        /// 目标类型,生成的类型都会通过设计器被用户设计
        /// <para>
        /// 返回的对象必须实现<see cref="IPropertyGroupItem"/>或者<see cref="IItemGroupPart"/>,如果2个都实现了,则2个都会被使用
        /// </para>
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// 创建类型
        /// </summary>
        /// <param name="default">是否是默认创建</param>
        /// <returns></returns>
        object Create(bool @default);
        /// <summary>
        /// 是否请求默认
        /// </summary>
        bool Default { get; }
    }
}
