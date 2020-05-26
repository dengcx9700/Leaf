using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Flow
{
    /// <summary>
    /// 数据视图
    /// </summary>
    public interface IDataView
    {
        /// <summary>
        /// 数据值
        /// </summary>
        object Value { get; }
    }
    /// <summary>
    /// 强类型的数据视图
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataView<T> : IDataView
    {
        /// <summary>
        /// 强类型的数据值
        /// </summary>
        new T Value { get; }
    }
}
