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

            var map = docElements.ToDictionary(element => element.Name.ToString(), element => element.Value);

            map.Remove(map.ElementAt(0).Key);

            return map;
        }
        
        public static string[] GetPrimaryXmlInfo(FileSystemEventArgs e)
        {
            var doc = new XmlDocument();
            doc.Load(e.FullPath);

            var uniqueCode = doc.GetElementsByTagName("UNIQUE_CODE")[0].InnerText;
            var customerName = doc.GetElementsByTagName("COLLECTION_NAME")[0].InnerText;
            var date = doc.GetElementsByTagName("TIMESTAMP")[0].InnerText;
            var metaData = doc.GetElementsByTagName("Item")[0].InnerText;
            var basketFilePath = e.FullPath;

            date = date.Remove(10, 13);
            
            var day = date.Substring(8, 2);
            var month = date.Substring(5, 2);
            var year = date.Substring(0, 4);
            date = day + "-" + month + "-" + year;
            
            var orderInfo = new string[6];
            orderInfo[0] = uniqueCode;
            orderInfo[1] = customerName;
            orderInfo[2] = date;
            orderInfo[3] = metaData;
            orderInfo[4] = basketFilePath;
            orderInfo[5] = "New";
                
            return orderInfo;
        }
    }
}