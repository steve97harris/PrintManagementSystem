using UnityEngine;
using TMPro;

namespace DefaultNamespace
{
    public class OrderEntryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI entryUniqueCode = null;
        [SerializeField] private TextMeshProUGUI entryDate = null;
        [SerializeField] private TextMeshProUGUI entryStatus = null;
        [SerializeField] private TextMeshProUGUI entryInformation = null;
        [SerializeField] private TextMeshProUGUI entryCustomerName = null;

        public void Initialise(OrderEntry orderEntry)
        {
            entryUniqueCode.text = orderEntry.uniqueCode;
            entryDate.text = orderEntry.date;
            entryStatus.text = orderEntry.status;
            entryInformation.text = orderEntry.metaDataPath;
            entryCustomerName.text = orderEntry.customerName;
        }
    }
}