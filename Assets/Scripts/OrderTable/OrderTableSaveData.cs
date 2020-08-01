using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    [Serializable]
    public class OrderTableSaveData
    {
        public List<OrderEntry> orderEntries = new List<OrderEntry>();
    }
}