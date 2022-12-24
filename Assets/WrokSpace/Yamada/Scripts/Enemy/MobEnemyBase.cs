using System.Collections;
using UnityEngine;
using Useful;

namespace Enemy
{
    public class MobEnemyBase : MonoBehaviour
    {
        // このオブジェクトが生きているかを示す変数
        [System.NonSerialized]
        public bool isActive = false;

        // このオブジェクトのコンポーネント
        protected Collider2D myCollider;

        // プレイヤーが追尾/攻撃範囲内に入っているか示すフラグ
        [System.NonSerialized]
        public bool isPlayerInRange = false;

        // 次の弾を発射できるまでの待ち時間にあるかを示すフラグ
        protected CanIEnumeratorRefBool isBulletReloading;

        // プレイヤーコントローラーへの参照変数
        protected TestPlayerController playerControllerRef;

        // 必要なマネージャーたちへの参照変数
        protected EnemyBulletManager enemyBulletManagerRef;


        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            myCollider = this.GetComponent<Collider2D>();
            isBulletReloading = new CanIEnumeratorRefBool();

            SetReferenceOfManagers();
            SetReferenceOfPlayer();
        }


        /// <summary>
        /// 生成/再利用する際の初期化処理
        /// </summary>
        public virtual void Init()
        {
            // TODO. Initの中身を書く。現状だと使用する弾を確保する？
            isActive = true;

            myCollider.enabled = true;
            isBulletReloading.flag = false;

        }


        /// <summary>
        /// Update
        /// </summary>
        protected virtual void Update()
        {
            Shooting();
        }


        /// <summary>
        /// 弾を発射する。内容は継承先クラスで記述する。
        /// </summary>
        protected virtual void Shooting() { }


        /// <summary>
        /// このオブジェクトが消える処理
        /// </summary>
        public virtual void Disable()
        {
            isActive = false;
            myCollider.enabled = false;

            transform.position = enemyBulletManagerRef.bulletWaitingPosition;
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


        /// <summary>
        /// プレイヤーコントローラーへの参照を登録する。
        /// </summary>
        protected void SetReferenceOfPlayer()
        {
            playerControllerRef = (TestPlayerController)FindObjectOfType<TestPlayerController>();

            if (playerControllerRef == null) Debug.LogError($"TestPlayerController is not found. at: [{this.GetType().FullName}]");
        }


        /// <summary>
        /// 指定秒数後にフラグを反転させる。
        /// </summary>
        /// <param name="targetFlag"> 反転させるフラグ </param>
        /// <param name="waitTime_sec"> 反転させるために待つ秒数 </param>
        /// <returns></returns>
        protected IEnumerator ToggleFlagAfterSeconds(CanIEnumeratorRefBool targetFlag, float waitTime_sec)
        {
            yield return new WaitForSeconds(waitTime_sec);

            targetFlag.flag = !targetFlag.flag;

        }

    }
}