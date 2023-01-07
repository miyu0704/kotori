using System.Collections;
using UnityEngine;

public class StageController_Disappear : MonoBehaviour
{
    // このオブジェクトのコンポーネント
    SpriteRenderer mySprite;

    // 消滅までにかかる時間
    private float disappearingTime_Sec = 3f;


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        mySprite = GetComponentInChildren<SpriteRenderer>();
    }


    /// <summary>
    /// OnCollisionEnter2D
    /// </summary>
    /// <param name="collisionInfo"></param>
    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Disappearing());
        }
    }


    /// <summary>
    /// 時間と共に薄くなっていき、最後に消える。
    /// </summary>
    /// <returns></returns>
    private IEnumerator Disappearing()
    {
        yield return new WaitForSeconds(disappearingTime_Sec / 3);

        mySprite.color = mySprite.color - new Color32(0, 0, 0, 86);

        yield return new WaitForSeconds(disappearingTime_Sec / 3);

        mySprite.color = mySprite.color - new Color32(0, 0, 0, 86);

        yield return new WaitForSeconds(disappearingTime_Sec / 3);

        Destroy(this.gameObject);

    }
}
