using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ao.Plug
{
    /// <summary>
    /// 插件寻找器
    /// </summary>
    public class PlugLookup : IPlugLookup
    {
        private readonly IPlugSourceProvider[] providers;

        public PlugLookup(IPlugSourceProvider[] providers)
        {
            this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ITypeEntity> GetEnumerator()
        {
            return providers.SelectMany(p => p.TypeEntities).GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="conditioin"><inheritdoc/></param>
        /// <returns></returns>
        public ITypeEntity[] Gets(Predicate<ITypeEntity> conditioin)
        {
            return providers.SelectMany(p => p.TypeEntities.Where(t => conditioin(t)))
                        .ToArray();                    
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
