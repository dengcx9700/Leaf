using System;
using System.Linq;

namespace Ao.Plug
{
    /// <summary>
    /// 对类型<see cref="IPlugLookup"/>的扩展
    /// </summary>
    public static class PlugLookupExtensions
    {
        /// <summary>
        /// 从类型获取所有类型实体
        /// </summary>
        /// <param name="plugLookup"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static ITypeEntity[] Gets(this IPlugLookup plugLookup, Type targetType)
        {
            if (plugLookup is null)
            {
                throw new ArgumentNullException(nameof(plugLookup));
            }

            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            return plugLookup.Gets(t => t.TargetType.IsEquivalentTo(targetType));
        }
        /// <summary>
        /// <inheritdoc cref="Gets(IPlugLookup, Type)"/>
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="plugLookup"></param>
        /// <returns></returns>
        public static ITypeEntity[] Gets<T>(this IPlugLookup plugLookup)
        {
            return plugLookup.Gets(typeof(T));
        }
        private static readonly Func<ITypeEntity, object[]> defaultParamterGetter = _ =>
#if NET452
        new object[0]
#else
        Array.Empty<object>()
#endif
        ;
        /// <summary>
        /// 获取并生成符合条件的实例
        /// </summary>
        /// <param name="plugLookup"></param>
        /// <param name="condition"></param>
        /// <param name="paramterGetter"></param>
        /// <returns></returns>
        public static object[] GetInstances(this IPlugLookup plugLookup,Predicate<ITypeEntity> condition,Func<ITypeEntity,object[]> paramterGetter)
        {
            if (plugLookup is null)
            {
                throw new ArgumentNullException(nameof(plugLookup));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            paramterGetter = paramterGetter??defaultParamterGetter;
            return plugLookup.Gets(condition).Select(x => x.Make(paramterGetter(x))).ToArray();
        }
        /// <summary>
        /// <inheritdoc cref="GetInstances(IPlugLookup, Predicate{ITypeEntity}, Func{ITypeEntity, object[]})"/>
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="plugLookup"></param>
        /// <param name="condition">条件</param>
        /// <param name="paramterGetter">参数获取器</param>
        /// <returns></returns>
        public static T[] GetInstances<T>(this IPlugLookup plugLookup, Predicate<ITypeEntity> condition, Func<ITypeEntity, object[]> paramterGetter)
        {
            return plugLookup.GetInstances(condition, paramterGetter).OfType<T>().ToArray();
        }
        /// <summary>
        /// <inheritdoc cref="GetInstances(IPlugLookup, Predicate{ITypeEntity}, Func{ITypeEntity, object[]})"/>
        /// </summary>
        /// <param name="plugLookup"></param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static object[] GetInstances(this IPlugLookup plugLookup, Predicate<ITypeEntity> condition)
        {
            return plugLookup.GetInstances(condition,null);
        }
        /// <summary>
        /// <inheritdoc cref="GetInstances(IPlugLookup, Predicate{ITypeEntity}, Func{ITypeEntity, object[]})"/>
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="plugLookup"></param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T[] GetInstances<T>(this IPlugLookup plugLookup, Predicate<ITypeEntity> condition)
        {
            return plugLookup.GetInstances(condition).OfType<T>().ToArray();
        }

    }
}
