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
        private FileSystemWatcher watcher; 
        public static string[] currentOrder = new string[6];

        public static GameObject pms;
        void Start()
        {
            pms = GameObject.Find("PrintManagementSystem");
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
                currentOrder[i] = orderBasicInfo[i];
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
            var status = "New";
            var basketFilePath = e.FullPath;

            date = date.Remove(10, 13);
            
            var orderInfo = new string[6];
            orderInfo[0] = uniqueCode;
            orderInfo[1] = customerName;
            orderInfo[2] = date;
            orderInfo[3] = metaData;
            orderInfo[4] = status;
            orderInfo[5] = basketFilePath;
                
            return orderInfo;
        }
        
        public void Dispose()
        {
            // avoiding resource leak
            watcher.Changed -= OnChanged;
            this.watcher.Dispose();
        }

        public void BackToHomeScreen()
        {
            var homeScreenCanvas = pms.transform.Find("PMSMainCanvas").gameObject;
            var orderDetailsCanvas = pms.transform.Find("OrderDetailsCanvas(Clone)").gameObject;
            
            homeScreenCanvas.SetActive(true);
            Destroy(orderDetailsCanvas);
        }
    }
}