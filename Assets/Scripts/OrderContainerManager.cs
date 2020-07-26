using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class OrderContainerManager : MonoBehaviour
    {
        public Transform entryContainer;
        public Transform entryTemplate;
        private List<Transform> _orderCatalogueEntryTransformList;

        private void Awake()
        {
            // AddOrderEntry("#182734", "12/32/2312","avocado", "infor",  "In Progress");
            // RemoveLeaderboardEntry("");
            
            // sort by time
            // for (int i = 0; i < leaderboard.leaderboardEntryList.Count; i++)
            // {
            //     for (int j = i+1; j < leaderboard.leaderboardEntryList.Count; j++)
            //     {
            //         if (leaderboard.leaderboardEntryList[j].deltaTime < leaderboard.leaderboardEntryList[i].deltaTime)
            //         {
            //             var temp = leaderboard.leaderboardEntryList[i];
            //             leaderboard.leaderboardEntryList[i] = leaderboard.leaderboardEntryList[j];
            //             leaderboard.leaderboardEntryList[j] = temp;
            //         }
            //     }
            // }
            
            // entryContainer = transform.Find("OrderContainer");
            entryTemplate = entryContainer.Find("OrderTemplate");
            
            entryTemplate.gameObject.SetActive(false);

            var jsonString = PlayerPrefs.GetString("orderTable");
            var orderCatalogue = JsonUtility.FromJson<OrderCatalogue>(jsonString);
            
            _orderCatalogueEntryTransformList = new List<Transform>();
            foreach (var orderEntry in orderCatalogue.orderEntryList)
            {
                CreateOrderEntryTransform(orderEntry, entryContainer, _orderCatalogueEntryTransformList);
            }
        }
        
        private void CreateOrderEntryTransform(OrderEntry orderEntry, Transform container, List<Transform> transformList)
        {
            var templateHeight = 130f;
            var entryTransform = Instantiate(entryTemplate.transform, entryContainer.transform);
            var entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0,-templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            var rank = transformList.Count + 1;
            var rankString = "";

            switch (rank)
            {
                case 1:
                    rankString = "1";
                    break;
                case 2:
                    rankString = "2";
                    break;
                case 3:
                    rankString = "3";
                    break;
                default:
                    rankString = rank.ToString();
                    break;
            }

            var entryDate = orderEntry.date;
            var entryOrderNumber = orderEntry.orderNumber;
            var entryCustomerName = orderEntry.customerName;
            
            entryTransform.Find("Date").GetComponent<Text>().text = entryDate;
            entryTransform.Find("OrderNumber").GetComponent<Text>().text = entryOrderNumber;
            entryTransform.Find("CustomerName").GetComponent<Text>().text = entryCustomerName;
            
            transformList.Add(entryTransform);
        }
        
        public void AddOrderEntry(string orderNumber, string date, string customerName, string information, string status)
        {
            // create entry
            var orderEntry = new OrderEntry()
            {
                orderNumber = orderNumber,
                date = date,
                customerName = customerName,
                information = information,
                status = status
            };
            
            // load saved entries
            var jsonString = PlayerPrefs.GetString("orderTable");
            var orderCatalogue = JsonUtility.FromJson<OrderCatalogue>(jsonString);
            
            // add new entry to leaderboard
            orderCatalogue.orderEntryList.Add(orderEntry);
            
            // save updated leaderboard
            var json = JsonUtility.ToJson(orderCatalogue);
            PlayerPrefs.SetString("orderTable", json);
            PlayerPrefs.Save();
        }

        public void RemoveOrderEntry(string customerName)
        {
            // load saved entries
            var jsonString = PlayerPrefs.GetString("orderTable");
            var orderCatalogue = JsonUtility.FromJson<OrderCatalogue>(jsonString);
            
            // remove item
            for (int i = 0; i < orderCatalogue.orderEntryList.Count; i++)
            {
                if (orderCatalogue.orderEntryList[i].customerName == customerName)
                {
                    orderCatalogue.orderEntryList.RemoveAt(i);
                }
            }
            
            // save updated leaderboard
            var json = JsonUtility.ToJson(orderCatalogue);
            PlayerPrefs.SetString("orderTable", json);
            PlayerPrefs.Save();
        }
    }
}