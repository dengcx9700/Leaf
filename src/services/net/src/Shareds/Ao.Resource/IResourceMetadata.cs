using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 表示资源元数据
    /// </summary>
    public interface IResourceMetadata: IDisposable
    {
        /// <summary>
        /// 资源描述
        /// </summary>
        string Descript { get; set; }
        /// <summary>
        /// 资源是否已经释放了
        /// </summary>
        bool IsDisponsed { get; }
        /// <summary>
        /// 表示子资源项
        /// </summary>
        ObservableCollection<IResourceMetadata> Items { get; }
        /// <summary>
        /// 资源名字
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 父亲
        /// </summary>
        IResourceMetadata Parent { get; }
        /// <summary>
        /// 是否移除就释放
        /// </summary>
        bool RemoveToDisponse { get; set; }
        /// <summary>
        /// 资源名字路径
        /// </summary>
        string ResourceNamePath { get; }
        /// <summary>
        /// 资源是否被加载了
        /// </summary>
        bool IsLoaded { get; }
        /// <summary>
        /// 表示资源是否的状态改变了
        /// </summary>
        event Action<bool> IsDisponsedChanged;
        /// <summary>
        /// <see cref="Stream"/>被初始化了
        /// </summary>
        event Action<IResourceMetadata> ValueInited;
        /// <summary>
        /// 表示<see cref="ResourceNamePath"/>被加载了
        /// </summary>
        event Action ResourceNamePathLoad;
        /// <summary>
        /// 从条件获取资源
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        IEnumerable<IResourceMetadata> FindByCondition(Func<IResourceMetadata, bool> condition);
        /// <summary>
        /// 从名字获取资源
        /// </summary>
        /// <param name="name">资源名字</param>
        /// <param name="stringComparison">比较选项</param>
        /// <returns></returns>
        IEnumerable<IResourceMetadata> FindByName(string name, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// 获取此节点的所有资源
        /// </summary>
        /// <returns></returns>
        IEnumerable<IResourceMetadata> GetAll();
        /// <summary>
        /// 创建资源流
        /// </summary>
        /// <returns></returns>
        Task<Stream> CreateStreamAsync();
        /// <summary>
        /// 保存/覆盖资源
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
        /// <summary>
        /// 重设资源名字路径
        /// </summary>
        void ResetResourceNamePath();
        /// <summary>
        /// 设置加载状态
        /// </summary>
        /// <param name="isLoad"></param>
        void SetLoadStatus(bool isLoad);
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="target"></param>
        void CopyTo(IResourceMetadata target);
    }
    /// <summary>
    /// 资源加载失败的处理器
    /// </summary>
    /// <param name="resource">发起资源</param>
    /// <param name="info">失败信息</param>
    public delegate void ResourceLoadFailHandle(IResourceMetadata resource, ResourceLoadFailInfo info);
}