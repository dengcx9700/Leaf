using Ao.Core;
using System.Reflection;

namespace Ao.Resource
{
    /// <summary>
    /// 资源创建器的包
    /// </summary>
    public class ResourceCreatorPackage : PackageBase<IResourceCreatorMetadata>
    {
        public ResourceCreatorPackage(Assembly assembly) 
            : base(assembly)
        {
        }
    }
}
