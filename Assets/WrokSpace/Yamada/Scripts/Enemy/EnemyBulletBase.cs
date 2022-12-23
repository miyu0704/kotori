using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBulletBase : MonoBehaviour
    {
        // このオブジェクトが生きているか示す変数
        [System.NonSerialized] public bool isActive = false;

        // このオブジェクトのRigidbody
        protected Rigidbody2D myRigidbody;

        // プレイヤーコントローラーへの参照変数
        protected TestPlayerController playerControllerRef;

        // 生成/再利用時のプレイヤー座標を保存する変数
        protected Vector2 playerPositionAtInit;


        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            myRigidbody = this.GetComponent<Rigidbody2D>();
            DoSettingsOfComponents();

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

            playerPositionAtInit = playerControllerRef.transform.position;
        }


        /// <summary>
        /// Update
        /// </summary>
        protected virtual void Update()
        {
            Move();
        }


        /// <summary>
        /// 移動を行う。内容は継承先クラスで記述する。
        /// </summary>
        protected virtual void Move() { }


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
