using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ao.SavableConfig
{
    /// <summary>
    /// <inheritdoc cref="ISettingMapSource"/>
    /// </summary>
    public class SettingMapSource: ISettingMapSource,IEnumerable<KeyValuePair<object, SettingMapNode[]>>
    {
        /// <summary>
        /// 初始化<see cref="SettingMapSource"/>
        /// </summary>
        public SettingMapSource()
        {
            nodes = new Dictionary<object, SettingMapNode[]>();
        }
        private readonly Dictionary<object, SettingMapNode[]> nodes;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyDictionary<object, SettingMapNode[]> Nodes => nodes;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SettingMapNode[] AllNodes => Nodes.Values.SelectMany(v => v).ToArray();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<ISettingMapSource,object> MapSourceAdded;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        public void Add(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            var props = obj.GetType().GetProperties(SettingMapNode.IncludePropertyFlag);
            var nodes = SettingMapNode.PropertyInfosAsNode(obj.GetType().FullName, obj, props);
            this.nodes.Add(obj, nodes);
            MapSourceAdded?.Invoke(this,obj);
        }
        /// <summary>
        /// 生成所有树节点
        /// </summary>
        /// <returns></returns>
        public void GenNodes()
        {
            var nodes = AllNodes;
            while (nodes.Length != 0)
            {
                foreach (var item in nodes)
                {
                    _ = item.Nexts;
                }
                nodes = nodes.SelectMany(n => n.Nexts).ToArray();
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<object, SettingMapNode[]>> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// 从对象集合生成对象图
        /// </summary>
        /// <param name="objs">对象集合</param>
        /// <returns></returns>
        public static SettingMapSource FromObjects(params object[] objs)
        {
            if (objs is null)
            {
                throw new ArgumentNullException(nameof(objs));
            }

            var map = new SettingMapSource();
            foreach (var item in objs)
            {
                map.Add(item);
            }
            return map;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public virtual ISettingMap Build()
        {
            return new SettingMap(this);
        }
    }
}
