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

        private static string _basketDataPath = "";

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
            _basketDataPath = orderEntry.GetComponent<OrderEntryUi>().entryBasketDataPath;

            var basicMetaPath = SavesPath + @"Meta\";
            SetFilePathArrays(basicMetaPath, MetaDataPaths);

            var basicPrintThumbnailPath = SavesPath + "Print";
            SetFilePathArrays(basicPrintThumbnailPath, PrintThumbnailPathsPerOrder);
        }

        private void SetFilePathArrays(string basicPath, List<string> pathsArray)
        {
            var orderUniqueCode = orderEntry.GetComponent<OrderEntryUi>().entryUniqueCode.text;
            var printThumbnailFiles = Directory.GetFiles(basicPath);
            
            for (int i = 0; i < printThumbnailFiles.Length; i++)
            {
                if (!printThumbnailFiles[i].Contains(orderUniqueCode)) 
                    continue;
                
                if (!pathsArray.Contains(printThumbnailFiles[i]))
                    pathsArray.Add(printThumbnailFiles[i]);
            }
        }

        #endregion

        #region Buttons

        public void OrderNumberClicked()
        {
            MetaDataPaths.Clear();
            PrintThumbnailPathsPerOrder.Clear();
            
            Instantiate(orderDetailsCanvas, OrderWatcher.PrintManagementSystem.transform);
            
            GetFilePaths();
            
            SetBasketData(_basketDataPath);

            if (MetaDataPaths.Count != 0)
            {
                SetMetaData(MetaDataPaths[0]);
                SetItemNumberText(_currentMetaIndex, MetaDataPaths.Count, "MetaNumber");
            }
            else if (MetaDataPaths.Count == 0)
            {
                SetItemNumberText(-1,0,"MetaNumber");
                SetInvalidMetaDataText("Meta file not found");
            }

            if (PrintThumbnailPathsPerOrder.Count != 0)
            {
                SetArtworkThumbnail(_currentPrintIndex);
                SetItemNumberText(_currentPrintIndex, PrintThumbnailPathsPerOrder.Count, "ArtworkNumber");
            }
            else if (PrintThumbnailPathsPerOrder.Count == 0)
            {
                DisplayInvalidPrintPathText();
                SetItemNumberText(-1, 0, "ArtworkNumber");
            }
            
            _pmsMainCanvas.SetActive(false);
        }

        public void MetaDataRightButton()
        {
            _currentMetaIndex = UpIndex(_currentMetaIndex, MetaDataPaths.Count);
            SetMetaData(MetaDataPaths[_currentMetaIndex]);
            SetItemNumberText(_currentMetaIndex, MetaDataPaths.Count, "MetaNumber");
        }
        
        public void MetaDataLeftButton()
        {
            _currentMetaIndex = DownIndex(_currentMetaIndex);
            SetMetaData(MetaDataPaths[_currentMetaIndex]);
            SetItemNumberText(_currentMetaIndex, MetaDataPaths.Count, "MetaNumber");
        }

        public void ArtworkButtonRight()
        {
            _currentPrintIndex = UpIndex(_currentPrintIndex, PrintThumbnailPathsPerOrder.Count);
            SetArtworkThumbnail(_currentPrintIndex);
            SetItemNumberText(_currentPrintIndex, PrintThumbnailPathsPerOrder.Count, "ArtworkNumber");
        }

        public void ArtworkButtonLeft()
        {
            _currentPrintIndex = DownIndex(_currentPrintIndex);
            SetArtworkThumbnail(_currentPrintIndex);
            SetItemNumberText(_currentPrintIndex, PrintThumbnailPathsPerOrder.Count, "ArtworkNumber");
        }

        private int DownIndex(int index)
        {
            index--;

            if (index >= 0) 
                return index;
            
            index = 0;
            return index;

        }

        private int UpIndex(int index, int arrayCount)
        {
            index++;

            if (index <= arrayCount - 1) 
                return index;
            
            index = arrayCount - 1;
            return index;

        }

        private void SetItemNumberText(int index, int arrayCount, string objectName)
        {
            var artworkNumberObjects = GameObjectFinder.FindObjectsByName(objectName);
            var artworkNumberObject = artworkNumberObjects[0];
            artworkNumberObject.GetComponent<TMP_Text>().text = (index + 1) + "/" + arrayCount;
        }

        #endregion

        #region Implement Order Details

        private void SetBasketData(string filePath)       
        {
            SetData(filePath, "BasketData");
        }
        
        private void SetMetaData(string filePath)    
        {
            SetData(filePath, "MetaData");
        }

        private void SetData(string filePath, string gameObjectName)
        {
            var dataMap = XmlReader.ExtractXmlData(filePath);

            var dataDisplay = GameObjectFinder.FindSingleObjectByName(gameObjectName);
            var dataString = dataMap.Aggregate("", (current, pair) => current + ($"{pair.Key}: {pair.Value}" + System.Environment.NewLine));

            dataDisplay.GetComponent<TextMeshProUGUI>().text = dataString;
        }

        private void SetArtworkThumbnail(int printIndex)
        {
            var artworkDisplay = GameObjectFinder.FindSingleObjectByName("ArtworkThumbnail");
            var sprite = SpriteCreator.LoadNewSprite(PrintThumbnailPathsPerOrder[printIndex]);
            artworkDisplay.GetComponent<Image>().sprite = sprite;
        }

        private void DisplayInvalidPrintPathText()
        {
            var invalidPrintPathTextObject = GameObjectFinder.FindSingleObjectByName("InvalidPrintPathText");
            invalidPrintPathTextObject.SetActive(true);
        }

        private void SetInvalidMetaDataText(string text)
        {
            var metaText = GameObjectFinder.FindSingleObjectByName("MetaData");
            metaText.GetComponent<TMP_Text>().text = text;
            metaText.GetComponent<TMP_Text>().color = Color.red;
        }

        #endregion

    }
}