using Ao.Core.Bytes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Core.Test
{
    [TestClass]
    public class TestByteBuilder
    {
        [TestMethod]
        public void TestAddInt()
        {
            var byteBuilder = new BytesBuilder();
            byteBuilder.Add(1);
            Assert.AreEqual(0x00, byteBuilder[0]);
            Assert.AreEqual(0x00, byteBuilder[1]);
            Assert.AreEqual(0x00, byteBuilder[2]);
            Assert.AreEqual(0x01, byteBuilder[3]);
        }
        [TestMethod]
        public void TestAddInt_MulBytes()
        {
            var byteBuilder = new BytesBuilder();
            byteBuilder.Add(0x12345678);
            Assert.AreEqual(0x12, byteBuilder[0]);
            Assert.AreEqual(0x34, byteBuilder[1]);
            Assert.AreEqual(0x56, byteBuilder[2]);
            Assert.AreEqual(0x78, byteBuilder[3]);
        }
        
    }
    
}
