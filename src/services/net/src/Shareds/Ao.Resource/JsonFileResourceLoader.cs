using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 表示json文件资源加载器
    /// </summary>
    public class JsonFileResourceLoader : IResourceLoader
    {
        public int Order => 0;

        public string ExtensionName => ".json";

        public IResourceMetadata Load(ResourceLoadContext context)
        {
            if (context.FileExtensions.ToLower()== ExtensionName)
            {
                return new JsonFileResourceMedata(context.FilePath);
            }
            return null;
        }
    }
}
