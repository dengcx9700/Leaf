using System;

namespace Ao.Project
{
    /// <summary>
    /// 工程部分提供者基类
    /// </summary>
    public abstract class ProjectPartProviderBase : IProjectPartProvider
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Descript { get; protected set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Default { get; protected set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Type Type { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="default"><inheritdoc/></param>
        /// <returns></returns>
        public abstract object Create(bool @default);
    }
}
