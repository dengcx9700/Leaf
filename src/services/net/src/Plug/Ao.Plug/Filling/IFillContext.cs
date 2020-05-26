using System.Collections;
using System.Reflection;

namespace Ao.Plug.Filling
{
    public interface IFillContext
    {
        /// <summary>
        /// 目标的属性项
        /// </summary>
        PropertyInfo Property { get; }
        /// <summary>
        /// 填充对象
        /// </summary>
        object Target { get; }
        /// <summary>
        /// 填充的集合
        /// </summary>
        IList TargetList { get; }
        /// <summary>
        /// 填充的值集合
        /// </summary>
        object[] Values { get; }
    }
}