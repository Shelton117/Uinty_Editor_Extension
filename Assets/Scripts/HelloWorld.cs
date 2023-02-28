using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class HelloWorld : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, GameObject> keyValuePairs 
        = new Dictionary<string, GameObject>();

    [SerializeField] Action action;
}
