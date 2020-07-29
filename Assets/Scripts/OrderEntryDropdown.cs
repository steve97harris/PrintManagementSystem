using System;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class OrderEntryDropdown : MonoBehaviour
    {
        private string uniqueCode;
        private void Start()
        {
            var orderEntry = transform.parent.gameObject;
            uniqueCode = orderEntry.GetComponent<OrderEntryUI>().entryUniqueCode.text;
        }

        public void OnStatusChanged(int val)
        {
            switch (val)
            {
                case 0:            // New
                    SetCurrentStatusInJson("New");
                    break;
                case 1:            // In Prog
                    SetCurrentStatusInJson("In Progress");
                    break;
                case 2:            // Print Q 1
                    SetCurrentStatusInJson("Print Queue 1");
                    break;
                case 3:            // Collection
                    SetCurrentStatusInJson("Collection");
                    break;
                case 4:            // Trash
                    SetCurrentStatusInJson("Trash");
                    break;   
            }
        }

        private void SetCurrentStatusInJson(string status)
        {
            var savedOrders = OrderTable.GetSavedOrders();
            var x = savedOrders.orderEntries;
            foreach (var entry in x)
            {
                if (uniqueCode == entry.uniqueCode)
                {
                    entry.currentStatus = status;
                }
            }
            OrderTable.SaveOrders(savedOrders);
        }
    }
}