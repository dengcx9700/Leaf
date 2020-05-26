#pragma warning disable IDE0034 // 简化 "default" 表达式
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Command
{
    /// <summary>
    /// 命令上下文, 参数会根据顺序定位
    /// </summary>
    /// <example>
    /// prefxSplit = ':'
    /// paramterSplit = ' '
    /// expresionSplit = '='
    /// commandSource = "ex:Execute hello say=yyy"
    /// 会获取到命令Execute 参数1:hello 参数2:yyy
    /// </example>
    public class ExecuteContext : Dictionary<string, StringValue>, IExecuteContext
    {
        /// <summary>
        /// 初始化<see cref="ExecuteContext"/>
        /// </summary>
        /// <param name="commandSource">命令源</param>
        public ExecuteContext(string commandSource, ISplitIdentity splitIdentity)
        {
            CommandSource = commandSource ?? string.Empty;
            Spliter = splitIdentity ?? throw new ArgumentNullException(nameof(splitIdentity));
            stringValues = new List<StringValue>();
            anonymousValues = new List<StringValue>();
            Parse();
        }
        protected readonly List<StringValue> anonymousValues;
        protected readonly List<StringValue> stringValues;
        protected string prefx;
        protected string name;
        protected bool parseSucceed;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string CommandSource { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Prefx => prefx;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name => name;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ISplitIdentity Spliter { get; }
        /// <summary>
        /// 解析是否成功
        /// </summary>
        public bool ParseSucceed => parseSucceed;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns></returns>
        public StringValue this[int index] =>Get(index);
        /// <summary>
        /// 解析命令
        /// </summary>
        protected virtual void Parse()
        {
            if (string.IsNullOrEmpty(CommandSource))
            {
                prefx = name = string.Empty;
                return;
            }
            var prefxAndCommand = CommandSource.Split(Spliter.PrefxSplit);
            if (prefxAndCommand.Length == 2)
            {
                prefx = prefxAndCommand[0];
            }
            var exp = prefxAndCommand.Last().Split(new[] { Spliter.ParamterSplit }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            name = exp.FirstOrDefault();
            ParseParamter(exp.Skip(1).ToArray());
            parseSucceed = true;
        }
        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="paramterExpression"></param>
        protected virtual void ParseParamter(string[] paramters)
        {
            foreach (var item in paramters
#if NETSTANDARD2_0
                .AsSpan()
#endif
                )
            {
                var nameAndValue=item.Split(Spliter.ExpressionSplit);
                if (nameAndValue.Length == 2)
                {
                    var name = nameAndValue[0];
                    var value = new StringValue(name, nameAndValue[1]);
                    this[name] = value;
                    stringValues.Add(value);
                }
                else if (!string.IsNullOrWhiteSpace(nameAndValue[0]))
                {
                    var value = new StringValue(null, nameAndValue[0]);
                    anonymousValues.Add(value);
                    stringValues.Add(value);
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>,如果获取失败，返回默认<see cref="StringValue"/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StringValue Get(int index)
        {
            if (stringValues.Count>index)
            {
                return stringValues[index];
            }
            return default(StringValue);
        }

        public StringValue GetFromAnonymous(int index)
        {
            if (anonymousValues.Count>index)
            {
                return anonymousValues[index];
            }
            return default(StringValue);
        }
        /// <summary>
        /// 从默认的分割器创建执行上下文
        /// </summary>
        /// <param name="commandSource">命令源</param>
        /// <returns></returns>
        public static ExecuteContext FromDefault(string commandSource)
        {
            return new ExecuteContext(commandSource,SplitIdentity.Default);
        }
    }
}
#pragma warning restore IDE0034 // 简化 "default" 表达式
