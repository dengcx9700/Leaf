using System;

namespace Ao.Command
{
    /// <summary>
    /// 表示字符串值
    /// </summary>
    public struct StringValue : IEquatable<StringValue>, IFormatProvider
    {
        private readonly Lazy<long?> number;
        private readonly Lazy<ulong?> unumber;
        private readonly Lazy<decimal?> single;
        /// <summary>
        /// 初始化<see cref="StringValue"/>
        /// </summary>
        /// <param name="origin"></param>
        public StringValue(string name,string origin)
        {
            Name = name;
            Origin = origin;
            number = new Lazy<long?>(()=>CaseNumber(origin), true);
            unumber = new Lazy<ulong?>(() => CaseUNumber(origin), true);
            single = new Lazy<decimal?>(() => CaseSingle(origin), true);
        }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 源文本
        /// </summary>
        public string Origin;
        /// <summary>
        /// <see cref="Origin"/>是否<see cref="null"/>或者空串
        /// </summary>
        public bool OriginNullOrEmpty => string.IsNullOrEmpty(Origin);
        /// <summary>
        /// <see cref="Origin"/>是否<see cref="null"/>或空白
        /// </summary>
        public bool OriginNullOrWhiteSpace => string.IsNullOrWhiteSpace(Origin);
        /// <summary>
        /// 转为有符号数值
        /// </summary>
        public long? Long => number.Value;
        /// <summary>
        /// 转为无符号数值
        /// </summary>
        public ulong? ULong => unumber.Value;
        /// <summary>
        /// 转为浮点数
        /// </summary>
        public decimal? Decimal => single.Value;

        private static ulong? CaseUNumber(string origin)
        {
            if (ulong.TryParse(origin, out var value))
            {
                return value;
            }
            return null;
        }
        private static decimal? CaseSingle(string origin)
        {
            if (decimal.TryParse(origin, out var value))
            {
                return value;
            }
            return null;
        }
        private static long? CaseNumber(string origin)
        {
            if (long.TryParse(origin, out var value))
            {
                return value;
            }
            return null;
        }
        public static implicit operator byte(StringValue value)
        {
            return (byte)value.Long;
        }
        public static implicit operator short(StringValue value)
        {
            return (short)value.Long;
        }
        public static implicit operator int(StringValue value)
        {
            return (int)value.Long;
        }
        public static implicit operator long(StringValue value)
        {
            return (long)value.Long;
        }
        public static implicit operator ushort(StringValue value)
        {
            return (ushort)value.ULong;
        }
        public static implicit operator uint(StringValue value)
        {
            return (uint)value.ULong;
        }
        public static implicit operator ulong(StringValue value)
        {
            return (ulong)value.ULong;
        }
        public static implicit operator float(StringValue value)
        {
            return (float)value.Decimal;
        }
        public static implicit operator double(StringValue value)
        {
            return (double)value.Decimal;
        }
        public static implicit operator decimal(StringValue value)
        {
            return (decimal)value.Decimal;
        }


        public static implicit operator byte?(StringValue value)
        {
            return (byte?)value.Long;
        }
        public static implicit operator short?(StringValue value)
        {
            return (short?)value.Long;
        }
        public static implicit operator int?(StringValue value)
        {
            return (int?)value.Long;
        }
        public static implicit operator long?(StringValue value)
        {
            return value.Long;
        }
        public static implicit operator ushort?(StringValue value)
        {
            return (ushort?)value.ULong;
        }
        public static implicit operator uint?(StringValue value)
        {
            return (uint?)value.ULong;
        }
        public static implicit operator ulong?(StringValue value)
        {
            return value.ULong;
        }
        public static implicit operator float?(StringValue value)
        {
            return (float?)value.Decimal;
        }
        public static implicit operator double?(StringValue value)
        {
            return (double?)value.Decimal;
        }
        public static implicit operator decimal?(StringValue value)
        {
            return value.Decimal;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Origin;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Origin?.GetHashCode() ?? 0;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is null||!obj.GetType().IsEquivalentTo(typeof(StringValue)))
            {
                return false;
            }
            return Equals((StringValue)obj);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"><inheritdoc/></param>
        /// <returns></returns>
        public bool Equals(StringValue other)
        {
            return other.Origin == Origin;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="formatType"><inheritdoc/></param>
        /// <returns></returns>
        public object GetFormat(Type formatType)
        {
            return Convert.ChangeType(Origin, formatType);
        }
    }
}
