using System;
using System.Diagnostics.CodeAnalysis;

namespace Leaf.Data
{
    /// <summary>
    /// <inheritdoc cref="IHitTrigger"/>
    /// </summary>
    public class HitTrigger:IComparable<HitTrigger>
    {
        /// <summary>
        /// 需要命中的次数
        /// </summary>
        public int NeedHitCount { get; set; }
        /// <summary>
        /// 命中达成后增加的时间
        /// </summary>
        public long IncMillSec { get; set; }

        public int CompareTo([AllowNull] HitTrigger other)
        {
            if (other==null)
            {
                return 1;
            }
            return NeedHitCount - other.NeedHitCount;
        }
    }
}
