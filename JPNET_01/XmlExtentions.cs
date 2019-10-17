using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace JPNET_01
{
    static class XmlExtentions
    {
        public static IEnumerable<XElement> Find(this XObject xObject, int minValue)
        {
            return xObject.Document.
                Descendants("country").
                Where(c => int.Parse(c.Attribute("population").Value.Split(' ')[0]) > minValue);
        }
    }
}
