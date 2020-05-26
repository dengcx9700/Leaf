using Ao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置对象图节点
    /// </summary>
    public class SettingMapNode
    {
        /// <summary>
        /// 包含的属性绑定值
        /// </summary>
        public readonly static BindingFlags IncludePropertyFlag = BindingFlags.Public | BindingFlags.Instance;
        /// <summary>
        /// 部分分割的字符串
        /// </summary>
        public readonly static string PartSpliter = ".";
        private SettingMapNode(string prevPath)
        {
            getter = new Lazy<AoMemberGetter<object>>(() => CompileGetter(Source, PropertyInfo));
            setter = new Lazy<AoMemberSetter<object>>(() => CompileSetter(Source, PropertyInfo));
            nexts = new Lazy<SettingMapNode[]>(GetSettingNodes);
            if (prevPath==null)
            {
                prevPath = string.Empty;
            }
            PrevPath = prevPath;
            path = new Lazy<string>(() => GeneratePath());
        }
        /// <summary>
        /// 初始化<see cref="SettingMapNode"/>
        /// </summary>
        /// <param name="prevPath"><inheritdoc cref="PrevPath"/></param>
        /// <param name="source"><inheritdoc cref="Source"/></param>
        /// <param name="propertyName">属性名</param>
        public SettingMapNode(string prevPath,object source, string propertyName)
            :this(prevPath)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            Source = source ?? throw new ArgumentNullException(nameof(source));
            PropertyInfo = Source.GetType().GetProperty(propertyName);
        }
        /// <summary>
        /// <inheritdoc cref="SettingMapNode(string,object, string)"/>
        /// </summary>
        /// <param name="prevPath"><inheritdoc cref="PrevPath"/></param>
        /// <param name="source"><inheritdoc cref="Source"/></param>
        /// <param name="propertyInfo"><inheritdoc cref="PropertyInfo"/></param>
        public SettingMapNode(string prevPath,object source, PropertyInfo propertyInfo)
            :this(prevPath)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }
        private readonly Lazy<string> path;
        private readonly Lazy<AoMemberGetter<object>> getter;
        private readonly Lazy<AoMemberSetter<object>> setter;
        private readonly Lazy<SettingMapNode[]> nexts;

        /// <summary>
        /// 上一个路径
        /// </summary>
        public string PrevPath { get; }
        /// <summary>
        /// 源实例
        /// </summary>
        public object Source { get; }
        /// <summary>
        /// 对象获取器
        /// </summary>
        public AoMemberGetter<object> Getter => getter.Value;
        /// <summary>
        /// 对象设置器
        /// </summary>
        public AoMemberSetter<object> Setter => setter.Value;
        /// <summary>
        /// 获取或设置一个值，此值为当前目标值，或设置为目标值
        /// </summary>
        public object Value
        {
            get => getter.Value();
            set => setter.Value.Invoke(value);
        }
        /// <summary>
        /// 当前路径
        /// </summary>
        public string Path => path.Value;
        /// <summary>
        /// 目标属性信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; }
        /// <summary>
        /// 下一批节点
        /// </summary>
        public SettingMapNode[] Nexts => nexts.Value;
        /// <summary>
        /// 寻找符合条件的下一些节点
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IEnumerable<SettingMapNode> FindNexts(Predicate<SettingMapNode> condition)
        {
            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            var nodes = new[] { this };
            while (nodes.Length!=0)
            {
                foreach (var item in nodes)
                {
                    if (condition(item))
                    {
                        yield return item;
                    }
                }
                nodes = nodes.SelectMany(n => n.Nexts).ToArray();
            }
        }
        /// <summary>
        /// <para><inheritdoc cref="FindNexts(Predicate{SettingMapNode})"/></para>
        /// 包含自身
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IEnumerable<SettingMapNode> Find(Predicate<SettingMapNode> condition)
        {
            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            var includeThis = condition(this);
            var nodes= FindNexts(condition);
            if (includeThis)
            {
                return new[] { this }.Concat(nodes);
            }
            return nodes;
        }
        private string GeneratePath()
        {
            return string.Join(PartSpliter, PrevPath, PropertyInfo.Name);
        }
        private SettingMapNode[] GetSettingNodes()
        {
            return GetSettingNodes(this);
        }
        /// <summary>
        /// 获取设置的下一些节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SettingMapNode[] GetSettingNodes(SettingMapNode node)
        {
            var val = node.Value;
            if (val != null)
            {
                var type = val.GetType();
                if (!type.IsValueType &&!type.IsPrimitive&& !type.IsEquivalentTo(typeof(string)))
                {
                    var props = type.GetProperties(IncludePropertyFlag);
                    return PropertyInfosAsNode(node.Path, node.Value, props);
                }
            }
            return Array.Empty<SettingMapNode>();
        }
        /// <summary>
        /// 属性信息转为对象图节点
        /// </summary>
        /// <param name="path">当前路径</param>
        /// <param name="souce">源</param>
        /// <param name="propertyInfos">属性信息集合</param>
        /// <returns></returns>
        public static SettingMapNode[] PropertyInfosAsNode(string path,object souce,PropertyInfo[] propertyInfos)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("message", nameof(path));
            }

            if (souce is null)
            {
                throw new ArgumentNullException(nameof(souce));
            }

            if (propertyInfos is null)
            {
                throw new ArgumentNullException(nameof(propertyInfos));
            }

            return propertyInfos.Select(p => new SettingMapNode(path, souce, p)).ToArray();

        }
        private static AoMemberGetter<object> CompileGetter(object souce,PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanRead)
            {
                return ReflectionHelper.GetGetter<object>(souce, propertyInfo.GetGetMethod());
            }
            return null;
        }
        private static AoMemberSetter<object> CompileSetter(object souce, PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanWrite)
            {
                return ReflectionHelper.GetSetter<object>(souce, propertyInfo.PropertyType
                    , propertyInfo.GetSetMethod());
            }
            return null;
        }
    }
}
