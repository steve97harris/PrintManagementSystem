using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
// using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class OrderDetails : MonoBehaviour
    {
        [SerializeField] private GameObject orderDetailsCanvas;
        [SerializeField] private GameObject orderEntry;
        
        private GameObject _pmsMainCanvas;

        private string _metaPath = "";
        private string _basketPath = "";
        private string _printThumbnailPath = "";

        public const string SavesPath = @"C:\YR\Saves\";

        private void Start()
        {
            _pmsMainCanvas = GameObject.Find("PMSMainCanvas");
        }

        public void OrderNumberClicked()
        {
            SetFilePaths();
            InstantiateMetaData();

            _pmsMainCanvas.SetActive(false);
        }

        private void InstantiateMetaData()
        {
            var metaData = ExtractXmlData(_metaPath);

            Instantiate(orderDetailsCanvas, OrderManager.pms.transform);
            var metaDataDisplay = transform.Find("/PrintManagementSystem/OrderDetailsCanvas(Clone)/OrderData/AllMeta").gameObject;
            
            var metaUi = "";
            foreach (var pair in metaData)
            { 
                metaUi += $"{pair.Key}: {pair.Value}" + System.Environment.NewLine;
            }
            
            metaDataDisplay.GetComponent<TextMeshProUGUI>().text = metaUi;
        }

        private void SetFilePaths()
        {
            _basketPath = orderEntry.GetComponent<OrderEntryUI>().basketDataPath;

            var orderUniqueCode = orderEntry.GetComponent<OrderEntryUI>().entryUniqueCode.text;
            var orderMetaFileName = orderEntry.GetComponent<OrderEntryUI>().entryMetaData.text;
            
            var basicMetaPath = SavesPath + @"Meta\";
            _metaPath = basicMetaPath + orderMetaFileName;
            
            var basicPrintThumbnailPath = SavesPath + "Print";
            var allPrintThumbnailFiles = Directory.GetFiles(basicPrintThumbnailPath);
            
            for (int i = 0; i < allPrintThumbnailFiles.Length; i++)
            {
                if (allPrintThumbnailFiles[i].Contains(orderUniqueCode))
                {
                    _printThumbnailPath = allPrintThumbnailFiles[i];
                }
            }
            
            Debug.LogError("MetaPath: " + _metaPath);
            Debug.LogError("BasketPath: " + _basketPath);
            Debug.LogError("PrintPath: " + _printThumbnailPath);
        }

        private Dictionary<string,string> ExtractXmlData(string xmlDocPath)
        {
            var doc = XDocument.Load(xmlDocPath);
            var x = doc.Descendants();

            var map = new Dictionary<string,string>();
            foreach (var element in x)
            {
                map.Add(element.Name.ToString(), element.Value);
            }

            map.Remove(map.ElementAt(0).Key);

            return map;
        }
    }
}