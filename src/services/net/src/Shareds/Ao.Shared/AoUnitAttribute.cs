using System;

namespace Ao
{
    /// <summary>标识可解析单元</summary>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Field| AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AoUnitAttribute : Attribute
    {
        /// <summary>是否可以被解析</summary>
        public virtual bool CanAnalize()
        {
            return true;
        }
    }

}
