using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DefaultNamespace;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    // private FileSystemWatcher watcher;
    void Start()
    {
        Watcher();
    }

    private void Watcher()
    {
        var path = @"c:\YR\ExampleOrders\Meta\";
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
        
        var doc = new XmlDocument();
        doc.Load(e.FullPath);
        var uniqueCode = doc.GetElementsByTagName("UNIQUE_CODE");
        Debug.Log("unique code = " + uniqueCode[0].InnerText);

        var orderPanel = gameObject.AddComponent<OrderContainerManager>();
        // orderPanel.AddOrderEntry();
    }

    // public void Dispose()
    // {
    //     // avoiding resource leak
    //     watcher.Changed -= OnChanged;
    //     this.watcher.Dispose();
    // }
}
