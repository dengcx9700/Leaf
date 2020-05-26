using Ao.Command.Attributes;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Ao.Command.Benchmark
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
        private readonly ICommander commander;
        public TestClass()
        {
            var cm = new CommandManager();
            cm.Add(new ObjectCommandSource(new TestNoPrefxCommand()));
            cm.Add(new ObjectCommandSource(new TestPrefxCommand()));
            commander = cm.BuildDefault();
        }
        [Benchmark]
        public async Task TestInvokeAsync()
        {
            await commander.ExecuteCommandAsync("AddSix 111");
        }
        [Benchmark]
        public async Task TestMoreParamtersAsync()
        {
            await commander.ExecuteCommandAsync("AddSix 111 222 333");
        }
        [Benchmark]
        public async Task TestFailAsync()
        {
            await commander.ExecuteCommandAsync("adsadsa");
        }
        [Benchmark]
        public async Task TestLessParamtersAsync()
        {
            await commander.ExecuteCommandAsync("AddSix");
        }
        [Benchmark]
        public async Task TestPrefxInvokeAsync()
        {
            await commander.ExecuteCommandAsync("prefx:AddSix 11");
        }
        [Benchmark]
        public async Task TestAliasInvokeAsync()
        {
            await commander.ExecuteCommandAsync("prefx:six 11");
        }
    }
    public class TestNoPrefxCommand
    {
        public int AddOne(int a)
        {
            return a +1;
        }
        public int AddTwo(int a)
        {
            return a + 2;
        }
        public int AddThree(int a)
        {
            return a + 3;
        }
        public int AddFour(int a)
        {
            return a + 4;
        }
        public int AddFive(int a)
        {
            return a + 5;
        }
        public int AddSix(int a)
        {
            return a + 6;
        }
    }
    [Prefx("prefx")]
    public class TestPrefxCommand
    {
        public int AddOne(int a)
        {
            return a + 1;
        }
        public int AddTwo(int a)
        {
            return a + 2;
        }
        public int AddThree(int a)
        {
            return a + 3;
        }
        public int AddFour(int a)
        {
            return a + 4;
        }
        public int AddFive(int a)
        {
            return a + 5;
        }
        [Alias("six")]
        public int AddSix(int a)
        {
            return a + 6;
        }
    }

}
