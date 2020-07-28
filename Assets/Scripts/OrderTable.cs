using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderTable : MonoBehaviour
    {
        [SerializeField] private int maxNumberOfEntries = 1000;
        [SerializeField] private Transform orderHolderTransform = null;
        [SerializeField] private GameObject orderEntryObject = null;
        
        private List<Transform> _orderCatalogueEntryTransformList;

        private string[] previousOrder = new string[6];

        public static string JsonTablePath => $"{Application.persistentDataPath}/orderTable.json";

        
        private void Start()
        {
            var savedOrders = GetSavedOrders();

            _orderCatalogueEntryTransformList = new List<Transform>();
            UpdateUI(savedOrders, _orderCatalogueEntryTransformList);

            SaveOrders(savedOrders);
        }

        private void Update()
        {
            // if unique code has changed, new order has arrived
            // if unique code is the same, new order will not be added
            if (OrderManager.currentOrder[0] != null && OrderManager.currentOrder[0] != previousOrder[0])
            {
                AddEntry(OrderManager.currentOrder[0], OrderManager.currentOrder[1], OrderManager.currentOrder[2], OrderManager.currentOrder[3], OrderManager.currentOrder[4], OrderManager.currentOrder[5]);
                for (int i = 0; i < previousOrder.Length; i++)
                {
                    previousOrder[i] = OrderManager.currentOrder[i];
                }
            }
        }

        private void AddEntry(string uniqueCode, string customerName, string date, string metaFile, string status, string basketPath)
        {
            AddEntry(new OrderEntry()
            {
                uniqueCode = uniqueCode,
                customerName = customerName,
                date = date,
                metaData = metaFile,
                status = status,
                basketDataPath = basketPath
            });
        }

        private void AddEntry(OrderEntry orderEntry)
        {
            var savedOrders = GetSavedOrders();

            if (savedOrders.orderEntries.Count < maxNumberOfEntries)
                savedOrders.orderEntries.Add(orderEntry);

            if (savedOrders.orderEntries.Count > maxNumberOfEntries)
            {
                Debug.LogError("Error - Max number of orders have been entered into order table");
            }
            
            _orderCatalogueEntryTransformList = new List<Transform>();
            UpdateUI(savedOrders, _orderCatalogueEntryTransformList);
            SaveOrders(savedOrders);
        }

        private void UpdateUI(OrderTableSaveData savedOrders, List<Transform> transformList)
        {
            foreach (Transform child in orderHolderTransform)
            {
                Destroy(child.gameObject);
            }

            foreach (var entry in savedOrders.orderEntries)
            {
                var templateHeight = 100f;
                var entryObject = Instantiate(orderEntryObject, orderHolderTransform);
                entryObject.GetComponent<OrderEntryUI>().Initialise(entry);
                var entryRectTransform = entryObject.GetComponent<RectTransform>();
                entryRectTransform.anchoredPosition = new Vector2(0,-templateHeight * transformList.Count);
                entryObject.gameObject.SetActive(true);
                
                transformList.Add(entryObject.transform);
            }
        }

        private OrderTableSaveData GetSavedOrders()
        {
            if (!File.Exists(JsonTablePath))
            {
                Debug.LogError("OrderTable does not exist - creating new one");
                
                File.Create(JsonTablePath).Dispose();
                return new OrderTableSaveData();
            }

            using (StreamReader stream = new StreamReader(JsonTablePath))
            {
                var json = stream.ReadToEnd();
                return JsonUtility.FromJson<OrderTableSaveData>(json);
            }
        }

        private void SaveOrders(OrderTableSaveData orderTableSaveData)
        {
            using (StreamWriter stream = new StreamWriter(JsonTablePath))
            {
                var json = JsonUtility.ToJson(orderTableSaveData, true);
                stream.Write(json);
            }
        }
    }
}