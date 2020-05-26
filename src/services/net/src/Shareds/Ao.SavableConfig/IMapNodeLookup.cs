using System;
using System.Linq.Expressions;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示对象图的查找
    /// </summary>
    /// <typeparam name="TCurrent">当前节点类型</typeparam>
    /// <typeparam name="TNext">下一个节点的类型</typeparam>
    public interface IMapNodeLookup<TCurrent, TNext>
    {
        /// <summary>
        /// 获取当前路径的节点
        /// </summary>
        SettingMapNode Node { get; }
        /// <summary>
        /// 当前的路径
        /// </summary>
        string Path { get; }
        /// <summary>
        /// 当前的值
        /// </summary>
        object Value { get; }
        /// <summary>
        /// 然后
        /// </summary>
        /// <typeparam name="TThen">下一个节点的类型</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IMapNodeLookup<TNext, TThen> Then<TThen>(Expression<Func<TNext, TThen>> expression);
    }
}