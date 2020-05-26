using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Ao.Shared.ForView.Input;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 建造失败时的处理器
    /// </summary>
    /// <param name="vm">视图模型</param>
    /// <param name="propertyItem">当前的属性项</param>
    public delegate void NoBuildHandle(object vm, AoAnalizedPropertyItemBase propertyItem);
    /// <summary>
    /// 根据<see cref="ForViewAttribute"/>和默认建造view
    /// </summary>
    public class ForViewBuilder<TView> : ICollection<IViewBuilder<TView>>, IViewBuildable<TView>
    {
        private readonly Dictionary<Type, IViewBuilder<TView>> customBuilders;
        private readonly List<IViewBuilder<TView>> viewBuilders;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<IViewBuilder<TView>> ViewBuilders => viewBuilders;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyDictionary<Type, IViewBuilder<TView>> CustomBuilders => customBuilders;
        /// <summary>
        /// 当前视图建造器的格式,<see cref="ViewBuilders"/>
        /// </summary>
        public int Count => viewBuilders.Count;

        bool ICollection<IViewBuilder<TView>>.IsReadOnly => false;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IStringProvider StringProvider { get; }
        /// <summary>
        /// 获取或设置当前集合的元素
        /// </summary>
        /// <param name="index">所在位置</param>
        /// <returns></returns>
        public IViewBuilder<TView> this[int index] 
        {
            get => viewBuilders[index];
            set => viewBuilders[index] = value;
        }

        /// <summary>
        /// 触发会在建造失败时
        /// </summary>
        public event NoBuildHandle NoBuilt;
        /// <summary>
        /// 初始化<see cref="ForViewBuilder{TView}"/>
        /// </summary>
        public ForViewBuilder()
        {
            viewBuilders = new List<IViewBuilder<TView>>();
            customBuilders = new Dictionary<Type, IViewBuilder<TView>>();
        }
        /// <summary>
        /// <inheritdoc cref="ForViewBuilder{TView}"/>
        /// </summary>
        /// <param name="stringProvider">字符串提供者</param>
        public ForViewBuilder(IStringProvider stringProvider)
            :this()
        {
            StringProvider = stringProvider;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="vm"><inheritdoc/></param>
        /// <param name="propertyItem"><inheritdoc/></param>
        /// <returns></returns>
        public TView Build(object vm, AoAnalizedPropertyItemBase propertyItem)
        {
            var context = new ViewBuildContext<TView>(vm, this);

            if (propertyItem.ValueType!=null)
            {
                var attr = propertyItem.GetCustomAttribute<ForViewAttribute>();
                if (attr != null)
                {
                    if (!customBuilders.TryGetValue(attr.BuildType, out var builder))
                    {
                        builder = Activator.CreateInstance(attr.BuildType) as IViewBuilder<TView>;
                        if (builder == null)
                        {
                            throw new ArgumentException($"{attr.BuildType.FullName} 必须实现IViewBuilder<TView>");
                        }
                        customBuilders.Add(attr.BuildType, builder);
                    }
                    return builder.BuildView(context, propertyItem);
                }
            }            
            var view = default(TView);
            foreach (var item in ViewBuilders)
            {
                if (item.Condition(propertyItem.ValueType))
                {
                    view = item.BuildView(context, propertyItem);
                    if (view != null)
                    {
                        break;
                    }
                }
            }
            if (view == null)
            {
                NoBuilt?.Invoke(vm, propertyItem);
            }
            return view;
        }
        /// <summary>
        /// 添加一个视图建造者
        /// </summary>
        /// <param name="builder"></param>
        public void Add(IViewBuilder<TView> builder)
        {
            viewBuilders.Add(builder);
            Sort();
        }
        /// <summary>
        /// 对建造器进行排序
        /// </summary>
        protected virtual void Sort()
        {
            viewBuilders.Sort(ViewBuilderComparer<TView>.Default);
        }
        /// <summary>
        /// 添加一个视图建造者的集合
        /// </summary>
        /// <param name="builder">建造者的集合</param>
        public void AddRange(IEnumerable<IViewBuilder<TView>> builder)
        {
            foreach (var item in builder)
            {
                viewBuilders.Add(item);
            }
            Sort();
        }
        /// <summary>
        /// 添加一个视图建造者的集合
        /// </summary>
        /// <param name="builder">建造者的集合</param>
        public void AddRange(params IViewBuilder<TView>[] builder)
        {
            foreach (var item in builder)
            {
                viewBuilders.Add(item);
            }
            Sort();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="builder"><inheritdoc/></param>
        /// <returns></returns>
        public bool Remove(IViewBuilder<TView> builder)
        {
            return viewBuilders.Remove(builder);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear()
        {
            viewBuilders.Clear();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns></returns>
        public bool Contains(IViewBuilder<TView> item)
        {
            return viewBuilders.Contains(item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="arrayIndex"><inheritdoc/></param>
        public void CopyTo(IViewBuilder<TView>[] array, int arrayIndex)
        {
            viewBuilders.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IViewBuilder<TView>> GetEnumerator()
        {
            return viewBuilders.GetEnumerator();
        }
        /// <summary>
        /// 创建一个动态视图建造器
        /// </summary>
        /// <param name="analizer">分析器</param>
        /// <param name="vm">视图模型</param>
        /// <param name="viewWraper">视图包装器</param>
        /// <returns></returns>
        public IDynamicBuilder<TView> GetDynamicBuilder(IAoAnalizer analizer, object vm,IViewWraper<TView> viewWraper=null)
        {
            return new DynamicBuilder<TView>(analizer, vm, this,viewWraper);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns></returns>
        public int IndexOf(IViewBuilder<TView> item)
        {
            return viewBuilders.IndexOf(item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="item"><inheritdoc/></param>
        public void Insert(int index, IViewBuilder<TView> item)
        {
            viewBuilders.Insert(index, item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        public void RemoveAt(int index)
        {
            viewBuilders.RemoveAt(index);
        }
    }
}
