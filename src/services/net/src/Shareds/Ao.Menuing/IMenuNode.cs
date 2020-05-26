using System.Collections.ObjectModel;

namespace Pd.Services.Menu
{
    /// <summary>
    /// 表示菜单节点
    /// </summary>
    public interface IMenuNode : IReadonlyMenuNode
    {
        /// <summary>
        /// <inheritdoc cref="IReadonlyMenuNode.Nexts"/>
        /// </summary>
        new ObservableCollection<IMenuNode> Nexts { get; }
    }
}