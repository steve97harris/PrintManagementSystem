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
            private FileSystemWatcher watcher;
            
            void Start()
            {
                Watcher();
            }

            void Update()
            {
                
            }

            private void Watcher()
            {
                var path = @"c:\YR\ExampleOrders\Basket\";
                watcher = new FileSystemWatcher
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

                // OrderContainerManager.AddOrderEntry(xmlBasicInfo[0], xmlBasicInfo[1], xmlBasicInfo[2], xmlBasicInfo[3], xmlBasicInfo[4]);
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
                basicInfo[1] = dateNode[0].InnerText;;
                basicInfo[2] = "New";
                basicInfo[3] = infoNode[0].InnerText;;
                basicInfo[4] = customerNameNode[0].InnerText;

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