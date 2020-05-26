using Ao.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Pd.Services.Menu
{
    public delegate void NodeActionHandle(IMenuNode sender, IMenuNode target);

    /// <summary>
    /// 菜单节点
    /// </summary>
    public class MenuNode : IMenuNode
    {
        /// <summary>
        /// 路径分割器
        /// </summary>
        public static readonly string PathSpliter = "/";

        public MenuNode(IMenuMetadata metadata)
            : this()
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }
        private MenuNode()
        {
            Nexts = new ObservableCollection<IMenuNode>();
            Nexts.CollectionChanged += Nexts_CollectionChanged;
        }
        public static IMenuNode MakeRoot()
        {
            return new MenuNode();
        }
        private void Nexts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var ns = e.NewItems.OfType<MenuNode>();
                    foreach (var item in ns)
                    {
                        if (item.Parent != null)
                        {
                            throw new InvalidOperationException("此节点已在另一个节点上了");
                        }
                        item.Parent = this;
                        NodeAdded?.Invoke(this, item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ns = e.OldItems.OfType<MenuNode>();
                    foreach (var item in ns)
                    {
                        item.Parent = null;
                        NodeRemoved?.Invoke(this, item);
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

        private IMenuNode parent;
        private string[] pathPart;
        private string path;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IMenuNode Parent
        {
            get
            {
                return parent;
            }
            private set
            {
                var old = parent;
                parent = value;
                OnParentChanged(old, value);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IMenuMetadata Metadata { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<IMenuNode> Nexts { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string[] PathPart
        {
            get
            {
                if (pathPart == null)
                {
                    var part = new List<string>();
                    var now = (IMenuNode)this;
                    while (now != null)
                    {
                        if (now.Metadata != null)
                        {
                            part.Add(now.Metadata.Id);
                        }
                        now = now.Parent;
                    }
                    part.Reverse();
                    pathPart = part.ToArray();
                }
                return pathPart;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Path
        {
            get
            {
                if (path == null)
                {
                    path = string.Join(PathSpliter, PathPart.ToArray());
                }
                return path;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsRoot => Metadata == null;
        IReadOnlyCollection<IMenuNode> IReadonlyMenuNode.Nexts => Nexts;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event NodeActionHandle NodeAdded;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event NodeActionHandle NodeRemoved;
        /// <summary>
        /// 表示<see cref="Parent"/>属性更改了
        /// </summary>
        /// <param name="old">旧的值</param>
        /// <param name="new">新的值</param>
        protected virtual void OnParentChanged(IMenuNode old, IMenuNode @new)
        {
            pathPart = null;
            path = null;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="path"><inheritdoc/></param>
        /// <returns></returns>
        public IMenuNode[] Find(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            var parts = path.Split(PathSpliter[0]);
            if (Metadata != null && parts[0] != Metadata.Id)//当前节点都不符合
            {
                return null;
            }
            var okPart = 0;
            var currentNodes = Nexts.ToArray();//将所有的节点装进去筛选
            while (true)
            {
                var oksNodes = new List<IMenuNode>();
                foreach (var item in currentNodes)
                {
                    if (item.Metadata.Id == parts[okPart])//看一下路径path是否可以
                    {
                        oksNodes.Add(item);
                    }
                }
                okPart++;
                if (oksNodes.Count == 0|| okPart >= parts.Length)//已经没东西可找了
                {
                    currentNodes = oksNodes.ToArray();
                    break;
                }
                currentNodes = oksNodes.SelectMany(n => n.Nexts).ToArray();//可用的节点的所有Nexts

            }
            return currentNodes;
        }
    }
}
