using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    TestPlayerController playerController;

    private void Awake()
    {
        playerController = (TestPlayerController)FindObjectOfType<TestPlayerController>();
    }

    private void Update()
    {
        transform.position = new Vector3(playerController.transform.position.x, this.transform.position.y, this.transform.position.z);
    }
}
