namespace Ao.Shared.ForView
{
    /// <summary>
    /// 对<see cref="BuildResualItem{TView}"/>的扩展
    /// </summary>
    public static class BuildResualItemExtensions
    {
        /// <summary>
        /// 查找属性的<see cref="GroupAttribute"/>并且获得组名，如果无返回null
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetGroupName<TView>(this BuildResualItem<TView> item)
        {
            var attr = item.PropertyItem.GetCustomAttribute<GroupAttribute>();
            return attr?.GetValue(item.PropertyItem.Source);
        }
    }
}
