using System;
using System.Collections;

namespace Ao
{
    /// <summary>
    /// 表示列表属性的属性项
    /// </summary>
    public class AoAnalizedListPropertyItem : AoAnalizedPropertyItemBase
    {
        /// <summary>
        /// 表示列表泛型类型
        /// </summary>
        public Type GenericType { get; }
        /// <summary>
        /// 表示当前列表
        /// </summary>
        public IEnumerable Value { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override string ValueName { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override Type ValueType => GenericType;
        /// <summary>
        /// 初始化<see cref="AoAnalizedListPropertyItem"/>
        /// </summary>
        /// <param name="source">目标集合</param>
        /// <param name="valueName">值名</param>
        public AoAnalizedListPropertyItem(IEnumerable source,string valueName)
            : base(source)
        {
            Value = source ?? throw new ArgumentNullException(nameof(source));
            if (Value.GetType().GenericTypeArguments.Length==0)
            {
                throw new NotSupportedException("仅支持泛型列表");
            }
            GenericType = Value.GetType().GenericTypeArguments[0];
            ValueName = valueName;
            getter = () => Value;
            setter = val =>
            {
                throw new NotSupportedException();
            };
        }
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
