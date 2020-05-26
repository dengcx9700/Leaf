using System;

namespace Ao
{
    /// <summary>忽略解析</summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class AoIgnoreAttribute : Attribute
    {
    }
}
