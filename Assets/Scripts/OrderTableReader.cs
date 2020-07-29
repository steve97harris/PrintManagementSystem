using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class OrderTableReader : MonoBehaviour
    {
        [SerializeField] private GameObject orderEntryContent;

        private List<OrderEntry> GetOrderEntries()
        {
            var savedOrders = OrderTable.GetSavedOrders();
            var orderEntries = savedOrders.orderEntries;
            return orderEntries;
        }

        private List<GameObject> GetOrderEntryGameObjects()
        {
            var orderEntryObjects = new List<GameObject>();
            for (int i = 0; i < orderEntryContent.transform.childCount; i++)
            {
                var orderEntryObject = orderEntryContent.transform.GetChild(i).gameObject;
                orderEntryObjects.Add(orderEntryObject);
            }

            return orderEntryObjects;
        }

        public void UpdateOrderWithRespectToStatus()
        {
            var orderEntries = GetOrderEntries();

            var statusCollection = new List<List<OrderEntry>>();
            var newOrders = new List<OrderEntry>();
            var inProgressOrders = new List<OrderEntry>();
            var printQueue1Orders = new List<OrderEntry>();
            var collectionOrders = new List<OrderEntry>();
            var trashOrders = new List<OrderEntry>();
            for (int i = 0; i < orderEntries.Count; i++)
            {
                switch (orderEntries[i].currentStatus)
                {
                    case "New":
                        newOrders.Add(orderEntries[i]);
                        break;
                    case "In Progress":
                        inProgressOrders.Add(orderEntries[i]);
                        break;
                    case "Print Queue 1":
                        printQueue1Orders.Add(orderEntries[i]);
                        break;
                    case "Collection":
                        collectionOrders.Add(orderEntries[i]);
                        break;
                    case "Trash":
                        trashOrders.Add(orderEntries[i]);
                        break;
                }
            }
            statusCollection.Add(newOrders);
            statusCollection.Add(inProgressOrders);
            statusCollection.Add(printQueue1Orders);
            statusCollection.Add(collectionOrders);
            statusCollection.Add(trashOrders);

            var newOrder = new List<OrderEntry>();
            for (int i = 0; i < statusCollection.Count; i++)
            {
                for (int j = 0; j < statusCollection[i].Count; j++)
                {
                    newOrder.Add(statusCollection[i][j]);
                }
            }

            var orderEntryObjects = GetOrderEntryGameObjects();
            for (int i = 0; i < newOrder.Count; i++)
            {
                for (int j = 0; j < orderEntryObjects.Count; j++)
                {
                    if (orderEntryObjects[j].GetComponent<OrderEntry>().uniqueCode == newOrder[i].uniqueCode)
                    {
                        orderEntryObjects[j].transform.position = orderEntryObjects[i].transform.position;
                    }
                }
            }
        }
    }
}