using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderTableFilterFunctions : MonoBehaviour
    {
        #region Buttons

        public void AllOrdersClicked()
        {
            FilterOrdersByStatus("All");
        }

        public void NewOrdersClicked()
        {
            FilterOrdersByStatus("New");
        }
        
        public void InProgressOrdersClicked()
        {
            FilterOrdersByStatus("In Progress");
        }
        
        public void PrintQueue1OrdersClicked()
        {
            FilterOrdersByStatus("Print Queue 1");
        }
        
        public void CollectionOrdersClicked()
        {
            FilterOrdersByStatus("Collection");
        }
        
        public void TrashOrdersClicked()
        {
            FilterOrdersByStatus("Trash");
        }

        public void ReorderByDate()
        {
            var downArrow = GameObjectFinder.FindSingleObjectByName("DateFilter");
            downArrow.transform.rotation = Quaternion.Euler(0,0, 90);
            
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

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
        
        #endregion

        #region Button Helper Functions

        private void FilterOrdersByStatus(string status)
        {
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

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
                orderEntryObjects[i].SetActive(orderEntryObjects[i].GetComponent<OrderEntryUniqueCode>().entryStatus == status);
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
            var year = objectA.transform.GetChild(2).GetComponent<TMP_Text>().text.Substring(6,4);
            int.TryParse(year, out var res);
            return res;
        }
        
        private int GetMonth(GameObject objectA)
        {
            var month = objectA.transform.GetChild(2).GetComponent<TMP_Text>().text.Substring(3,2);
            int.TryParse(month, out var res);
            return res;
        }
        
        private int GetDay(GameObject objectA)
        {
            var day = objectA.transform.GetChild(2).GetComponent<TMP_Text>().text.Substring(0,2);
            int.TryParse(day, out var res);
            return res;
        }

        #endregion
    }
}