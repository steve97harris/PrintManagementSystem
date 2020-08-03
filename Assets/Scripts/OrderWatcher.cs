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
        
        void Start()
        {
            PrintManagementSystem = GameObject.Find("PrintManagementSystem");
            Watcher();
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
                
            var orderUniqueCode = XmlReader.GetUniqueCode(e);
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