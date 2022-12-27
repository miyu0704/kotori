using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController_Disappear : MonoBehaviour
{
    SpriteRenderer modelSprite;

    private float disappearingTime_Sec = 3f;


    private void Awake()
    {
        modelSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Disappearing());
        }
    }


    private IEnumerator Disappearing()
    {
        yield return new WaitForSeconds(disappearingTime_Sec / 3);

        modelSprite.color = modelSprite.color - new Color32(0, 0, 0, 86);

        yield return new WaitForSeconds(disappearingTime_Sec / 3);

        modelSprite.color = modelSprite.color - new Color32(0, 0, 0, 86);

        yield return new WaitForSeconds(disappearingTime_Sec / 3);

        Destroy(this.gameObject);

    }
}
