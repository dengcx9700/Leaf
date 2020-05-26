using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Resource.Test
{
    [TestClass]
    public class ResouceServiceTest
    {
        private ResourceService Get(string path)
        {
            var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            var rs = new ResourceService(p);
            rs.AddIncludeFilePattern(".*");
            return rs;
        }
        [TestMethod]
        public void TestLoadResource()
        {
            var rs = Get("TestPath1");
            rs.AddResourceLoader(new BinResourceLoader());
            var all = rs.Root.ResourceMedatas;
            Assert.AreEqual(1, all.Count);
        }
        [TestMethod]
        public void TestFindResource()
        {
            var rs = Get("TestPath1");
            rs.AddResourceLoader(new BinResourceLoader());
            var metadata = rs.FindAndEnsure<BinResourceMetadata>("testfile1.txt");
            Assert.IsNotNull(metadata);
        }
    }
    public class BinResourceLoader : IResourceLoader
    {
        public int Order => -100;

        public string ExtensionName => "*";

        public IResourceMetadata Load(ResourceLoadContext context)
        {
            return new BinResourceMetadata(context.FilePath);
        }
    }
    public class TextReousceLoader : IResourceLoader
    {
        public int Order => 0;

        public string ExtensionName => ".txt";

        public IResourceMetadata Load(ResourceLoadContext context)
        {
            return new BinResourceMetadata(context.FilePath);
        }
    }
    public class BinResourceMetadata : FileResourceMetadata
    {
        public BinResourceMetadata(string filePath) 
            : base(filePath)
        {
        }
    }
}
