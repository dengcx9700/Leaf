using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ao.SavableConfig
{
    internal class MapNodeLookup<TCurrent, TNext> : IMapNodeLookup<TCurrent, TNext>
    {
        private readonly List<string> pathBlocks;
        private readonly ISettingMap map;

        public MapNodeLookup(List<string> pathBlocks, ISettingMap map)
        {
            this.pathBlocks = pathBlocks ?? throw new ArgumentNullException(nameof(pathBlocks));
            this.map = map ?? throw new ArgumentNullException(nameof(map));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Path =>
#if NETSTANDARD2_1
         string.Join('.', pathBlocks);
#else
         string.Join(".", pathBlocks);
#endif
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SettingMapNode Node => map[Path];
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object Value => Node?.Value;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <typeparam name="TThen">下一个节点的类型</typeparam>
        /// <param name="expression">访问表达式</param>
        /// <returns></returns>
        public IMapNodeLookup<TNext, TThen> Then<TThen>(Expression<Func<TNext, TThen>> expression)
        {
            if (expression.Body is MemberExpression exp)
            {
                pathBlocks.Add(exp.Member.Name);
                return new MapNodeLookup<TNext, TThen>(pathBlocks, map);
            }
            throw new InvalidOperationException("Can't use this expression, it is not member");
        }
    }
}
