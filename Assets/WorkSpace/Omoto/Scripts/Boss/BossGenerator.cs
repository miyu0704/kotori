using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public static GameObject Generate(string name)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Boss/" + name);
        return prefab == null ? 
            null : Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
    }
}