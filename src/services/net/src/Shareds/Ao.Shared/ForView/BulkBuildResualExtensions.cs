using System.Collections.Generic;
using System.Linq;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 对<see cref="BulkBuildResual{TView}"/>的扩展
    /// </summary>
    public static class BulkBuildResualExtensions
    {
        /// <summary>
        /// 分组创建视图后的结果
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="resual"></param>
        /// <param name="defaultName">默认组的名字</param>
        /// <returns></returns>
        public static Dictionary<string, List<BuildResualItem<TView>>> GroupByName<TView>(this BulkBuildResual<TView> resual,string defaultName)
        {
            var dic = new Dictionary<string, List<BuildResualItem<TView>>>();
            var r = resual.GetAll();
            foreach (var item in r)
            {
                var key = item.GetGroupName() ?? defaultName;
                if (!dic.TryGetValue(key, out var items))
                {
                    items = new List<BuildResualItem<TView>>();
                    dic.Add(key, items);
                }
                items.Add(item);
            }
            return dic;
        }
        public static IEnumerable<TView> GetAllView<TView>(this BulkBuildResual<TView> resual)
        {
            return resual.GetAll().Select(v => v.View);
        }
    }
}
