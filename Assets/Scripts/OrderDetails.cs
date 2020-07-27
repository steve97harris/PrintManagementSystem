using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderDetails : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI orderInfo;

        public void OrderNumberClicked()
        {
            var metaDataFileName = orderInfo.text;
            
            var metaDataPath = @"C:\YR\L11470_USC\Saves\Meta\" + metaDataFileName;
        }
        
        private void GetSavedOrders()
        {
            if (!File.Exists(OrderTable.SavePath))
            {
                Debug.LogError("OrderTable does not exist - can't extract order data");
            }

            using (StreamReader stream = new StreamReader(OrderTable.SavePath))
            {
                var json = stream.ReadToEnd();
                var x = JsonConvert.DeserializeObject<List<OrderEntry>>(json);
                foreach (var orderEntry in x)
                {
                    // x
                }
            }
        }
    }
}