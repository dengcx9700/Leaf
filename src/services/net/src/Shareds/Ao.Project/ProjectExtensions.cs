using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ao.Project
{
    public static class ProjectExtensions
    {
        /// <summary>
        /// 确保获取工程的元数据，如果获取失败抛出异常
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="project"></param>
        /// <param name="key">键</param>
        /// <returns></returns>
        /// <exception cref="NotFoundProjectPartException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public static T EnsureGetMetadata<T>(this IProject project,string key)
        {
            if (!project.Metadatas.ContainsKey(key))
            {
                ThrowNotFound(key);
            }
            return (T)project.Metadatas[key];
        }
        private static void ThrowNotFound(string key)
        {
            var str = $"未能找到工程部件键{key}";

            throw new Exception(str);
        }
        /// <summary>
        /// 确保获取工程的特性，如果获取失败抛出异常
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="project"></param>
        /// <param name="key">键</param>
        /// <returns></returns>
        /// <exception cref="NotFoundProjectPartException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public static T EnsureGetFeature<T>(this IProject project, string key)
        {
            if (!project.Features.ContainsKey(key))
            {
                ThrowNotFound(key);
            }
            return (T)project.Features[key];
        }
        /// <summary>
        /// 寻找符合的<see cref="ItemGroupPart"/>
        /// </summary>
        /// <param name="project"></param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static IEnumerable<ItemGroupPart> FindItemGroupParts(this IProject project,Func<ItemGroupPart,bool> condition)
        {
            return project.ItemGroups.SelectMany(g => g.Items.Where(condition));
        }
        /// <summary>
        /// 寻找相同类型的<see cref="ItemGroupPart"/>
        /// </summary>
        /// <param name="project"></param>
        /// <param name="type">目标类型</param>
        /// <returns></returns>
        public static IEnumerable<ItemGroupPart> FindItemGroupParts(this IProject project,Type type)
        {
            return project.FindItemGroupParts(p => p.GetType().IsEquivalentTo(type));
        }
        /// <summary>
        /// <inheritdoc cref="FindItemGroupParts(IProject, Type)"/>
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="project"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindItemGroupParts<T>(this IProject project)
            where T:ItemGroupPart
        {
            return project.FindItemGroupParts(typeof(T)).OfType<T>();
        }
    }
}
