using Ao.Command.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ao.Command
{
    /// <summary>
    /// 默认的命令执行器
    /// </summary>
    public class DefaultCommandExecuter : ICommandExecuter
    {
        private static readonly ConcurrentDictionary<Type, TypeConverter> typeConverters = new ConcurrentDictionary<Type, TypeConverter>();

        private static readonly DefaultTypeConverter defaultTypeConverter = new DefaultTypeConverter();
        /// <summary>
        /// 初始化<see cref="DefaultCommandExecuter"/>
        /// </summary>
        /// <param name="target">目标实例</param>
        /// <param name="method">方法</param>
        public DefaultCommandExecuter(object target,MethodInfo method)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Invoker = ReflectionHelper.GetInvoker(target, method);            

            var paramInfos = new List<IExecuteParamterInfo>();
            foreach (var item in method.GetParameters())
            {
                var convert = item.GetCustomAttribute<ParamTypeConvertAttribute>();
                TypeConverter typeConverter = null;
                if (convert!=null)
                {
                    typeConverters.GetOrAdd(convert.ConvertType, t =>
                     {
                         return typeConverter=(TypeConverter)Activator.CreateInstance(convert.ConvertType);
                     });
                }
                var alias = item.GetCustomAttributes<AliasAttribute>();
                paramInfos.Add(new ExecuteParamterInfo
                {
                    Optional = item.HasDefaultValue,
                    Default = item.DefaultValue,
                    Type = item.ParameterType,
                    TypeConverter = typeConverter,
                    Alias = new[] { item.Name.ToLower() }.Concat(alias.Select(a => a.Alias.ToLower()))
                                               .Distinct()
                                               .ToArray()
                });
            }
            Prefx = method.GetCustomAttribute<PrefxAttribute>()?.Prefx ??
                    target.GetType().GetCustomAttribute<PrefxAttribute>()?.Prefx ??
                    string.Empty;

            ParamterInfos = paramInfos.ToArray();
            var malias = method.GetCustomAttributes<AliasAttribute>();
            methodAlias = new HashSet<string>(
                new[] { method.Name.ToLower() }
                    .Concat(malias.Select(a => a.Alias))
                    .Distinct());
        }
        private readonly HashSet<string> methodAlias;
        /// <summary>
        /// 调用器
        /// </summary>
        public AoMemberInvoker<object> Invoker { get; }
        /// <summary>
        /// 方法
        /// </summary>
        public MethodInfo Method { get; }
        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ReturnType => Method.ReturnType;
        /// <summary>
        /// 目标对象
        /// </summary>
        public object Target{ get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IExecuteParamterInfo[] ParamterInfos { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Prefx { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyCollection<string> MethodAlias => methodAlias.ToArray();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context"><inheritdoc/></param>
        /// <returns></returns>
        public bool CanExecute(IExecuteContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.Prefx)&&!string.Equals(context.Prefx,Prefx, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(context.Name) || !methodAlias.Contains(context.Name))
            {
                return false;
            }
            return GetExecuteParams(context, true) != null;
        }
        /// <summary>
        /// 获取执行的参数
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <param name="ignoreValue">是否忽略值</param>
        /// <returns></returns>
        protected virtual object[] GetExecuteParams(IExecuteContext context,bool ignoreValue)
        {
            var callParamters = new List<object>();
            var @params = ParamterInfos
#if NETSTANDARD2_0
                .AsSpan()
#endif
                ;
            for (int i = 0; i < @params.Length; i++)
            {
                var param = @params[i];
                var value = param.Default;
                var found = false;
                StringValue stringValue = default(StringValue);
#if NET452
                var alias = param.Alias;
#else
                var alias = param.Alias.Length > 10 ?
                   param.Alias.AsSpan() :
                   param.Alias;
#endif
                //先寻找命名的
                foreach (var item in alias)
                {
                    if (context.TryGetValue(item, out stringValue))
                    {
                        found = true;
                        break;
                    }
                }
                //找不到就找位置的
                if (!found)
                {
                    stringValue = context.GetFromAnonymous(i);
                    found = !stringValue.OriginNullOrWhiteSpace;//输入不会作为空的
                }
                if (!found && !param.Optional)
                {
                    return null;
                }
                if (!ignoreValue)
                {
                    if (found)
                    {
                        var convert = param.TypeConverter ?? defaultTypeConverter;
                        value = convert.ConvertTo(stringValue, param.Type);
                    }
                }
                callParamters.Add(value);
            }
            return callParamters.ToArray();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context"><inheritdoc/></param>
        /// <returns></returns>
        public async Task<IExecuteReuslt> ExecuteAsync(IExecuteContext context)
        {
            var callParamters = GetExecuteParams(context,false);
            if (callParamters==null)
            {
                return new ExecuteReuslt
                {
                    Paramters = null,
                    Result = null,
                    Succeed = false
                };
            }
            var res = Invoker(Target, callParamters.ToArray());
            if (res is Task task)
            {
                await task;
            }
            return new ExecuteReuslt
            {
                Paramters = callParamters,
                Result = res,
                Succeed = true
            };
        }
    }
}
