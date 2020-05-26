using System;

namespace Ao.Core
{
    /// <summary>
    /// 表示元数据信息
    /// </summary>
    public interface IMedataInfo: IIdentitiyable
    {
        /// <summary>
        /// 当前版本
        /// </summary>
        Version Version { get; }
    }
}
