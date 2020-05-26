using System;
using System.ComponentModel;

namespace Ao.Command
{
    /// <summary>
    /// 实现了<see cref="IExecuteParamterInfo"/>
    /// </summary>
    public class ExecuteParamterInfo : IExecuteParamterInfo
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 类型转换器
        /// </summary>
        public TypeConverter TypeConverter { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object Default { get; set; }
        /// <summary>
        /// 是否可选的
        /// </summary>
        public bool Optional { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string[] Alias { get; set; }
    }
}
