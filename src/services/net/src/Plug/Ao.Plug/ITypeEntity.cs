using System;

namespace Ao.Plug
{
    /// <summary>
    /// 类型实体
    /// </summary>
    public interface ITypeEntity
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// 制作实例
        /// </summary>
        /// <param name="params">参数列表</param>
        /// <returns></returns>
        object Make(params object[] @params);
    }
}
