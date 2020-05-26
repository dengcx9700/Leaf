using System;
using System.Linq;

namespace Ao.Command
{
    /// <summary>
    /// 默认的命令器
    /// </summary>
    public class DefaultCommander : ICommander
    {
        /// <summary>
        /// 初始化<see cref="DefaultCommander"/>
        /// </summary>
        /// <param name="commandSources">命令源</param>
        /// <param name="spliter">分割器</param>
        public DefaultCommander(ICommandSource[] commandSources, ISplitIdentity spliter)
        {
            if (commandSources is null)
            {
                throw new ArgumentNullException(nameof(commandSources));
            }

            Spliter = spliter ?? throw new System.ArgumentNullException(nameof(spliter));
            CommandExecuters = commandSources.SelectMany(c => c.GetCommandExecuters()).ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICommandExecuter[] CommandExecuters { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISplitIdentity Spliter { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="commandSource"><inheritdoc/></param>
        /// <returns></returns>
        public IExecuteContext GetContext(string commandSource)
        {
            return new ExecuteContext(commandSource, Spliter);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context"><inheritdoc/></param>
        /// <returns></returns>
        public ICommandExecuter GetExecuter(IExecuteContext context)
        {
            var executers = CommandExecuters
#if NETSTANDARD2_0
            .AsSpan()

#endif
            ;

            foreach (var item in CommandExecuters)
            {
                if (item.CanExecute(context))
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 从默认的<see cref="SplitIdentity.Default"/>创建<see cref="DefaultCommander"/>
        /// </summary>
        /// <param name="commandSources">命令源</param>
        /// <returns></returns>
        public static DefaultCommander FromDefaultSplit(ICommandSource[] commandSources)
        {
            return new DefaultCommander(commandSources, SplitIdentity.Default);
        }
    }
}
