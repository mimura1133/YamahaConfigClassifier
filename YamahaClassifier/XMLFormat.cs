using System.Collections.Generic;
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
        [XmlElement("Format")] public string Format;
        [XmlElement("Model")] public string Model;
        [XmlElement("Name")] public string Name;
        [XmlElement("Setting")] public string Setting;
        [XmlElement("Title")] public string Title;
    }
}