using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ao.Resource
{
    /// <summary>
    /// <inheritdoc/>
    /// 表示基于文件的资源
    /// </summary>
    [Serializable]
    public abstract class FileResourceMetadataBase : ResourceMetadataBase
    {
        private string filePath;
        public FileResourceMetadataBase(string filePath)
        {
            InitPath(filePath);
            Name = Path.GetFileName(filePath);
        }

        /// <summary>
        /// 文件全路径
        /// </summary>
        public string FilePath => filePath;

        /// <summary>
        /// 更改文件路径,如果文件流被创建了，则不允许修改
        /// </summary>
        /// <param name="newFilePath"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ResetFilePath(string newFilePath)
        {
            if (IsDisponsed)
            {
                ThrowObjectDisposedException();
            }
            filePath = newFilePath;
        }

        private void InitPath(string filePath)
        {
            this.filePath = Path.GetFullPath(filePath);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override Task SaveAsync()
        {
            //因为读写都是基于文件的，所有不用手动保存
#if NET452
            return Task.FromResult(0);
#else
            return Task.CompletedTask;
#endif
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override Task<Stream> OnCreateStreamAsync()
        {
            return Task.FromResult(GetStream());
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="resourceMedata"></param>
        protected override void OnFirstMedataAdd(ResourceMetadataBase resourceMedata)
        {
            if (IsDisponsed)
            {
                ThrowObjectDisposedException();
            }
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
        /// <summary>
        /// 获取一个流
        /// </summary>
        /// <returns></returns>
        protected virtual Stream GetStream()
        {
            if (IsDisponsed)
            {
                ThrowObjectDisposedException();
            }
            if (File.Exists(filePath))
            {
                return File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
            }
            return File.Create(filePath);
        }
    }
}
