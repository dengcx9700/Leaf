using Portable.Xaml;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// 表示xaml文件资源元数据
    /// </summary>
    /// <typeparam name="TEntity">目的实体类型</typeparam>
    public abstract class XamlFileResourceMetadata<TEntity> : FileResourceMetadataBase,IResourceMetadata
    {
        private readonly object locker = new object();
        private bool instOnly;
        private TEntity instEntity;
        private string fileText;
        /// <summary>
        /// 表示文件的内容
        /// </summary>
        public string FileText
        {
            get
            {
                if (fileText==null)
                {
                    lock (locker)
                    {
                        if (fileText==null)
                        {
                            var streamTask = CreateStreamAsync();
                            streamTask.Wait(60*1000);
                            using (var stream = streamTask.Result)
                            using (var sr = new StreamReader(stream))
                                fileText = sr.ReadToEnd();
                        }
                    }
                }
                return fileText;
            }
        }
        private void BuildView()
        {
            try
            {
                var obj = XamlServices.Parse(FileText);

                if (!(obj is TEntity))
                {
                    throw new InvalidCastException($"{Name},此文档不能转为{typeof(TEntity).FullName}类型");
                }
                instEntity = (TEntity)obj;

            }
            catch (Exception ex)
            {
                RaiseResourceLoadFailed(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="instOnly">是否view只是同一实例</param>
        public XamlFileResourceMetadata(string filePath, bool instOnly)
            : base(filePath)
        {
            this.instOnly = instOnly;
            this.ResourceNamePathLoad += XamlFileResourceMetadata_ResourceNamePathLoad;
        }

        private void XamlFileResourceMetadata_ResourceNamePathLoad()
        {
            lock (locker)
            {
                fileText = null;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public TEntity Create()
        {
            if (!instOnly||instEntity==null)
            {
                BuildView();
            }
            return instEntity;
        }
        /// <summary>
        /// 获取保存的实体
        /// </summary>
        /// <returns></returns>
        protected virtual TEntity GetSaveEntity()
        {
            return instEntity;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        Task IResourceMetadata.SaveAsync()
        {
            var entity = GetSaveEntity();
            if (entity!=null)
            {
                return SaveAsync(entity);
            }
#if NET452
            return Task.FromResult(0);
#else
            return Task.CompletedTask;
#endif
        }
        /// <summary>
        /// <inheritdoc cref="IResourceMetadata.SaveAsync"/>
        /// </summary>
        /// <param name="value">对应的类型</param>
        /// <returns></returns>
        public async Task SaveAsync(object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (!typeof(TEntity).IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException($"对象不是或不继承于类型{typeof(TEntity).FullName}");
            }
            var str = XamlServices.Save(value);
            using (var stream = await CreateStreamAsync())
            using (var sw = new StreamWriter(stream))
            {
                sw.BaseStream.SetLength(0);
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.Write(str);
                sw.Flush();
            }
            lock (locker)
            {
                fileText = str;
            }
            OnSaved();
        }
        /// <summary>
        /// 被保存之后
        /// </summary>
        protected virtual void OnSaved()
        {
        }
        public override void CopyTo(IResourceMetadata target)
        {
            if (target is XamlFileResourceMetadata<TEntity> m)
            {
                m.instEntity = instEntity;
                m.instOnly = instOnly;
            }
            base.CopyTo(target);
        }
    }

}
