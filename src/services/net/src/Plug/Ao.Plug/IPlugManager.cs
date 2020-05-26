using System.Collections.Generic;

namespace Ao.Plug
{
    /// <summary>
    /// 表示插件管理器
    /// </summary>
    public interface IPlugManager : IList<IPlugSourceProvider>
    {
        /// <summary>
        /// 生成<see cref="IPlugLookup"/>
        /// </summary>
        /// <returns></returns>
        IPlugLookup Build();
    }
}
