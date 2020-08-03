using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class OrderEntryUniqueCode : MonoBehaviour
    {
        [SerializeField] public string entryUniqueCode = null;
        [SerializeField] public string entryStatus;

        public void InitialiseOrderEntry(OrderEntry orderEntry)
        {
            entryUniqueCode = orderEntry.uniqueCode;
            entryStatus = orderEntry.currentStatus;
        }
    }
}