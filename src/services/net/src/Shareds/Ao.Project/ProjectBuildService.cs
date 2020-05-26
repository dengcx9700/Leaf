using Ao.Core;
using System.Reflection;

namespace Ao.Project
{
    /// <summary>
    /// <inheritdoc cref="IProjectBuildService"/>
    /// </summary>
    public class ProjectBuildService : PackagingService<ProjectBuilderPackage, IProjectBuilder, ProjectBuilderPackage>, IProjectBuildService
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Condition(IProjectBuilder type)
        {
            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <returns></returns>
        protected override ProjectBuilderPackage MakePackage(Assembly assembly)
        {
            return new ProjectBuilderPackage(assembly);
        }
    }
}
