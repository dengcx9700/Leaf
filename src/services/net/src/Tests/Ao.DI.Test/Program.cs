using Ao.DI.InjectWay;
using Ao.DI.Lookup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ao.DI.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var sc = new ServiceCollection();
            sc.AddTransient<A>();
            sc.AddTransient<B>();
            var p =sc.BuildServiceProvider();//new ExpressionServiceCreator()
            var st = Stopwatch.GetTimestamp();
            for (int i = 0; i < 10_000_000; i++)
            {
                p.GetService<B>();
                //516.4563
                //GC0: 1,GC1: 1,GC2: 1


            }
            Console.WriteLine(new TimeSpan(Stopwatch.GetTimestamp()-st).TotalMilliseconds);
            Console.WriteLine($"GC0:{GC.CollectionCount(0)},GC1:{GC.CollectionCount(1)},GC2:{GC.CollectionCount(2)}");
        }
    }
    [AoService(ServiceLifetime.Singleton,typeof(A))]
    public interface IA
    {

    }
    public interface IB
    {

    }
    public class A:IA
    {
        public A()
        {
            //Console.WriteLine(GetHashCode());
        }
    }
    [AoService(ServiceLifetime.Transient)]
    public class C 
    {
        public C(B b,IA a)
        {
           //Console.WriteLine(a.GetHashCode());
        }
    }
    [AoService(ServiceLifetime.Scoped)]
    public class B:IB
    {
        public B(A a)
        {
            //Console.WriteLine(a.GetHashCode());
        }
    }
}
