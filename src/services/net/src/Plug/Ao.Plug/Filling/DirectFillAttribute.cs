using System;

namespace Ao.Plug.Filling
{
    /// <summary>
    /// 直接放入集合的填充
    /// </summary>
    public class DirectFillAttribute : FillAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="refType"><inheritdoc/></param>
        public DirectFillAttribute(Type refType)
            : base(refType)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context"><inheritdoc/></param>
        public override void PutIn(FillContext context)
        {
            foreach (var item in context.Values)
            {
                context.TargetList.Add(item);
            }
        }
    }
}
