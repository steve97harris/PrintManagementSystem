﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderTableFilterFunctions : MonoBehaviour
    {
        private List<OrderEntry> GetOrderEntries()
        {
            var savedOrders = OrderTable.GetSavedOrders();
            var orderEntries = savedOrders.orderEntries;
            return orderEntries;
        }
        
        public void AllOrdersClicked()
        {
            DisplaySpecifiedOrders("All");
        }

        public void NewOrdersClicked()
        {
            DisplaySpecifiedOrders("New");
        }
        
        public void InProgressOrdersClicked()
        {
            DisplaySpecifiedOrders("In Progress");
        }
        
        public void PrintQueue1OrdersClicked()
        {
            DisplaySpecifiedOrders("Print Queue 1");
        }
        
        public void CollectionOrdersClicked()
        {
            DisplaySpecifiedOrders("Collection");
        }
        
        public void TrashOrdersClicked()
        {
            DisplaySpecifiedOrders("Trash");
        }

        private void DisplaySpecifiedOrders(string status)
        {
            var orderEntryObjects = GameObjectFinder.FindObjectsByName("OrderEntry(Clone)");

            if (status == "All")
            {
                foreach (var entryObject in orderEntryObjects)
                {
                    entryObject.SetActive(true);
                }
                return;
            }
            
            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                orderEntryObjects[i].SetActive(orderEntryObjects[i].GetComponent<OrderEntryUI>().entryStatus == status);
            }
        }

        public void ReorderByDate()
        {
            var orderEntryObjects = GameObjectFinder.FindObjectsByName("OrderEntry(Clone)");

            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                for (int j = i+1; j < orderEntryObjects.Length; j++)
                {
                    var yearI = GetYear(orderEntryObjects[i]);
                    var monthI = GetMonth(orderEntryObjects[i]);
                    var dayI = GetDay(orderEntryObjects[i]);

                    var yearJ = GetYear(orderEntryObjects[j]);
                    var monthJ = GetMonth(orderEntryObjects[j]);
                    var dayJ = GetDay(orderEntryObjects[j]);

                    if (yearI < yearJ)
                    {
                        SwapTransformPositions(orderEntryObjects[i], orderEntryObjects[j]);
                    }

                    if (yearI != yearJ) 
                        continue;
                    
                    if (monthI < monthJ)
                    {
                        SwapTransformPositions(orderEntryObjects[i], orderEntryObjects[j]);
                    }

                    if (monthI != monthJ) 
                        continue;
                        
                    if (dayI < dayJ)
                    {
                        SwapTransformPositions(orderEntryObjects[i], orderEntryObjects[j]);
                    }
                }
            }
        }

        private void SwapTransformPositions(GameObject object01, GameObject object02)
        {
            var temp = object01.transform.position;
            object01.transform.position = object02.transform.position;
            object02.transform.position = temp;
        }

        private int GetYear(GameObject objectA)
        {
            var year = objectA.GetComponent<OrderEntryUI>().entryDate.text.Substring(6,4);
            int.TryParse(year, out var res);
            return res;
        }
        
        private int GetMonth(GameObject objectA)
        {
            var month = objectA.GetComponent<OrderEntryUI>().entryDate.text.Substring(3,2);
            int.TryParse(month, out var res);
            return res;
        }
        
        private int GetDay(GameObject objectA)
        {
            var day = objectA.GetComponent<OrderEntryUI>().entryDate.text.Substring(0,2);
            int.TryParse(day, out var res);
            return res;
        }
    }
}