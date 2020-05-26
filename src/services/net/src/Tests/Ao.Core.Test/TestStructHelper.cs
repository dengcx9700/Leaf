using Ao.Core.Bytes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ao.Core.Test
{
    [TestClass]
    public class TestStructHelper
    {
        [TestMethod]
        public void TestAsBytes()
        {
            var a = new TestA(111, "aaa");
            var bytes=StructHelper.ToByes(a);
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length > 0);
        }
        [TestMethod]
        public void TestExchanged()
        {
            var a = new TestA(111, "aaa");
            var bytes = StructHelper.ToByes(a);
            var b = StructHelper.ToStruce<TestA>(bytes);
            Assert.AreEqual(111, a.A);
            Assert.AreEqual("aaa", a.B);
        }
        [TestMethod]
        public void TestBytes_Validate()
        {
            var a = new MessageA(1, 0x1234);
            var bytes = StructHelper.ToByes(a);
            Assert.AreEqual(0x01, bytes[0]);
            Assert.AreEqual(0x12, bytes[1]);
            Assert.AreEqual(0x34, bytes[2]);
        }
        [TestMethod]
        public void TestStruct()
        {
            var bytes = StructHelper.ToByes(new TestStruct(0x1234, 0x56));
            Assert.AreEqual(0x12, bytes[0]);
            Assert.AreEqual(0x34, bytes[1]);
            Assert.AreEqual(0x56, bytes[2]);
        }
    }
    [StructLayout(LayoutKind.Sequential,Size =1)]
    public struct TestStruct
    {
        public short A;

        public byte B;

        public TestStruct(short a, byte b)
        {
            A = a;
            B = b;
        }
    }
    [StructLayout(LayoutKind.Sequential,Size =1)]
    public struct MessageA
    {
        public byte Index;

        public short Func;

        public MessageA(byte index, short func)
        {
            Index = index;
            Func = func;
        }
    }
    public struct TestA
    {
        public int A;

        public string B;

        public TestA(int a, string b)
        {
            A = a;
            B = b;
        }
    }
}
