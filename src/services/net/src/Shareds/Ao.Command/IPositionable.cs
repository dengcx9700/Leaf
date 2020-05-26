namespace Ao.Command
{
    /// <summary>
    /// 表示可以定位数据的
    /// </summary>
    /// <typeparam name="TValue">数据类型</typeparam>
    public interface IPositionable<TValue>
    {
        /// <summary>
        /// <inheritdoc cref="Get(int)"/>
        /// </summary>
        /// <param name="index">获取位置</param>
        /// <returns></returns>
        TValue this[int index] { get; }
        /// <summary>
        /// 获取指定位置的数据
        /// </summary>
        /// <param name="index">获取位置</param>
        /// <returns></returns>
        TValue Get(int index);
    }
}
