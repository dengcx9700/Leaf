namespace Ao.Shared.ForView
{
    /// <summary>
    /// 视图包装器
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public interface IViewWraper<TView>
    {
        /// <summary>
        /// 包装
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        TView Wraper(TView view);
    }
}
