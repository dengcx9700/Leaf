namespace Pd.Services.Menu
{
    /// <summary>
    /// 查找操作是否可以进行的结果类型
    /// </summary>
    public enum ActionTestResultTypes
    {
        /// <summary>
        /// 可以被操作
        /// </summary>
        OK,
        /// <summary>
        /// 路径寻找失败
        /// </summary>
        NotFound,
        /// <summary>
        /// 拒绝被替换
        /// </summary>
        RefuseReplace,
        /// <summary>
        /// 未知
        /// </summary>
        Unknow
    }
}
