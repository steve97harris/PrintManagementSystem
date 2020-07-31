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
                
            var orderBasicInfo = ReadBasicXmlInfo(e);
                
            for (int i = 0; i < orderBasicInfo.Length; i++)
            {
                CurrentOrder[i] = orderBasicInfo[i];
            }
        }
        
        private string[] ReadBasicXmlInfo(FileSystemEventArgs e)
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

        public void BackToHomeScreen()
        {
            var homeScreenCanvas = PrintManagementSystem.transform.Find("PMSMainCanvas").gameObject;
            var orderDetailsCanvas = PrintManagementSystem.transform.Find("OrderDetailsCanvas(Clone)").gameObject;
            
            homeScreenCanvas.SetActive(true);
            Destroy(orderDetailsCanvas);
        }
    }
}