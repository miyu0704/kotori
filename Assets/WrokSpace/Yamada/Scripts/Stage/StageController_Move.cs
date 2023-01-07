using UnityEngine;
using Useful;

public class StageController_Move : MonoBehaviour
{
    // 
    private CanIEnumeratorRefBool isMoveReverse = new CanIEnumeratorRefBool();

    private Vector2 initPosition;

    [SerializeField]
    private Vector2 moveAmount;

    private float moveCompleteRatio_Deg = 0;

    private const float timeForCompleteMoveHalf = 5f;


    private void Awake()
    {
        initPosition = this.transform.position;
    }


    private void Update()
    {
        Move();
    }


    private void Move()
    {
        moveCompleteRatio_Deg += 360 / timeForCompleteMoveHalf * Time.deltaTime;

        Vector2 moveVec = moveAmount / timeForCompleteMoveHalf * Time.deltaTime * Mathf.Sin(moveCompleteRatio_Deg * Mathf.Deg2Rad) * 3.14f;

        transform.Translate(moveVec);
    }


    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.transform.parent = this.transform;
        }
    }


    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.transform.parent = null;
        }
    }

}
