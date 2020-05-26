using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Pd.Services.Menu
{
    /// <summary>
    /// 菜单的元数据
    /// </summary>
    public interface IMenuMetadata : INotifyPropertyChanged
    {
        /// <summary>
        /// 关键id，寻找时会根据此id获取,此属性不能为null
        /// <para>
        /// 如果id相同，则寻找适合会分叉寻找
        /// </para>
        /// </summary>
        string Id { get; }
        /// <summary>
        /// 插入的路径,会根据{id1}/{id2}..../{idn}的路径寻找方式插入,如果什么都没有，则插入到根，如果未能找到目标路径
        /// <para>
        /// 1.如果<see cref="IsCompromise"/>为true，则插入杂项菜单
        /// </para>
        /// <para>
        /// 2.如果<see cref="IsCompromise"/>为false,则调用<see cref="InsertFailed"/>
        /// </para>
        /// </summary>
        string InsertPath { get; }
        /// <summary>
        /// 插入的方式
        /// </summary>
        MenuActionTypes ActionType { get; }
        /// <summary>
        /// 此属性表示此菜单被替换时候是否需要请求
        /// <para>
        /// 如果此属性为true，则当有其它菜单元数据想请求替换的适合，会调用<see cref="ConditionReplace"/>来指示是否可以被替换
        /// </para>
        /// </summary>
        bool RequestReplace { get; }
        /// <summary>
        /// 表示此菜单元数据是否是分隔符
        /// </summary>
        bool IsSeparator { get; }
        /// <summary>
        /// 此菜单的标题
        /// </summary>
        string Title { get; }
        /// <summary>
        /// 此菜单的描述
        /// </summary>
        string Descript { get; }
        /// <summary>
        /// 此菜单的命令
        /// </summary>
        ICommand Command { get; }
        /// <summary>
        /// 插入失败，并且不妥协
        /// </summary>
        /// <param name="result">测试的结果</param>
        void InsertFailed(ActionTestResult result);
        /// <summary>
        /// 是否可以被替换
        /// </summary>
        bool ConditionReplace();
        /// <summary>
        /// 替换失败
        /// </summary>
        /// <param name="result">测试的结果</param>
        void ReplaceFailed(ActionTestResult result);
    }
}
