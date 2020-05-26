using Ao.Core;
using Ao.SavableConfig;
using Ao.SavableConfig.Benchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;

[assembly:JsonConfigurationSource("a.txt",typeof(TestSettings))]

namespace Ao.SavableConfig.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TestClass>();
            Console.ReadLine();
        }
    }
    
    public class TestSettings
    {
        public bool UseDefault { get; set; }

        public int Delay { get; set; }
    }
    [MemoryDiagnoser]
    public class TestClass
    {
        private readonly SettingDesignerService<int> designerSer;
        private readonly SettingService service;
        private readonly SettingIniter<int> initer;
        private readonly Random random;
        public TestClass()
        {
            random = new Random();
            designerSer = new SettingDesignerService<int>();
            service = new SettingService();
            initer = new SettingIniter<int>(service, designerSer);
            initer.WithAssembly(GetType().Assembly);
        }

        [Benchmark]
        public void TestChange()
        {
            var node=designerSer.SettingMap.GetNode<TestSettings, int>(x => x.Delay);
            if (node!=null)
            {
                node.Value = random.Next(0,500000);
            }
        }
    }
}
