using Microsoft.Extensions.Configuration;
using System.IO;
using System.Xml;

namespace Ao.Lang.Sources
{
    public class XmlConfigurationProvider : FileConfigurationProvider
    {
        public XmlConfigurationProvider(FileConfigurationSource source, IXmlAcquirer acquirer)
            : base(source)
        {
            Acquirer=acquirer;
        }
        /// <summary>
        /// xml数据获取器
        /// </summary>
        public IXmlAcquirer Acquirer { get; }

        public override void Load(Stream stream)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            var node = xmlDoc.DocumentElement.SelectSingleNode("//" + Acquirer.RootNode);
            if (node!=null)
            {
                StepIn(node.Name, node);
            }
        }
        private void StepIn(string baseName,XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                var name = Acquirer.GetName(item);
                if (!string.IsNullOrEmpty(name))
                {
                    name = baseName + ":" + name;
                    var value = Acquirer.GetValue(item);
                    if (Data.ContainsKey(name))
                    {
                        Data[name] = value;
                    }
                    else
                    {
                        Data.Add(name, value);
                    }
                }
                StepIn(name, item);
            }
        }
    }
}
