using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Language.Intellisense;
using Yamaha_scraping;

namespace YamahaClassifier
{
    internal static class YamahaData
    {
        public static List<string> Keywords;
        public static List<YamahaXMLElement> Elements;
        public static List<Completion> CompList;

        public static void LoadData()
        {
            using (var fs = File.OpenRead("yamaha.xml"))
            {
                var serializer = new XmlSerializer(typeof (YamahaXMLRoot));
                var data = serializer.Deserialize(fs) as YamahaXMLRoot;

                var keywords = new List<string>();
                foreach (var element in data.Element)
                {
                    var n = element.Name.Split(" ".ToCharArray());
                    for (var i = 0; i < n.Length; i++)
                    {
                        var key = "";
                        for (var j = 0; j <= i; j++)
                        {
                            key += n[j] + " ";
                        }
                        if (!keywords.Contains(key))
                            keywords.Add(key.Substring(0, key.Length - 1));
                    }
                }
                Keywords = keywords;

                Elements = data.Element;

                CompList =
                    new List<Completion>(Elements.Select(
                        n =>
                            new Completion(n.Name, n.Name,
                                n.Title + "\n----\n" + n.Format + "\n---\n" + n.Setting + "\n---\nSupported : " +
                                n.Model, null, null)));
            }
        }
    }
}