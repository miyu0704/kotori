using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBulletBase : MonoBehaviour
    {
        // このオブジェクトが生きているか示す変数
        [System.NonSerialized]
        public bool isActive = false;

        // このオブジェクトのコンポーネントたち
        protected Collider2D myCollider;
        protected Rigidbody2D myRigidbody;

        // プレイヤーコントローラーへの参照変数
        protected TestPlayerController playerControllerRef;

        // 必要かもしれないマネージャへの参照変数
        protected EnemyBulletManager enemyBulletManagerRef;

        // 生成/再利用時のプレイヤー座標を保存する変数
        protected Vector2 playerPositionAtInit;


        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            myCollider = this.GetComponent<Collider2D>();
            myRigidbody = this.GetComponent<Rigidbody2D>();
            DoSettingsOfComponents();

            SetReferenceOfManagers();
            SetReferenceOfPlayer();
        }


        /// <summary>
        /// 忘れがちなコンポーネントの設定を一括で行う。
        /// </summary>
        protected virtual void DoSettingsOfComponents()
        {
            this.GetComponent<Collider2D>().isTrigger = true;
            myRigidbody.gravityScale = 0f;
        }


        /// <summary>
        /// 生成/再利用する際の初期化処理
        /// </summary>
        public virtual void Init()
        {
            // TODO. Initの中身を書く。
            isActive = true;
            myCollider.enabled = true;
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;

            playerPositionAtInit = playerControllerRef.transform.position;
        }


        /// <summary>
        /// Update
        /// </summary>
        protected virtual void Update()
        {
            if (isActive == false)
                return;

            Move();
        }


        /// <summary>
        /// 移動を行う。内容は継承先クラスで記述する。
        /// </summary>
        protected virtual void Move() { }


        /// <summary>
        /// オブジェクトが消える処理
        /// </summary>
        public virtual void Disable()
        {
            isActive = false;
            myCollider.enabled = false;
            myRigidbody.bodyType = RigidbodyType2D.Static;

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
    }
}
