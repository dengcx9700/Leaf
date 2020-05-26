using Microsoft.Extensions.Configuration;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示可以保存的配置节
    /// </summary>
    public interface ISavableConfigurationSection : IConfigurationSection
    {
        /// <summary>
        /// 值被改变了
        /// </summary>
        event ConfigurationSectionValueChangedHandle ValueChanged;
    }
}
