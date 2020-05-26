using Ao.DI.Lookup;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ao.DI.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BenckmarkTest>();
        }
    }
    public class BenckmarkTest
    {
        private IServiceProvider provider;
        [GlobalSetup]
        public void Setup()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<C>();
            sc.AddScoped<B>();
            sc.AddTransient<A>();
            sc.AddTransient<ArefBC>();
            provider=sc.BuildServiceProvider(new ExpressionServiceCreator());
        }
        [Benchmark]
        public void Null1_000_000()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                provider.GetService<Program>();
            }
        }
        [Benchmark]
        public void Singleton100()
        {
            for (int i = 0; i < 100; i++)
            {
                provider.GetService<C>();
            }
        }
        [Benchmark]
        public void Singleton1_000_000()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                provider.GetService<C>();
            }
        }
        [Benchmark]
        public void Singleton10_000_000()
        {
            for (int i = 0; i < 10_000_000; i++)
            {
                provider.GetService<C>();
            }
        }
        [Benchmark]
        public void Scoped100()
        {
            for (int i = 0; i < 100; i++)
            {
                provider.GetService<B>();
            }
        }
        [Benchmark]
        public void Scoped1_000_000()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                provider.GetService<B>();
            }
        }
        [Benchmark]
        public void Scoped10_000_000()
        {
            for (int i = 0; i < 10_000_000; i++)
            {
                provider.GetService<B>();
            }
        }

        [Benchmark]
        public void Transient100()
        {
            for (int i = 0; i < 100; i++)
            {
                provider.GetService<A>();
            }
        }
        [Benchmark]
        public void Transient1_000_000()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                provider.GetService<A>();
            }
        }
        [Benchmark]
        public void Transient10_000_000()
        {
            for (int i = 0; i < 10_000_000; i++)
            {
                provider.GetService<A>();
            }
        }

        [Benchmark]
        public void Ref100()
        {
            for (int i = 0; i < 100; i++)
            {
                provider.GetService<ArefBC>();
            }
        }
        [Benchmark]
        public void Ref1_000_000()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                provider.GetService<ArefBC>();
            }
        }
        [Benchmark]
        public void Ref10_000_000()
        {
            for (int i = 0; i < 10_000_000; i++)
            {
                provider.GetService<ArefBC>();
            }
        }
    }
    public class A
    {

    }
    public class B
    {

    }
    public class C
    {

    }
    public class ArefBC
    {
        public ArefBC(B b,C c)
        {

        }
    }
}
