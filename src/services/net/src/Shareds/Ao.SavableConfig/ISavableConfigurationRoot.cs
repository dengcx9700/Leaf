using Microsoft.Extensions.Configuration;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示可以保存的配置根
    /// </summary>
    public interface ISavableConfigurationRoot: IConfigurationRoot, IModifyableConfiguration
    {

    }
}
