using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ao.Plug.NetCore
{
    /// <summary>
    /// 文件插件源提供者
    /// </summary>
    public class FilePlugSourceProvider : IPlugSourceProvider
    {
        public FilePlugSourceProvider(string componentAssemblyPath, string filePath)
        {
            if (string.IsNullOrWhiteSpace(componentAssemblyPath))
            {
                throw new ArgumentException("message", nameof(componentAssemblyPath));
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("message", nameof(filePath));
            }
            ComponentAssemblyPath = componentAssemblyPath;
            FilePath = filePath;
            var fileName = Path.GetFileName(filePath);
            loadContext = new PlugLoadContext(ComponentAssemblyPath, fileName, true);
            Assembly = loadContext.LoadFromAssemblyPath(filePath);
            TypeEntities = Assembly.ExportedTypes.Select(t => MakeTypeEntity(t)).ToArray();
        }
        private PlugLoadContext loadContext;
        private bool isDispose;
        /// <summary>
        /// 组件程序集的路径
        /// </summary>
        public string ComponentAssemblyPath { get; }
        /// <summary>
        /// 加载得到的程序集
        /// </summary>
        public Assembly Assembly { get; }
        /// <summary>
        /// 加载上下文
        /// </summary>
        public PlugLoadContext LoadContext => loadContext;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; }
        /// <summary>
        /// 类型实体
        /// </summary>
        public ITypeEntity[] TypeEntities { get; }
        /// <summary>
        /// 释放已经被释放
        /// </summary>
        public bool IsDispose => isDispose;
        /// <summary>
        /// 制作<see cref="ITypeEntity"/>类型
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <returns></returns>
        protected virtual ITypeEntity MakeTypeEntity(Type type)
        {
            return new CacheNewerTypeEntity(type);
        }
        public void Dispose()
        {
            if (IsDispose)
            {
                return;
            }
            if (loadContext!=null&&loadContext.IsCollectible)
            {
                loadContext?.Unload();
            }
            loadContext = null;
            isDispose = true;
        }
    }
}
