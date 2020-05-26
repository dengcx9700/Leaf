using System.Threading.Tasks;

namespace Ao.Command
{
    /// <summary>
    /// 对<see cref="ICommander"/>的扩展
    /// </summary>
    public static class CommanderExtensions
    {
        /// <summary>
        /// 获取<see cref="ICommandExecuter"/>从命令
        /// </summary>
        /// <param name="commander"></param>
        /// <param name="commandSource">命令源</param>
        /// <param name="context">返回的上下文</param>
        /// <returns></returns>
        public static ICommandExecuter GetCommandExecuter(this ICommander commander, string commandSource,out IExecuteContext context)
        {
            if (commander is null)
            {
                throw new System.ArgumentNullException(nameof(commander));
            }

            context = commander.GetContext(commandSource);
            if (context!=null)
            {
                return commander.GetExecuter(context);
            }
            return null;
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commander"></param>
        /// <param name="commandSource">命令源</param>
        /// <returns></returns>
        public static Task<IExecuteReuslt> ExecuteCommandAsync(this ICommander commander, string commandSource)
        {
            if (commander is null)
            {
                throw new System.ArgumentNullException(nameof(commander));
            }

            var executer = commander.GetCommandExecuter(commandSource,out var context);
            if (executer!=null)
            {
                return executer.ExecuteAsync(context);
            }
            return Task.FromResult<IExecuteReuslt>(null);
        }
    }
}
