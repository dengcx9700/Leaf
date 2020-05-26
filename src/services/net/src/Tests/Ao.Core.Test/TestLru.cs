using Ao.Core.Lru;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Core.Test
{
    [TestClass]
    public class TestLru
    {
        [TestMethod]
        public void TestAddAndGet()
        {
            var lru = new LruCacher<string,int>(5);
            lru.Add("1", 2);
            lru.Add("3", 4);
            lru.Add("100", 1);
            var get3 = lru.Get("3");
            var get100 = lru.Get("100");
            Assert.AreEqual(4, get3);
            Assert.AreEqual(1, get100);
        }
        [TestMethod]
        public void TestGetNone()
        {
            var lru = new LruCacher<string, int>(5);
            lru.Add("1", 2);
            lru.Add("3", 4);
            lru.Add("100", 1);
            var get3 = lru.Get("100000000");
            Assert.AreEqual(0, get3);
        }
        [TestMethod]
        public void TestAddOverflowAndGet()
        {
            var lru = new LruCacher<string, int>(3);
            lru.Add("1", 2);
            lru.Add("3", 4);
            lru.Add("100", 1);
            lru.Get("1");
            lru.Add("444", 30);
            var get1 = lru.Get("1");
            var get444 = lru.Get("444");
            Assert.AreEqual(2, get1);
            Assert.AreEqual(30, get444);
        }
        [TestMethod]
        public void TestTaskAdd()
        {
            const int taskCount = 100;
            const int runCount= 10_000;
            var lru = new LruCacher<string, int>(100);
            var tasks = new Task[taskCount];
            var random = new Random();
            var max = taskCount * runCount;
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < runCount; j++)
                    {
                        lru.Add(random.Next(1, max).ToString(), j);
                    }
                });
            }
            Task.WaitAll(tasks);
        }
    }
}
