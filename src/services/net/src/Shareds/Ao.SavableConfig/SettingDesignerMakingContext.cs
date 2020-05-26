using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置设计器生成的上下文
    /// </summary>
    public class SettingDesignerMakingContext :ISettingDesignerMakingContext
    {
        /// <summary>
        /// 初始化<see cref="SettingDesignerMakingContext"/>
        /// </summary>
        /// <param name="designerContext"><inheritdoc cref="DesignerContext"/></param>
        /// <param name="objectPath"><inheritdoc cref="ObjectPath"/></param>
        /// <param name="settingMapNode"><inheritdoc cref="SettingMapNode"/></param>
        public SettingDesignerMakingContext(ISettingDesignerContext designerContext, string objectPath, SettingMapNode settingMapNode)
        {
            DesignerContext = designerContext;
            ObjectPath = objectPath;
            SettingMapNode = settingMapNode;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISettingDesignerContext DesignerContext { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ObjectPath { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SettingMapNode SettingMapNode { get; }
    }
}
