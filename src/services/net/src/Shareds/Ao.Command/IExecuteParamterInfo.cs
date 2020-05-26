using System;
using System.ComponentModel;

namespace Ao.Command
{
    /// <summary>
    /// 表示执行命令的参数信息
    /// </summary>
    public interface IExecuteParamterInfo
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// 类型转换器
        /// </summary>
        TypeConverter TypeConverter { get; }
        /// <summary>
        /// 别名
        /// </summary>
        string[] Alias { get; }
        /// <summary>
        /// 默认值
        /// </summary>
        object Default { get; }
        /// <summary>
        /// 是否可选的
        /// </summary>
        bool Optional { get; }
    }
}
