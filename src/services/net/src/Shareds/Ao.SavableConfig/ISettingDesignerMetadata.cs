namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示设置设计器的元数据
    /// </summary>
    public interface ISettingDesignerMetadata<TUI>
    {
        /// <summary>
        /// 排序键
        /// </summary>
        int Order { get; }
        /// <summary>
        /// 设计器提供者
        /// </summary>
        ISettingDesignerProvider<TUI> DesignerProvider { get; }
    }
}
