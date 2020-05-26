using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Shared.ForView.Input
{
    /// <summary>
    /// 表示可以初始化的
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public interface IInitable<TView>
    {
        /// <summary>
        /// 附加的属性项
        /// </summary>
        AoAnalizedPropertyItemBase PropertyItem { get; }
        /// <summary>
        /// 附加的上下文
        /// </summary>
        ViewBuildContext<TView> Context { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertyItem"></param>
        void Init(ViewBuildContext<TView> context,AoAnalizedPropertyItemBase propertyItem);
    }

}
