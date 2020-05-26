using System.Threading.Tasks;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置保存器
    /// </summary>
    public interface ISettingChangeSaver
    {
        /// <summary>
        /// 加载更改配置
        /// </summary>
        /// <param name="config">更改配置的配置</param>
        /// <returns></returns>
        ValueTask<SettingChangeLoadResult> LoadAsync(SettingChangeLoadConfig config);
        /// <summary>
        /// 保存更改配置
        /// </summary>
        /// <param name="root">须保存的配置根</param>
        /// <returns></returns>
        ValueTask<SettingChangeSaveResult> SaveAsync(ISavableConfigurationRoot root);
    }
}
