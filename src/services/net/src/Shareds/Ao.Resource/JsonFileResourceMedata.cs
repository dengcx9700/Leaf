using Newtonsoft.Json.Linq;

namespace Ao.Resource
{
    /// <summary>
    /// 表示linq json object文件资源
    /// </summary>
    public class JsonFileResourceMedata : JsonFileResourceMetadata<JObject>
    {
        public JsonFileResourceMedata(string filePath)
            : base(filePath)
        {
        }
    }
}
