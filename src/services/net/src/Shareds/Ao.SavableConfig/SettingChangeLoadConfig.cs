using System;
using System.IO;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置更改加载的配置
    /// </summary>
    public struct SettingChangeLoadConfig
    {
        private readonly Lazy<Stream> content;
        /// <summary>
        /// 根路径
        /// </summary>
        public string RootPath;
        /// <summary>
        /// 文件全路径
        /// </summary>
        public string FileName;
        /// <summary>
        /// 内容流
        /// </summary>
        public Stream Content=>content.Value;
        /// <summary>
        /// <see cref="Content"/>是否已经被打开了
        /// </summary>
        public bool IsContentLoad => content.IsValueCreated;
        /// <summary>
        /// 目录路径
        /// </summary>
        public string FilePath => Path.Combine(RootPath, FileName);
        /// <summary>
        /// 初始化<see cref="SettingChangeLoadConfig"/>
        /// </summary>
        /// <param name="rootPath"><inheritdoc cref="RootPath"/></param>
        /// <param name="fileName"><inheritdoc cref="FileName"/></param>
        public SettingChangeLoadConfig(string rootPath,string fileName)
        {
            RootPath = rootPath;
            FileName = fileName;
            content = new Lazy<Stream>(() => File.OpenRead(Path.Combine(rootPath,fileName)));
        }
    }
}
