namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示值提供器
    /// </summary>
    /// <typeparam name="T">提供值的类型</typeparam>
    public interface IValueProvider<T>
    {
        /// <summary>
        /// 获取一个值
        /// </summary>
        /// <param name="inst">目标值</param>
        /// <returns></returns>
        T GetValue(object inst);
    }
}
