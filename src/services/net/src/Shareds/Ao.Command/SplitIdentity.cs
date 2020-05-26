#pragma warning disable IDE0034 // 简化 "default" 表达式

namespace Ao.Command
{
    /// <summary>
    /// 实现了<see cref="ISplitIdentity"/>的分割标识器
    /// </summary>
    public class SplitIdentity : ISplitIdentity
    {
        public static readonly char DefaultPrefxSplit = ':';
        public static readonly char DefaultParamterSplit = ' ';
        public static readonly char DefaultExpressionSplit = '=';
        /// <summary>
        /// 默认的标识
        /// </summary>
        public static readonly ISplitIdentity Default = new SplitIdentity(DefaultPrefxSplit, DefaultParamterSplit, DefaultExpressionSplit);
        /// <summary>
        /// 初始化<see cref="SplitIdentity"/>
        /// </summary>
        /// <param name="prefxSplit">前缀分割器</param>
        /// <param name="paramterSplit">参数分割器</param>
        /// <param name="expresionSplit">表达式分割器</param>
        public SplitIdentity(char prefxSplit, char paramterSplit, char expressionSplit)
        {
            PrefxSplit = prefxSplit;
            ParamterSplit = paramterSplit;
            ExpressionSplit = expressionSplit;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public char PrefxSplit { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public char ParamterSplit { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public char ExpressionSplit { get; }
    }
}
#pragma warning restore IDE0034 // 简化 "default" 表达式
