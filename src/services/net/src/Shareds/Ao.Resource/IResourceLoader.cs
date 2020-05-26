using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 表示一个资源加载器
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// 加载器排序，越小排得越后,默认0
        /// </summary>
        int Order { get; }
        /// <summary>
        /// 文件后缀名
        /// </summary>
        string ExtensionName { get; }
        /// <summary>
        /// 加载资源，不能加载就返回Null
        /// </summary>
        /// <returns></returns>
        IResourceMetadata Load(ResourceLoadContext context);
    }
}
