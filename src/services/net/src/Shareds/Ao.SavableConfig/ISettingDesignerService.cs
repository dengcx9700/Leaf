using Ao.Core;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Ao.SavableConfig
{
    public delegate void SettingMapRebuildHandle<TUI>(ISettingDesignerService<TUI> source);

    public interface ISettingDesignerService<TUI> : IPackagingService<DefaultPackage<ISettingDesignerMetadata<TUI>>, ISettingDesignerMetadata<TUI>, DefaultPackage<ISettingDesignerMetadata<TUI>>>
    {
        /// <summary>
        /// 设置映射源
        /// </summary>
        ISettingMapSource SettingMapSource { get; }
        /// <summary>
        /// 设置映射
        /// </summary>
        ISettingMap SettingMap { get; }
        /// <summary>
        /// 获取<see cref="IConfigurationSection"/>的设计器
        /// </summary>
        /// <param name="context">设置设计器上下文</param>
        /// <returns></returns>
        SettingDesigner<TUI> GetDesigner(ISettingDesignerContext context);
        /// <summary>
        /// <see cref="SettingMap"/>属性被重新生成了
        /// </summary>
        event SettingMapRebuildHandle<TUI> SettingMapRebuild;
    }
}
