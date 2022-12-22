using UnityEngine;

namespace Enemy
{
    public class EnemyChasingRangeController : MonoBehaviour
    {
        MobEnemyBase parentController = null;


        void Awake()
        {
            // MobEnemyBaseを継承したコントローラーを取得する。
            parentController = transform.parent.GetComponent<MobEnemyBase>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                parentController.isPlayerInRange = true;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                parentController.isPlayerInRange = false;
            }
        }

    }
}