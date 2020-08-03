using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
            SetBasicOrderInfo();
        }

        #region Buttons
        
        public void OrderNumberClicked()
        {
            var uniqueCodeOfOrderClicked = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;

            Instantiate(orderDetailsCanvas, OrderWatcher.PrintManagementSystem.transform);

            SetFilePaths(uniqueCodeOfOrderClicked);

            SetBasketData(_basketDataPath);

            InitialiseMetaData();

            InitialisePrintData();

            var basketMap = XmlReader.ExtractXmlData(_basketDataPath);
            var itemMetaList = GetMetaDataLists();
            
            ReceiptCreator.CreateReceiptPdf(basketMap, itemMetaList);
            
            _pmsMainCanvas = GameObjectFinder.FindSingleObjectByName("PMSMainCanvas");
            _pmsMainCanvas.SetActive(false);
        }

        private void InitialiseMetaData()
        {
            if (MetaDataPaths.Count != 0)
            {
                SetMetaData(MetaDataPaths[0]);
                SetItemNumberText(_currentMetaIndex, MetaDataPaths.Count, "MetaNumber");
            }
            else if (MetaDataPaths.Count == 0)
            {
                SetInvalidMetaDataText("Meta file not found");
                SetItemNumberText(-1,0,"MetaNumber");
            }
        }

        private void InitialisePrintData()
        {
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

        #region Private Methods

        private void SetBasicOrderInfo()
        {
            var uniqueCode = orderEntry.GetComponent<OrderEntryUniqueCode>().entryUniqueCode;
            SetFilePaths(uniqueCode);

            var metaDataLists = GetMetaDataLists();
            float totalPrice = 0;
            foreach (var pair in metaDataLists.SelectMany(map => map.Where(pair => pair.Key == "ITEM_PRICE")))
            {
                float.TryParse(pair.Value, out var itemPrice);
                totalPrice += itemPrice;
            }

            var basketMap = XmlReader.ExtractXmlData(_basketDataPath);
            var date = basketMap["TIMESTAMP"];
            date = date.Remove(10, 13);
            
            var day = date.Substring(8, 2);
            var month = date.Substring(5, 2);
            var year = date.Substring(0, 4);
            date = day + "-" + month + "-" + year;
            
            
            orderEntry.transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>().text = uniqueCode;
            orderEntry.transform.GetChild(2).GetComponent<TMP_Text>().text = date;
            orderEntry.transform.GetChild(3).GetComponent<TMP_Text>().text = basketMap["COLLECTION_NAME"] + " " + basketMap["SURNAME"];
            orderEntry.transform.GetChild(4).GetComponent<TMP_Text>().text = metaDataLists.Count.ToString();
            orderEntry.transform.GetChild(5).GetComponent<TMP_Text>().text = totalPrice.ToString(CultureInfo.InvariantCulture);
        }
        
        private void SetFilePaths(string uniqueCode)
        {
            MetaDataPaths.Clear();
            PrintThumbnailPathsPerOrder.Clear();
            
            var basicBasketPath = SavesPath + @"Basket\";
            var basketPaths = new List<string>();
            SetFilePathArrays(basicBasketPath, basketPaths, uniqueCode);
            _basketDataPath = basketPaths[0];

            var basicMetaPath = SavesPath + @"Meta\";
            SetFilePathArrays(basicMetaPath, MetaDataPaths, uniqueCode);

            var basicPrintThumbnailPath = SavesPath + "Print";
            SetFilePathArrays(basicPrintThumbnailPath, PrintThumbnailPathsPerOrder, uniqueCode);
        }

        private void SetFilePathArrays(string basicPath, List<string> pathsArray, string uniqueCode)
        {
            var files = Directory.GetFiles(basicPath);
            
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Contains(uniqueCode)) 
                    continue;
                
                if (!pathsArray.Contains(files[i]))
                    pathsArray.Add(files[i]);
            }
        }
        
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
            var xmlFileMap = XmlReader.ExtractXmlData(filePath);

            var dataDisplay = GameObjectFinder.FindSingleObjectByName(gameObjectName);
            var dataString = "";
            foreach (var pair in xmlFileMap)
            {
                dataString += ($"{pair.Key}: {pair.Value}" + System.Environment.NewLine);
            }

            dataDisplay.GetComponent<TextMeshProUGUI>().text = dataString;
        }

        private void SetArtworkThumbnail(int printIndex)
        {
            var artworkDisplay = GameObjectFinder.FindSingleObjectByName("ArtworkThumbnail");
            var sprite = SpriteCreator.LoadNewSprite(PrintThumbnailPathsPerOrder[printIndex]);
            artworkDisplay.GetComponent<Image>().sprite = sprite;
        }

        private List<Dictionary<string, string>> GetMetaDataLists()
        {
            var itemMetaList = new List<Dictionary<string,string>>();
            for (int i = 0; i < MetaDataPaths.Count; i++)
            {
                var metaMap = XmlReader.ExtractXmlData(MetaDataPaths[i]);
                itemMetaList.Add(metaMap);
            }

            return itemMetaList;
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