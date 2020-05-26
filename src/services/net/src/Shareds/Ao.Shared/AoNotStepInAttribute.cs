using System;
using System.Collections.Generic;
using System.Text;

namespace Ao
{
    /// <summary>
    /// 表示此属性不能步入解析
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =false)]
    public class AoNotStepInAttribute:Attribute
    {
        /// <summary>
        /// 表示当前情况是否可以踏入
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <param name="propertyItem">属性项</param>
        /// <returns></returns>
        public virtual bool CanStepIn(AoAnalizeContext context, AoAnalizedPropertyItemBase propertyItem)
        {
            return false;
        }
    }
}
