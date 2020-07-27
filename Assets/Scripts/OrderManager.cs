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
                var path = @"C:\YR\L11470_USC\Saves\Basket";
                var watcher = new FileSystemWatcher
                {
                    Path = path, 
                    NotifyFilter = NotifyFilters.LastWrite, 
                    Filter = "*.*"
                };
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnCreated);
                watcher.EnableRaisingEvents = true;
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
                
                var basicInfo = new string[5];
                basicInfo[0] = uniqueCodeNode[0].InnerText;;
                basicInfo[1] = customerNameNode[0].InnerText;;
                basicInfo[2] = dateNode[0].InnerText;
                basicInfo[3] = infoNode[0].InnerText;;
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