using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DefaultNamespace;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderManager : MonoBehaviour
    {
            public static string[] currentOrder = new string[6]; 
            void Start()
            {
                Watcher();
            }

            private void Watcher()
            {
                var basketPath = OrderDetails._savesPath + @"\Basket";
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
                
                var xmlBasicInfo = ReadBasicXmlInfo(e);
                
                for (int i = 0; i < xmlBasicInfo.Length; i++)
                {
                    currentOrder[i] = xmlBasicInfo[i];
                }
            }
        
            private string[] ReadBasicXmlInfo(FileSystemEventArgs e)
            {
                var doc = new XmlDocument();
                doc.Load(e.FullPath);
                
                var uniqueCodeNode = doc.GetElementsByTagName("UNIQUE_CODE");
                var dateNode = doc.GetElementsByTagName("TIMESTAMP");
                var infoNode = doc.GetElementsByTagName("Item");
                var customerNameNode = doc.GetElementsByTagName("COLLECTION_NAME");
                
                var basicInfo = new string[6];
                basicInfo[0] = uniqueCodeNode[0].InnerText;
                basicInfo[1] = customerNameNode[0].InnerText;
                basicInfo[2] = dateNode[0].InnerText;
                basicInfo[3] = infoNode[0].InnerText;
                basicInfo[4] = "New";
                basicInfo[5] = e.FullPath;
                
                return basicInfo;
            }
        
            // public void Dispose()
            // {
            //     // avoiding resource leak
            //     watcher.Changed -= OnChanged;
            //     this.watcher.Dispose();
            // }
    }
}