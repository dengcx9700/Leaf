using Microsoft.Extensions.Configuration;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示一个可保持的配置器
    /// </summary>
    public interface ISavableConfigurationBuilder : IConfigurationBuilder
    {
        /// <summary>
        /// 建造一个可保持的配置器
        /// </summary>
        /// <returns></returns>
        new ISavableConfigurationRoot Build();
    }
}
