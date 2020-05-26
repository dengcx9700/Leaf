using System;
using System.Collections;
namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示当前已知的类型
    /// </summary>
    public static class KnowTypes
    {
        /// <summary>
        /// 枚举类型<see cref="IEnumerable"/>
        /// </summary>
        public static readonly Type EnumerableType = typeof(IEnumerable);
        /// <summary>
        /// 字典类型<see cref="IDictionary"/>
        /// </summary>
        public static readonly Type IDictionaryType = typeof(IDictionary);
        /// <summary>
        /// 集合类型<see cref="ICollection"/>
        /// </summary>
        public static readonly Type CollectionType = typeof(ICollection);
        /// <summary>
        /// 值类型
        /// </summary>
        public static readonly Type[] ValueTypes = {typeof(sbyte), typeof(short), typeof(int),
                             typeof(long), typeof(byte), typeof(ushort),
                             typeof(uint), typeof(ulong), typeof(decimal),
                             typeof(float), typeof(double), typeof(string) };
        /// <summary>
        /// 获取一个值，指示此类型是否为枚举类型
        /// </summary>
        /// <param name="type">目标类型，此参数不能为null</param>
        /// <returns></returns>
        public static bool IsEmunerable(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsArray || type.GetInterface(EnumerableType.FullName) != null
                &&type.GetInterface(IDictionaryType.FullName)==null;
        }
        /// <summary>
        /// 获取一个值，指示此类型是否是字典类型
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <returns></returns>
        public static bool IsDictionay(Type type)
        {
            return type.GetInterface(IDictionaryType.FullName) != null;
        }
    }
}
