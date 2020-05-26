using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Core
{
    public static class TimeHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimestamp(DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000; //除10000调整为13位
            return t;
        }
    }
}
