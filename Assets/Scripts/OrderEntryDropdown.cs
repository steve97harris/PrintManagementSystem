using System;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class OrderEntryDropdown : MonoBehaviour
    {
        private string _uniqueCode;
        private void Start()
        {
            var orderEntry = transform.parent.gameObject;
            _uniqueCode = orderEntry.GetComponent<OrderEntryUI>().entryUniqueCode.text;
        }

        public void OnStatusChanged(int val)
        {
            switch (val)
            {
                case 0:            // New
                    SetEntryStatus("New");
                    break;
                case 1:            // In Prog
                    SetEntryStatus("In Progress");
                    break;
                case 2:            // Print Q 1
                    SetEntryStatus("Print Queue 1");
                    break;
                case 3:            // Collection
                    SetEntryStatus("Collection");
                    break;
                case 4:            // Trash
                    SetEntryStatus("Trash");
                    break;   
            }
        }

        private void SetEntryStatus(string status)
        {
            var savedOrders = OrderTable.GetSavedOrders();
            var orderEntries = savedOrders.orderEntries;
            
            foreach (var entry in orderEntries.Where(entry => _uniqueCode == entry.uniqueCode))
            {
                entry.currentStatus = status;
            }
            
            OrderTable.SaveOrders(savedOrders);
        }
    }
}