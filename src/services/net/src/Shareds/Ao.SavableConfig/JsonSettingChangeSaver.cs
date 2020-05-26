using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ao.SavableConfig
{
    /// <summary>
    /// json的设置保存器
    /// </summary>
    public class JsonSettingChangeSaver : ISettingChangeSaver
    {
        /// <summary>
        /// 默认的更改保存器
        /// </summary>
        public static JsonSettingChangeSaver Default { get; } = new JsonSettingChangeSaver();
        /// <summary>
        /// <para>
        /// 会以json的方式加载
        /// </para>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="config"><inheritdoc/></param>
        /// <returns></returns>
        public ValueTask<SettingChangeLoadResult> LoadAsync(SettingChangeLoadConfig config)
        {
            try
            {
                //不释放在上一层控制释放
                config.Content.Seek(0, SeekOrigin.Begin);
                var streamReader = new StreamReader(config.Content);
                var str = streamReader.ReadToEnd();
                var jobj = JObject.Parse(str);
                var dic = new List<KeyValuePair<string,string>>(jobj.Count);
                using (var enu = jobj.GetEnumerator())
                {
                    while (enu.MoveNext())
                    {
                        dic.Add(new KeyValuePair<string, string>(enu.Current.Key, enu.Current.Value?.ToString()));
                    }
                }
                return new ValueTask<SettingChangeLoadResult>(SettingChangeLoadResult.SucceedResult(dic.ToArray()));
            }
            catch (Exception ex)
            {
                return new ValueTask<SettingChangeLoadResult>(SettingChangeLoadResult.FailResult(ex));
            }
        }
        /// <summary>
        /// <para>
        /// 会以json方式保存更改
        /// </para>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="root"><inheritdoc/></param>
        /// <returns></returns>
        public async ValueTask<SettingChangeSaveResult> SaveAsync(ISavableConfigurationRoot root)
        {
            try
            {
                var jobj = new JObject();
                var changes = root.Changes;
                foreach (var item in changes)
                {
                    jobj.Add(item.Key, item.Value);
                }
                var memStream = new MemoryStream();
                var streamWrite = new StreamWriter(memStream);
                await streamWrite.WriteAsync(jobj.ToString());
                await streamWrite.FlushAsync();
                memStream.Seek(0, SeekOrigin.Begin);
                return SettingChangeSaveResult.SucceedResult(memStream);
            }
            catch (Exception ex)
            {
                return SettingChangeSaveResult.FailResult(ex);
            }
        }
    }
}
