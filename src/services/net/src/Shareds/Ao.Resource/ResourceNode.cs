using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;
using Ao.Core;
using System.Threading;

namespace Ao.Resource
{
    /// <summary>
    /// 文件更改通知处理
    /// </summary>
    /// <param name="node">发起方</param>
    /// <param name="context">上下文</param>
    public delegate void SystemInfoAlertHandle(ResourceNode node, FileAlertContext context);
    /// <summary>
    /// 元数据集合更改了
    /// </summary>
    /// <param name="node"></param>
    /// <param name="context"></param>
    public delegate void MedataCollectionChangedHandle(ResourceNode node, MetadataCollectionChangeContext context);
    /// <summary>
    /// 表示资源结点
    /// </summary>
    public class ResourceNode : NotifyableObject, IDisposable,INodeble
    {
        private ResourceNode parent;
        private bool isDisponsed;
        private DirectoryInfo directory;
        private FileSystemWatcher fileWatcher;
        private ObservableCollection<IResourceMetadata> resourceMedatas;
        private ObservableCollection<ResourceNode> nexts;
        private ObservableCollection<INodeble> allNexts;
        public ResourceNode(DirectoryInfo directory, IResourceService resourceService)
        {
            ResourceManager = resourceService;
            this.directory = directory;
            Name = directory.Name;
            BeginWatcher();
        }
        private void BeginWatcher()
        {
            fileWatcher = new FileSystemWatcher(directory.FullName);
            FileWatcher.Changed += FileWatcher_Changed;
            FileWatcher.Deleted += FileWatcher_Deleted;
            FileWatcher.Renamed += FileWatcher_Renamed;
            FileWatcher.Disposed += FileWatcher_Disposed;
            FileWatcher.Created += FileWatcher_Created;
            FileWatcher.EnableRaisingEvents = true;
        }
        private void StopWatcher()
        {
            fileWatcher.EnableRaisingEvents = false;
            fileWatcher.Dispose();
            fileWatcher = null;
        }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 此结点是否已被释放
        /// </summary>
        public bool IsDisponsed { get; }
        /// <summary>
        /// 父亲
        /// </summary>
        public ResourceNode Parent
        {
            get
            {
                ThrowIfDisponsed();
                return parent;
            }
        }
        /// <summary>
        /// 文件检测
        /// </summary>
        public FileSystemWatcher FileWatcher 
        {
            get
            {
                ThrowIfDisponsed();
                return fileWatcher;
            }
        }
        /// <summary>
        /// 当前目录
        /// </summary>
        public DirectoryInfo Directory
        {
            get
            {
                ThrowIfDisponsed();
                return directory;
            }
        }
        /// <summary>
        /// 下一些结点
        /// </summary>
        public IReadOnlyCollection<ResourceNode> Nexts
        {
            get
            {
                ThrowIfDisponsed();
                if (nexts==null)
                {
                    InitNexts();
                }
                return nexts;
            }
        }
        /// <summary>
        /// 全部的下一个节点
        /// </summary>
        public IReadOnlyCollection<INodeble> AllNexts
        {
            get
            {
                ThrowIfDisponsed();
                if (allNexts==null)
                {
                    var coll = new ObservableCollection<INodeble>();
                    foreach (var item in Nexts)
                    {
                        coll.Add(item);
                    }
                    foreach (var item in ResourceMedatas)
                    {
                        coll.Add(new FileNode(item, this));
                    }
                    allNexts = coll;
                }
                return allNexts;
            }
        }
        /// <summary>
        /// 依赖服务资源管理
        /// </summary>
        protected IResourceService ResourceManager { get; }
        /// <summary>
        /// 资源元数据
        /// </summary>
        public IReadOnlyCollection<IResourceMetadata> ResourceMedatas
        {
            get
            {
                ThrowIfDisponsed();
                if (resourceMedatas==null)
                {
                    var files = Directory.GetFiles();
                    resourceMedatas = new ObservableCollection<IResourceMetadata>();
                    foreach (var item in files)
                    {
                        var result= ResourceManager.Load(item.FullName);
                        if (result.Succeed)
                        {
                            AddToResource(result.Metadata);
                        }
                    }
                }
                return resourceMedatas;
            }
        }
        private void AddToResource(IResourceMetadata metadata)
        {
            _ = AllNexts;
            _ = ResourceMedatas;
            allNexts.Add(new FileNode(metadata, this));
            resourceMedatas.Add(metadata);
        }
        private void RemoveFromResource(IResourceMetadata metadata)
        {
            _ = AllNexts;
            _ = ResourceMedatas;
            var node = AllNexts.Where(n => n is FileNode).OfType<FileNode>().First(n => n.ResourceMetadata == metadata);
            allNexts.Remove(node);
            resourceMedatas.Remove(metadata);
        }
        /// <summary>
        /// 表示文件或者目录改变了
        /// </summary>
        public event SystemInfoAlertHandle SystemInfoAlerted;
        /// <summary>
        /// 元数据项更改了,会被触发在
        /// <para>
        /// <see cref="Create(string, byte[])"/>
        /// <para>
        /// <see cref="Delete(string)"/>
        /// </para>
        /// <para>
        /// <see cref="Rename(string, string)"/>
        /// </para>
        /// </para>
        /// </summary>
        public event MedataCollectionChangedHandle MedataCollectionChanged;
        /// <summary>
        /// 是否引用的所有资源，包括子结点
        /// </summary>
        public void Dispose()
        {
            ThrowIfDisponsed();
            OnDisponse(true);
        }
        /// <summary>
        /// 重新扫描
        /// </summary>
        public void Reset()
        {
            resourceMedatas = null;
            _ = ResourceMedatas;
        }
        private void OnDisponse(bool disponseWatcher)
        {
            isDisponsed = true;
            if (parent != null)//确认不是根节点
            {
                //先断链
                parent.nexts.Remove(this);
                if (disponseWatcher)
                {
                    fileWatcher.EnableRaisingEvents = false;
                    fileWatcher.Dispose();
                }
                //释放资源
                if (resourceMedatas != null)
                {
                    foreach (var item in resourceMedatas)
                    {
                        item.Dispose();
                    }
                }
                //释放子结点
                foreach (var item in Nexts)
                {
                    item.Dispose();
                }
            }
            fileWatcher = null;
        }

