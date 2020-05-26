using Ao;
using Ao.Core;
using Ao.Core.Lru;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Ao.Project
{
    /// <summary>
    /// 表示工程部分的服务的实现
    /// </summary>
    public class ProjectPartService : PackagingService<ProjectPartPackage, IProjectPartProvider, ProjectPartPackage>, IProjectPartService
    {
        private readonly LruCacher<Type, AoMemberNewer<object>> cacher = new LruCacher<Type, AoMemberNewer<object>>(30);
        private readonly ObservableCollection<IProjectPartProvider> groupParts;
        private readonly ObservableCollection<IProjectPartProvider> propertyParts;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<IProjectPartProvider> GroupParts => groupParts;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<IProjectPartProvider> PropertyParts => propertyParts;
        public ProjectPartService()
        {
            groupParts = new ObservableCollection<IProjectPartProvider>();
            propertyParts = new ObservableCollection<IProjectPartProvider>();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Condition(IProjectPartProvider type)
        {
            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public object Create(Type type)
        {
            var newer = cacher.Get(type);
            if (newer!=null)
            {
                newer = ReflectionHelper.GetNewer(type);
                cacher.Add(type, newer);
            }
            return newer();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <returns></returns>
        protected override ProjectPartPackage MakePackage(Assembly assembly)
        {
            return new ProjectPartPackage(assembly);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="views"><inheritdoc/></param>
        /// <returns></returns>
        public override IProjectPartProvider[] Add(Assembly assembly, params IProjectPartProvider[] views)
        {
            var res= base.Add(assembly, views);
            foreach (var item in res)
            {
                if (item.Type==null)
                {
                    throw new ArgumentNullException(item.Name);
                }
                if (item.Type.GetInterface(typeof(IItemGroupPart).FullName)!=null)
                {
                    groupParts.Add(item);
                }
                if (item.Type.GetInterface(typeof(IPropertyGroupItem).FullName) != null)
                {
                    propertyParts.Add(item);
                }
            }
            return res;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="removedPkg"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Remove(Assembly assembly, ProjectPartPackage removedPkg)
        {
            var res= base.Remove(assembly, removedPkg);
            if (removedPkg!=null)
            {
                foreach (var item in removedPkg.Medatas)
                {
                    if (item.Type.GetInterface(typeof(IItemGroupPart).FullName) != null)
                    {
                        groupParts.Remove(item);
                    }
                    if (item.Type.GetInterface(typeof(IPropertyGroupItem).FullName) != null)
                    {
                        propertyParts.Remove(item);
                    }
                }
            }
            return res;
        }
    }
}
