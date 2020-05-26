namespace Ao.Command
{
    /// <summary>
    /// 命令解析者
    /// </summary>
    public interface ICommander
    {
        /// <summary>
        /// 命令执行器
        /// </summary>
        ICommandExecuter[] CommandExecuters { get; }
        /// <summary>
        /// 分割器
        /// </summary>
        ISplitIdentity Spliter { get; }
        /// <summary>
        /// 从命令源获取命令执行器
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <returns></returns>
        ICommandExecuter GetExecuter(IExecuteContext context);
        /// <summary>
        /// 从命令源获取执行上下文
        /// </summary>
        /// <param name="commandSource">命令源</param>
        /// <returns></returns>
        IExecuteContext GetContext(string commandSource);
    }
}
