using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示设置设计器的上下文
    /// </summary>
    public interface ISettingDesignerContext
    {
        /// <summary>
        /// 配置节
        /// </summary>
        IConfigurationSection Configuration { get; }
        /// <summary>
        /// 源实例
        /// </summary>
        object Source { get; }
        /// <summary>
        /// 目标属性信息
        /// </summary>
        PropertyInfo TargetProperty { get; }
    }
}
