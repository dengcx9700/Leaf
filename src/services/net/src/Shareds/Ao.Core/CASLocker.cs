using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ao.Core
{
    /// <summary>
    /// 原子锁
    /// </summary>
    public class CASLocker
    {
        private int locker = 0;
        /// <summary>
        /// 获取一个原子锁凭证
        /// </summary>
        /// <returns></returns>
        public ICASToken GetToken()
        {
            if (Interlocked.CompareExchange(ref locker,1,0)==0)
            {
                return new CASToken(() => Interlocked.Exchange(ref locker, 0));   
            }
            return null;
        }
        class CASToken : ICASToken
        {
            private Action release;

            public CASToken(Action release)
            {
                this.release = release;
            }

            public void Release()
            {
                release?.Invoke();
                release = null;
            }
        }
    }
}
