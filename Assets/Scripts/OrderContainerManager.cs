using System;
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

        private void FixedUpdate()
        {
            
        }

        private void Awake()
        {
            
            // AddOrderEntry("#slug", "12/32/2312","sled", "infor",  "In Progress");
            // RemoveOrderEntry("");
            
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
            var entryUniqueCode = orderEntry.uniqueCode;
            var entryCustomerName = orderEntry.customerName;
            
            entryTransform.Find("Date").GetComponent<Text>().text = entryDate;
            entryTransform.Find("UniqueCode").GetComponent<Text>().text = entryUniqueCode;
            entryTransform.Find("CustomerName").GetComponent<Text>().text = entryCustomerName;
            
            transformList.Add(entryTransform);
        }
        
        public static void AddOrderEntry(string uniqueCode, string date, string customerName, string information, string status)
        {
            Debug.Log("Scaramuchi");
            
            // create entry
            var orderEntry = new OrderEntry()
            {
                uniqueCode = uniqueCode,
                date = date,
                customerName = customerName,
                information = information,
                status = status
            };
            
            Debug.Log(uniqueCode);
            Debug.Log(date);
            Debug.Log(customerName);
            Debug.Log(information);
            Debug.Log(status);
            
            // load saved entries
            var jsonString = PlayerPrefs.GetString("orderTable");
            var orderCatalogue = JsonUtility.FromJson<OrderCatalogue>(jsonString);
            
            Debug.Log(orderCatalogue);
            
            // add new entry to leaderboard
            orderCatalogue.orderEntryList.Add(orderEntry);
            
            // save updated leaderboard
            var json = JsonUtility.ToJson(orderCatalogue);
            PlayerPrefs.SetString("orderTable", json);
            PlayerPrefs.Save();
        }

        public static void RemoveOrderEntry(string customerName)
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