using Ao.Shared.ForView.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示可以建造视图
    /// </summary>
    /// <typeparam name="TView">目标视图</typeparam>
    public interface IViewBuildable<TView> : IList<IViewBuilder<TView>>,ICollection<IViewBuilder<TView>>,IEnumerable<IViewBuilder<TView>>,IEnumerable
    {
        /// <summary>
        /// 字符串提供者
        /// </summary>
        IStringProvider StringProvider { get; }
        /// <summary>
        /// 当前的视图建造器集合
        /// </summary>
        IReadOnlyCollection<IViewBuilder<TView>> ViewBuilders { get; }
        /// <summary>
        /// 自定义视图建造器,对于特定类型
        /// </summary>
        IReadOnlyDictionary<Type, IViewBuilder<TView>> CustomBuilders { get; }
        /// <summary>
        /// 创建一个视图
        /// </summary>
        /// <param name="vm">视图模型</param>
        /// <param name="propertyItem">实现项</param>
        /// <returns></returns>
        TView Build(object vm,AoAnalizedPropertyItemBase propertyItem);
    }
}