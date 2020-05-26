using System.Globalization;

namespace Ao.Lang
{
    /// <summary>
    /// 表示文化标识器
    /// </summary>
    public interface ICultureIdentity
    {
        /// <summary>
        /// 语言
        /// </summary>
        CultureInfo Culture { get; }
    }
}
