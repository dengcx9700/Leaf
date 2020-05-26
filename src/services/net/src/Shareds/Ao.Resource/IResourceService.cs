using Ao.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Ao.Resource
{
    /// <summary>
    /// 资源服务
    /// <para>
    /// 加载，引入等等都可以从此服务进入
    /// </para>
    /// </summary>
    public interface IResourceService : IPackagingService<ResourceCreatorPackage, IResourceCreatorMetadata, ResourceCreatorPackage>,INotifyPropertyChanged
    {
        /// <summary>
        /// 表示资源根结点
        /// </summary>
        ResourceNode Root { get; }
        /// <summary>
        /// 不包含的文件,相对路径正则匹配
        /// </summary>
        IReadOnlyCollection<string> ExcludeFilePatterns { get; }
        /// <summary>
        /// 包含的文件,相对路径正则匹配
        /// </summary>
        IReadOnlyCollection<string> IncludeFilePatterns { get; }
        /// <summary>
        /// 表示一个资源加载器
        /// </summary>
        IReadOnlyDictionary<int, IList<IResourceLoader>> ResourceLoaders { get; }
        /// <summary>
        /// 属性<see cref="Root"/>改变了
        /// </summary>
        event Action<IResourceService> RootChanged;
        /// <summary>
        /// 添加一个不包含文件匹配模式
        /// </summary>
        /// <param name="pattern"></param>
        void AddExcludeFilePattern(string pattern);
        /// <summary>
        /// 添加一个包含文件匹配模式
        /// </summary>
        /// <param name="pattern"></param>
        void AddIncludeFilePattern(string pattern);
        /// <summary>
        /// 添加一个资源加载器
        /// </summary>
        /// <param name="resourceLoader">资源加载器</param>
        void AddResourceLoader(IResourceLoader resourceLoader);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <returns></returns>
        LoadResourceResult Load(string filePath);
    }
}
