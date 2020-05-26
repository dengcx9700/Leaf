using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TView"><inheritdoc/></typeparam>
    public class DynamicBuilder<TView> : IDynamicBuilder<TView>
    {
        /// <summary>
        /// 初始化<see cref="DynamicBuilder{TView}"/>
        /// </summary>
        /// <param name="analizer">分析器</param>
        /// <param name="vm">视图模型</param>
        /// <param name="viewBuilders">视图建造器</param>
        /// <param name="viewWraper">视图装饰器</param>
        public DynamicBuilder(IAoAnalizer analizer, object vm, IViewBuildable<TView> viewBuilders,
            IViewWraper<TView> viewWraper = null)
        {
            this.vm = vm;
            this.Analizer = analizer;
            this.ViewBuilders = viewBuilders;
            this.viewWraper = viewWraper;
            list = new List<object>();
            Views = new ObservableCollection<TView>();
        }
        private readonly object vm;
        private IList list;
        private readonly IViewWraper<TView> viewWraper;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IAoAnalizer Analizer { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IViewBuildable<TView> ViewBuilders { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<TView> Views { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <param name="deep"><inheritdoc/></param>
        /// <param name="options"><inheritdoc/></param>
        /// <param name="condition"><inheritdoc/></param>
        public void With(object obj, bool deep = true, AnalizingOptions options = null, Predicate<AoAnalizedPropertyItemBase> condition = null)
        {
            var res = Analizer.Analize(obj, deep, options);
            var props = res.MemberItems;
            if (condition != null)
            {
                props = res.MemberItems.Where(m => condition(m)).ToArray();
            }
            foreach (var item in props)
            {
                var view = ViewBuilders.Build(vm, item);
                if (view != null)
                {
                    With(view);
                }
            }
            list.Add(obj);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="view"></param>
        public void With(TView view)
        {
            if (viewWraper != null)
            {
                view = viewWraper.Wraper(view);
            }
            Views.Add(view);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IList ToList()
        {
            var lst = new List<object>();
            foreach (var item in list)
            {
                lst.Add(item);
            }
            return lst;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="list"><inheritdoc/></param>
        /// <param name="deep"><inheritdoc/></param>
        public void FromList(IList list, bool deep = true)
        {
            Views.Clear();
            foreach (var item in list)
            {
                With(item, deep);
            }
            this.list = list;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Reset()
        {
            Views.Clear();
            list.Clear();
        }
    }
}
