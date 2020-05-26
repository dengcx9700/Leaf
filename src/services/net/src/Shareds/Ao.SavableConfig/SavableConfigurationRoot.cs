using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 可保存的配置根
    /// </summary>
    public class SavableConfigurationRoot : ConfigurationRoot, ISavableConfigurationRoot, IConfigurationRoot, IModifyableConfiguration
    {
        private readonly ConcurrentDictionary<string, string> changes;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyDictionary<string, string> Changes => new ReadOnlyDictionary<string,string>(changes);
        /// <summary>
        /// 初始化<see cref="SavableConfigurationRoot"/>
        /// </summary>
        /// <param name="providers">配置提供者</param>
        public SavableConfigurationRoot(IList<IConfigurationProvider> providers)
            : base(providers)
        {
            changes = new ConcurrentDictionary<string, string>();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns></returns>
        public new string this[string key]
        {
            get => base[key];
            set
            {
                AddChange(key, value);
                base[key] = value;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="path"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        public void AddChange(string path, string value)
        {
            changes.AddOrUpdate(path, value, (key, val) => value);
        }
    }
}
