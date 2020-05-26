using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Ao.Plug
{
    /// <summary>
    /// 实现了<see cref="IPlugManager"/>的插件管理器
    /// </summary>
    public class PlugManager : List<IPlugSourceProvider>, IPlugManager, IList<IPlugSourceProvider>, ICollection<IPlugSourceProvider>, IEnumerable<IPlugSourceProvider>
    {
        void IList<IPlugSourceProvider>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        bool ICollection<IPlugSourceProvider>.Remove(IPlugSourceProvider item)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IPlugLookup Build()
        {
            return new PlugLookup(ToArray());
        }
    }
}
