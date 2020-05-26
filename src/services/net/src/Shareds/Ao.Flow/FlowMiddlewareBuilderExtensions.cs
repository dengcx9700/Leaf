using Ao.Middleware;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Flow
{
    public static class FlowMiddlewareBuilderExtensions
    {
        /// <summary>
        /// 延时任务中间件
        /// </summary>
        /// <typeparam name="TContext">上下文类型</typeparam>
        /// <param name="context"></param>
        /// <param name="delayTime">延时时间</param>
        /// <returns></returns>
        public static IMiddlewareBuilder<TContext> UseDelay<TContext>(this IMiddlewareBuilder<TContext> context,TimeSpan delayTime)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Use(_ => Task.Delay(delayTime));
            return context;
        }


    }
}
