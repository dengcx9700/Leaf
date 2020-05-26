using System;
using System.Runtime.Serialization;

namespace Ao.Resource
{
    /// <summary>
    /// 表示文件资源
    /// </summary>
    public class FileResourceMetadata : FileResourceMetadataBase
    {
        public FileResourceMetadata(string filePath) 
            : base(filePath)
        {
        }
    }
}
