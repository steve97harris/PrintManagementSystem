using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace DefaultNamespace
{
    public class OrderEntryDropdown : MonoBehaviour
    {
        private string _uniqueCode;
        private void Start()
        {
            var orderEntry = transform.parent.gameObject;
            _uniqueCode = orderEntry.GetComponent<OrderEntryUniqueCode>().entryUniqueCode;
        }

        #region Buttons

        public void OnStatusChanged(int val)
        {
            switch (val)
            {
                case 0:            
                    SetEntryStatusChange("New");
                    break;
                case 1:            
                    SetEntryStatusChange("In Progress");
                    break;
                case 2:            
                    SetEntryStatusChange("Print Queue 1");
                    break;
                case 3:            
                    SetEntryStatusChange("Collection");
                    break;
                case 4:            
                    SetEntryStatusChange("Trash");
                    break;   
            }
        }

        #endregion

        #region Private Methods

        private void SetEntryStatusChange(string status)
        {
            var savedOrders = OrderTable.GetSavedOrders();
            var orderEntries = savedOrders.orderEntries;
            var orderEntryObject = transform.parent.gameObject;
            
            foreach (var entry in orderEntries.Where(entry => _uniqueCode == entry.uniqueCode))
            {
                entry.currentStatus = status;
                orderEntryObject.GetComponent<OrderEntryUniqueCode>().entryStatus = status;
            }
            
            OrderTable.SaveOrders(savedOrders);
        }

        #endregion
    }
}