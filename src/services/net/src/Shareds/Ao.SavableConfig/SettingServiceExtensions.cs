using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 对类型<see cref="ISettingService"/>的扩展
    /// </summary>
    public static class SettingServiceExtensions
    {

        /// <summary>
        /// 确保配置源生成，只有是类型<see cref="FileConfigurationSourceAttributeBase"/>才会生成
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static async ValueTask<int> EnusreConfigurationSourceAsync(this ISettingService service)
        {
            var makeCount = 0;
            foreach (var item in service.ConfigurationSources)
            {
                if (item is FileConfigurationSourceAttributeBase fileConfig)
                {
                    var status = item.GetMakeSourceStatus();
                    if (status == MakeSourceStatus.NotMakeCanMakeDefault)
                    {
                        var str = item.MakeDefault();
#if NETSTANDARD2_1
                        await File.WriteAllTextAsync(fileConfig.FileFullPath, str);
#else
                        File.WriteAllText(fileConfig.FileFullPath, str);
#endif
                        makeCount++;
                    }
                }
            }
            return makeCount;
        }
        /// <summary>
        /// 创建设计器
        /// </summary>
        /// <typeparam name="TUI">目标UI</typeparam>
        /// <param name="settingService">设置服务</param>
        /// <param name="settingDesignerService">设置设计器服务</param>
        /// <returns></returns>
        public static SettingDesigner<TUI>[] MakeDesigner<TUI>(this ISettingService settingService, ISettingDesignerService<TUI> settingDesignerService)
        {
            var mapNodes = settingDesignerService.SettingMap.Values;
            var uies = new List<SettingDesigner<TUI>>();
            foreach (var item in mapNodes)
            {
                var configPath = item.GetConfigurationPath();
                var section = settingService.Root.GetSection(configPath);
                var context = new SettingDesignerContext(section, item.Source, item.PropertyInfo);
                var designer = settingDesignerService.GetDesigner(context);
                uies.Add(designer);
            }
            return uies.ToArray();
        }
        /// <summary>
        /// 使用默认的保存器保存
        /// </summary>
        /// <param name="service">设置服务</param>
        /// <returns></returns>
        public static Task<bool> DefaultSaveChangesAsync(this ISettingService service)
        {
            return service.SaveChangesAsync(JsonSettingChangeSaver.Default, DefaultSettingConfig.DefaultSaver);
        }
        /// <summary>
        /// 添加一个更改
        /// </summary>
        /// <typeparam name="TUI"></typeparam>
        /// <param name="settingDesignerSer"></param>
        /// <param name="changes">更改项</param>
        public static void AddChanges<TUI>(this ISettingDesignerService<TUI> settingDesignerSer, KeyValuePair<string, string>[] changes)
        {
            foreach (var item in changes)
            {
                var key = item.Key.Replace(':', '.');
                if (settingDesignerSer.SettingMap.TryGetValue(key, out var node) && node.Setter != null)
                {

#if NETSTANDARD2_1
                    if (node.PropertyInfo.PropertyType.IsEnum && Enum.TryParse(node.PropertyInfo.PropertyType, item.Value, out var val))
                    {
                        node.Setter(val);
                    }
#else
                    object val = null;
                    var ok = false;
                    try
                    {
                        val = Enum.Parse(node.PropertyInfo.PropertyType, item.Value);
                        ok = true;
                    }
                    catch (Exception) { }
                    if(ok)
                    {
                        node.Setter(val);
                    }
#endif

                    else if (node.PropertyInfo.PropertyType.IsPrimitive)
                    {
                        val = Convert.ChangeType(item.Value, node.PropertyInfo.PropertyType);
                        node.Setter(val);
                    }
                }
            }
        }

    }
}
