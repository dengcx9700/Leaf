using Ao.Core;
using System.Windows;
using System.Windows.Input;

namespace Pd.Services.Menu
{
    /// <summary>
    /// 菜单元数据的基类
    /// <para>
    /// <inheritdoc cref="IMenuMetadata"/>
    /// </para>
    /// </summary>
    public abstract class MenuMetadataBase : NotifyableObject, IMenuMetadata
    {
        public MenuMetadataBase()
        {
            ActionType = MenuActionTypes.InnerAfter;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string InsertPath { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public MenuActionTypes ActionType { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool RequestReplace { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsSeparator { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Descript { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICommand Command { get; protected set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public virtual bool ConditionReplace()
        {
            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="result"> <inheritdoc/></param>
        public virtual void InsertFailed(ActionTestResult result)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="result"> <inheritdoc/></param>
        public virtual void ReplaceFailed(ActionTestResult result)
        {
        }
    }
}
