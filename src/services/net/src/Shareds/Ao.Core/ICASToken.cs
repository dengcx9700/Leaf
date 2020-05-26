namespace Ao.Core
{
    /// <summary>
    /// 原子锁凭证
    /// </summary>
    public interface ICASToken
    {
        /// <summary>
        /// 释放原子锁
        /// </summary>
        void Release();
    }
}
