using UnityEngine;

namespace Enemy
{
    public class MobEnemyBase : MonoBehaviour
    {
        // このオブジェクトが生きているかを示す変数。
        [System.NonSerialized] public bool isActive = false;

        [System.NonSerialized] public bool isPlayerInRange = false;

        // 必要なマネージャーたちへの参照変数
        protected EnemyBulletManager enemyBulletManagerRef;


        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            SetReferenceOfManagers();
        }


        /// <summary>
        /// 生成/再利用する際の初期化処理
        /// </summary>
        public virtual void Init()
        {
            // TODO. Initの中身を書く。現状だと使用する弾を確保する？
            isActive = true;
        }


        /// <summary>
        /// 必要なマネージャーを持つオブジェクトを検索し、自分の参照変数に登録する。
        /// </summary>
        protected void SetReferenceOfManagers()
        {
            // 必要なマネージャーが増えたとき、この下に同じ形で追加する。
            enemyBulletManagerRef = (EnemyBulletManager)FindObjectOfType<EnemyBulletManager>();

            // 必要なマネージャーが増えたとき、この下に同じ形でnullチェックを追加する。
            if (enemyBulletManagerRef == null) Debug.LogError($"EnemyBulletManager is not found. at: [{this.GetType().FullName}]");
        }
    }
}