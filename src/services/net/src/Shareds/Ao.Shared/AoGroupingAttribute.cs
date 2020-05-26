using System;

namespace Ao
{
    /// <summary>属性分组</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class AoGroupingAttribute : Attribute
    {
        /// <summary>
        /// 获取一个值，表示分组名
        /// </summary>
        /// <returns></returns>
        public override abstract string ToString();
    }
}
