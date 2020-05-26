namespace Ao.SavableConfig
{
    /// <summary>
    /// 制作配置源的状态
    /// </summary>
    public enum MakeSourceStatus
    {
        /// <summary>
        /// 能制作配置源
        /// </summary>
        CanMake,
        /// <summary>
        /// 不能制作配置源，但能能制作默认
        /// </summary>
        NotMakeCanMakeDefault,
        /// <summary>
        /// 不能制作配置源，不能制作默认
        /// </summary>
        NotMakeNotDefault
    }
}
