using System;

namespace Ao
{
    /// <summary>
    /// 表示一个组名
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class AoNameGroupAttribute : AoGroupingAttribute
    {
        /// <summary>
        /// 默认的组名
        /// </summary>
        public static readonly string DefaultGroupName = "Default";
        /// <summary>
        /// 初始化<see cref="AoNameGroupAttribute"/>
        /// </summary>
        public AoNameGroupAttribute()
        {
            GroupName = DefaultGroupName;
        }
        /// <summary>
        /// 初始化<see cref="AoNameGroupAttribute"/>，并且设定默认组名
        /// </summary>
        /// <param name="groupName">默认组名</param>
        public AoNameGroupAttribute(string groupName)
        {
            this.GroupName = groupName;

        }
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (obj is string str) && str == GroupName;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return GroupName.GetHashCode();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GroupName;
        }
    }
}
