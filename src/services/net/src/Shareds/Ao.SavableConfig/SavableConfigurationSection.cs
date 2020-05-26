using Microsoft.Extensions.Configuration;

namespace Ao.SavableConfig
{
    public delegate void ConfigurationSectionValueChangedHandle(ISavableConfigurationSection source, string old, string @new);

    /// <summary>
    /// 可保持的配置节
    /// </summary>
    public class SavableConfigurationSection : ConfigurationSection, ISavableConfigurationSection, IConfigurationSection
    {
        private readonly IModifyableConfiguration modifyableConfiguration;
        /// <summary>
        /// 初始化<see cref="SavableConfigurationSection"/>
        /// </summary>
        /// <param name="modifyableConfiguration">可保存的配置</param>
        /// <param name="root">配置根</param>
        /// <param name="path">路径</param>
        public SavableConfigurationSection(IModifyableConfiguration modifyableConfiguration, IConfigurationRoot root, string path)
            : base(root, path)
        {
            this.modifyableConfiguration = modifyableConfiguration;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public new string Value
        {
            get => base.Value;
            set
            {
                if (value!=base.Value)
                {
                    var old = base.Value;
                    modifyableConfiguration.AddChange(Path, value);
                    base.Value = value;
                    ValueChanged?.Invoke(this, old, value);
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event ConfigurationSectionValueChangedHandle ValueChanged;
    }
}
