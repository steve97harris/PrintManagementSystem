using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class XmlReader : MonoBehaviour
    {
        public static Dictionary<string,string> ExtractXmlData(string xmlDocPath)
        {
            var doc = XDocument.Load(xmlDocPath);
            var docElements = doc.Descendants();

            var map = new Dictionary<string, string>();
            foreach (var element in docElements)
            {
                if (!map.ContainsKey(element.Name.ToString()))
                    map.Add(element.Name.ToString(), element.Value);
            }

            map.Remove(map.ElementAt(0).Key);

            return map;
        }

        public static string GetUniqueCode(FileSystemEventArgs e)
        {
            var doc = new XmlDocument();
            doc.Load(e.FullPath);

            var uniqueCode = doc.GetElementsByTagName("UNIQUE_CODE")[0].InnerText;
            return uniqueCode;
        }
    }
}