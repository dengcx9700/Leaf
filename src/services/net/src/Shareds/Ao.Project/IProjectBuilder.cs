namespace Ao.Project
{
    /// <summary>
    /// 工程建筑者
    /// </summary>
    public interface IProjectBuilder
    {
        /// <summary>
        /// 正在创建工程
        /// </summary>
        /// <param name="project"></param>
        void Creating(IProject project);
        /// <summary>
        /// 正在保存工程
        /// </summary>
        /// <param name="project"></param>
        void Saving(IProject project);
        /// <summary>
        /// 正在另存为工厂
        /// </summary>
        /// <param name="project"></param>
        void SavingAs(IProject project);
        /// <summary>
        /// 正在打开工程
        /// </summary>
        /// <param name="project"></param>
        void Opening(IProject project);
        /// <summary>
        /// 正在关闭工程
        /// </summary>
        /// <param name="project"></param>
        void Closing(IProject project);
    }
}
