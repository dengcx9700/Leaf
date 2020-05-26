using Ao.Plug.Filling;
using Ao.Plug.NetCore;
using Ao.Plug.Test.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ao.Plug.Test
{
    [TestClass]
    public class TestPlugManager
    {
        private string dllPath => new Uri(GetType().Assembly.GetName().CodeBase).LocalPath;
        [TestMethod]
        public void TestFind()
        {
            var plugManager = new PlugManager();
            plugManager.Add(new FilePlugSourceProvider(Environment.CurrentDirectory, dllPath));
            var lookup=plugManager.Build();
            var plugTypes = lookup.Gets(x => typeof(Adder).IsAssignableFrom(x.TargetType));
            Assert.AreEqual(plugTypes.Length, 1);
        }
        [TestMethod]
        public void TestUnload()
        {
            var plugManager = new PlugManager();
            plugManager.Add(new FilePlugSourceProvider(Environment.CurrentDirectory, dllPath));
            foreach (var item in plugManager)
            {
                item.Dispose();
            }            
        }
        [TestMethod]
        public void TestFill()
        {
            var plugManager = new PlugManager();
            plugManager.Add(new FilePlugSourceProvider(Environment.CurrentDirectory, dllPath));
            var lookup = plugManager.Build();
            var fill = new TestFiller();
            lookup.MakeFillerFromObject(fill)
                .Fill();
            Assert.AreEqual(fill.Adders.Count, 1);
        }
        [TestMethod]
        public void TestFill_ThrowIfNotClass()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var plugManager = new PlugManager();
                plugManager.Add(new FilePlugSourceProvider(Environment.CurrentDirectory, dllPath));
                var lookup = plugManager.Build();
                var fill = new TestFiller();
                lookup.MakeFillerFromObject(111)
                    .Fill();
            });
        }
    }
    public class TestFiller
    {
        public TestFiller()
        {
            Adders = new List<Adder>();
        }
        [DirectFill(typeof(Adder))]
        public List<Adder> Adders { get; }
    }
    public class NormalAdder : Adder
    {
        public override int Add(int a, int b)
        {
            return a + b;
        }
    }
}
