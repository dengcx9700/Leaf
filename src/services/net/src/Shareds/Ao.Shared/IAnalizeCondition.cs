namespace Ao.Shared
{
    /// <summary>
    /// 表示解析条件
    /// </summary>
    public interface IAnalizeCondition
    {
        /// <summary>
        /// 返回一个值，指示当前是否可以被解析
        /// </summary>
        /// <param name="analizer">解析器</param>
        /// <param name="propertyItem">属性项</param>
        /// <returns></returns>
        bool Condition(IAoAnalizer analizer,AoAnalizedPropertyItemBase propertyItem);
    }
}
