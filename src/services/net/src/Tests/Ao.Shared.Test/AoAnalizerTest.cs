using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Shared.Test
{
    [TestClass]
    public class AoAnalizerTest
    {
        private AoAnalizer analizer;

        [TestInitialize]
        public void Init()
        {
            analizer = new AoAnalizer();
        }
        [TestMethod]
        public void TestAnalizeOnly()
        {
            var prop = new Person();
            var res=analizer.Analize(prop);
            Assert.IsTrue(res.MemberItems.Length == 2);
            Assert.IsTrue(res.MethodItems.Length == 0);
            Assert.IsTrue(res.Nexts.Length == 0);
            Assert.IsTrue(res.MemberItems.Any(m => m.ValueName == nameof(Person.Name)));
            Assert.IsTrue(res.MemberItems.Any(m => m.ValueName == nameof(Person.Age)));
        }
        [TestMethod]
        public void TestAnalizeNotStepIn()
        {
            var prop = new Human();
            var res = analizer.Analize(prop);
            Assert.IsTrue(res.MemberItems.Length == 2);
            Assert.IsTrue(res.MethodItems.Length == 0);
            Assert.IsTrue(res.Nexts.Length == 0);
        }
        [TestMethod]
        public void TestAnalizeLoop()
        {
            var h = new Human();
            var h2 = new Human();
            h.HumanNext = h2;
            h2.HumanNext = h;
            var res = analizer.Analize(h);
            Assert.AreEqual(res.Nexts.Length, 1);
            Assert.AreEqual(res.MemberItems.Length, 2);
        }
        [TestMethod]
        public void TestAnalizeList()
        {
            var sh = new SheHui();
            analizer.Settings.NotStepInArray = true;
            var res = analizer.Analize(sh);
            Assert.AreEqual(res.Nexts.Length, 0);
            Assert.AreEqual(res.MemberItems.Length, 1);
        }
        [TestMethod]
        public void TestAnalizeIgnore()
        {
            var obj = new Obj();
            var res = analizer.Analize(obj);
            Assert.AreEqual(res.Nexts.Length, 0);
            Assert.AreEqual(res.MemberItems.Length, 1);
            Assert.AreEqual(res.MemberItems[0].ValueName, nameof(Obj.Arg3));
        }
    }

    public class Person
    {
        public string Name { get; set; }

        public string Age { get; set; }
    }
    public class Human
    {
        [AoNotStepIn]
        public Person Person { get; set; } = new Person();

        public Human HumanNext { get; set; }
    }
    public class SheHui
    {
        public List<Person> People { get; } = new List<Person>();
    }
    public class Obj
    {
        [AoIgnore]
        public string Arg1 { get; set; }
        [AoIgnore]
        public string Arg2 { get; set; }
        public string Arg3 { get; set; }
    }
}
