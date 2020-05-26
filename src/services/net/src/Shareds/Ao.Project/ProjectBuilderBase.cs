namespace Ao.Project
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract class ProjectBuilderBase : IProjectBuilder
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="project"></param>
        public virtual void Closing(IProject project)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="project"></param>
        public virtual void Creating(IProject project)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="project"></param>
        public virtual void Opening(IProject project)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="project"></param>
        public virtual void Saving(IProject project)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="project"></param>
        public virtual void SavingAs(IProject project)
        {
        }
    }
}
