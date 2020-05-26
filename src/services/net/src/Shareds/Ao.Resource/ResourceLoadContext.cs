using System.IO;

namespace Ao.Resource
{
    /// <summary>
    /// 表示资源加载的上下文
    /// </summary>
    public class ResourceLoadContext
    {
        private FileInfo fileInfo;

        public ResourceLoadContext(string filePath)
        {
            FilePath = Path.GetFullPath(filePath);
            FileExtensions = Path.GetExtension(FilePath);
            FolderPath = Path.GetDirectoryName(FilePath);
        }
        /// <summary>
        /// 文件绝对路径
        /// </summary>
        public string FilePath { get; }
        /// <summary>
        /// 文后缀名,如.json
        /// </summary>
        public string FileExtensions { get; }
        /// <summary>
        /// 目录路径
        /// </summary>
        public string FolderPath { get; }
        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfo 
        {
            get
            {
                if (fileInfo==null)
                {
                    fileInfo = new FileInfo(FilePath);
                }
                return fileInfo;
            }
        }
    }
}
