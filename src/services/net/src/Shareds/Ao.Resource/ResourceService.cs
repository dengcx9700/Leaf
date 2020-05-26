using Ao.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResourceService : PackagingService<ResourceCreatorPackage,IResourceCreatorMetadata,ResourceCreatorPackage>,IResourceService
    {
        private ResourceNode root;
        private readonly object rootLocker = new object();
        private readonly object locker = new object();
        private readonly ConcurrentBag<string> excludeFilePatterns;
        private readonly ConcurrentBag<string> includeFilePatterns;
        private readonly ConcurrentBag<Regex> excludeFileRegexs;
        private readonly ConcurrentBag<Regex> includeFileRegexs;
        private readonly SortedList<int, IList<IResourceLoader>> resourceLoaders;
        //private readonly IWorkingService workingService;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<string> ExcludeFilePatterns =>
#if NET452
            excludeFilePatterns.ToArray()
#else
            excludeFilePatterns
#endif
            ;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>

        public IReadOnlyCollection<string> IncludeFilePatterns =>
#if NET452
            includeFilePatterns.ToArray()
#else
            includeFilePatterns
#endif
            ;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyDictionary<int, IList<IResourceLoader>> ResourceLoaders =>
#if NET452
            new ReadOnlyDictionary<int,IList<IResourceLoader>>(resourceLoaders)
#else
            resourceLoaders
#endif
            ;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<IResourceService> RootChanged;
        /// <summary>
        /// 根目录
        /// </summary>
        public string RootPath { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ResourceNode Root
        {
            get
            {
                if (root == null && !string.IsNullOrEmpty(RootPath)) 
                {
                    lock (rootLocker)
                    {
                        if (root==null)
                        {
                            root = new ResourceNode(new DirectoryInfo(RootPath), this);
                            RootChanged?.Invoke(this);
                            RaisePropertyChanged(nameof(Root));
                        }
                    }
                }
                return root;
            }
        }
        public ResourceService(string rootPath)
        {
            RootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
            excludeFilePatterns = new ConcurrentBag<string>();
            includeFilePatterns = new ConcurrentBag<string>();
            excludeFileRegexs = new ConcurrentBag<Regex>();
            includeFileRegexs = new ConcurrentBag<Regex>();
            resourceLoaders = new SortedList<int, IList<IResourceLoader>>(new OrderComparer());
            //workingService = DIHelper.EnsureGet<IWorkingService>();
            //workingService.CurrentProjectChanged += WorkingService_CurrentProjectChanged;
        }

        //private void WorkingService_CurrentProjectChanged(IWorkingService sender, Project old, Project @new)
        //{
        //    root?.Dispose();
        //    root = null;
        //    RootChanged?.Invoke(this);
        //    _ = Root;
        //    RaisePropertyChanged(nameof(Root));
        //}

        class OrderComparer : IComparer<int>
        {
            public int Compare( int x,  int y)
            {
                if (x<0&&y>=0)
                {
                    return 1;
                }
                return y - x;
            }

        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="pattern"><inheritdoc/></param>
        public void AddExcludeFilePattern(string pattern)
        {
            if (!excludeFilePatterns.Any(p=>p==pattern))
            {
                excludeFilePatterns.Add(pattern);
                excludeFileRegexs.Add(new Regex(pattern, RegexOptions.Compiled));
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="pattern"><inheritdoc/></param>
        public void AddIncludeFilePattern(string pattern)
        {
            if (!includeFilePatterns.Any(p => p == pattern))
            {
                includeFilePatterns.Add(pattern);
                includeFileRegexs.Add(new Regex(pattern, RegexOptions.Compiled));
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="resourceLoader"><inheritdoc/></param>
        public void AddResourceLoader(IResourceLoader resourceLoader)
        {
            lock (locker)
            {
                if (!resourceLoaders.TryGetValue(resourceLoader.Order,out var lst))
                {
                    lst = new List<IResourceLoader>();
                    resourceLoaders.Add(resourceLoader.Order, lst);
                }
                lst.Add(resourceLoader);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="filePath"><inheritdoc/></param>
        /// <returns></returns>
        public LoadResourceResult Load(string filePath)
        {
            //先看看需不需要跳过
            foreach (var item in excludeFileRegexs)
            {
                if (item.IsMatch(filePath))
                {
                    return LoadResourceResult.SkipResult;
                }
            }
            //再看看需不需要加载
            var needToLoad = false;
            foreach (var item in includeFileRegexs)
            {
                if (item.IsMatch(filePath))
                {
                    needToLoad = true;
                    break;
                }
            }
            if (!needToLoad)
            {
                return LoadResourceResult.SkipResult;
            }
            IResourceMetadata res = null;
            var context = new ResourceLoadContext(filePath);
            foreach (var item in resourceLoaders)
            {
                foreach (var loader in item.Value)
                {
                    res = loader.Load(context);
                    if (res != null)
                    {
                        break;
                    }
                }
                if (res!=null)
                {
                    break;
                }
            }
            return new LoadResourceResult(res, false);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <returns></returns>
        protected override ResourceCreatorPackage MakePackage(Assembly assembly)
        {
            return new ResourceCreatorPackage(assembly);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Condition(IResourceCreatorMetadata type)
        {
            return true;
        }
    }
}
