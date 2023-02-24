using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HelloWorld : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, GameObject> keyValuePairs 
        = new Dictionary<string, GameObject>();
}
