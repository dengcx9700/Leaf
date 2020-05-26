using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置初始化器
    /// </summary>
    public class SettingIniter<TUI>
    {
        private readonly ISettingService settingService;
        private readonly ISettingDesignerService<TUI> settingDesignerService;
        /// <summary>
        /// 依附的设置服务
        /// </summary>
        public ISettingService SettingService => settingService;
        /// <summary>
        /// 依附的设置设计器服务
        /// </summary>
        public ISettingDesignerService<TUI> SettingDesignerService => settingDesignerService;
        /// <summary>
        /// 初始化<see cref="SettingIniter{TUI}"/>
        /// </summary>
        /// <param name="settingService"><inheritdoc cref="SettingService"/></param>
        /// <param name="settingDesignerService"><inheritdoc cref="SettingDesignerService"/></param>
        public SettingIniter(ISettingService settingService, ISettingDesignerService<TUI> settingDesignerService)
        {
            this.settingService = settingService ?? throw new ArgumentNullException(nameof(settingService));
            this.settingDesignerService = settingDesignerService ?? throw new ArgumentNullException(nameof(settingDesignerService));
        }
        /// <summary>
        /// <inheritdoc cref="WithAttributes(Assembly, ConfigurationSourceAttributeBase[])"/>，程序集取当前调用方的程序集
        /// </summary>
        /// <param name="sources">配置源</param>
        /// <returns></returns>
        public SettingIniter<TUI> WithAttributes(params ConfigurationSourceAttributeBase[] sources) 
        {
            var assembly = Assembly.GetExecutingAssembly();
            return WithAttributes(assembly, sources);
        }
        /// <summary>
        /// 与上一些属性
        /// </summary>
        /// <param name="assembly">目标程序集</param>
        /// <param name="sources">配置源</param>
        /// <returns></returns>
        public SettingIniter<TUI> WithAttributes(Assembly assembly,params ConfigurationSourceAttributeBase[] sources)
        {
            foreach (var item in sources)
            {
                item.Attack(assembly);
                settingService.ConfigurationSources.Add(item);
            }
            var bindTypes = sources.Where(s => s.BindType != null).Select(s => s.BindType).ToArray();
            var settingEntites = bindTypes.Select(s => Activator.CreateInstance(s))
                .ToArray();
            foreach (var item in settingEntites)
            {
                settingDesignerService.SettingMapSource.Add(item);
            }
            settingService.Add(assembly, bindTypes);
            //加入源
            var configSouces = sources.Select(s => s.GetConfigurationSource()).ToArray();
            foreach (var configSource in configSouces)
            {
                settingService.Builder.Add(configSource);
            }
            return this;
        }
        /// <summary>
        /// 与上一个程序集，在此程序集寻找符合的信息
        /// </summary>
        /// <param name="assembly">目标程序集</param>
        /// <returns></returns>
        public SettingIniter<TUI> WithAssembly(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var sources=assembly.GetCustomAttributes<ConfigurationSourceAttributeBase>();
            return WithAttributes(assembly, sources.ToArray());
        }
    }
}
