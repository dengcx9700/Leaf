using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Ao.Command
{
    /// <summary>
    /// 命令管理器
    /// </summary>
    public interface ICommandManager : IList<ICommandSource>, IEnumerable<ICommandSource>
    {
        /// <summary>
        /// 生成命令器
        /// </summary>
        /// <returns></returns>
        ICommander Build(ISplitIdentity splitIdentity);
    }
}
