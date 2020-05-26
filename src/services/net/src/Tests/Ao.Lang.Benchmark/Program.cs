using Ao.Core;
using Ao.Lang.Sources;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Ao.Lang.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TestClass>();
            Console.ReadLine();
        }
    }
    [MemoryDiagnoser]
    public class TestClass
    {
        private readonly ILanguageService langSer;
        public const string ResourcesBlock = "Resources";
        private static string BasePath => AppDomain.CurrentDomain.BaseDirectory;
        private string GetPath(params string[] blocks)
        {
            return Path.Combine(blocks);
        }
        public TestClass()
        {
            langSer = new LanguageService();
            langSer.Add(new CurrentCultureLanguageMetadata(new ResxConfigurationSource
            {
                FileProvider = new PhysicalFileProvider(BasePath),
                Path = GetPath(ResourcesBlock, "Test.resx")
            }));
        }
        [Benchmark]
        public void TestLoad()
        {
            langSer.ReBuild();
            _ = langSer.GetCurrentRoot();
        }

        [Benchmark]
        public void TestErrorGet()
        {
            _ = langSer.GetCurrentRoot()["1:1232:312"];
        }
        [Benchmark]
        public void TestGet()
        {
            _ = langSer.GetCurrentRoot()["String77"];
        }

    }
}
