using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置映射
    /// </summary>
    public class SettingMap : ISettingMap, IReadOnlyDictionary<string, SettingMapNode>
    {
        private readonly Dictionary<string, SettingMapNode> nodes;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SettingMapNode this[string key] 
        {
            get
            {
                if (nodes.TryGetValue(key,out var node))
                {
                    return node;
                }
                return null;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<string> Keys => nodes.Keys;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<SettingMapNode> Values => nodes.Values;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => nodes.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISettingMapSource Source { get; }
        /// <summary>
        /// 初始化<see cref="SettingMap"/>
        /// </summary>
        /// <param name="source">设置图的源</param>
        public SettingMap(ISettingMapSource source)
        {
            Source = source;
            nodes = new Dictionary<string, SettingMapNode>();
            InitSource();
        }

        private void InitSource()
        {
            var nodes = Source.AllNodes;
            while (nodes.Length!=0)
            {
                foreach (var node in nodes)
                {
                    this.nodes.Add(node.Path, node);
                }
                nodes = nodes.SelectMany(n => n.Nexts).ToArray();
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// <paramref name="key"><inheritdoc/></paramref>
        /// </summary>
        public bool ContainsKey(string key)
        {
            return nodes.ContainsKey(key);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerator<KeyValuePair<string, SettingMapNode>> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out SettingMapNode value)
        {
            return nodes.TryGetValue(key,out value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
