using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 对类型<see cref="ISettingMap"/>的扩展
    /// </summary>
    public static class SettingMapExtensions
    {
        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="map"></param>
        /// <param name="memberSelector"></param>
        /// <returns></returns>
        public static SettingMapNode GetNode<TObject,TMember>(this ISettingMap map,Expression<Func<TObject, TMember>> memberSelector)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (memberSelector is null)
            {
                throw new ArgumentNullException(nameof(memberSelector));
            }

            return map.From<TObject>().Then(memberSelector).Node;
        }
        /// <summary>
        /// 从某一个对象开始
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IMapNodeLookup<T, T> From<T>(this ISettingMap map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }
            var typeFullName = typeof(T).FullName;
            var name = typeFullName;
            return new MapNodeLookup<T, T>(new List<string> { name},map);
        }
    }
}
