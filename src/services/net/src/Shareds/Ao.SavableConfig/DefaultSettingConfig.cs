using System;
using System.IO;
using System.Threading.Tasks;

namespace Ao.SavableConfig
{
    public static class DefaultSettingConfig
    {
        /// <summary>
        /// 设置文件夹的名字
        /// </summary>
        public static readonly string SettingFolderName = "Settings";
        /// <summary>
        /// 更改的文件名
        /// </summary>
        public static readonly string ChangesFileName = "changes.bin";
        /// <summary>
        /// 更改的文件备份名
        /// </summary>
        public static readonly string ChangesFileBakName = "changes.bak.bin";
        /// <summary>
        /// 改变文件的路径
        /// </summary>
        public static readonly string ChangesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ChangesFileName);
        /// <summary>
        /// 改变文件的备份路径
        /// </summary>
        public static readonly string ChangesFileBakPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ChangesFileBakName);
        /// <summary>
        /// 设置文件夹
        /// </summary>
        public static readonly string SettingFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingFolderName);
        /// <summary>
        /// 默认的流保存者
        /// </summary>
        public static readonly IStreamSaver DefaultSaver = new FileStreamSaver(ChangesFilePath);

        static DefaultSettingConfig()
        {
            EnsureSettingFolderCreated();
        }
        /// <summary>
        /// 确保目录<see cref="SettingFolder"/>创建
        /// </summary>
        public static void EnsureSettingFolderCreated()
        {
            if (!Directory.Exists(SettingFolder))
            {
                Directory.CreateDirectory(SettingFolder);
            }
        }
    }
    /// <summary>
    /// 文件流的保存者
    /// </summary>
    public class FileStreamSaver : IStreamSaver
    {
        /// <summary>
        /// 初始化<see cref="FileStreamSaver"/>
        /// </summary>
        /// <param name="targetFile">目标文件</param>
        public FileStreamSaver(string targetFile)
        {
            if (string.IsNullOrEmpty(targetFile))
            {
                throw new ArgumentException("message", nameof(targetFile));
            }
            FileMode = FileMode.OpenOrCreate;
            TargetFile = targetFile;
        }
        /// <summary>
        /// 打开文件的模式
        /// </summary>
        public FileMode FileMode { get; set; }
        /// <summary>
        /// 目标文件
        /// </summary>
        public string TargetFile { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="stream"><inheritdoc/></param>
        /// <returns></returns>
        public async Task SaveAsync(Stream stream)
        {
            using (var file = File.Open(TargetFile, FileMode))
            {
                file.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(file);
                await file.FlushAsync();
            }
        }
    }
}
