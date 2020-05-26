using Ao.Core;
using Ao.Lang.Sources;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Ao.Lang.Test
{
    [TestClass]
    public class LanguageServiceTest
    {
        public const string ResourcesBlock = "Resources";
        private static string BasePath => AppDomain.CurrentDomain.BaseDirectory;
        private string GetPath(params string[] blocks)
        {
            return Path.Combine(blocks);
        }
        [TestMethod]
        public void TestResx()
        {
            var langSer = new LanguageService();
            langSer.Add(new CurrentCultureLanguageMetadata(new ResxConfigurationSource
            {
                FileProvider = new PhysicalFileProvider(BasePath),
                Path = GetPath(ResourcesBlock, "Test.resx")
            }));
            var root=langSer.GetCurrentRoot();
            Assert.IsNotNull(root);
            var str = root["Resx.Title"];
            Assert.AreEqual("hello", str);
        }
        [TestMethod]
        public void TestJson()
        {
            var langSer = new LanguageService();
            langSer.Add(new CurrentCultureLanguageMetadata(new JsonConfigurationSource
            {
                FileProvider = new PhysicalFileProvider(BasePath),
                Path = GetPath(ResourcesBlock, "Test.json")
            }));
            var root = langSer.GetCurrentRoot();
            Assert.IsNotNull(root);
            var str = root["Json.Title"];
            Assert.AreEqual("hello", str);
        }
        [TestMethod]
        public void TestJson_Inner()
        {
            var langSer = new LanguageService();
            langSer.Add(new CurrentCultureLanguageMetadata(new JsonConfigurationSource
            {
                FileProvider = new PhysicalFileProvider(BasePath),
                Path = GetPath(ResourcesBlock, "Test.json")
            }));
            var root = langSer.GetCurrentRoot();
            Assert.IsNotNull(root);
            var str = root["Json.Arg:Name1"];
            Assert.AreEqual("name1", str);
        }
        [TestMethod]
        public void TestXml_Inner()
        {
            var langSer = new LanguageService();
            langSer.Add(new CurrentCultureLanguageMetadata(new XmlConfigurationSource(DefaultXmlAcquirer.ValueInAttribute("langs","name","value"))
            {
                FileProvider = new PhysicalFileProvider(BasePath),
                Path = GetPath(ResourcesBlock, "Test.xml")
            }));
            var root = langSer.GetCurrentRoot();
            Assert.IsNotNull(root);
            var str = root["langs:Xml:Title"];
            Assert.AreEqual("hello", str);
        }
    }
}
