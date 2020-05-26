using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Resource
{
    public static class ResourceServiceExtensions
    {
        /// <summary>
        /// 查找第一个资源，并且确认它的类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="resourceService"></param>
        /// <param name="resourceNamePath">资源路径</param>
        /// <returns></returns>
        public static T FindAndEnsure<T>(this IResourceService resourceService,string resourceNamePath)
            where T:class
        {
            if (resourceService.Root == null)
            {
                ThrowHelper.ThrowProjectNotLoad();
            }

            if (string.IsNullOrEmpty(resourceNamePath))
            {
                throw new ArgumentException("message", nameof(resourceNamePath));
            }
            var metadatas = resourceService.Root.Find(metadata => metadata.ResourceNamePath == resourceNamePath, 1);
            if (metadatas.Length == 0)
            {
                ThrowHelper.ThrowResourceNotFound(resourceNamePath);
            }
            var factory = metadatas[0] as T;
            if (factory == null)
            {
                ThrowHelper.ThrowNotImplType<T>(metadatas[0].GetType());
            }
            return factory;
        }
    }
}
