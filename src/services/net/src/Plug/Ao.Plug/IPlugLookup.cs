using System;
using System.Collections.Generic;

namespace Ao.Plug
{
    /// <summary>
    /// 插件寻找器
    /// </summary>
    public interface IPlugLookup : IEnumerable<ITypeEntity>
    {
        /// <summary>
        /// 从条件获取类型实体集合
        /// </summary>
        /// <param name="conditioin">条件</param>
        /// <returns></returns>
        ITypeEntity[] Gets(Predicate<ITypeEntity> conditioin);
    }
}
