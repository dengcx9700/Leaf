using System;

namespace Ao
{
    /// <summary>
    /// 表示一个正在分析的方法项，此方法项是自定义的
    /// </summary>
    public class MethodAoAnalizedPropertyItem : AoAnalizedPropertyItemBase
    {
        /// <summary>
        /// 初始化<see cref="MethodAoAnalizedPropertyItem"/>
        /// </summary>
        /// <param name="source">目标源</param>
        /// <param name="getter">取值器</param>
        /// <param name="setter">设置器</param>
        public MethodAoAnalizedPropertyItem(object source, AoMemberGetter<object> getter, AoMemberSetter<object> setter)
            : base(source)
        {
            base.getter = getter;
            base.setter = setter;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Type ValueType => Source?.GetType();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string ValueName => ValueType?.Name;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override AoMemberGetter<object> InitGetter()
        {
            return getter;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override AoMemberSetter<object> InitSetter()
        {
            return setter;
        }
    }

}
