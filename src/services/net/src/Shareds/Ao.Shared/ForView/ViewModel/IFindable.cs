namespace Ao.Shared.ForView.ViewModel
{
    /// <summary>
    /// 表示可以被新增的
    /// </summary>
    public interface IFindable
    {
        /// <summary>
        /// 搜寻
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <returns></returns>
        bool Find(string key);
    }
}