using System;
using System.IO;
using System.Reflection;

namespace Ao.Plug
{
    /// <summary>
    /// 插件源提供者
    /// </summary>
    public interface IPlugSourceProvider : IDisposable
    {
        /// <summary>
        /// 目标程序集
        /// </summary>
        ITypeEntity[] TypeEntities { get; }
    }
}
