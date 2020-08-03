using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderTable : MonoBehaviour
    {
        [SerializeField] private int maxNumberOfEntries = 1000;
        [SerializeField] private GameObject orderEntryObject = null;
        private Transform _orderHolderTransform = null;

        private List<Transform> _orderCatalogueEntryTransformList;

        private string _previousOrder = "";

        private static string JsonTablePath => $"{Application.persistentDataPath}/orderTable.json";

        #region Event Functions

        private void Start()
        {
            var savedOrders = GetSavedOrders();
            var orderEntryContentHolder = GameObjectFinder.FindObjectsByName("OrderEntryContent");
            _orderHolderTransform = orderEntryContentHolder[0].transform;

            _orderCatalogueEntryTransformList = new List<Transform>();
            UpdateUi(savedOrders, _orderCatalogueEntryTransformList);

            SaveOrders(savedOrders);
        }

        private void Update()
        {
            if (OrderWatcher.CurrentOrderUniqueCode == "" || OrderWatcher.CurrentOrderUniqueCode == _previousOrder) 
                return;

            var currentOrderArray = OrderWatcher.CurrentOrderUniqueCode;
            SetOrderEntryInfo(currentOrderArray);

            _previousOrder = OrderWatcher.CurrentOrderUniqueCode;
        }

        #endregion

        #region New Order Entry Functions

        private void SetOrderEntryInfo(string uniqueCode)
        {
            AddOrderEntry(new OrderEntry()
            {
                uniqueCode = uniqueCode,
                currentStatus = "New"
            });
        }

        private void AddOrderEntry(OrderEntry orderEntry)
        {
            var savedOrders = GetSavedOrders();

            if (savedOrders.orderEntries.Count < maxNumberOfEntries)
                savedOrders.orderEntries.Add(orderEntry);

            if (savedOrders.orderEntries.Count > maxNumberOfEntries)
            {
                Debug.LogError("Error - Max number of orders have been entered into order table");
            }
            
            _orderCatalogueEntryTransformList = new List<Transform>();
            UpdateUi(savedOrders, _orderCatalogueEntryTransformList);
            SaveOrders(savedOrders);
        }

        #endregion

        #region UI Functions

        private void UpdateUi(OrderTableSaveData savedOrders, List<Transform> transformList)
        {
            foreach (Transform child in _orderHolderTransform)
            {
                Destroy(child.gameObject);
            }

            foreach (var entry in savedOrders.orderEntries)
            {
                var templateHeight = 100f;
                var entryObject = Instantiate(orderEntryObject, _orderHolderTransform);
                entryObject.GetComponent<OrderEntryUniqueCode>().InitialiseOrderEntry(entry);
                var entryRectTransform = entryObject.GetComponent<RectTransform>();
                entryRectTransform.anchoredPosition = new Vector2(0,-templateHeight * transformList.Count);
                entryObject.gameObject.SetActive(true);
                
                SetStatusDropdownOption(entry.currentStatus, entryObject.transform);
                
                transformList.Add(entryObject.transform);
            }
        }

        private void SetStatusDropdownOption(string currentStatus, Transform entryTransform)
        {
            var dropdown = entryTransform.GetChild(6).gameObject;
            var val = 0;
            switch (currentStatus)
            {
                case "In Progress":
                    val = 1;
                    break;
                case "Print Queue 1":
                    val = 2;
                    break;
                case "Collection":
                    val = 3;
                    break;
                case "Trash":
                    val = 4;
                    break;
            }

            dropdown.GetComponent<TMP_Dropdown>().value = val;
        }

        #endregion

        #region Json Functions

        public static OrderTableSaveData GetSavedOrders()
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

        public static void SaveOrders(OrderTableSaveData orderTableSaveData)
        {
            using (StreamWriter stream = new StreamWriter(JsonTablePath))
            {
                var json = JsonUtility.ToJson(orderTableSaveData, true);
                stream.Write(json);
            }
        }

        #endregion
    }
}