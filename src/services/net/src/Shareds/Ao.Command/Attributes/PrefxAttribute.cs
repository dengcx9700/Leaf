using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Command.Attributes
{
    /// <summary>
    /// 表示前缀
    /// </summary>
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method,AllowMultiple =false,Inherited =false)]
    public class PrefxAttribute : Attribute
    {
        /// <summary>
        /// 初始化<see cref="PrefxAttribute"/>
        /// </summary>
        /// <param name="prefx"><inheritdoc cref="Prefx"/></param>
        public PrefxAttribute(string prefx)
        {
            if (string.IsNullOrWhiteSpace(prefx))
            {
                throw new ArgumentException("message", nameof(prefx));
            }

            Prefx = prefx;
        }
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefx { get; }
    }
}
