using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace Ao.Project
{
    public interface IProject: IItemGroupPart, IPropertyGroupItem
    {
        /// <summary>
        /// 表示表示特征集
        /// </summary>
        ConcurrentDictionary<string, object> Features { get; }
        /// <summary>
        /// 项组
        /// </summary>
        ObservableCollection<ItemGroup> ItemGroups { get; }
        /// <summary>
        /// 表示元数据集
        /// </summary>
        ConcurrentDictionary<string, object> Metadatas { get; }
        /// <summary>
        /// 属性组
        /// </summary>
        ObservableCollection<PropertyGroup> PropertyGroups { get; }
        /// <summary>
        /// 表示工程根目录
        /// </summary>
        string RootPath { get; }
    }
}