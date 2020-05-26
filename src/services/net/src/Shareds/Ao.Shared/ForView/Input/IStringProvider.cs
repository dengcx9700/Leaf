using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Ao.Shared.ForView.Input
{
    /// <summary>
    /// 表示字符串提供者
    /// </summary>
    public interface IStringProvider
    {
        /// <summary>
        /// 表示字符串提供者的时区
        /// </summary>
        CultureInfo CultureInfo { get; }
        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string GetString(string key);
    }
}
