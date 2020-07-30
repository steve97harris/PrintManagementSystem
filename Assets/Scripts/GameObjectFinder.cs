using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameObjectFinder : MonoBehaviour
    {
        public static GameObject[] FindObjectsByName(string objectName)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == objectName).ToArray();
            return objects;
        }
    }
}