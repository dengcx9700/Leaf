using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace Ao.SavableConfig
{
    /// <summary>
    /// json的配置源
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple =true,Inherited =false)]
    public class JsonConfigurationSourceAttribute : FileConfigurationSourceAttributeBase
    {
        /// <summary>
        /// 初始化<see cref="JsonConfigurationSourceAttribute"/>
        /// </summary>
        /// <param name="fileName"><inheritdoc cref="FileConfigurationSourceAttributeBase.FileName"/></param>
        public JsonConfigurationSourceAttribute(string fileName) : base(fileName)
        {
        }
        /// <summary>
        /// <inheritdoc cref="JsonConfigurationSourceAttribute.JsonConfigurationSourceAttribute(string)"/>
        /// </summary>
        /// <param name="fileName"><inheritdoc cref="FileConfigurationSourceAttributeBase.FileName"/></param>
        /// <param name="bindType">绑定类型</param>
        public JsonConfigurationSourceAttribute(string fileName, Type bindType) : base(fileName, bindType)
        {
        }
        /// <summary>
        /// <inheritdoc cref="JsonConfigurationSourceAttribute.JsonConfigurationSourceAttribute(string)"/>
        /// </summary>
        /// <param name="fileName"><inheritdoc cref="FileConfigurationSourceAttributeBase.FileName"/></param>
        /// <param name="optonal"><inheritdoc cref="FileConfigurationSourceAttributeBase.Optonal"/></param>
        /// <param name="useLocalPath"><inheritdoc cref="FileConfigurationSourceAttributeBase.UseLocalPath"/></param>
        public JsonConfigurationSourceAttribute(string fileName, bool optonal = true, bool useLocalPath = true) : base(fileName, optonal, useLocalPath)
        {
        }
        /// <summary>
        /// <inheritdoc cref="JsonConfigurationSourceAttribute(string)"/>
        /// </summary>
        /// <param name="fileName"><inheritdoc cref="FileConfigurationSourceAttributeBase.FileName"/></param>
        /// <param name="bindType">绑定类型</param>
        /// <param name="optonal"><inheritdoc cref="FileConfigurationSourceAttributeBase.Optonal"/></param>
        /// <param name="useLocalPath"><inheritdoc cref="FileConfigurationSourceAttributeBase.UseLocalPath"/></param>
        public JsonConfigurationSourceAttribute(string fileName, Type bindType, bool optonal = true, bool useLocalPath = true) : base(fileName, bindType, optonal, useLocalPath)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override IConfigurationSource GetConfigurationSource()
        {
            return new JsonConfigurationSource
            {
                Path = FileName,
                FileProvider = new PhysicalFileProvider(RootPath),
                Optional = true
            };
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string MakeDefault()
        {
            if (BindType==null)
            {
                return null;
            }
            var obj = Activator.CreateInstance(BindType);
            var wraper = new JObject();
            var content=JObject.FromObject(obj);
            wraper.Add(BindType.FullName, content);
            return wraper.ToString();
        }
    }
}
