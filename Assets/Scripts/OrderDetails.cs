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
        private static int _currentPrintIndex = 0;
        private static readonly List<string> PrintThumbnailPathsPerOrder = new List<string>();

        public const string SavesPath = @"C:\YR\Saves\";

        private void Start()
        {
            _pmsMainCanvas = GameObject.Find("PMSMainCanvas");
            GetFilePaths();
        }

        #region File Path Functions

        private void GetFilePaths()
        {
            _basketPath = orderEntry.GetComponent<OrderEntryUi>().entryBasketDataPath;

            var orderUniqueCode = orderEntry.GetComponent<OrderEntryUi>().entryUniqueCode.text;
            var orderMetaFileName = orderEntry.GetComponent<OrderEntryUi>().entryMetaData.text;
            
            var basicMetaPath = SavesPath + @"Meta\";
            _metaPath = basicMetaPath + orderMetaFileName;
            
            var basicPrintThumbnailPath = SavesPath + "Print";
            var printThumbnailFiles = Directory.GetFiles(basicPrintThumbnailPath);
            
            for (int i = 0; i < printThumbnailFiles.Length; i++)
            {
                if (!printThumbnailFiles[i].Contains(orderUniqueCode)) 
                    continue;
                
                if (!PrintThumbnailPathsPerOrder.Contains(printThumbnailFiles[i]))
                    PrintThumbnailPathsPerOrder.Add(printThumbnailFiles[i]);
            }

            // Debug.LogError("MetaPath: " + _metaPath);
            // Debug.LogError("BasketPath: " + _basketPath);
            // Debug.LogError("PrintPath: " + _printThumbnailPath);
        }

        #endregion

        #region Buttons

        public void OrderNumberClicked()
        {
            PrintThumbnailPathsPerOrder.Clear();
            
            Instantiate(orderDetailsCanvas, OrderManager.PrintManagementSystem.transform);
            GetFilePaths();
            SetData(_basketPath);
    
            if (PrintThumbnailPathsPerOrder.Count != 0)
                SetArtworkThumbnail(_currentPrintIndex);
            
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

        public void ArtworkButtonRight()
        {
            _currentPrintIndex++;

            if (_currentPrintIndex > PrintThumbnailPathsPerOrder.Count - 1)
            {
                _currentPrintIndex = PrintThumbnailPathsPerOrder.Count - 1;
                return;
            }
            
            SetArtworkThumbnail(_currentPrintIndex);
        }

        public void ArtworkButtonLeft()
        {
            _currentPrintIndex--;

            if (_currentPrintIndex < 0)
            {
                _currentPrintIndex = 0;
                return;
            }

            SetArtworkThumbnail(_currentPrintIndex);
        }

        #endregion

        #region Implement Order Details

        private void SetData(string filePath)        // Needs updating to handle multiple meta files
        {
            var dataMap = XmlReader.ExtractXmlData(filePath);

            var dataDisplay = transform.Find("/PrintManagementSystem/OrderDetailsCanvas(Clone)/OrderInformationOptions/OrderData/OrderInformation").gameObject;
            
            var dataString = dataMap.Aggregate("", (current, pair) => current + ($"{pair.Key}: {pair.Value}" + System.Environment.NewLine));

            dataDisplay.GetComponent<TextMeshProUGUI>().text = dataString;
        }

        private void SetArtworkThumbnail(int printIndex)
        {
            var artworkDisplay = transform.Find("/PrintManagementSystem/OrderDetailsCanvas(Clone)/ArtworkPrintOptions/ArtworkThumbnail").gameObject;
            var sprite = SpriteCreator.LoadNewSprite(PrintThumbnailPathsPerOrder[printIndex]);
            artworkDisplay.GetComponent<Image>().sprite = sprite;
        }

        #endregion

    }
}