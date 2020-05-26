using Microsoft.Extensions.Configuration;
using Ao.SavableConfig.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ao.Core;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置服务
    /// </summary>
    public class SettingService : PackagingService<DefaultPackage<Type>, Type, DefaultPackage<Type>>, ISettingService
    {

        private readonly ObservableCollection<ConfigurationSourceAttributeBase> configurationSources;
        private ISavableConfigurationBuilder builder;
        private Lazy<ISavableConfigurationRoot> root;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISavableConfigurationBuilder Builder => builder;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISavableConfigurationRoot Root
        {
            get
            {
                if (!root.IsValueCreated)
                {
                    _ = root.Value;
                    RootChanged?.Invoke(this);
                }
                return root.Value;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsLoadRoot => root.IsValueCreated;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IList<ConfigurationSourceAttributeBase> ConfigurationSources => configurationSources;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<ISettingService> BuilderReset;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<ISettingService> Saved;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<NotifyCollectionChangedEventArgs> SourcesChanged;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event SettingReloadHandle Reloaded;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event SettingRootChangedHandle RootChanged;
        /// <summary>
        /// 初始化<see cref="SettingService"/>
        /// </summary>
        public SettingService()
        {
            configurationSources = new ObservableCollection<ConfigurationSourceAttributeBase>();
            configurationSources.CollectionChanged += ConfigurationSources_CollectionChanged;
            ResetBuilder();
        }

        private void ConfigurationSources_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SourcesChanged?.Invoke(e);
            if (IsLoadRoot)
            {
                root = new Lazy<ISavableConfigurationRoot>(BuildRoot, true);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Condition(Type type)
        {
            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {

        }
        private ISavableConfigurationRoot BuildRoot()
        {
            return Builder.Build();   
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <returns></returns>
        protected override DefaultPackage<Type> MakePackage(Assembly assembly)
        {
            return new DefaultPackage<Type>(assembly);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="changeSaver"><inheritdoc/></param>
        /// <param name="streamSaver">流保存器</param>
        /// <returns></returns>
        /// <exception cref="SettingRootNotLoadException"><see cref="IsLoadRoot"/>此属性如果为false时，调用此方法会触发此异常</exception>
        /// <exception cref="IOException"><see cref="IsLoadRoot"/>在读写文件错误时可能触发</exception>
        public async Task<bool> SaveChangesAsync(ISettingChangeSaver changeSaver, IStreamSaver streamSaver)
        {
            if (changeSaver is null)
            {
                throw new ArgumentNullException(nameof(changeSaver));
            }

            if (streamSaver is null)
            {
                throw new ArgumentNullException(nameof(streamSaver));
            }

            if (!IsLoadRoot)
            {
                throw new SettingRootNotLoadException();
            }
            var res = await changeSaver.SaveAsync(Root);
            if (res.Succeed)
            {
                using (var content = res.Content)
                {
                    await streamSaver.SaveAsync(content);
                }
                Saved?.Invoke(this);
            }
            return res.Succeed;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="changeSaver"><inheritdoc/></param>
        /// <param name="config"><inheritdoc/></param>
        /// <param name="settingDesignerService">设置设计器服务</param>
        /// <returns></returns>
        public async Task<SettingReloadResult> ReloadAsync<TUI>(ISettingDesignerService<TUI> settingDesignerService, ISettingChangeSaver changeSaver,SettingLoadConfig config)
        {
            var tmpRoot = IsLoadRoot ? Root : null;
            var needToRestore = false;
            try
            {
                SettingChangeSaveResult? saveResult = null;
                if (IsLoadRoot&& !config.IgnoreChanges)
                {
                    saveResult = await changeSaver.SaveAsync(tmpRoot);
                    if (!saveResult.Value.Succeed)
                    {
                        return new SettingReloadResult(null, saveResult, null);
                    }
                }
                //备份一下文件，如果有
                if (File.Exists(DefaultSettingConfig.ChangesFilePath))
                {
                    File.Copy(DefaultSettingConfig.ChangesFilePath, DefaultSettingConfig.ChangesFileBakPath, true);
                    needToRestore = true;
                }
                var loadResult =await changeSaver.LoadAsync(new SettingChangeLoadConfig(AppDomain.CurrentDomain.BaseDirectory, DefaultSettingConfig.ChangesFilePath));
                if (loadResult.Succeed)
                {
                    root = new Lazy<ISavableConfigurationRoot>(() => BuildRootWithChanges(settingDesignerService,loadResult.ConfigurationPairs));
                }
                Reloaded?.Invoke(this, true);
                return new SettingReloadResult(null, saveResult, loadResult);
            }
            catch (Exception ex)
            {
                if (needToRestore)
                {
                    File.Copy(DefaultSettingConfig.ChangesFileBakPath, DefaultSettingConfig.ChangesFilePath, true);
                }
                root = new Lazy<ISavableConfigurationRoot>(() => tmpRoot);
                Reloaded?.Invoke(this, false);
                return SettingReloadResult.FromOutterException(ex);
            }
        }
        private ISavableConfigurationRoot BuildRootWithChanges<TUI>(ISettingDesignerService<TUI> settingDesignerService,KeyValuePair<string,string>[] changes)
        {
            var root = Root;
            settingDesignerService.AddChanges(changes);
            return root;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ResetBuilder()
        {
            builder = new SavableConfiurationBuilder();
            root = new Lazy<ISavableConfigurationRoot>(BuildRoot);
            BuilderReset?.Invoke(this);
        }
    }
}
