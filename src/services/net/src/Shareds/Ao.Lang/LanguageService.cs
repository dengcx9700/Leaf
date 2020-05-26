using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Ao.Core;

namespace Ao.Lang
{
    /// <summary>
    /// 语言服务的实现
    /// </summary>
    public class LanguageService : PackagingService<DefaultPackage<ILanguageMetadata>, ILanguageMetadata, DefaultPackage<ILanguageMetadata>>, ILanguageService
    {
        private bool reBuildIfCollectionChanged;
        private readonly Dictionary<CultureInfo, ILanguageNode> cultureToLangs= new Dictionary<CultureInfo, ILanguageNode>();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns></returns>
        public string this[string key]
        {
            get => this.GetCurrentValue(key);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool ReBuildIfCollectionChanged
        {
            get => reBuildIfCollectionChanged;
            set => RaisePropertyChanged(ref reBuildIfCollectionChanged, value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<CultureInfo> SupportCultures => cultureToLangs.Keys;

        /// <summary>
        /// <inheritdoc/>,如果获取失败返回null
        /// </summary>
        /// <param name="cultureInfo"><inheritdoc/></param>
        /// <returns></returns>
        public ILanguageRoot GetRoot(CultureInfo cultureInfo)
        {
            if (cultureToLangs.TryGetValue(cultureInfo,out var node))
            {
                return node.Root;
            }
            return null;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="cultureInfo"><inheritdoc/></param>
        /// <returns></returns>
        public bool IsBuilt(CultureInfo cultureInfo)
        {
            if (cultureInfo is null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            if (cultureToLangs.TryGetValue(cultureInfo,out var node))
            {
                return node.IsBuilt;
            }
            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <returns></returns>
        protected override DefaultPackage<ILanguageMetadata> MakePackage(Assembly assembly)
        {
            return new DefaultPackage<ILanguageMetadata>(assembly);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="views"><inheritdoc/></param>
        /// <returns></returns>
        public override ILanguageMetadata[] Add(Assembly assembly, params ILanguageMetadata[] views)
        {
            foreach (var item in views)
            {
                if (!cultureToLangs.TryGetValue(item.Culture,out var node))
                {
                    node = new LanguageNode(item.Culture);
                    cultureToLangs.Add(item.Culture, node);
                }
                node.Add(item);
            }
            return base.Add(assembly, views);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="removedPkg"><in</param>
        /// <returns></returns>
        public override bool Remove(Assembly assembly, DefaultPackage<ILanguageMetadata> removedPkg)
        {
            var result= base.Remove(assembly, removedPkg);
            var emptyCultures = new List<CultureInfo>();
            foreach (var item in removedPkg)
            {
                if (cultureToLangs.TryGetValue(item.Culture,out var node))
                {
                    node.Remove(item);
                    if (node.Count==0)
                    {
                        emptyCultures.Add(item.Culture);
                    }
                }
            }
            foreach (var item in emptyCultures)
            {
                cultureToLangs.Remove(item);
            }
            return result;
        }
        /// <summary>
        /// 重建节点
        /// </summary>
        public void ReBuild()
        {
            foreach (var item in cultureToLangs.Values)
            {
                item.ReBuild();
            }
        }
    }
}
