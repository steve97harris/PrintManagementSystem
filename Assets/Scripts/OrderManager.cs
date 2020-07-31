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
    public class OrderManager : MonoBehaviour
    {
        public static readonly string[] CurrentOrder = new string[6];
        public static GameObject PrintManagementSystem;
        
        void Start()
        {
            PrintManagementSystem = GameObject.Find("PrintManagementSystem");
            Watcher();
        }

        #region Watcher Functions

        private void Watcher()
        {
            var basketPath = OrderDetails.SavesPath + @"\Basket";
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
                
            var orderBasicInfo = XmlReader.GetPrimaryXmlInfo(e);
                
            for (int i = 0; i < orderBasicInfo.Length; i++)
            {
                CurrentOrder[i] = orderBasicInfo[i];
            }
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