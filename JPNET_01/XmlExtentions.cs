using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JPNET_01
{
    static class XmlExtentions
    {

        public static XmlNode GetByAttribute(this IEnumerable<XmlNode> nodes, string attr, string value)
        {
            return nodes.FirstOrDefault(c => c.Attributes[attr].Value == value);
        }

        public static IEnumerable<XmlNode> ToEnumerable(this XmlNodeList list)
        {
            var nodes = new List<XmlNode>();

            foreach (XmlNode node in list)
            {
                nodes.Add(node);
            }
            return nodes;
        }

        public static IEnumerable<XmlNode> GetElementsByName(this XmlNode nodes, string name)
        {
            return nodes.ChildNodes.ToEnumerable().Where(n => n.Name == name);
        }
    }
}
