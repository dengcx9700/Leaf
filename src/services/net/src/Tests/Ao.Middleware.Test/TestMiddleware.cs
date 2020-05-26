using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Middleware.Test
{
    [TestClass]
    public class TestMiddleware
    {
        [TestMethod]
        public void TestAddMiddleware()
        {
            var builder = new MiddlewareBuilder<Adder>();
            builder.Use((ctx) =>
#if NET452
                Task.FromResult(0)
#else
                Task.CompletedTask
#endif
            );
            Assert.AreEqual(1, builder.Handlers.Count);
        }
        [TestMethod]
        public void TestUseMiddleware()
        {
            var builder = new MiddlewareBuilder<Adder>();
            for (int i = 0; i < 3; i++)
            {
                builder.Use(new AddOneMiddleware());
            }
            var root =builder.Build();
            var adder = new Adder();
            root(new MiddlewareContext<Adder>(null, adder));
            Assert.AreEqual(3, adder.Sum);
        }
        [TestMethod]
        public void TestAbortMiddleware()
        {
            var builder = new MiddlewareBuilder<Adder>();
            for (int i = 0; i < 3; i++)
            {
                builder.Use(new AddOneMiddleware());
            }
            builder.Use(new CheckIfMiddleware(3));
            builder.Use(new AddOneMiddleware());
            var root = builder.Build();
            var adder = new Adder();
            root(new MiddlewareContext<Adder>(null,adder));
            Assert.AreEqual(3, adder.Sum);
        }
    }
    public class CheckIfMiddleware : IMiddleware<Adder>
    {
        private readonly int target;

        public CheckIfMiddleware(int target)
        {
            this.target = target;
        }

        public Task InvokeAsync(MiddlewareContext<Adder> context, Handler<Adder> next)
        {
            if (context.Context.Sum>=target)
            {
#if NET452
                return Task.FromResult(0);
#else
                return Task.CompletedTask;
#endif
            }
            return next(context);
        }
    }
    public class AddOneMiddleware : IMiddleware<Adder>
    {
        public Task InvokeAsync(MiddlewareContext<Adder> context, Handler<Adder> next)
        {
            context.Context.Sum++;
            return next(context);
        }
    }
    public class Adder
    {
        public int Sum { get; set; }
    }
}
