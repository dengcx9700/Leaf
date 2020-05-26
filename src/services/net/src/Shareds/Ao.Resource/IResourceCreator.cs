using System.IO;
using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 表示资源创建器
    /// </summary>
    public interface IResourceCreator
    {
        /// <summary>
        /// 创建资源
        /// </summary>
        /// <param name="file">目标文件</param>
        /// <returns></returns>
        Task CreateAsync(FileInfo file);
    }
}
