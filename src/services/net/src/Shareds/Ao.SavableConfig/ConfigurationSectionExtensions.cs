using Microsoft.Extensions.Configuration;

namespace Ao.SavableConfig
{
    public static class ConfigurationSectionExtensions
    {
        /// <summary>
        /// 从配置路径获取设置路径
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public static string GetSettingPath(this IConfigurationSection section)
        {
            return section.Path.Replace(":", SettingMapNode.PartSpliter);
        }
        /// <summary>
        /// 从设置映射节点获取配置路径
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetConfigurationPath(this SettingMapNode node)
        {
            return node.Path.Replace(SettingMapNode.PartSpliter, ":");
        }
    }
}
