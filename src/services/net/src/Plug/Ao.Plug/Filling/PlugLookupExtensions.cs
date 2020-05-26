namespace Ao.Plug.Filling
{
    /// <summary>
    /// 对<see cref="IPlugLookup"/>的扩展
    /// </summary>
    public static class PlugLookupExtensions
    {
        /// <summary>
        /// 制作填充器从对象
        /// </summary>
        /// <param name="plugLookup"></param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static Filler MakeFillerFromObject(this IPlugLookup plugLookup,object target)
        {
            return new Filler(plugLookup, target);
        }
    }
}
