using Ao.Core;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置设计器服务
    /// </summary>
    public class SettingDesignerService<TUI> : PackagingService<DefaultPackage<ISettingDesignerMetadata<TUI>>, ISettingDesignerMetadata<TUI>, DefaultPackage<ISettingDesignerMetadata<TUI>>>, ISettingDesignerService<TUI>
    {
        private Lazy<ISettingMap> settingMap;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISettingMapSource SettingMapSource { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISettingMap SettingMap => settingMap.Value;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event SettingMapRebuildHandle<TUI> SettingMapRebuild;
        /// <summary>
        /// 初始化<see cref="SettingDesignerService{TUI}"/>
        /// </summary>
        public SettingDesignerService()
        {
            SettingMapSource = new SettingMapSource();
            //SettingMapSource.MapSourceAdded += SettingMapSource_MapSourceAdded;
            SettingMapSource_MapSourceAdded(null, null);
        }


        private void SettingMapSource_MapSourceAdded(ISettingMapSource arg1, object arg2)
        {
            settingMap = new Lazy<ISettingMap>(GenerateSettingMap, true);
        }
        private ISettingMap GenerateSettingMap()
        {
            SettingMapRebuild?.Invoke(this);
            return SettingMapSource.Build();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context"><inheritdoc/></param>
        /// <returns></returns>
        public SettingDesigner<TUI> GetDesigner(ISettingDesignerContext context)
        {
            var objPath = context.Configuration.GetSettingPath();
            SettingMap.TryGetValue(objPath, out var node);
            var ctx = new SettingDesignerMakingContext(context,objPath,node);
            TUI uie =default;
            foreach (var item in base.Inherits)
            {
                if (item.DesignerProvider.Condition(ctx))
                {
                    uie = item.DesignerProvider.Make(ctx);
                    break;
                }
            }
            return new SettingDesigner<TUI>(uie,ctx);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Condition(ISettingDesignerMetadata<TUI> type)
        {
            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <returns></returns>
        protected override DefaultPackage<ISettingDesignerMetadata<TUI>> MakePackage(Assembly assembly)
        {
            return new DefaultPackage<ISettingDesignerMetadata<TUI>>(assembly);
        }
    }
}
