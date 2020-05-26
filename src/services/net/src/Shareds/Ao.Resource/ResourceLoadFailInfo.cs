using System;

namespace Ao.Resource
{
    /// <summary>
    /// 资源加载失败的上下文
    /// </summary>
    public struct ResourceLoadFailInfo
    {
        public Exception Exception;

        public string Msg;
        public ResourceLoadFailInfo(Exception exception)
        {
            Exception = exception;
            Msg = exception?.Message;
        }
        public ResourceLoadFailInfo(Exception exception, string msg)
        {
            Exception = exception;
            Msg = msg;
        }
    }
}