using System.Collections.Generic;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置图
    /// </summary>
    public interface ISettingMap : IReadOnlyDictionary<string, SettingMapNode>
    {
        /// <summary>
        /// 设置源
        /// </summary>
        ISettingMapSource Source { get; }
    }
}
