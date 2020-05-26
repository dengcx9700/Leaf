using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Ao.Project
{
    /// <summary>
    /// 表示工程部分组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ProjectPartGroup<T>: ProjectPart
        where T:ProjectPart
    {

        public ObservableCollection<T> Items { get; }

        public ProjectPartGroup()
        {
            Items = new ObservableCollection<T>();
            Items.CollectionChanged += Items_CollectionChanged;
        }
        protected override void OnProjectChanged(Project project)
        {
            foreach (var item in Items)
            {
                item.Project = Project;
            }
        }
        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Done)
            {
                throw new InvalidOperationException("结束操作后,集合不能改变");
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var items = e.NewItems.OfType<T>();
                    foreach (var item in items)
                    {
                        if (item.Project != null)
                        {
                            throw new ArgumentException("此部分已存在于另一个组");
                        }
                        item.Project = Project;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    items = e.OldItems.OfType<T>();
                    foreach (var item in items)
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
