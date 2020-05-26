using System.Collections.Generic;

namespace Ao.Command
{
    /// <summary>
    /// 实现了<see cref="ICommandManager"/>的命令管理器
    /// </summary>
    public class CommandManager : List<ICommandSource>, ICommandManager
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public ICommander Build(ISplitIdentity splitIdentity)
        {
            return new DefaultCommander(ToArray(), splitIdentity);
        }
        /// <summary>
        /// 从默认的分割器<see cref="SplitIdentity.Default"/>生成命令器
        /// </summary>
        /// <returns></returns>
        public ICommander BuildDefault()
        {
            return DefaultCommander.FromDefaultSplit(ToArray());
        }
    }
}
