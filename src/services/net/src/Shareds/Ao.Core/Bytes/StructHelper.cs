using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ao.Core.Bytes
{
    public static class StructHelper
    {
        /// <summary>
        /// 将结构体转为Byte数组
        /// </summary>
        /// <param name="value">目标结构体</param>
        /// <returns></returns>
        public static byte[] ToByes(object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!value.GetType().IsValueType)
            {
                throw new ArgumentNullException("类型不是值类型");
            }
            var size = Marshal.SizeOf(value);
            var buff = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(value, buff, false);
                var bytes = new byte[size];
                Marshal.Copy(buff, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buff);
            }
        }
        /// <summary>
        /// 将byte数组转为结构体
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <param name="type">转为的类型</param>
        /// <returns></returns>
        public static object ToStruce(byte[] bytes, Type type)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsValueType)
            {
                throw new ArgumentNullException("类型不是值类型");
            }
            var size = Marshal.SizeOf(type);
            var buff = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buff, size);

                return Marshal.PtrToStructure(buff, type);
            }
            finally
            {
                Marshal.FreeHGlobal(buff);
            }
        }
        /// <summary>
        /// <inheritdoc cref="ToStruce(byte[], Type)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T ToStruce<T>(byte[] bytes)
            where T:struct
        {
            return (T)ToStruce(bytes, typeof(T));
        }
    }
}
