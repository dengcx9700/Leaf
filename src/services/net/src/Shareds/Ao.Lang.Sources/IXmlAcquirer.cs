using System.Xml;

namespace Ao.Lang.Sources
{
    /// <summary>
    /// xml数据获取器
    /// </summary>
    public interface IXmlAcquirer
    {
        /// <summary>
        /// 根节点
        /// </summary>
        string RootNode { get; }
        /// <summary>
        /// 获取节点提供的名字
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        string GetName(XmlNode node);
        /// <summary>
        /// 获取节点提供的值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        string GetValue(XmlNode node);
    }
}
