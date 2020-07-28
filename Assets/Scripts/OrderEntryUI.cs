using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class OrderEntryUI : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI entryUniqueCode = null;
        [SerializeField] public TextMeshProUGUI entryDate = null;
        [SerializeField] public TextMeshProUGUI entryStatus = null;
        [SerializeField] public TextMeshProUGUI entryMetaData = null;
        [SerializeField] public TextMeshProUGUI entryCustomerName = null;
        [SerializeField] public string basketDataPath = null;

        public void Initialise(OrderEntry orderEntry)
        {
            entryUniqueCode.text = orderEntry.uniqueCode;
            entryDate.text = orderEntry.date;
            entryStatus.text = orderEntry.status;
            entryMetaData.text = orderEntry.metaData;
            entryCustomerName.text = orderEntry.customerName;
            basketDataPath = orderEntry.basketDataPath;
        }
    }
}