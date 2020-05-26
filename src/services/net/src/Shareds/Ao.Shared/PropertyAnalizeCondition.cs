using System;
using System.Linq.Expressions;

namespace Ao.Shared
{
    /// <summary>
    /// 属性分析条件器
    /// </summary>
    public class PropertyAnalizeCondition : IAnalizeCondition
    {
        /// <summary>
        /// 初始化<see cref="PropertyAnalizeCondition"/>
        /// </summary>
        /// <param name="propertyName"><inheritdoc cref="PropertyName"/></param>
        public PropertyAnalizeCondition(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            PropertyName = propertyName;
        }
        /// <summary>
        /// 初始化<see cref="PropertyAnalizeCondition"/>
        /// </summary>
        /// <param name="targetType"><inheritdoc cref="TargetType"/></param>
        /// <param name="propertyName"><inheritdoc cref="PropertyName"/></param>
        public PropertyAnalizeCondition(Type targetType, string propertyName)
            :this(propertyName)
        {
            TargetType = targetType;
        }
        /// <summary>
        /// 初始化<see cref="PropertyAnalizeCondition"/>
        /// </summary>
        /// <param name="sourceType"><inheritdoc cref="SourceType"/></param>
        /// <param name="targetType"><inheritdoc cref="TargetType"/></param>
        /// <param name="propertyName"><inheritdoc cref="PropertyName"/></param>
        public PropertyAnalizeCondition(Type sourceType, Type targetType, string propertyName)
            :this(targetType,propertyName)
        {
            SourceType = sourceType;
        }
        /// <summary>
        /// 源类型，此值可以为null
        /// </summary>
        public Type SourceType { get; }
        /// <summary>
        /// 目标类型，此值可以为null
        /// </summary>
        public Type TargetType { get; }
        /// <summary>
        /// 目标属性名
        /// </summary>
        public string PropertyName { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="analizer"><inheritdoc/></param>
        /// <param name="propertyItem"><inheritdoc/></param>
        /// <returns></returns>
        public bool Condition(IAoAnalizer analizer, AoAnalizedPropertyItemBase propertyItem)
        {
            if (SourceType!=null&&SourceType.IsEquivalentTo(propertyItem.SourceType))
            {
                return false;
            }
            if (TargetType!=null&&TargetType.IsEquivalentTo(propertyItem.ValueType))
            {
                return false;
            }
            return PropertyName == propertyItem.ValueName;
        }
    }
}
