using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Ao.SavableConfig.Exceptions
{
    /// <summary>
    /// 设置的根没加载异常
    /// </summary>
    public class SettingRootNotLoadException : Exception
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public SettingRootNotLoadException()
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public SettingRootNotLoadException(string message) : base(message)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        /// <param name="innerException"><inheritdoc/></param>
        public SettingRootNotLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="info"><inheritdoc/></param>
        /// <param name="context"><inheritdoc/></param>
        protected SettingRootNotLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
