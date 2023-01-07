using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController_Needle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.gameObject.GetComponent<TestPlayerController>().Dead();
        }
    }
}
