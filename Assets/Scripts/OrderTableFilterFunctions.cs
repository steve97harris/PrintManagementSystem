using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderTableFilterFunctions : MonoBehaviour
    {
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
        
        private List<OrderEntry> GetOrderEntries()
        {
            var savedOrders = OrderTable.GetSavedOrders();
            var orderEntries = savedOrders.orderEntries;
            return orderEntries;
        }
    }
}