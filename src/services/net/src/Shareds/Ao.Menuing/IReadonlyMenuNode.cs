using System.Collections.Generic;

namespace Pd.Services.Menu
{
    /// <summary>
    /// 表示只读的菜单节点
    /// </summary>
    public interface IReadonlyMenuNode
    {
        /// <summary>
        /// 表示此节点是否根节点
        /// </summary>
        bool IsRoot { get; }
        /// <summary>
        /// 菜单元数据
        /// </summary>
        IMenuMetadata Metadata { get; }
        /// <summary>
        /// 节点父亲
        /// </summary>
        IMenuNode Parent { get; }
        /// <summary>
        /// 表示此节点的路径
        /// </summary>
        string Path { get; }
        /// <summary>
        /// 表示此节点的路径部分
        /// </summary>
        string[] PathPart { get; }
        /// <summary>
        /// 根据路径寻找节点,路径分隔符<see cref="PathSpliter"/>
        /// </summary>
        /// <param name="path">目标路径</param>
        /// <returns></returns>
        IMenuNode[] Find(string path);
        /// <summary>
        /// 表示节点被加入
        /// </summary>
        event NodeActionHandle NodeAdded;
        /// <summary>
        /// 表示节点被移除
        /// </summary>
        event NodeActionHandle NodeRemoved;
        /// <summary>
        /// 下一些节点
        /// </summary>
        IReadOnlyCollection<IMenuNode> Nexts { get; }
    }
}