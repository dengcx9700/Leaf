using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Ao.SavableConfig
{
    /// <summary>
    /// <para>
    /// 实现<see cref="ISettingDesignerContext"/>的类
    /// </para>
    /// <inheritdoc cref="ISettingDesignerContext"/>
    /// </summary>
    public class SettingDesignerContext : ISettingDesignerContext
    {
        /// <summary>
        /// 初始化<see cref="SettingDesignerContext"/>
        /// </summary>
        /// <param name="configuration"><inheritdoc cref="Configuration"/></param>
        /// <param name="source"><inheritdoc cref="Source"/></param>
        /// <param name="targetProperty"><inheritdoc cref="TargetProperty"/></param>
        public SettingDesignerContext(IConfigurationSection configuration, object source, PropertyInfo targetProperty)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            TargetProperty = targetProperty ?? throw new ArgumentNullException(nameof(targetProperty));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IConfigurationSection Configuration { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object Source { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public PropertyInfo TargetProperty { get; }
    }
}
