using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [System.Serializable]
    public class OrderEntry
    {
        public string uniqueCode;
        public string date;
        public string status;        // may need changing due to dropdown option
        [FormerlySerializedAs("information")] public string metaDataPath;
        public string customerName;
        public string basketDataPath;
    }
}