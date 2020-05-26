using System.Collections.Generic;

namespace Ao.Project
{
    /// <summary>
    /// 表示工程部分
    /// </summary>
    public interface IProjectPart
    {
        /// <summary>
        /// 附加的项目
        /// </summary>
        Project Project { get; }
        /// <summary>
        /// 重设，并且设置<see cref="Done"/>为<see cref="false"/>
        /// </summary>
        void Reset();
    }
}
