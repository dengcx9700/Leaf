using System;
using System.IO;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置更改保存的结果
    /// </summary>
    public struct SettingChangeSaveResult : IDisposable
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Succeed;
        /// <summary>
        /// 返回的内容流
        /// </summary>
        public Stream Content;
        /// <summary>
        /// 出现的异常，如果有
        /// </summary>
        public Exception Exception;
        /// <summary>
        /// 初始化<see cref="SettingChangeSaveResult"/>
        /// </summary>
        /// <param name="succeed"><inheritdoc cref="Succeed"/></param>
        /// <param name="content"><inheritdoc cref="Content"/></param>
        /// <param name="exception"><inheritdoc cref="Exception"/></param>
        public SettingChangeSaveResult(bool succeed,Stream content, Exception exception)
        {
            Succeed = succeed;
            Content = content;
            Exception = exception;
        }
        /// <summary>
        /// 生成一个失败的结果
        /// </summary>
        /// <param name="exception">出现的异常</param>
        /// <returns></returns>
        public static SettingChangeSaveResult FailResult(Exception exception)
        {
            return new SettingChangeSaveResult(false, null, exception);
        }
        /// <summary>
        /// 生成一个成功的结果
        /// </summary>
        /// <param name="content">内容流,此参数不能为null</param>
        /// <returns></returns>
        public static SettingChangeSaveResult SucceedResult(Stream content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            return new SettingChangeSaveResult(true, content, null);
        }
        /// <summary>
        /// 释放<see cref="Content"/>
        /// </summary>
        public void Dispose()
        {
            Content?.Dispose();
            Content = null;
        }
    }
}
