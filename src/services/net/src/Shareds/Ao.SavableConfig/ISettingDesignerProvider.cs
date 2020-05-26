using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Ao.SavableConfig
{
    /// <summary>
    /// <see cref="JToken"/>对象的设计器提供者
    /// </summary>
    public interface ISettingDesignerProvider<TUI>
    {
        /// <summary>
        /// 设计器提供者的条件，如果返回true，则认为此提供者可以被制作设计器
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        bool Condition(ISettingDesignerMakingContext context);
        /// <summary>
        /// 制作设计器
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        TUI Make(ISettingDesignerMakingContext context);
    }
}
