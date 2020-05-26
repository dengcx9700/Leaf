#pragma warning disable IDE0034 // 简化 "default" 表达式

namespace Ao.Command
{
    /// <summary>
    /// 表示分割标制
    /// </summary>
    public interface ISplitIdentity
    {
        /// <summary>
        /// 前缀分割符
        /// </summary>
        char PrefxSplit { get; }
        /// <summary>
        /// 参数分割符
        /// </summary>
        char ParamterSplit { get; }
        /// <summary>
        /// 表达式分析器
        /// </summary>
        char ExpressionSplit { get; }
    }
}
#pragma warning restore IDE0034 // 简化 "default" 表达式
