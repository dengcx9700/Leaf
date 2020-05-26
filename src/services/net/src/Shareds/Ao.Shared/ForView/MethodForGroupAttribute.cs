using System;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示一个组名创建器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class MethodForGroupAttribute : GroupAttribute
    {
        /// <summary>
        /// 初始化<see cref="MethodForGroupAttribute"/>
        /// </summary>
        /// <param name="methodName"><inheritdoc cref="MethodName"/></param>
        /// <param name="key"><inheritdoc cref="Key"/></param>
        public MethodForGroupAttribute(string methodName, string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
        }
        private AoMemberInvoker<string> nameGetter;
        /// <summary>
        /// 参数键
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// 获取值调用的方法名，此方法必须是公开的并且是string (string)类型
        /// </summary>
        public string MethodName { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="inst"><inheritdoc/></param>
        /// <returns></returns>
        public override string GetValue(object inst)
        {
            if (nameGetter == null)
            {
                nameGetter = ReflectionHelper.GetInvoker<string,string>(inst, MethodName);
            }
            return nameGetter(Key);
        }
    }

}
