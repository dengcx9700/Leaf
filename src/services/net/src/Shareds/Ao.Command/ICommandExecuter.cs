using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ao.Command
{
    /// <summary>
    /// 命令执行器
    /// </summary>
    public interface ICommandExecuter : ICommandIdentity
    {
        /// <summary>
        /// 命令参数列表
        /// </summary>
        IExecuteParamterInfo[] ParamterInfos { get; }
        /// <summary>
        /// 方法别名
        /// </summary>
        IReadOnlyCollection<string> MethodAlias { get; }
        /// <summary>
        /// 返回一个值，表示执行上下文指示的命令是否可以被执行
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <returns></returns>
        bool CanExecute(IExecuteContext context);
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <returns></returns>
        Task<IExecuteReuslt> ExecuteAsync(IExecuteContext context);
    }
}
