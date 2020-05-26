using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Middleware
{
    public static class MiddlewareBuilderExtensions
    {
        /// <summary>
        /// 加入直接中间件
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder">建造器</param>
        /// <param name="action">中间件方法</param>
        public static void Use<TContext>(this IMiddlewareBuilder<TContext> builder,Handler<TContext> action)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            builder.Use(next => new DirectMiddleware<TContext>(action, next).InvokeAsync);
        }
        /// <summary>
        /// 加入一个中间件处理
        /// </summary>
        /// <typeparam name="TContext">上下文</typeparam>
        /// <param name="builder"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IMiddlewareBuilder<TContext> Use<TContext>(this IMiddlewareBuilder<TContext> builder, Func<Handler<TContext>, Handler<TContext>> func)
        {
            builder.Handlers.Add(func);
            return builder;
        }
        /// <summary>
        /// 加入一个中间件
        /// </summary>
        /// <typeparam name="TContext">上下文类型</typeparam>
        /// <param name="builder">建造器</param>
        /// <param name="middleware">中间件实例</param>
        public static void Use<TContext>(this IMiddlewareBuilder<TContext> builder, IMiddleware<TContext> middleware)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (middleware is null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }
            builder.Use(next => context => middleware.InvokeAsync(context, next));
        }
        /// <summary>
        /// 在运行时从上下文取出中间件并使用
        /// </summary>
        /// <typeparam name="TContext">上下文类型</typeparam>
        /// <param name="builder"></param>
        /// <param name="middlewareTypeGetter">中间件实例获取器</param>
        public static void Use<TContext>(this IMiddlewareBuilder<TContext> builder, Func<TContext,IMiddleware<TContext>> middlewareTypeGetter)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (middlewareTypeGetter is null)
            {
                throw new ArgumentNullException(nameof(middlewareTypeGetter));
            }

            builder.Use((context) => middlewareTypeGetter(context));
        }
    }
}
