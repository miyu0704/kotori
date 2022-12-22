using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * 0.1f, Input.GetAxisRaw("Vertical") * 0.1f, 0));
    }
}
