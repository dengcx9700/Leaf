using System;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 设置重新加载的结果
    /// </summary>
    public struct SettingReloadResult
    {
        /// <summary>
        /// 保证非加载器的异常，外部异常
        /// </summary>
        public Exception OutterException;
        /// <summary>
        /// 保存的结果
        /// </summary>
        public SettingChangeSaveResult? SaveResult;
        /// <summary>
        /// 加载的结果，如果有
        /// </summary>
        public SettingChangeLoadResult? LoadResult;
        /// <summary>
        /// 是否完全成功
        /// </summary>
        public bool FullSucceed => SaveResult != null && LoadResult != null &&
            SaveResult.Value.Succeed && LoadResult.Value.Succeed;
        /// <summary>
        /// 初始化<see cref="SettingReloadResult"/>
        /// </summary>
        /// <param name="outterException"><inheritdoc cref="OutterException"/></param>
        /// <param name="saveResult"><inheritdoc cref="SaveResult"/></param>
        /// <param name="loadResult"><inheritdoc cref="LoadResult"/></param>
        public SettingReloadResult(Exception outterException, SettingChangeSaveResult? saveResult, SettingChangeLoadResult? loadResult)
        {
            OutterException = outterException;
            SaveResult = saveResult;
            LoadResult = loadResult;
        }
        /// <summary>
        /// 创建一个外部异常重置结果
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="loadResult">加载的结果</param>
        /// <param name="saveResult">保存的结果</param>
        /// <returns></returns>
        public static SettingReloadResult FromOutterException(Exception ex, SettingChangeSaveResult? saveResult=null, SettingChangeLoadResult? loadResult=null)
        {
            return new SettingReloadResult(ex, saveResult,loadResult);
        }
    }
}
