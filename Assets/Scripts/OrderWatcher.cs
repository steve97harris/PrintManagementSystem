using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DefaultNamespace;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderWatcher : MonoBehaviour
    {
        public static string CurrentOrderUniqueCode = "";
        public static GameObject PrintManagementSystem;

        public static string _previousOrderCode;
        
        void Start()
        {
            PrintManagementSystem = GameObject.Find("PrintManagementSystem");
            CheckForExistingFiles();
            Watcher();
        }

        private void CheckForExistingFiles()
        {
            var basketPath = OrderAnalysis.SavesPath + @"Basket\";
            if (!Directory.Exists(basketPath))
            {
                var folderNames = new string[] {"Basket", "Mask", "Meta", "Print", "Thumbnail"};
                for (int i = 0; i < folderNames.Length; i++)
                {
                    Directory.CreateDirectory(OrderAnalysis.SavesPath + folderNames[i]);
                }
                
                Debug.LogError("Saves path not found, directory has been created");
            }
            else
            {
                var currentBasketFiles = Directory.GetFiles(basketPath);
                var orderManagerObj = GameObjectFinder.FindSingleObjectByName("OrderManager");
                var orderTable = orderManagerObj.GetComponent<OrderTable>();
                var orderTableSaveData = orderTable.GetSavedOrders();
                var uniqueCodesList = new List<string>();
                
                if (orderTableSaveData != null)
                {
                    var orderEntries = orderTableSaveData.orderEntries;
                    
                    for (int i = 0; i < orderEntries.Count; i++)
                    {
                        if (!uniqueCodesList.Contains(orderEntries[i].uniqueCode))
                            uniqueCodesList.Add(orderEntries[i].uniqueCode);
                    }
                }

                for (int i = 0; i < currentBasketFiles.Length; i++)
                {
                    var orderUniqueCode = XmlReader.GetUniqueCodeWithFilePath(currentBasketFiles[i]);
                    
                    if (!uniqueCodesList.Contains(orderUniqueCode))
                        orderTable.SetOrderEntryInfo(orderUniqueCode);
                }
            }
        }

        #region Watcher Functions

        private void Watcher()
        {
            var basketPath = OrderAnalysis.SavesPath + @"Basket\";
            var basketWatcher = new FileSystemWatcher
            {
                Path = basketPath, 
                NotifyFilter = NotifyFilters.LastWrite, 
                Filter = "*.*"
            };
            basketWatcher.Changed += new FileSystemEventHandler(OnChanged);
            basketWatcher.Created += new FileSystemEventHandler(OnCreated);
            basketWatcher.EnableRaisingEvents = true;
        }
            
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            //Copies file to another directory.
        }
        
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            Debug.LogError("File Created: " + e.Name + ", Path: " + e.FullPath);
                
            var orderUniqueCode = XmlReader.GetUniqueCodeWithEventsArgs(e);
            Debug.LogError(orderUniqueCode);

            CurrentOrderUniqueCode = orderUniqueCode;
        }

        #endregion
        
        #region Buttons

        public void BackToHomeScreen()
        {
            var homeScreenCanvas = PrintManagementSystem.transform.Find("PMSMainCanvas").gameObject;
            var orderDetailsCanvas = PrintManagementSystem.transform.Find("OrderDetailsCanvas(Clone)").gameObject;
            
            homeScreenCanvas.SetActive(true);
            Destroy(orderDetailsCanvas);
        }

        #endregion
    }
}