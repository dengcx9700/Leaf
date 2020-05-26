using System.Collections.Generic;

namespace Ao.Command
{
    /// <summary>
    /// 表示执行上下文
    /// </summary>
    public interface IExecuteContext : ICommandIdentity, IPositionable<StringValue>, IReadOnlyDictionary<string, StringValue>, IEnumerable<KeyValuePair<string, StringValue>>
    {
        /// <summary>
        /// 命令源
        /// </summary>
        string CommandSource { get; }
        /// <summary>
        /// 分割标识器
        /// </summary>
        ISplitIdentity Spliter { get; }
        /// <summary>
        /// 获取值从匿名参数
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns></returns>
        StringValue GetFromAnonymous(int index);
    }
}
