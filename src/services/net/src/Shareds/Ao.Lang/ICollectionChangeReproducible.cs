using System.Collections.Generic;

namespace Ao.Lang
{
    /// <summary>
    /// 表示可以集合改变重建
    /// </summary>
    /// <typeparam name="T">目标集合</typeparam>
    public interface ICollectionChangeReproducible: IReproducible
    {
        /// <summary>
        /// 重建如果集合改变了
        /// </summary>
        bool ReBuildIfCollectionChanged { get; set; }
    }

}
