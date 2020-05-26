using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Project
{
    /// <summary>
    /// 表示一个工程
    /// </summary>
    public class Project : ProjectPart, IItemGroupPart, IPropertyGroupItem, IProject
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string RootPath { get; internal set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ConcurrentDictionary<string, object> Features { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ConcurrentDictionary<string, object> Metadatas { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<PropertyGroup> PropertyGroups { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<ItemGroup> ItemGroups { get; }
        public Project()
        {
            Features = new ConcurrentDictionary<string, object>();
            Metadatas = new ConcurrentDictionary<string, object>();
            PropertyGroups = new ObservableCollection<PropertyGroup>();
            ItemGroups = new ObservableCollection<ItemGroup>();
            PropertyGroups.CollectionChanged += ProjectParts_CollectionChanged;
            ItemGroups.CollectionChanged += ProjectParts_CollectionChanged;
        }
        public Project(string path)
            :this()
        {
            RootPath = path;
        }
        /// <summary>
        /// 重设根路径
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public bool ResetRootPath(string newPath)
        {
            var chars = Path.GetInvalidPathChars();
            if (newPath.All(p=> !chars.Contains(p)))
            {
                RootPath = newPath;
                RaisePropertyChanged(nameof(RootPath));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 重设特征集和元数据集
        /// </summary>
        protected override void OnReset(bool status)
        {
            Features.Clear();
            Metadatas.Clear();
        }
        /// <summary>
        /// 执行所有<see cref="PropertyGroups"/>的<see cref="IPropertyGroupItem.Decorate"/>方法
        /// </summary>
        public void Decorate()
        {
            foreach (var group in PropertyGroups)
            {
                foreach (var item in group.Items)
                {
                    item.Decorate();
                }
            }
        }
        /// <summary>
        /// 执行所有<see cref="ItemGroups"/>的<see cref="IItemGroupPart.ConductAsync"/>方法
        /// </summary>
        public async Task ConductAsync()
        {
            foreach (var group in ItemGroups)
            {
                foreach (var item in group.Items)
                {
                   await item.ConductAsync();
                }
            }
        }
        private void ProjectParts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var parts = e.NewItems.OfType<ProjectPart>();
                    foreach (var item in parts)
                    {
                        if (item.Project != null)
                        {
                            throw new InvalidOperationException("此项目部分已存在于另一个项目中了");
                        }
                        item.Project = this;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    parts = e.OldItems.OfType<ProjectPart>();
                    foreach (var item in parts)
                    {
                        item.Project = null;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
    }
}
