using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 表示默认文件的资源加载器
    /// </summary>
    public class DefaultFileResourceLoader : IResourceLoader
    {
        public int Order => int.MinValue;

        public string ExtensionName => ".*";

        public IResourceMetadata Load(ResourceLoadContext context)
        {
            return new FileResourceMetadata(context.FilePath);
        }
    }
}
