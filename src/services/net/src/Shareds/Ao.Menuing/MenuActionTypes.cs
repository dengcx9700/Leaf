namespace Pd.Services.Menu
{
    /// <summary>
    /// 表示插入的类型
    /// </summary>
    public enum MenuActionTypes
    {
        /// <summary>
        /// 插入在前面
        /// </summary>
        After,
        /// <summary>
        /// 替换
        /// </summary>
        Replace,
        /// <summary>
        /// 插入在后面
        /// </summary>
        Before,
        /// <summary>
        /// 插入在里面前面
        /// </summary>
        InnerAfter,
        /// <summary>
        /// 插入在里面的最后
        /// </summary>
        InnerBefore
    }
}
