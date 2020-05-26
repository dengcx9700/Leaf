using System.Windows;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置设计器
    /// </summary>
    public class SettingDesigner<TUI>
    {
        /// <summary>
        /// 初始化<see cref="SettingDesigner{TUI}"/>
        /// </summary>
        /// <param name="uI">目标UI</param>
        /// <param name="context">制作上下文</param>
        public SettingDesigner(TUI uI, ISettingDesignerMakingContext context)
        {
            UI = uI;
            Context = context;
        }
        /// <summary>
        /// 生成的ui
        /// </summary>
        public TUI UI { get; }
        /// <summary>
        /// 生成的上下文
        /// </summary>
        public ISettingDesignerMakingContext Context { get; }        
    }
}
