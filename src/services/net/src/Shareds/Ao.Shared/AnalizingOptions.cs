using System.Collections.Generic;
using System.Text;

namespace Ao.Shared
{
    /// <summary>
    /// 解析的可选项
    /// </summary>
    public class AnalizingOptions
    {
        /// <summary>
        /// 解析跳过条件集合
        /// </summary>
        public IAnalizeCondition[] SkipConditions { get; set; }
    }
}
