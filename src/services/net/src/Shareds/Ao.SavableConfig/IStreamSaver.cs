using System.IO;
using System.Threading.Tasks;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 流保存者
    /// </summary>
    public interface IStreamSaver
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="stream">目标流</param>
        /// <returns></returns>
        Task SaveAsync(Stream stream);
    }
}
