namespace Ao.Command
{
    /// <summary>
    /// 命令标识器
    /// </summary>
    public interface ICommandIdentity
    {
        /// <summary>
        /// 命令前缀
        /// </summary>
        string Prefx { get; }
        /// <summary>
        /// 命令名
        /// </summary>
        string Name { get; }
    }
}
