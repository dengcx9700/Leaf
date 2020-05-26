using Ao.Core;
using System.Reflection;

namespace Ao.Project
{
    public class ProjectPartPackage : PackageBase<IProjectPartProvider>
    {
        public ProjectPartPackage(Assembly assembly) : base(assembly)
        {
        }
    }

}
