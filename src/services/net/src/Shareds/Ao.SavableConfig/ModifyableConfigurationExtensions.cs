using Newtonsoft.Json.Linq;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 对与<see cref="IModifyableConfiguration"/>的扩展
    /// </summary>
    public static class ModifyableConfigurationExtensions
    {
        /// <summary>
        /// 将修改的信息转为json对象
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static JObject ModifyAsJson(this IModifyableConfiguration configuration)
        {
            var changes = configuration.Changes;
            var jobj = new JObject();
            foreach (var item in changes)
            {
                jobj.Add(item.Key, item.Value);
            }
            return jobj;
        }
    }
}
