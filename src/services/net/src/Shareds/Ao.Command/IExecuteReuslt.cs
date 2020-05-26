using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Ao.Command
{
    /// <summary>
    /// 执行的结果
    /// </summary>
    public interface IExecuteReuslt
    {
        /// <summary>
        /// 参数列表
        /// </summary>
        object[] Paramters { get; }
        /// <summary>
        /// 执行结果
        /// </summary>
        object Result { get; }
        /// <summary>
        /// 是否成功
        /// </summary>
        bool Succeed { get; }
    }
    /// <summary>
    /// 对<see cref="IExecuteReuslt"/>的扩展
    /// </summary>
    public static class ExecuteResultExtensions
    {
        /// <summary>
        /// 获取真实的结果，如果是<see cref="Task{TResult}"/>的则获取TResult,如果是直接类型，则获取直接类型
        /// </summary>
        /// <param name="result">执行结果</param>
        /// <returns></returns>
        public static object GetRealyResult(this IExecuteReuslt result)
        {
            if (result is null)
            {
                return null;
            }
            if (result.Result is Task&&result.Result.GetType().IsGenericType&&
                result.Result.GetType().GetGenericTypeDefinition().IsEquivalentTo(typeof(Task<>)))
            {
                return ((dynamic)result.Result).Result;
            }
            return result.Result;
        }
    }
}
