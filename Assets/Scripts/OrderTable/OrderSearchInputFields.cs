using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderSearchInputFields : MonoBehaviour
    {
        #region Buttons

        public void OnInputValueChangedNameField(string customerName)
        {
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                var entryName = orderEntryObjects[i].transform.GetChild(3).GetComponent<TMP_Text>().text;

                orderEntryObjects[i].SetActive(entryName.Contains(customerName));
            }
        }

        public void OnDeselectNameField(string customerName)
        {
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

            if (customerName != "") 
                return;
            
            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                orderEntryObjects[i].SetActive(true);
            }
        }

        public void DateStartField(string inputDate)
        {
            if (inputDate.Length < 10)
                return;
            
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                var entryDate = orderEntryObjects[i].transform.GetChild(2).GetComponent<TMP_Text>().text;

                orderEntryObjects[i].SetActive(IsValidEntryDateStart(entryDate, inputDate));
            }
        }

        public void DateEndField(string inputDate)
        {
            if (inputDate.Length < 10)
                return;
            
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                var entryDate = orderEntryObjects[i].transform.GetChild(2).GetComponent<TMP_Text>().text;

                orderEntryObjects[i].SetActive(IsValidEntryDateEnd(entryDate, inputDate));
            }
        }

        public void OnDeselectDateStart(string inputDate)
        {
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

            if (inputDate != "") 
                return;
            
            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                orderEntryObjects[i].SetActive(true);
            }
        }

        public void OnDeselectDateEnd(string inputDate)
        {
            var orderEntryObjects = GameObjectFinder.FindMultipleObjectsByName("OrderEntry(Clone)");

            if (inputDate != "") 
                return;
            
            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                orderEntryObjects[i].SetActive(true);
            }
        }

        #endregion

        #region Private Methods

        private bool IsValidEntryDateStart(string entryDate, string inputDate)
        {
            var yearI = GetYear(entryDate);
            var monthI = GetMonth(entryDate);
            var dayI = GetDay(entryDate);

            var yearJ = GetYear(inputDate);
            var monthJ = GetMonth(inputDate);
            var dayJ = GetDay(inputDate);

            if (yearI < yearJ)
                return false;

            if (yearI != yearJ) 
                return true;
            
            if (monthI < monthJ)
                return false;

            if (monthI != monthJ) 
                return true;
                
            return dayI >= dayJ;
        }

        private bool IsValidEntryDateEnd(string entryDate, string inputDate)
        {
            var yearI = GetYear(entryDate);
            var monthI = GetMonth(entryDate);
            var dayI = GetDay(entryDate);

            var yearJ = GetYear(inputDate);
            var monthJ = GetMonth(inputDate);
            var dayJ = GetDay(inputDate);

            if (yearI > yearJ)
                return false;

            if (yearI != yearJ) 
                return true;
            
            if (monthI > monthJ)
                return false;

            if (monthI != monthJ) 
                return true;
                
            return dayI <= dayJ;
        }
        
        private int GetYear(string str)
        {
            var year = str.Substring(6,4);
            int.TryParse(year, out var res);
            return res;
        }
        
        private int GetMonth(string str)
        {
            var month = str.Substring(3,2);
            int.TryParse(month, out var res);
            return res;
        }
        
        private int GetDay(string str)
        {
            var day = str.Substring(0,2);
            int.TryParse(day, out var res);
            return res;
        }

        #endregion
    }
}