using Microsoft.Extensions.Configuration;

namespace Ao.Lang.Sources
{
    public class XmlConfigurationSource : FileConfigurationSource
    {
        public XmlConfigurationSource(IXmlAcquirer xmlAcquirer)
        {
            XmlAcquirer = xmlAcquirer ?? throw new System.ArgumentNullException(nameof(xmlAcquirer));
        }
        /// <summary>
        /// xml获取器
        /// </summary>
        public IXmlAcquirer XmlAcquirer { get; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new XmlConfigurationProvider(this, XmlAcquirer);
        }
    }
}
