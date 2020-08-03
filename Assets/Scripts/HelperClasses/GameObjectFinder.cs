﻿using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameObjectFinder : MonoBehaviour
    {
        public static GameObject[] FindMultipleObjectsByName(string objectName)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == objectName).ToArray();
            return objects;
        }
        
        public static GameObject FindSingleObjectByName(string objectName)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == objectName).ToArray();
            return objects[0];
        }
    }
}