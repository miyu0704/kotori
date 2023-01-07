using UnityEngine;

namespace Enemy
{
    public class EnemyChasingRangeController : MonoBehaviour
    {
        // 親オブジェクトのMobEnemyController_*** が入る。
        private MobEnemyBase parentController = null;


        /// <summary>
        /// Awake
        /// </summary>
        void Awake()
        {
            // MobEnemyBaseを継承したコントローラーを取得する。
            parentController = transform.parent.GetComponent<MobEnemyBase>();
        }


        /// <summary>
        /// OnTriggerEnter2D
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                parentController.isPlayerInRange = true;
            }
        }


        /// <summary>
        /// OnTriggerExit2D
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                parentController.isPlayerInRange = false;
            }
        }


    }
}