using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ao
{
    /// <summary>
    /// 表示正在分析的属性项基类
    /// </summary>
    public abstract class AoAnalizedPropertyItemBase : AoAnalizedItem
    {
        /// <summary>
        /// 表示默认的设置器
        /// </summary>
        protected static readonly AoMemberSetter<object> EmptySetter = _ => ThrowNoSupport();
        /// <summary>
        /// 表示默认的取值器
        /// </summary>
        protected static readonly AoMemberGetter<object> EmptyGetter = () => ThrowNoSupport();
        /// <summary>
        /// 取值器
        /// </summary>
        protected AoMemberGetter<object> getter;
        /// <summary>
        /// 设置器
        /// </summary>
        protected AoMemberSetter<object> setter;
        /// <summary>
        /// 初始化<see cref="AoAnalizedPropertyItemBase"/>
        /// </summary>
        /// <param name="source">目标源</param>
        public AoAnalizedPropertyItemBase(object source) : base(source)
        {
        }
        /// <summary>
        /// 属性类型
        /// </summary>
        public abstract Type ValueType { get; }
        /// <summary>
        /// 属性名
        /// </summary>
        public abstract string ValueName { get; }
        /// <summary>
        /// 属性设值器
        /// </summary>
        public virtual AoMemberSetter<object> Setter => setter ?? InitSetter();
        /// <summary>
        /// 属性取值器
        /// </summary>
        public virtual AoMemberGetter<object> Getter => getter ?? InitGetter();
        /// <summary>
        /// 此属是否可以设值
        /// </summary>
        public bool CanSet => Setter != EmptySetter;
        /// <summary>
        /// 此属性是否可以取值
        /// </summary>
        public bool CanGet => Getter != EmptyGetter;
        /// <summary>
        /// 初始化取值器
        /// </summary>
        /// <returns></returns>
        protected abstract AoMemberGetter<object> InitGetter();
        /// <summary>
        /// 初始化设置器
        /// </summary>
        /// <returns></returns>
        protected abstract AoMemberSetter<object> InitSetter();

        private static object ThrowNoSupport()
        {
            throw new NotSupportedException("当前对象不支持此操作");
        }
        /// <summary>
        /// 从此属性获取一个特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <returns></returns>
        public virtual T GetCustomAttribute<T>()
            where T:Attribute
        {
            return ValueType.GetCustomAttribute<T>();
        }
        /// <summary>
        /// 从此属性获取特性集合
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> GetCustomAttributes<T>()
            where T : Attribute
        {
            return ValueType.GetCustomAttributes<T>();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"SourceType:{SourceType},ValueType:{ValueType},ValueName:{ValueName}";
        }
    }

}
