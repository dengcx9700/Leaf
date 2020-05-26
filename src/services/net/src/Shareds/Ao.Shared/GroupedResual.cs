using System.Collections.Generic;
namespace Ao
{
    /// <summary>
    /// 表示分组结果
    /// </summary>
    public class GroupedResual
    {
        /// <summary>
        /// 默认组
        /// </summary>
        public static readonly string DefaultGroup = "Defualt";
        /// <summary>
        /// 初始化<see cref="GroupedResual"/>
        /// </summary>
        public GroupedResual()
        {
            Properties = new Dictionary<string, IList<AoAnalizedPropertyItemBase>>
            {
                {DefaultGroup,new List<AoAnalizedPropertyItemBase>() }
            };
            Methods = new Dictionary<string, IList<AoAnalizedMethodItemBase>>
            {
                {DefaultGroup,new List<AoAnalizedMethodItemBase>() }
            };
        }
        /// <summary>
        /// 分组后的属性
        /// </summary>
        public Dictionary<string, IList<AoAnalizedPropertyItemBase>> Properties { get; }
        /// <summary>
        /// 分组后的方法
        /// </summary>
        public Dictionary<string, IList<AoAnalizedMethodItemBase>> Methods { get; }

    }
}
