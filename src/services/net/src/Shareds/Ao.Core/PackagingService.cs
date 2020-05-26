using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ao.Core
{
    public abstract class PackagingService<TPackage, TInheritType, TAddingType> : NotifyableObject,INotifyPropertyChanged, IPackagingService<TPackage, TInheritType, TAddingType>, IEnumerable<TPackage>
            where TPackage : PackageBase<TInheritType>
    {
        /// <summary>
        /// 创建者的程序集
        /// </summary>
        private readonly Assembly createAssembly;
        protected readonly ObservableCollection<TPackage> packages;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event Action<NotifyCollectionChangedAction, TPackage> PackagesChanged;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<TPackage> Packages => packages;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<TInheritType> Inherits => packages.SelectMany(p => p).ToArray();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object SyncRoot { get; }

        public PackagingService()
        {
            SyncRoot = new object();
            createAssembly = Assembly.GetCallingAssembly();
            packages = new ObservableCollection<TPackage>();
            packages.CollectionChanged += ViewPackages_CollectionChanged;
        }
        private void ViewPackages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                var vps = e.NewItems.OfType<TPackage>();
                foreach (var item in vps)
                {
                    PackagesChanged?.Invoke(e.Action, item);
                }
            }
            if (e.OldItems != null)
            {
                var vps = e.OldItems.OfType<TPackage>();
                foreach (var item in vps)
                {
                    PackagesChanged?.Invoke(e.Action, item);
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="views"><inheritdoc/></param>
        /// <returns></returns>
        public virtual TInheritType[] Add(Assembly assembly, params TInheritType[] views)
        {
            var viewTypes = views.Where(v => Condition(v));
            var caller = assembly;
            lock (SyncRoot)
            {
                var pkg = packages.FirstOrDefault(p => p.Assembly == caller);
                if (pkg != null)
                {
                    pkg.medatas.AddRange(views);
                }
                else
                {
                    pkg = MakePackage(caller);
                    pkg.medatas.AddRange(views);
                    packages.Add(pkg);
                }
            }
            return viewTypes.ToArray();
        }
        protected abstract TPackage MakePackage(Assembly assembly);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public virtual bool Condition(TInheritType type)
        {
            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="removedPkg"><inheritdoc/></param>
        /// <returns></returns>
        public virtual bool Remove(Assembly assembly, TPackage removedPkg)
        {
            var succeed = false;
            var caller = Assembly.GetCallingAssembly();
            lock (SyncRoot)
            {
                removedPkg = Packages.FirstOrDefault(v => v.Assembly == assembly);
                if (removedPkg != null)
                {
                    //如果包程序集是调用程序集或者，调用程序集是创建者程序集就认为可以移除
                    if (removedPkg.Assembly == caller || createAssembly == caller)
                    {
                        packages.Remove(removedPkg);
                        succeed = true;
                    }
                }
            }
            return succeed;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="removes"><inheritdoc/></param>
        /// <returns></returns>
        public virtual TInheritType[] Remove(Assembly assembly, params TInheritType[] removes)
        {
            var succeeds = new List<TInheritType>();
            foreach (var item in removes)
            {
                var package = packages.FirstOrDefault(p => p.Contains(item));
                if (package != null&&package.medatas.Contains(item))
                {
                    package.medatas.Remove(item);
                    succeeds.Add(item);
                }
            }
            return succeeds.ToArray();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Clear()
        {
            var caller = Assembly.GetCallingAssembly();
            if (caller != createAssembly)//认为没有权限
            {
                return false;
            }
            lock (SyncRoot)
            {
                packages.Clear();
            }
            return true;
        }

        public IEnumerator<TPackage> GetEnumerator()
        {
            return Packages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
