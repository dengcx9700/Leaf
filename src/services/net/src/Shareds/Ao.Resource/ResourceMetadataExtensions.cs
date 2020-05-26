using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
#if NETSTANDARD2_1
using System.Buffers;
#endif
namespace Ao.Resource
{
    public static class ResourceMetadataExtensions
    {
        /// <summary>
        /// 读取资源为文本
        /// </summary>
        /// <param name="resourceMedata"></param>
        /// <returns></returns>
        public static async Task<string> ReadAsStringAsync(this IResourceMetadata resourceMedata)
        {
            var stream =await EnsureGetStreamAsync(resourceMedata);
            using (var sr = new StreamReader(stream))
                return await sr.ReadToEndAsync();
        }
        /// <summary>
        /// 同步读取资源为文本
        /// </summary>
        /// <param name="resourceMedata"></param>
        /// <returns></returns>
        public static string ReadAsString(this IResourceMetadata resourceMedata)
        {
            var task = EnsureGetStreamAsync(resourceMedata);
            task.Wait();
            var stream =task.Result;
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }
        /// <summary>
        /// 读取转为字节流
        /// </summary>
        /// <param name="resourceMedata"></param>
        /// <param name="threadSafe">是否必须为线程安全</param>
        /// <returns></returns>
        public static async Task<byte[]> ReadAsBytesAsync(this IResourceMetadata resourceMedata,bool threadSafe=true)
        {
            var stream =await EnsureGetStreamAsync(resourceMedata);
            byte[] res;
#if !NETSTANDARD2_1
            res = new byte[stream.Length];
#else
            if (threadSafe)
            {
                res = new byte[stream.Length];
            }
            else
            {
                res = ArrayPool<byte>.Shared.Rent((int)stream.Length);
            }
#endif
            
            using (var ms = new MemoryStream(res))
            {
                await stream.CopyToAsync(ms);
#if NETSTANDARD2_1
                if (!threadSafe)
                {
                    ArrayPool<byte>.Shared.Return(res);
                }
#endif
                return ms.GetBuffer();
            }
        }
        /// <summary>
        /// 读取转为json
        /// </summary>
        /// <typeparam name="T">转为的类型</typeparam>
        /// <param name="resourceMedata"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static async Task<T> ReadAsObjectAsync<T>(this IResourceMetadata resourceMedata,JsonSerializerSettings settings=null)
        {
            var str =await resourceMedata.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str, settings);
        }
        /// <summary>
        /// 读取转为<see cref="JObject"/>
        /// </summary>
        /// <param name="resourceMedata"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static async Task<JObject> ReadAsJsonAsync(this IResourceMetadata resourceMedata, JsonSerializerSettings settings = null)
        {
            return await resourceMedata.ReadAsObjectAsync<JObject>(settings);
        }
        private static async Task<Stream> EnsureGetStreamAsync(IResourceMetadata resourceMedata)
        {
            var stream =await resourceMedata.CreateStreamAsync();
            if (stream == null)
            {
                throw new InvalidOperationException($"资源[{resourceMedata.Name}]返回的流为null");
            }
            if (stream.CanRead)
            {
                throw new InvalidOperationException($"资源[{resourceMedata.Name}]返回的流不可读");
            }
            return stream;
        }
    }
}
