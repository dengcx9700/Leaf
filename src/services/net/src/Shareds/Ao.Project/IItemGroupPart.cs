using System.Threading.Tasks;

namespace Ao.Project
{
    /// <summary>
    /// 表示项组部分
    /// </summary>
    public interface IItemGroupPart
    {
        /// <summary>
        /// 表示实施装饰
        /// </summary>
        Task ConductAsync();
    }
}
