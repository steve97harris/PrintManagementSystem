using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class OrderDetails : MonoBehaviour
    {
        [SerializeField] private GameObject orderDetailsCanvas;
        [SerializeField] private GameObject orderEntry;
        
        private GameObject _pmsMainCanvas;

        private static string _metaPath = "";
        private static string _basketPath = "";
        private static string _printThumbnailPath = "";
        
        public const string SavesPath = @"C:\YR\Saves\";

        private void Start()
        {
            _pmsMainCanvas = GameObject.Find("PMSMainCanvas");
            SetFilePaths();
        }

        public void OrderNumberClicked()
        {
            Instantiate(orderDetailsCanvas, OrderManager.PrintManagementSystem.transform);
            SetFilePaths();
            SetData(_basketPath);
            DisplayArtworkThumbnail();
            
            _pmsMainCanvas.SetActive(false);
        }

        public void DisplayMetaInfo()
        {
            SetData(_metaPath);
        }

        public void DisplayBasketInfo()
        {
            SetData(_basketPath);
        }

        private void DisplayArtworkThumbnail()
        {
            var artworkDisplay = transform.Find("/PrintManagementSystem/OrderDetailsCanvas(Clone)/ArtworkPrintOptions/ArtworkThumbnail").gameObject;
            var sprite = SpriteCreator.LoadNewSprite(_printThumbnailPath);
            artworkDisplay.GetComponent<Image>().sprite = sprite;
        }
        
        private void SetData(string filePath)        // Needs updating to handle multiple meta files
        {
            var dataMap = ExtractXmlData(filePath);

            var dataDisplay = transform.Find("/PrintManagementSystem/OrderDetailsCanvas(Clone)/OrderInformationOptions/OrderData/OrderInformation").gameObject;
            
            var dataString = dataMap.Aggregate("", (current, pair) => current + ($"{pair.Key}: {pair.Value}" + System.Environment.NewLine));

            dataDisplay.GetComponent<TextMeshProUGUI>().text = dataString;
        }

        private void SetFilePaths()
        {
            _basketPath = orderEntry.GetComponent<OrderEntryUI>().entryBasketDataPath;

            var orderUniqueCode = orderEntry.GetComponent<OrderEntryUI>().entryUniqueCode.text;
            var orderMetaFileName = orderEntry.GetComponent<OrderEntryUI>().entryMetaData.text;
            
            var basicMetaPath = SavesPath + @"Meta\";
            _metaPath = basicMetaPath + orderMetaFileName;
            
            var basicPrintThumbnailPath = SavesPath + "Print";
            var allPrintThumbnailFiles = Directory.GetFiles(basicPrintThumbnailPath);
            
            for (int i = 0; i < allPrintThumbnailFiles.Length; i++)
            {
                if (allPrintThumbnailFiles[i].Contains(orderUniqueCode))
                    _printThumbnailPath = allPrintThumbnailFiles[i];
            }
            
            // Debug.LogError("MetaPath: " + _metaPath);
            // Debug.LogError("BasketPath: " + _basketPath);
            // Debug.LogError("PrintPath: " + _printThumbnailPath);
        }

        private Dictionary<string,string> ExtractXmlData(string xmlDocPath)
        {
            var doc = XDocument.Load(xmlDocPath);
            var x = doc.Descendants();

            var map = x.ToDictionary(element => element.Name.ToString(), element => element.Value);

            map.Remove(map.ElementAt(0).Key);

            return map;
        }
    }
}