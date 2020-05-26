using Ao.DI.Lookup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.DI.UnitTest
{
    [TestClass]
    public class BuildTest
    {
        private IServiceProvider provider;
        [TestInitialize]
        public void Init()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<A>();
            sc.AddScoped<B>();
            sc.AddTransient<C>();
            sc.LookUpAssembly();
            sc.AddScoped<ArefSB>();
            provider = sc.BuildServiceProvider(new ExpressionServiceCreator());
        }
        [TestMethod]
        public void TestNull()
        {
            var inst = provider.GetService<BuildTest>();
            Assert.IsNull(inst);
        }
        [TestMethod]
        public void TestSingleton()
        {
            var inst = provider.GetService<A>();
            Assert.IsNotNull(inst);
        }
        [TestMethod]
        public void TestSingleton_SameInst()
        {
            var inst = provider.GetService<A>();
            var inst2 = provider.GetService<A>();
            Assert.AreEqual(inst,inst2);
        }
        [TestMethod]
        public void TestScope()
        {
            var inst = provider.GetService<B>();
            Assert.IsNotNull(inst);
        }
        [TestMethod]
        public void TestTransient()
        {
            var inst = provider.GetService<C>();
            Assert.IsNotNull(inst);
        }
        [TestMethod]
        public void TestAttr()
        {
            var inst = provider.GetService<SA>();
            Assert.IsNotNull(inst);
        }
        [TestMethod]
        public void TestRefScope()
        {
            var inst = provider.GetService<ArefSB>();
            Assert.IsNotNull(inst);
        }
    }
}
