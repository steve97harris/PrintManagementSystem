using System;
using System.Collections.Generic;
using System.IO;
// using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderDetails : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI uniqueCode;
        [SerializeField] private TextMeshProUGUI orderInfo;
        
        private string metaPath;
        private string printThumbnailPath;

        public static string _savesPath = @"C:\YR\Saves\";

        public void OrderNumberClicked()
        {
            var basicMetaPath = _savesPath + "Meta";
            metaPath = basicMetaPath + orderInfo.text;
            Debug.LogError("MetaPath: " + metaPath);
            
            var basicPrintThumbnailPath = _savesPath + "Print";

            var allPrintThumbnailFiles = Directory.GetFiles(basicPrintThumbnailPath, ".jpg");

            for (int i = 0; i < allPrintThumbnailFiles.Length; i++)
            {
                Debug.Log(allPrintThumbnailFiles[i]);
            }
        }
        
        private void GetSavedOrders()
        {
            if (!File.Exists(OrderTable.JsonTablePath))
            {
                Debug.LogError("OrderTable does not exist - can't extract order data");
            }

            using (StreamReader stream = new StreamReader(OrderTable.JsonTablePath))
            {
                var json = stream.ReadToEnd();
                // var x = JsonConvert.DeserializeObject<List<OrderEntry>>(json);
                // foreach (var orderEntry in x)
                // {
                //     // x
                // }
            }
        }
    }
}