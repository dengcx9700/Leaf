using System;
using System.IO;
using System.Reflection;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 从文件的配置源
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple =true,Inherited =false)]
    public abstract class FileConfigurationSourceAttributeBase : ConfigurationSourceAttributeBase
    {
        /// <summary>
        /// 初始化<see cref="FileConfigurationSourceAttributeBase"/>
        /// </summary>
        /// <param name="fileName">目标文件名</param>
        protected FileConfigurationSourceAttributeBase(string fileName)
        {
            FileName = fileName;
        }
        /// <summary>
        /// 初始化<see cref="FileConfigurationSourceAttributeBase"/>
        /// </summary>
        /// <param name="fileName">目标文件名</param>
        /// <param name="bindType">绑定类型</param>
        protected FileConfigurationSourceAttributeBase(string fileName,Type bindType) 
            : base(bindType)
        {
            FileName = fileName;
        }
        /// <summary>
        /// 初始化<see cref="FileConfigurationSourceAttributeBase"/>
        /// </summary>
        /// <param name="fileName">目标文件名</param>
        /// <param name="optonal">是否可选的</param>
        /// <param name="useLocalPath">是否使用本地路径</param>
        protected FileConfigurationSourceAttributeBase(string fileName, bool optonal=true, bool useLocalPath=true) 
            : this(fileName)
        {
            Optonal = optonal;
            UseLocalPath = useLocalPath;
        }
        /// <summary>
        /// 初始化<see cref="FileConfigurationSourceAttributeBase"/>
        /// </summary>
        /// <param name="fileName">目标文件名</param>
        /// <param name="bindType">绑定的类型</param>
        /// <param name="optonal">是否可选的</param>
        /// <param name="useLocalPath">是否使用本地路径</param>
        protected FileConfigurationSourceAttributeBase(string fileName, Type bindType, bool optonal = true, bool useLocalPath = true)
            : this(fileName,bindType)
        {
            Optonal = optonal;
            UseLocalPath = useLocalPath;
        }
        /// <summary>
        /// 目标文件
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// 是否可选的
        /// </summary>
        public bool Optonal { get; }
        /// <summary>
        /// 是否使用插件本地的路径,如果不是则存到默认路径
        /// </summary>
        public bool UseLocalPath { get; }
        /// <summary>
        /// 文件根目录
        /// </summary>
        public string RootPath
        {
            get
            {
                if (UseLocalPath)
                {
                    return Path.GetDirectoryName(Assembly.Location);
                }
                return DefaultSettingConfig.SettingFolder;
            }
        }
        /// <summary>
        /// 目标文件的全路径,如果<see cref="Assembly"/>为空(无依附到程序集)，返回为null
        /// </summary>
        public string FileFullPath
        {
            get
            {
                if (Assembly==null)
                {
                    return null;
                }
                return Path.Combine(RootPath, FileName);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override MakeSourceStatus GetMakeSourceStatus()
        {
            if (Assembly==null)
            {
                return MakeSourceStatus.NotMakeNotDefault;
            }
            if (File.Exists(FileFullPath))
            {
                return MakeSourceStatus.CanMake;
            }
            return MakeSourceStatus.NotMakeCanMakeDefault;
        }
    }
}
