using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;
using System.Xml.Serialization;

namespace Yamaha_scraping
{
    internal class Program
    {
        private static string baseurl = "http://www.rtpro.yamaha.co.jp/RT/manual/rt-common/";

        private static YamahaXMLElement ParsePage(string url)
        {
            var ret = new YamahaXMLElement();

            var doc = new HtmlDocument();
            doc.LoadHtml(Encoding.GetEncoding("UTF-8").GetString(new WebClient().DownloadData(url)).Replace("\n\n", "\n"));

            ret.Title = doc.DocumentNode.ChildNodes["html"].ChildNodes["head"].ChildNodes["title"].InnerText;


            var section =
                doc.DocumentNode.ChildNodes["html"].ChildNodes["body"].ChildNodes.First(n => n.Name == "div")
                    .SelectNodes("div[@class='section']");

            foreach (var element in section)
            {
                switch (element.ChildNodes["h2"].InnerText)
                {
                    case "[書式]":
                        ret.Format = string.Join("\n", element.ChildNodes["ul"].ChildNodes.Select(
                            n =>
                                n.ChildNodes.Where(m => m.Name == "span")
                                    .Where(m => m != null)
                                    .SelectMany(m => m.InnerText))
                            .Select(m => WebUtility.HtmlDecode(new string(m.ToArray())))
                            .Where(m => m != "")
                            .ToArray());
                        break;

                    case "[設定値及び初期値]":
                    {
                        string rtemp = "";

                        ret.Setting = "";
                        var t = new StringReader(element.ChildNodes["ul"].ChildNodes["li"].InnerText.Replace("\t","").Replace("\n\n\n","\n\n"));
                        while (true)
                        {
                            var line = t.ReadLine();
                            if (line == null) break;

                            if (line.IndexOf("[") > 0)
                            {
                                line = line.Substring(0, line.IndexOf("[")) + "\n\n" + line.Substring(line.IndexOf("["));
                            }


                            if (line.IndexOf("[設定値]") > 0) continue;
                            if (line.IndexOf("初期値") > 0) break;
                            rtemp += line + "\n";
                        }

                        bool parenthesissw = false;
                        t = new StringReader(rtemp);
                        while (true)
                        {
                            var line = t.ReadLine();
                            if (line == "")
                            {
                                if (parenthesissw)
                                    ret.Setting += "\n";
                                parenthesissw = false;
                                continue;
                            }
                            if (line == null) break;

                            if (parenthesissw)
                                ret.Setting += " / " + line + "";
                            else
                                ret.Setting += line + "";

                            parenthesissw = true;
                        }
                    }
                        break;

                    case "[適用モデル]":
                        ret.Model = element.ChildNodes["span"].InnerText;
                        break;

                    default:
                        break;
                }
            }

            return ret;
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Download Index...");
            var index =
                new StringReader(
                    Encoding.GetEncoding("EUC-JP").GetString(new WebClient().DownloadData(
                        baseurl + "cmdref_index.html"))
                    );

            var elements = new List<YamahaXMLElement>();
            while (true)
            {
                string line = index.ReadLine();
                if (line == null) break;
                if (line.IndexOf("a href") == -1) continue;


                line = line.Substring(line.IndexOf("a href") + 7);
                var key = line.Substring(line.IndexOf(">") + 1);
                var addr = line.Substring(0, line.IndexOf(">"));

                key = key.Substring(0, key.IndexOf("</a>"));

                Console.WriteLine(key + " -- " + addr);

                var element = ParsePage(baseurl + addr);
                element.Name = key;

                elements.Add(element);

                Thread.Sleep(100);
            }

            using (var fs = File.OpenWrite("yamaha.xml"))
            {
                var serializer = new XmlSerializer(typeof (YamahaXMLRoot));
                var data = new YamahaXMLRoot();
                data.Element = elements;
                serializer.Serialize(fs, data);
                fs.Flush();
            }
        }
    }
}
