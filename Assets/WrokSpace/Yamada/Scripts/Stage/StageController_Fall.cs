using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StageController_Fall : MonoBehaviour
{
    // このオブジェクトのコンポーネント
    private Rigidbody2D myRigidbody;

    // このオブジェクトが落下中か示すフラグ
    private bool isFalling = false;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>();
        myRigidbody.bodyType = RigidbodyType2D.Kinematic;
    }


    /// <summary>
    /// OnCollisionEnter2D
    /// </summary>
    /// <param name="collisionInfo"></param>
    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            StartFall();
            collisionInfo.transform.parent = this.transform;
        }
    }


    /// <summary>
    /// OnCollisionExit2D
    /// </summary>
    /// <param name="collisionInfo"></param>
    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.transform.parent = null;
        }
    }


    /// <summary>
    /// 落下を始める。
    /// </summary>
    private void StartFall()
    {
        if (isFalling)
            return;

        isFalling = true;
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;

    }
}
