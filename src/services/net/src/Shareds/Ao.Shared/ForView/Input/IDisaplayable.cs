using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Shared.ForView.Input
{
    /// <summary>
    /// 表示可用显示的
    /// </summary>
    public interface IDisaplayable
    {
        /// <summary>
        /// 显示的名字
        /// </summary>
        string DisplayName { get; set; }
    }
}
