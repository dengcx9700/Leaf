using Ao.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pd.Services.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Menuing.Test
{
    [TestClass]
    public class MenuServiceTest
    {
        [TestMethod]
        public void TestInser()
        {
            var menuSer = new SingleMenuService();
            menuSer.Add(new NormalMenu());
            Assert.AreEqual(1, menuSer.Menu.Nexts.Count);
        }
        [TestMethod]
        public void TestNeiber()
        {
            var menuSer = new SingleMenuService();
            menuSer.Add(new FileMenu());
            menuSer.Add(new EditMenu());
            Assert.AreEqual(2, menuSer.Menu.Nexts.Count);
            Assert.AreEqual("file", menuSer.Menu.Nexts[0].Metadata.Id);
            Assert.AreEqual("edit", menuSer.Menu.Nexts[1].Metadata.Id);
        }
        [TestMethod]
        public void TestInner()
        {
            var menuSer = new SingleMenuService();
            menuSer.Add(new FileMenu());
            menuSer.Add(new OpenMenu());
            Assert.AreEqual(1, menuSer.Menu.Nexts.Count);
            Assert.AreEqual("file", menuSer.Menu.Nexts[0].Metadata.Id);
            Assert.AreEqual("open", menuSer.Menu.Nexts[0].Nexts[0].Metadata.Id);
        }
        [TestMethod]
        public void TestRemove_Root()
        {
            var menuSer = new SingleMenuService();
            var fm = new FileMenu();
            menuSer.Add(fm);
            menuSer.Add(new EditMenu());
            var mds=menuSer.Remove(fm);
            Assert.AreEqual(1, mds.Length);
            Assert.AreEqual(1, menuSer.Menu.Nexts.Count);
            Assert.AreEqual("edit", menuSer.Menu.Nexts[0].Metadata.Id);
        }
        [TestMethod]
        public void TestRevInsert()
        {
            var menuSer = new SingleMenuService();
            menuSer.Add(new EditMenu());
            menuSer.Add(new FileMenu());
            Assert.AreEqual(2, menuSer.Menu.Nexts.Count);
            Assert.AreEqual("file", menuSer.Menu.Nexts[0].Metadata.Id);
            Assert.AreEqual("edit", menuSer.Menu.Nexts[1].Metadata.Id);
        }
        [TestMethod]
        public void TestRemoveRoot()
        {
            var menuSer = new SingleMenuService();
            var x = new FileMenu();
            menuSer.Add(x);
            menuSer.Add(new OpenMenu());
            var val =menuSer.Remove(x);
            Assert.AreEqual(1, val.Length);
            Assert.AreEqual(0, menuSer.Menu.Nexts.Count);
        }
        [TestMethod]
        public void TestRemoveNode()
        {
            var menuSer = new SingleMenuService();
            var x = new OpenMenu();
            menuSer.Add(x);
            menuSer.Add(new FileMenu());
            var val=menuSer.Remove(x);
            Assert.AreEqual(1, val.Length);
            Assert.AreEqual(1, menuSer.Menu.Nexts.Count);
            Assert.AreEqual(0, menuSer.Menu.Nexts[0].Nexts.Count);
        }
    }
    public class FileMenu : MenuMetadataBase
    {
        public FileMenu()
        {
            Id = "file";
            ActionType = MenuActionTypes.InnerAfter;
        }
    }
    public class OpenMenu : MenuMetadataBase
    {
        public OpenMenu()
        {
            Id = "open";
            ActionType = MenuActionTypes.InnerAfter;
            InsertPath = DefaultMenuIds.Combine("file");
        }
    }
    public class EditMenu : MenuMetadataBase
    {
        public EditMenu()
        {
            Id = "edit";
            InsertPath = "file";
            ActionType = MenuActionTypes.After;
        }
    }
    public class NormalMenu : MenuMetadataBase
    {
        public NormalMenu()
        {
            Title = "hello";
            Id = "normal";
            ActionType = MenuActionTypes.InnerAfter;
        }
    }
}
