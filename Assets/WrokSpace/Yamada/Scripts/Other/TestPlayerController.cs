using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * 0.1f, Input.GetAxisRaw("Vertical") * 0.1f, 0));

        if (Input.GetKeyDown(KeyCode.Space))
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));
    }
}
