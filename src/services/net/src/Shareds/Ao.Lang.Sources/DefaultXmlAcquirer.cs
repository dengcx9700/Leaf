using System.Xml;

namespace Ao.Lang.Sources
{
    public class DefaultXmlAcquirer : IXmlAcquirer
    {
        enum Types
        {
            FromAttribute,
            FromText
        }
        private readonly Types valueType;

        private DefaultXmlAcquirer()
        {
        }

        private DefaultXmlAcquirer(Types valueType, string rootNode, string namePath, string valuePath)
        {
            if (string.IsNullOrEmpty(rootNode))
            {
                throw new System.ArgumentException("message", nameof(rootNode));
            }

            if (string.IsNullOrEmpty(namePath))
            {
                throw new System.ArgumentException("message", nameof(namePath));
            }

            if (string.IsNullOrEmpty(valuePath))
            {
                throw new System.ArgumentException("message", nameof(valuePath));
            }

            this.valueType = valueType;
            RootNode = rootNode;
            NamePath = namePath;
            ValuePath = valuePath;
        }

        public string RootNode { get; }

        public string NamePath { get; }

        public string ValuePath { get; }

        public string GetName(XmlNode node)
        {
            return node.Attributes[NamePath]?.Value;
        }

        public string GetValue(XmlNode node)
        {
            if (valueType== Types.FromAttribute)
            {
                return node.Attributes[ValuePath]?.Value;
            }
            return node.InnerText;
        }
        public static DefaultXmlAcquirer ValueInAttribute(string rootNode, string namePath, string valuePath)
        {
            return new DefaultXmlAcquirer(Types.FromAttribute, rootNode, namePath, valuePath);
        }
        public static DefaultXmlAcquirer ValueInText(string rootNode, string namePath, string valuePath)
        {
            return new DefaultXmlAcquirer(Types.FromText, rootNode, namePath, valuePath);
        }
    }
}
