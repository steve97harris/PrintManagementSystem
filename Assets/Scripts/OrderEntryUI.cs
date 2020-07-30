using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class OrderEntryUI : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI entryUniqueCode = null;
        [SerializeField] public TextMeshProUGUI entryDate = null;
        [SerializeField] public TextMeshProUGUI entryMetaData = null;
        [SerializeField] public TextMeshProUGUI entryCustomerName = null;
        [SerializeField] public string entryBasketDataPath = null;
        [SerializeField] public string entryStatus;

        public void Initialise(OrderEntry orderEntry)
        {
            entryUniqueCode.text = orderEntry.uniqueCode;
            entryDate.text = orderEntry.date;
            entryMetaData.text = orderEntry.metaData;
            entryCustomerName.text = orderEntry.customerName;
            entryBasketDataPath = orderEntry.basketDataPath;
            entryStatus = orderEntry.currentStatus;
        }
    }
}