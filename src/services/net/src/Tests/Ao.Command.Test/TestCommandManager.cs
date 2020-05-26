using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Command.Test
{
    [TestClass]
    public class TestCommandManager
    {
        [TestMethod]
        public async Task TestFindAndInvokeCommand()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new AddCommand()));
            var commander=manager.BuildDefault();
            var context=commander.GetContext("add 111 222");
            Assert.AreEqual(context.Name, "add");
            Assert.IsNotNull(context);
            var res=await commander.GetExecuter(context).ExecuteAsync(context);
            Assert.AreEqual(true, res.Succeed);
            Assert.IsInstanceOfType(res.Result, typeof(int));
            Assert.AreEqual(333, res.Result);
        }
        [TestMethod]
        public async Task TestParamterLess()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new AddCommand()));
            var commander = manager.BuildDefault();
            var context =await commander.ExecuteCommandAsync("add 111");
            Assert.IsNull(context);
        }
        [TestMethod]
        public async Task TestInvokeAsyncCommand()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new AsyncAddCommand()));
            var commander = manager.BuildDefault();
            var res = await commander.ExecuteCommandAsync("addasync 1 2");
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Succeed);
            var realyResult = res.GetRealyResult();
            Assert.AreEqual(3, realyResult);
        }
        [TestMethod]
        public async Task TestPrefxInvokeCommand()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new PrefxCommand()));
            var commander = manager.BuildDefault();
            var res = await commander.ExecuteCommandAsync("calc:addone 2");
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Succeed);
            var realyResult = res.GetRealyResult();
            Assert.AreEqual(3, realyResult);
        }
        [TestMethod]
        public async Task TestPrefxInMethodInvokeCommand()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new PrefxInMethodCommand()));
            var commander = manager.BuildDefault();
            var res = await commander.ExecuteCommandAsync("inmethod:addone 2");
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Succeed);
            var realyResult = res.GetRealyResult();
            Assert.AreEqual(3, realyResult);
        }
        [TestMethod]
        public void TestNotCommand()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new TestNoCommand()));
            var commander = manager.BuildDefault();
            Assert.AreEqual(0, commander.CommandExecuters.Length);
        }
        [TestMethod]
        public void TestFailExcute()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new TestNoCommand()));
            var commander = manager.BuildDefault();
            var ctx = commander.ExecuteCommandAsync("dasdasdsa");
            Assert.IsNotNull(ctx);
        }
        [TestMethod]
        public async Task TestWriteParamters()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new AliasCommand()));
            var commander = manager.BuildDefault();
            var res = await commander.ExecuteCommandAsync("addone aa=1");
            Assert.AreEqual(2, res.GetRealyResult());
            res = await commander.ExecuteCommandAsync("ao 2");
            Assert.AreEqual(3, res.GetRealyResult());
            res = await commander.ExecuteCommandAsync("ao aa=3");
            Assert.AreEqual(4, res.GetRealyResult());
        }
        [TestMethod]
        public async Task TestUseDefaultArgs()
        {
            var manager = new CommandManager();
            manager.Add(ObjectCommandSource.FromObjectIgnore(new DefaultArgCommand()));
            var commander = manager.BuildDefault();
            var res = await commander.ExecuteCommandAsync("add a=1");
            Assert.AreEqual(2, res.GetRealyResult());
            res = await commander.ExecuteCommandAsync("add b=3");
            Assert.IsNull(res);
            res = await commander.ExecuteCommandAsync("add 333");
            Assert.AreEqual(334, res.GetRealyResult());
        }
    }
    public class DefaultArgCommand
    {
        public int Add(int a,int b=1)
        {
            return a + b;
        }
    }
}
