using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Yamaha_scraping
{
    [XmlRoot("Index")]
    public class YamahaXMLRoot
    {
        [XmlElement("command")]
        public List<YamahaXMLElement> Element { get; set; }
    }

    public class YamahaXMLElement
    {
        [XmlElement("Name")]
        public string Name;
        [XmlElement("Title")]
        public string Title;
        [XmlElement("Format")]
        public string Format;
        [XmlElement("Setting")]
        public string Setting;
        [XmlElement("Model")]
        public string Model;
    }
}
