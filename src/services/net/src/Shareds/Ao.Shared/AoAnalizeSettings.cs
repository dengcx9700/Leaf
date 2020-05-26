namespace Ao
{
    /// <summary>
    /// 表示解析设置
    /// </summary>
    public class AoAnalizeSettings
    {

        /// <summary>
        /// 如果为true，没有<see cref="AoUnitAttribute"/>就不解析
        /// </summary>
        public bool IgnoreWithoutUnit { get; set; }

        /// <summary>
        /// 忽略自循环
        /// </summary>
        public bool IgnoreSelfLoop { get; set; } = true;
        /// <summary>
        /// 忽略互相引用
        /// </summary>
        public bool IgnoreMutualReference { get; set; } = true;
        /// <summary>
        /// 不走进可枚举类型里
        /// </summary>
        public bool NotStepInArray { get; set; }
    }

}
