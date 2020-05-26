using Ao.Shared.ForView.Input;
using System;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 视图建造器
    /// </summary>
    /// <typeparam name="TView">目标视图</typeparam>
    public interface IViewBuilder<TView>
    {
        /// <summary>
        /// 排序键
        /// </summary>
        int Order { get; }
        /// <summary>
        /// 返回一个值，指示是否可用被建造
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <returns></returns>
        bool Condition(Type type);
        /// <summary>
        /// 建造视图
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="propertyItem">目标属性</param>
        /// <returns></returns>
        TView BuildView(ViewBuildContext<TView> context, AoAnalizedPropertyItemBase propertyItem);
    }
}
