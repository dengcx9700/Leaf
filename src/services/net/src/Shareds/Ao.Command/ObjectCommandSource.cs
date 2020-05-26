using Ao.Command.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ao.Command
{
    /// <summary>
    /// 实现了<see cref="ICommandSource"/>的命令源
    /// </summary>
    public class ObjectCommandSource : ICommandSource
    {
        /// <summary>
        /// <see cref="object"/>含有的方法
        /// </summary>
        public static readonly string[] ObjectMethods = 
            typeof(object).GetMembers().Select(m => m.Name).ToArray();
        /// <summary>
        /// 初始化<see cref="ObjectCommandSource"/>
        /// </summary>
        /// <param name="target"><inheritdoc cref="Target"/></param>
        /// <param name="ignoreMethodNames"><inheritdoc cref="IgnoreMethodNames"/></param>
        public ObjectCommandSource(object target,params string[] ignoreMethodNames)
        {
            Target = target;
            this.ignoreMethodNames = new HashSet<string>(ignoreMethodNames);
        }
        private readonly HashSet<string> ignoreMethodNames;
        /// <summary>
        /// 目标对象
        /// </summary>
        public object Target { get; }
        /// <summary>
        /// 忽略的方法名称
        /// </summary>
        public IReadOnlyCollection<string> IgnoreMethodNames => ignoreMethodNames.ToArray();
        /// <summary>
        /// 获取命令执行器
        /// </summary>
        /// <returns></returns>
        public ICommandExecuter[] GetCommandExecuters()
        {
            return Target.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => !IgnoreMethodNames.Contains(m.Name) && m.GetCustomAttribute<NotCommandAttribute>() == null)
                .Select(m => new DefaultCommandExecuter(Target, m))
                .ToArray();
        }
        /// <summary>
        /// 生成<see cref="ObjectCommandSource"/>并且添加<see cref="ObjectMethods"/>忽略方法
        /// </summary>
        /// <param name="target"><inheritdoc cref="Target"/></param>
        /// <returns></returns>
        public static ObjectCommandSource FromObjectIgnore(object target)
        {
            return new ObjectCommandSource(target, ObjectMethods);
        }
    }
}
