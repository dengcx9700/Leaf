using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Runtime.Serialization;

namespace Ao.Resource
{
    /// <summary>
    /// 表示json文件的资源
    /// </summary>
    /// <typeparam name="TEntity">目标实体</typeparam>
    public class JsonFileResourceMetadata<TEntity> : FileResourceMetadataBase
    {
        private bool loaded;
        private TEntity entity;
        /// <summary>
        /// 目标实体
        /// </summary>
        public TEntity Entity
        {
            get
            {
                if (!loaded&&entity==null)
                {
                    GetStream();
                }
                return entity;
            }
            set
            {
                loaded = true;
                entity = value;
            }
        }

        public JsonFileResourceMetadata(string filePath)
            : base(filePath)
        { 
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override Stream GetStream()
        {
            var stream= base.GetStream();
            using (var sr=new StreamReader(stream))
            {
                var str = sr.ReadToEnd();
                Entity = JsonConvert.DeserializeObject<TEntity>(str);
            }
            return stream;
        }
        /// <summary>
        /// <inheritdoc/>
        /// <para>
        /// 将<see cref="Entity"/>转为json文本并写入
        /// </para>
        /// </summary>
        /// <returns></returns>
        public async override Task SaveAsync()
        {
            var str = JsonConvert.SerializeObject(Entity);
            var stream = base.GetStream();
            var sw = new StreamWriter(stream);
            sw.BaseStream.SetLength(0);
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            await sw.WriteAsync(str);
        }
    }
}