        private void FileWatcher_Disposed(object sender, EventArgs e)
        {
            OnDisponse(false);
        }

        private void FileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                //文件变了
                if (resourceMedatas != null)
                {
                    var metadata = resourceMedatas.FirstOrDefault(m => m.Name == e.OldName);
                    var isLoad = metadata.IsLoaded;
                    if (metadata!=null)
                    {
                        RemoveFromResource(metadata);
                    }
                    var result = ResourceManager.Load(e.FullPath);
                    if (result.Succeed)
                    {
                        var newMetadata = result.Metadata;
                        newMetadata.SetLoadStatus(isLoad);
                        metadata.CopyTo(result.Metadata);
                        if (newMetadata == null)
                        {
                            ThrowNoSupportFile(e.FullPath);
                        }
                        AddToResource(newMetadata);
                        MedataCollectionChanged?.Invoke(this, new MetadataCollectionChangeContext(true, WatcherChangeTypes.Created, e, metadata, metadata));
                    }
                }
            }
            else
            {
                var node=Nexts.First(n => n.Directory.Name == e.OldName);
                var allNode = AllNexts.First(n => n.Name == e.OldName);
                if (node==null)
                {
                    Debug.Assert(allNode != null);
                    var n = new ResourceNode(new DirectoryInfo(e.FullPath),ResourceManager) { parent = this };
                    nexts.Add(n);
                    allNexts.Add(n);
                }
                else
                {
                    var n = nexts.First(nx => nx.directory.Name == e.OldName);
                    n.StopWatcher();
                    n.directory = new DirectoryInfo(e.FullPath);
                    n.BeginWatcher();
                    Debug.Assert(n.Directory.Exists);
                    foreach (var item in n.Nexts)
                    {
                        item.UpdateDirectoryInfo(e.FullPath, item);
                    }
                    node.Name = node.directory.Name;
                    allNode.Name = e.Name;
                }
            }
            RaiseSystemInfoAlert(e);

        }
        private void UpdateDirectoryInfo(string @base,ResourceNode node)
        {
            var name = node.directory.Name;
            node.directory = new DirectoryInfo(Path.Combine(@base, name));
            foreach (var item in Nexts)
            {
                item.UpdateDirectoryInfo(node.directory.FullName, item);
            }
        }
        private void FileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (!Nexts.Any(f => f.Name == e.Name))
            {
                if (resourceMedatas != null)
                {
                    var metadata = resourceMedatas.FirstOrDefault(m => m.Name == e.Name);
                    if (metadata!=null)//看看有没有这个文件
                    {
                        MedataCollectionChanged?.Invoke(this, new MetadataCollectionChangeContext(true, WatcherChangeTypes.Deleted, e, metadata, null));
                        RemoveFromResource(metadata);
                    }
                    RaiseFileAlert(e);
                }
            }
            else
            {
                var node = Nexts.FirstOrDefault(n => n.Directory.Name == e.Name);
                if (node!=null)
                {
                    nexts.Remove(node);//先断链
                    allNexts.Remove(node);
                    node.Dispose();
                }
            }
            RaiseFileAlert(e);
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                if (resourceMedatas != null)
                {
                    var result = ResourceManager.Load(e.FullPath);
                    if (result.Succeed)
                    {
                        AddToResource(result.Metadata);
                    }
                }
            }
            else if(System.IO.Directory.Exists(e.FullPath))
            {
                var node = new ResourceNode(new DirectoryInfo(e.FullPath),ResourceManager);
                nexts.Add(node);
                //找到最后一个resNode的位置
                var pos = AllNexts.Count(n => n is ResourceNode);
                allNexts.Insert(pos, node);
            }
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                if (resourceMedatas != null)
                {
                    var oldMetadata = ResourceMedatas.FirstOrDefault(m => m.Name == e.Name);
                    if (oldMetadata!=null)
                    {
                        var node = allNexts.Where(n => n is FileNode).OfType<FileNode>().First(n => n.ResourceMetadata == oldMetadata);
                        var nodeIndex = allNexts.IndexOf(node);
                        var result = ResourceManager.Load(e.FullPath);
                        if (!result.Skip&&!result.Succeed)
                        {
                            ThrowNoSupportFile(e.FullPath);
                        }
                        var index = resourceMedatas.IndexOf(oldMetadata);
                        result.Metadata.SetLoadStatus(oldMetadata.IsLoaded);
                        oldMetadata.CopyTo(result.Metadata);
                        resourceMedatas[index] = result.Metadata;
                        allNexts[nodeIndex] = new FileNode(result.Metadata, this);
                        oldMetadata.Dispose();
                        MedataCollectionChanged?.Invoke(this, new MetadataCollectionChangeContext(true, WatcherChangeTypes.Changed, e, oldMetadata, result.Metadata));
                    }
                }
            }
            //目录里面改变的事情不管
            RaiseFileAlert(e);
        }
        private void ThrowNoSupportFile(string filePath)
        {
            throw new NotSupportedException(filePath);

        }
        /// <summary>
        /// 获取文件在此结点的全路径
        /// </summary>
        /// <param name="fileName">文件名字</param>
        /// <returns></returns>
        public string GetFullFileName(string fileName)
        {
#if !NETSTANDARD2_1
            return Path.Combine(Directory.FullName, fileName);
#else
            return Path.GetFullPath(fileName, Directory.FullName);
#endif
        }
        /// <summary>
        /// 从结点路径创建一个文件
        /// </summary>
        /// <param name="fileName">仅是文件名</param>
        /// <param name="data">文件数据</param>
        /// <returns>创建返回的结果</returns>
        public FileAccessResult Create(string fileName,byte[] data=null)
        {
            ThrowIfDisponsed();
            try
            {
                bool succeed = false;
                //先创建磁盘文件
                if (data == null)
                {
#if NET452
                    data = new byte[0];
#else
                    data = Array.Empty<byte>();
#endif
                }
                var path = GetFullFileName(fileName);
                File.WriteAllBytes(path, data);
                return new FileAccessResult(WatcherChangeTypes.Created, this, succeed, null);
            }
            catch (Exception ex)
            {
                return new FileAccessResult(WatcherChangeTypes.Created, this, false, ex);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public FileAccessResult Delete(string fileName)
        {
            ThrowIfDisponsed();
            try
            {
                var succeed = false;
                var path = GetFullFileName(fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    succeed = true;
                }
                return new FileAccessResult(WatcherChangeTypes.Deleted, this, succeed, null);
            }
            catch (Exception ex)
            {
                return new FileAccessResult(WatcherChangeTypes.Deleted, this, false, ex);
            }
        }
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="oldName">旧文件名</param>
        /// <param name="newName">新文件名</param>
        /// <returns></returns>
        public FileAccessResult Rename(string oldName,string newName)
        {
            ThrowIfDisponsed();
            try
            {
                var succeed = false;
                var oldPath = GetFullFileName(oldName);
                var newPath = GetFullFileName(newName);
                if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                    succeed = true;
                }
                if (resourceMedatas!=null)
                {
                    var metadata = resourceMedatas.FirstOrDefault(m => m.Name == newName);
                    if (metadata!=null)
                    {
                        metadata.ResetResourceNamePath();
                    }
                }
                return new FileAccessResult(WatcherChangeTypes.Renamed, this, succeed, null);
            }
            catch (Exception ex)
            {
                return new FileAccessResult(WatcherChangeTypes.Renamed, this, false, ex);
            }
        }
        private void OnAfterFolderAlter()
        {
            var currentparent = this;//至根节点的都需要重设
            while (currentparent != null)
            {
                if (currentparent.resourceMedatas != null)
                {
                    foreach (var item in currentparent.resourceMedatas)
                    {
                        item.ResetResourceNamePath();
                    }

                }
                currentparent = currentparent.parent;
            }
        }
        /// <summary>
        /// 从此结点创建目录
        /// </summary>
        /// <param name="folderName">目录名字</param>
        /// <returns></returns>
        public FolderAccessResult CreateFolder(string folderName)
        {
            ThrowIfDisponsed();
            try
            {
                var path = Path.Combine(Directory.FullName, folderName);
                System.IO.Directory.CreateDirectory(path);
                return new FolderAccessResult(WatcherChangeTypes.Created, this, true, null);
            }
            catch (Exception ex)
            {
                return new FolderAccessResult(WatcherChangeTypes.Created, this, false, ex);
            }
        }
        /// <summary>
        /// 从此结点删除目录
        /// </summary>
        /// <param name="folderName">目录名字</param>
        /// <returns></returns>
        public FolderAccessResult DeleteFolder(string folderName)
        {
            ThrowIfDisponsed();
            try
            {
                var path = Path.Combine(Directory.FullName, folderName);
                System.IO.Directory.Delete(path);
                return new FolderAccessResult(WatcherChangeTypes.Deleted, this, true, null);
            }
            catch (Exception ex)
            {
                return new FolderAccessResult(WatcherChangeTypes.Deleted, this, false, ex);
            }
        }
        /// <summary>
        /// 重命名目录
        /// </summary>
        /// <param name="oldFolderName">旧目录名</param>
        /// <param name="newFolderName">新目录名</param>
        /// <returns></returns>
        public FolderAccessResult RenameFolder(string oldFolderName,string newFolderName)
        {
            ThrowIfDisponsed();
            try
            {
                var oldPath = Path.Combine(Directory.FullName, oldFolderName);
                var newPath = Path.Combine(Directory.FullName, newFolderName);
                System.IO.Directory.Move(oldPath, newPath);
                OnAfterFolderAlter();
                return new FolderAccessResult(WatcherChangeTypes.Renamed, this, true, null);
            }
            catch (Exception ex)
            {
                return new FolderAccessResult(WatcherChangeTypes.Renamed, this, false, ex);
            }
        }
        private void RaiseFileAlert(FileSystemEventArgs e,bool isInnerChanged=false)
        {
            SystemInfoAlerted?.Invoke(this, new FileAlertContext(isInnerChanged, e));
        }
        private void RaiseSystemInfoAlert(RenamedEventArgs e, bool isInnerChanged = false)
        {
            SystemInfoAlerted?.Invoke(this, new FileAlertContext(isInnerChanged, e));
        }
        /// <summary>
        /// 如果对象被释放了，抛出异常
        /// </summary>
        protected void ThrowIfDisponsed()
        {
            if (IsDisponsed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        private void InitNexts()
        {
            var dirs = Directory.EnumerateDirectories();
            nexts = new ObservableCollection<ResourceNode>(dirs.Select(dir => new ResourceNode(dir,ResourceManager) { parent = this }));
        }
        /// <summary>
        /// 寻找符合条件的元数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="max">最大查找多少个，-1为全部</param>
        /// <param name="forceLoad"></param>
        /// <returns></returns>
        public IResourceMetadata[] Find(Predicate<IResourceMetadata> predicate,int max=-1, bool forceLoad = true)
        {
            var res = new List<IResourceMetadata>();
            FindPart(res, this, predicate, max,forceLoad);
            return res.ToArray();
        }
        /// <summary>
        /// 寻找所有
        /// </summary>
        /// <returns></returns>
        public IResourceMetadata[] FindAll()
        {
            return Find(x => true);
        }
        /// <summary>
        /// 寻找所有已加载的资源
        /// </summary>
        /// <returns></returns>
        public IResourceMetadata[] FindAllLoaded()
        {
            return Find(x =>x.IsLoaded, -1, false);
        }
        /// <summary>
        /// 寻找此节点中符合的节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="node"></param>
        /// <param name="predicate">条件</param>
        /// <param name="max">搜索数量，-1为不限</param>
        /// <param name="forceLoad">是否全局搜索，不管资源有没有加载</param>
        protected internal void FindPart(List<IResourceMetadata> nodes,ResourceNode node, 
            Predicate<IResourceMetadata> predicate,int max=-1,bool forceLoad=true)
        {
            if (forceLoad||node.resourceMedatas!=null)
            {
                foreach (var item in node.ResourceMedatas)
                {
                    if (predicate(item))
                    {
                        nodes.Add(item);
                    }
                    if (max > 0 && nodes.Count > max)
                    {
                        break;
                    }
                }
            }
            foreach (var item in node.Nexts)
            {
                if (max > 0 && nodes.Count > max)
                {
                    break;
                }
                FindPart(nodes, item, predicate,max);
            }
        }
    }
}
