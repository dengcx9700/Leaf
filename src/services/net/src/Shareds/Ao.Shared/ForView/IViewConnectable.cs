using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示view可以与逻辑连接
    /// </summary>
    public interface IViewConnectable
    {
        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <param name="inst">属性的对象</param>
        /// <param name="propertyItem">解析后的属性</param>
        bool Bind(object inst, AoAnalizedPropertyItemBase propertyItem);
    }
}
