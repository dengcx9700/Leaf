using Ao.Core;

namespace Ao.Project
{
    /// <summary>
    /// 表示工程建筑者的服务
    /// </summary>
    public interface IProjectBuildService : IPackagingService<ProjectBuilderPackage, IProjectBuilder, ProjectBuilderPackage>
    {

    }
}
