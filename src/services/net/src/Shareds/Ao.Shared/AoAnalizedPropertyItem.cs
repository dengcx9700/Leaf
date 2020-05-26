using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ao
{
    /// <summary>
    /// 表示正在分析中的属性项
    /// </summary>
    public class AoAnalizedPropertyItem: AoAnalizedPropertyItemBase
    {
        /// <summary>
        /// 初始化<see cref="AoAnalizedPropertyItem"/>
        /// </summary>
        /// <param name="source">目标源</param>
        public AoAnalizedPropertyItem(object source) : base(source)
        {
        }
        /// <summary>
        /// 初始化<see cref="AoAnalizedPropertyItem"/>
        /// </summary>
        /// <param name="source">目标源</param>
        /// <param name="property">属性项</param>
        public AoAnalizedPropertyItem(object source, PropertyInfo property)
            : base(source)
        {
            Property = property;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual PropertyInfo Property { get; protected internal set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string ValueName => Property?.Name;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Type ValueType => Property?.PropertyType;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override AoMemberGetter<object> InitGetter()
        {
            if (Property.CanRead && Property.GetMethod.IsPublic && Property.GetMethod.GetParameters().Length == 0)
            {
                getter = ReflectionHelper.GetGetter<object>(Source, Property.GetMethod);
            }
            else
            {
                getter = EmptyGetter;
            }
            return getter;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override AoMemberSetter<object> InitSetter()
        {
            if (Property.CanWrite && Property.SetMethod.IsPublic && Property.SetMethod.GetParameters().Length == 1)
            {
                setter = ReflectionHelper.GetSetter<object>(Source, Property.PropertyType, Property.SetMethod);
            }
            else
            {
                setter = EmptySetter;
            }
            return setter;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override T GetCustomAttribute<T>()
        {
            return Property.GetCustomAttribute<T>();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override IEnumerable<T> GetCustomAttributes<T>()
        {
            return Property.GetCustomAttributes<T>();
        }
    }

}
