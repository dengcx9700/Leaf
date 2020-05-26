namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示正在设计的制作器上下文
    /// </summary>
    public interface ISettingDesignerMakingContext
    {

        /// <summary>
        /// 设计的上下文
        /// </summary>
        ISettingDesignerContext DesignerContext { get; }
        /// <summary>
        /// 对象路径
        /// </summary>
        string ObjectPath { get; }
        /// <summary>
        /// 设置的映射节点
        /// </summary>
        SettingMapNode SettingMapNode { get; }
    }
    public static class SettingDesignerMakingContextExtensions
    {
        public static void SetValue(this ISettingDesignerMakingContext context,object val)
        {
            context.SettingMapNode.Setter(val);
            context.DesignerContext.Configuration.Value = val.ToString();
        }
    }
}
