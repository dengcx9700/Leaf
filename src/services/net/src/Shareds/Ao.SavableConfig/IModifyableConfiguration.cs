using System.Collections.Generic;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示可保持的配置
    /// </summary>
    public interface IModifyableConfiguration
    {
        /// <summary>
        /// 改变了的数据
        /// </summary>
        IReadOnlyDictionary<string, string> Changes { get; }
        /// <summary>
        /// 加入修改
        /// </summary>
        /// <param name="path">设置路径</param>
        /// <param name="value">新值</param>
        void AddChange(string path, string value);
    }
}
