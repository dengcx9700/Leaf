using Ao.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ao.SavableConfig
{
    public delegate void SettingReloadHandle(ISettingService source, bool succeed);
    public delegate void SettingRootChangedHandle(ISettingService source);
    /// <summary>
    /// 表示设置的服务
    /// </summary>
    public interface ISettingService : IPackagingService<DefaultPackage<Type>, Type, DefaultPackage<Type>>, IDisposable
    {
        /// <summary>
        /// 配置源
        /// </summary>
        IList<ConfigurationSourceAttributeBase> ConfigurationSources { get; }
        /// <summary>
        /// 配置器
        /// </summary>
        ISavableConfigurationBuilder Builder { get; }
        /// <summary>
        /// 配置根
        /// </summary>
        ISavableConfigurationRoot Root { get; }
        /// <summary>
        /// <see cref="Root"/>是否被加载了
        /// </summary>
        bool IsLoadRoot { get; }
        /// <summary>
        /// 重置<see cref="Builder"/>
        /// </summary>
        void ResetBuilder();
        /// <summary>
        /// 保存已更改的设置
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync(ISettingChangeSaver changeSaver, IStreamSaver streamSaver);
        /// <summary>
        /// 重新加载配置器
        /// <para>
        /// 此操作会分成
        /// 1. 保存操作(如果需要)
        /// 2. 清除当前配置
        /// 3. 加载配置
        /// 4. 加载更改配置
        /// </para>
        /// </summary>
        /// <param name="settingDesignerService">设置设计服务</param>
        /// <param name="changeSaver">保存器</param>
        /// <param name="config">设置加载的配置</param>
        /// <returns></returns>
        /// <remarks>
        /// 此操作需要保持原子性
        /// </remarks>
        Task<SettingReloadResult> ReloadAsync<TUI>(ISettingDesignerService<TUI> settingDesignerService,ISettingChangeSaver changeSaver,SettingLoadConfig config);
        /// <summary>
        /// 配置建造器被重置了
        /// </summary>
        event Action<ISettingService> BuilderReset;
        /// <summary>
        /// <see cref="ConfigurationSources"/>集合改变了
        /// </summary>
        event Action<NotifyCollectionChangedEventArgs> SourcesChanged;
        /// <summary>
        /// 表示被保存了
        /// </summary>
        event Action<ISettingService> Saved;
        /// <summary>
        /// 表示被重载了
        /// </summary>
        event SettingReloadHandle Reloaded;
        /// <summary>
        /// 此事件表示属性<see cref="Root"/>改变了
        /// </summary>
        event SettingRootChangedHandle RootChanged;
    }
}
