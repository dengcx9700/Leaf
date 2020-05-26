using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Ao.Plug.NetCore
{
    /// <summary>
    /// 插件加载上下文
    /// </summary>
    public class PlugLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;
        /// <summary>
        /// 组件路径
        /// </summary>
        public string ComponentAssemblyPath { get; }

        public PlugLoadContext(string componentAssemblyPath,string name, bool isCollectible = false) 
            : base(name, isCollectible)
        {
            if (string.IsNullOrWhiteSpace(componentAssemblyPath))
            {
                throw new ArgumentException("message", nameof(componentAssemblyPath));
            }

            if (!Directory.Exists(componentAssemblyPath))
            {
                throw new DirectoryNotFoundException(componentAssemblyPath);
            }

            ComponentAssemblyPath = componentAssemblyPath;
            resolver = new AssemblyDependencyResolver(componentAssemblyPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {

            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}
