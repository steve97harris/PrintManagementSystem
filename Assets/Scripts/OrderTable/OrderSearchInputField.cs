using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderSearchInputField : MonoBehaviour
    {
        public void OnInputValueChanged(string customerName)
        {
            var orderEntryObjects = GameObjectFinder.FindObjectsByName("OrderEntry(Clone)");

            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                var entryName = orderEntryObjects[i].transform.GetChild(3).GetComponent<TMP_Text>().text;

                if (!entryName.Contains(customerName))
                    orderEntryObjects[i].SetActive(false);
            }
        }

        public void OnDeselect(string customerName)
        {
            var orderEntryObjects = GameObjectFinder.FindObjectsByName("OrderEntry(Clone)");

            if (customerName != "") 
                return;
            
            for (int i = 0; i < orderEntryObjects.Length; i++)
            {
                orderEntryObjects[i].SetActive(true);
            }
        }
    }
}