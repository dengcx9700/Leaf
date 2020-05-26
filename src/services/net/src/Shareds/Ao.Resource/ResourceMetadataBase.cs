using Ao.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 表示资源的元数据
    /// </summary>
    public abstract class ResourceMetadataBase : NotifyableObject, IDisposable, IResourceMetadata
    {
        /// <summary>
        /// 资源加载失败的事件
        /// </summary>
        public static event ResourceLoadFailHandle ResourceLoadFailed;

        private bool isDisponsed;
        private string name;
        private string descript;
        private string resourceNamePath;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IResourceMetadata Parent { get; internal set; }
        /// <summary>
        /// <inheritdoc/>
        /// <para>
        /// 如/ddd/www，/ddd
        /// </para>
        /// </summary>
        public string ResourceNamePath
        {
            get
            {
                if (resourceNamePath == null)
                {
                    var strs = new List<string> { Name };
                    var current = Parent;
                    while (current != null)
                    {
                        strs.Add(Parent.Name);
                        current = Parent.Parent;
                    }
#if NETSTANDARD2_1
                    resourceNamePath = string.Join('/', strs);
#else
                    resourceNamePath = string.Join("/", strs);
#endif
                    ResourceNamePathLoad?.Invoke();
                }
                return resourceNamePath;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<IResourceMetadata> Items { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action ResourceNamePathLoad;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool RemoveToDisponse { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (value.Contains('/'))
                {
                    throw new ArgumentException("名字不合法，不能存在/字符");
                }
                RaisePropertyChanged(ref name, value);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Descript
        {
            get => descript;
            set => RaisePropertyChanged(ref descript, value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsDisponsed
        {
            get => isDisponsed;
            private set
            {
                RaisePropertyChanged(ref isDisponsed, value);
                IsDisponsedChanged?.Invoke(value);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsLoaded { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<bool> IsDisponsedChanged;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<IResourceMetadata> ValueInited;
        internal ResourceMetadataBase()
        {
            Items = new ObservableCollection<IResourceMetadata>();
            Items.CollectionChanged += Items_CollectionChanged;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<Stream> CreateStreamAsync()
        {
            var stream = OnCreateStreamAsync();
            ValueInited?.Invoke(this);
            IsLoaded = true;
            return stream;
        }
        /// <summary>
        /// 创建资源流
        /// </summary>
        /// <returns></returns>
        protected abstract Task<Stream> OnCreateStreamAsync();

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDisponsed)
            {
                ThrowObjectDisposedException();
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var rms = e.NewItems.OfType<ResourceMetadataBase>();
                    foreach (var item in rms)
                    {
                        if (item.Parent != null)
                        {
                            throw new ArgumentException("资源已存在另一个集合中");
                        }
                        if (item.IsDisponsed)
                        {
                            ThrowObjectDisposedException();
                        }
                        item.Parent = this;
                        if (Items.Count==1)
                        {
                            OnFirstMedataAdd(item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    rms = e.OldItems.OfType<ResourceMetadataBase>();
                    foreach (var item in rms)
                    {
                        item.Parent = null;
                        if (RemoveToDisponse)
                        {
                            item.Dispose();
                        }
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
        /// <summary>
        /// 表示第一个元数据加入了
        /// </summary>
        protected virtual void OnFirstMedataAdd(ResourceMetadataBase resourceMedata)
        {

        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract Task SaveAsync();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IResourceMetadata> GetAll()
        {
            return GetPart(this, rm => true);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="condition"><<inheritdoc/>/param>
        /// <returns></returns>
        public IEnumerable<IResourceMetadata> FindByCondition(Func<IResourceMetadata, bool> condition)
        {
            return GetPart(this, condition);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="name"><inheritdoc/></param>
        /// <param name="stringComparison"><inheritdoc/></param>
        /// <returns></returns>
        public IEnumerable<IResourceMetadata> FindByName(string name, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return GetPart(this, (rm) => string.Compare(name, rm.Name, stringComparison) == 0);
        }
        private IEnumerable<IResourceMetadata> GetPart(IResourceMetadata current, Func<IResourceMetadata, bool> condition)
        {
            var list = new List<IResourceMetadata>();
            foreach (var item in current.Items)
            {
                if (condition(item))
                {
                    list.Add(item);
                    list.AddRange(GetPart(item, condition));
                }
            }
            return list;
        }
        public virtual void Dispose()
        {
            if (IsDisponsed)
            {
                ThrowObjectDisposedException();
            }
            IsDisponsed = true;
        }
        protected void ThrowObjectDisposedException()
        {
            throw new ObjectDisposedException($"{Name}已被释放，无法对此操作");
        }
        /// <summary>
        /// 重设资源路径
        /// </summary>
        public virtual void ResetResourceNamePath()
        {
            resourceNamePath = null;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string ToString()
        {
            return ResourceNamePath;
        }
        /// <summary>
        /// 发起资源加载失败的事件
        /// </summary>
        /// <param name="info">失败信息</param>
        protected void RaiseResourceLoadFailed(ResourceLoadFailInfo info)
        {
            ResourceLoadFailed?.Invoke(this, info);
        }
        /// <summary>
        /// <inheritdoc cref="RaiseResourceLoadFailed(ResourceLoadFailInfo)"/>
        /// </summary>
        /// <param name="ex">异常</param>
        protected void RaiseResourceLoadFailed(Exception ex)
        {
            ResourceLoadFailed?.Invoke(this, new ResourceLoadFailInfo(ex));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isLoad"></param>
        public void SetLoadStatus(bool isLoad)
        {
            IsLoaded = isLoad;
        }
        /// <summary>
        /// 复制资源
        /// </summary>
        /// <param name="target"></param>
        public virtual void CopyTo(IResourceMetadata target)
        {
        }
    }
}
