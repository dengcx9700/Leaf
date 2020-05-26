using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置更改加载的结果
    /// </summary>
    public struct SettingChangeLoadResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Succeed;
        /// <summary>
        /// 出现的异常，如果有
        /// </summary>
        public Exception Exception;
        /// <summary>
        /// 生成的配置源
        /// </summary>
        public KeyValuePair<string,string>[] ConfigurationPairs;
        /// <summary>
        /// 初始化<see cref="SettingChangeLoadResult"/>
        /// </summary>
        /// <param name="succeed"><inheritdoc cref="Succeed"/></param>
        /// <param name="exception"><inheritdoc cref="Exception"/></param>
        /// <param name="configurationPairs"><inheritdoc cref="ConfigurationPairs"/></param>
        public SettingChangeLoadResult(bool succeed, Exception exception, KeyValuePair<string, string>[] configurationPairs)
        {
            Succeed = succeed;
            Exception = exception;
            ConfigurationPairs = configurationPairs;
        }

        /// <summary>
        /// 生成一个失败的结果
        /// </summary>
        /// <param name="exception">出现的异常如果有</param>
        /// <returns></returns>
        public static SettingChangeLoadResult FailResult(Exception exception)
        {
            return new SettingChangeLoadResult(false, exception, null);
        }
        /// <summary>
        /// 生成一个成功的结果
        /// </summary>
        /// <param name="configurationPairs">配置部分</param>
        /// <returns></returns>
        public static SettingChangeLoadResult SucceedResult(KeyValuePair<string, string>[] configurationPairs)
        {
            if (configurationPairs is null)
            {
                throw new ArgumentNullException(nameof(configurationPairs));
            }

            return new SettingChangeLoadResult(true, null, configurationPairs);
        }
    }
}
