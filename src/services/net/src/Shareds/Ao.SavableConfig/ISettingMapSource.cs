using System;
using System.Collections.Generic;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示设置映射源
    /// </summary>
    public interface ISettingMapSource : IEnumerable<KeyValuePair<object, SettingMapNode[]>>
    {
        /// <summary>
        /// 当前实例的浅节点
        /// </summary>
        IReadOnlyDictionary<object, SettingMapNode[]> Nodes { get; }
        /// <summary>
        /// 所有的节点
        /// </summary>
        SettingMapNode[] AllNodes { get; }
        /// <summary>
        /// 生成设置图
        /// </summary>
        /// <returns></returns>
        ISettingMap Build();
        /// <summary>
        /// 添加一个设置对象
        /// </summary>
        /// <param name="obj">设置对象实例</param>
        void Add(object obj);
        /// <summary>
        /// 设置地图源添加了
        /// </summary>
        event Action<ISettingMapSource,object> MapSourceAdded;
    }
}
