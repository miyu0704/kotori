using UnityEngine;
using Enemy;

public class StageSizeController : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            other.GetComponent<EnemyBulletBase>().Disable();
        }
    }
}
