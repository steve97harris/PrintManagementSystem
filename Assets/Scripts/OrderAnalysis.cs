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
    public class OrderAnalysis : MonoBehaviour
    {
        [SerializeField] private GameObject orderDetailsCanvas;
        [SerializeField] private GameObject orderEntry;
        
        private GameObject _pmsMainCanvas;

        private static string _basketPath = "";

        private static int _currentMetaIndex = 0;
        private static readonly List<string> MetaDataPaths = new List<string>();
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
            
            var basicMetaPath = SavesPath + @"Meta\";
            var metaDataFiles = Directory.GetFiles(basicMetaPath);
            
            for (int i = 0; i < metaDataFiles.Length; i++)
            {
                if (metaDataFiles[i].Contains(orderUniqueCode))
                {
                    if (!MetaDataPaths.Contains(metaDataFiles[i]))
                        MetaDataPaths.Add(metaDataFiles[i]);
                }
            }

            foreach (var sPath in MetaDataPaths)
            {
                Debug.LogError(sPath);
            }
            
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
            MetaDataPaths.Clear();
            
            Instantiate(orderDetailsCanvas, OrderWatcher.PrintManagementSystem.transform);
            
            Debug.LogError("-----------------------------");
            GetFilePaths();
            
            SetBasketData(_basketPath);
            
            SetMetaData(MetaDataPaths[0]);
    
            if (PrintThumbnailPathsPerOrder.Count != 0)
                SetArtworkThumbnail(_currentPrintIndex);
            
            _pmsMainCanvas.SetActive(false);
        }

        public void MetaDataRightButton()
        {
            _currentMetaIndex++;

            if (_currentMetaIndex > MetaDataPaths.Count - 1)
            {
                _currentMetaIndex = MetaDataPaths.Count - 1;
                return;
            }
            
            Debug.LogError("currentMetaPath: " + MetaDataPaths[_currentMetaIndex]);
            SetMetaData(MetaDataPaths[_currentMetaIndex]);
        }
        
        public void MetaDataLeftButton()
        {
            _currentMetaIndex--;

            if (_currentMetaIndex < 0)
            {
                _currentMetaIndex = 0;
                return;
            }

            SetMetaData(MetaDataPaths[_currentMetaIndex]);
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

        private void SetBasketData(string filePath)       
        {
            SetData(filePath, "/PrintManagementSystem/OrderDetailsCanvas(Clone)/OrderInformationOptions/OrderBasketData/BasketData");
        }
        
        private void SetMetaData(string filePath)    
        {
            SetData(filePath, "/PrintManagementSystem/OrderDetailsCanvas(Clone)/OrderInformationOptions/OrderMetaData/MetaData");
        }

        private void SetData(string filePath, string gameObjectPath)
        {
            var dataMap = XmlReader.ExtractXmlData(filePath);

            var dataDisplay = transform.Find(gameObjectPath).gameObject;
            
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