using System.Collections;
using System.Reflection;

namespace Ao.Plug.Filling
{
    /// <summary>
    /// 填充上下文
    /// </summary>
    public class FillContext : IFillContext
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object Target { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public PropertyInfo Property { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object[] Values { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IList TargetList { get; set; }
    }
}
