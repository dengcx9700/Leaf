using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Shared.ForView.Input
{
    /// <summary>
    /// 对<see cref="IStringProvider"/>的扩展
    /// </summary>
    public static class StringProviderExtensions
    {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="stringProvider"></param>
        /// <param name="key">字符串键</param>
        /// <param name="paramters">装入参数</param>
        /// <returns></returns>
        public static string Format(this IStringProvider stringProvider,string key,params object[] paramters)
        {
            if (stringProvider is null)
            {
                throw new ArgumentNullException(nameof(stringProvider));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("message", nameof(key));
            }
            var str = stringProvider.GetString(key);
            if (!string.IsNullOrEmpty(str))
            {
                return string.Format(str, paramters);
            }
            return null;
        }
    }
}
