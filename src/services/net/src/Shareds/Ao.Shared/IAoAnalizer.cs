using Ao.Shared;

namespace Ao
{
    /// <summary>
    /// 表示解析器
    /// </summary>
    public interface IAoAnalizer
    {
        /// <summary>
        /// 解析设置
        /// </summary>
        AoAnalizeSettings Settings { get; }
        /// <summary>
        /// 解析实例
        /// </summary>
        /// <param name="inst">目标实例</param>
        /// <param name="deep">是否递归解析，如果为false，则只浅解析</param>
        /// <param name="options">解析可选项</param>
        /// <returns></returns>
        AoAnalizedResual Analize(object inst, bool deep = true, AnalizingOptions options=null);
    }
}