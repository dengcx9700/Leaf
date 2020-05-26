namespace Ao.Command
{
    /// <summary>
    /// 命令源
    /// </summary>
    public interface ICommandSource
    {
        /// <summary>
        /// 获取命令器
        /// </summary>
        /// <returns></returns>
        ICommandExecuter[] GetCommandExecuters();
    }
}
