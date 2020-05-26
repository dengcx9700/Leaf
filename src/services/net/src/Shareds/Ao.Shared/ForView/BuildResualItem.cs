using System.Reflection;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示创建视图后的结果
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public class BuildResualItem<TView>
    {
        /// <summary>
        /// 初始化<see cref="BuildResualItem{TView}"/>
        /// </summary>
        /// <param name="view">目标视图</param>
        /// <param name="propertyItem">目标属性项</param>
        public BuildResualItem(TView view, AoAnalizedPropertyItemBase propertyItem)
        {
            View = view;
            PropertyItem = propertyItem;
        }
        /// <summary>
        /// 视图结果
        /// </summary>
        public TView View { get; }
        /// <summary>
        /// 属性项
        /// </summary>
        public AoAnalizedPropertyItemBase PropertyItem { get; }
    }
}
