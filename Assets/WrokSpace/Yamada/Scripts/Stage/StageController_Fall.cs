using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StageController_Fall : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    private bool isFalling = false;


    private void Awake()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>();
        myRigidbody.bodyType = RigidbodyType2D.Kinematic;
    }


    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            StartFall();
            collisionInfo.transform.parent = this.transform;
        }
    }


    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            StartFall();
            collisionInfo.transform.parent = null;
        }
    }


    private void StartFall()
    {
        if (isFalling)
            return;

        isFalling = true;
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;

    }
}
