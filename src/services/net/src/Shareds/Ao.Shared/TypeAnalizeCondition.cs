using System;

namespace Ao.Shared
{
    /// <summary>
    /// 类型解析条件
    /// </summary>
    public class TypeAnalizeCondition : IAnalizeCondition
    {
        /// <summary>
        /// 初始化<see cref="TypeAnalizeCondition"/>
        /// </summary>
        /// <param name="targetType"><inheritdoc cref="TargetType"/></param>
        public TypeAnalizeCondition(Type targetType)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="analizer"><inheritdoc/></param>
        /// <param name="propertyItem"><inheritdoc/></param>
        /// <returns></returns>
        public bool Condition(IAoAnalizer analizer, AoAnalizedPropertyItemBase propertyItem)
        {
            return !TargetType.IsEquivalentTo(propertyItem.ValueType);
        }
    }
}
