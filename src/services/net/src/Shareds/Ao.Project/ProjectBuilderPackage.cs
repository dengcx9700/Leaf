using Ao.Core;
using System.Reflection;

namespace Ao.Project
{
    /// <summary>
    /// 工程建筑者的包
    /// </summary>
    public class ProjectBuilderPackage : PackageBase<IProjectBuilder>
    {
        public ProjectBuilderPackage(Assembly assembly) : base(assembly)
        {
        }
    }
}
