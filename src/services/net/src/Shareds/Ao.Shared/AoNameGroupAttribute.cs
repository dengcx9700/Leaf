using System;

namespace Ao
{
    /// <summary>
    /// ��ʾһ������
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class AoNameGroupAttribute : AoGroupingAttribute
    {
        /// <summary>
        /// Ĭ�ϵ�����
        /// </summary>
        public static readonly string DefaultGroupName = "Default";
        /// <summary>
        /// ��ʼ��<see cref="AoNameGroupAttribute"/>
        /// </summary>
        public AoNameGroupAttribute()
        {
            GroupName = DefaultGroupName;
        }
        /// <summary>
        /// ��ʼ��<see cref="AoNameGroupAttribute"/>�������趨Ĭ������
        /// </summary>
        /// <param name="groupName">Ĭ������</param>
        public AoNameGroupAttribute(string groupName)
        {
            this.GroupName = groupName;

        }
        /// <summary>
        /// ����
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
