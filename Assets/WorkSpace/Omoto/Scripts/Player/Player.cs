using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterState;
using EventState;
using Item;

using Utility;

public class Player : Character
{
    // オブジェクト変数
    //============================================
    [Header("自機の弾")]
    [SerializeField]
    private GameObject m_Bullet;
    public GameObject bullet => m_Bullet;

    // コンポーネント変数
    //============================================
    private Rigidbody2D m_Rb2d;
    private Animator    m_Anim;

    // 値変数
    //============================================
    private float  m_InputX;
    private float  m_InputY;
    private bool[] m_InputOthers;

    [System.Serializable]
    class MyParameter : Parameter
    {
        public float jump;          // ジャンプ力
        public float sliding;       // スライディング速度
    }

    class MyStatus
    {
        public bool  isGrounding;   // 接地しているか
        public bool  isJumping;     // ジャンプ中か
        public bool  isSticking;    // 張り付き中か

        public bool  isDead;        // 死亡状態か
    }

    [Header("パラメータ")]
    [SerializeField] MyParameter     m_Param;
                     MyStatus        m_Status;

    // その他（オブジェクト固有変数）
    //============================================
    [Header("Action Keys（Sliding, Attack, Bomb）")]
    [SerializeField]
    private KeyCode[] m_ActionKeys;     // アクション操作キー

    // その他（ステート）
    //============================================
    /// <summary>
    /// プレイヤーステート列挙体
    /// </summary>
    enum MyStatePriority
    {
        e_SLIDING = StatePriority.e_JUMP
    }

    class SlidingState : StateProcessor
    {
        public SlidingState(ref Rigidbody2D rb2d, Vector2 velocity)
        {
            rb2d.velocity = velocity;
        }

        public override StatePriority GetPriority()
        {
            return (StatePriority)MyStatePriority.e_SLIDING;
        }

        public override string GetStateName()
        {
            return "state:Sliding";
        }
    }

    // その他（イベント）
    //============================================
    class DamageEvent : EventProcessor
    {
        public DamageEvent(ref StateProcessor stateProcessor)
        {
            stateProcessor = new DamageState();

            // TODO：GameOverのUIを表示する

        }
    }

    /*
     * ダメージイベントの仕事
     * 
     * １．プレイヤーを死亡状態にする（m_StateをDamageStateに遷移する）
     * ２．GameOverのUIを表示する
     */

    // その他（アイテム）
    // => GameProcessorに登録処理を書くこと！
    //============================================
    public class Bullet : ItemProcessor 
    {
        Player player = UnityEngine.Object.FindObjectOfType<Player>(true);

        public Bullet()
        {
            ExecAction = Shot;
        }

        private void Shot()
        {
            Instantiate(player.bullet, player.transform.position, player.bullet.transform.rotation);
        }
    }

    public class BulletOnDebug : ItemDecorator
    {
        public BulletOnDebug(ItemProcessor bullet) : base(bullet)
        {
            DecoAction = Message;
        }

        private void Message()
        {
            Debug.Log("Shot!!");
        }
    }

    public class Bomb : ItemProcessor
    {
        public Bomb()
        {
            // 実行処理を定義
            ExecAction = Attack;
        }

        private void Attack()
        {
            // TODO：敵を全て滅ぼす処理
        }
    }

    ItemProcessor m_ShotBullet;
    ItemProcessor m_UseBomb;

    // 初期処理
    //============================================
    void Start()
    {
        // TODO：ゲーム開始時（TitleScene -> GameScene）にこれを行う。
        SingletonAttacher<GameManager>.instance.InitProcessor();

        // コンポーネント取得
        //============================================
        m_Rb2d = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();

        // ステート, ステータス初期化
        //============================================
        m_State = new IdleState();
        m_State.ExecAction = Idle;

        m_Status = new MyStatus();
        m_Status.isGrounding = true;
        m_Status.isJumping = false;
        m_Status.isSticking = false;
        m_Status.isDead = false;

        // 各操作キーの追加
        //============================================
        m_InputOthers = new bool[m_ActionKeys.Length];

        // アイテム関連の処理
        //============================================
    }

    // 更新処理
    //============================================
    void Update()
    {
        // 操作処理
        //============================================
        // TODO：ステート名の変更
        if (m_State.GetStateName() != "state:Damage") 
            Ctrl();

        // ステート処理
        //============================================
        CheckState();           // ステート条件に沿った遷移など
        m_State.Execute();      // ステート処理
    }

    // 衝突による処理
    //============================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 接地判定
        if (collision.gameObject.tag == "Ground")
        {
            m_Status.isGrounding = true;
            m_Status.isJumping   = false;
            return;
        }

        // 被弾判定
        if(collision.gameObject.tag == "Bullet")
        {
            OnDamage();
        }
        /*
        if (collision.gameObject.tag == "")
        {
            OnDamage();
        }
        */
    }

    // 以下 オブジェクトメソッド
    //============================================
    protected override void Ctrl()
    {
        // 移動操作
        //============================================
        m_InputX = Input.GetAxisRaw("Horizontal");
        m_InputY = Input.GetAxisRaw("Jump");

        // アクション操作
        //============================================
        foreach (var keys in m_ActionKeys.Select((value, index) => new { value, index }))
        {
            m_InputOthers[keys.index] = Input.GetKeyDown(keys.value);
        }
    }

    protected override void CheckState()
    {
        // ジャンプ or スライディング
        //============================================
        if (m_State < StatePriority.e_JUMP)
        {
            if (0 < m_InputY)            // Lshiftキー押下
            {
                m_State = new JumpState(ref m_Rb2d, new Vector2(m_Rb2d.velocity.x, m_Param.jump));
                m_State.ExecAction = Jump;
                m_State.ExecAction += Move;

                m_Status.isGrounding = false;
                m_Status.isJumping = true;
            }
            else if(m_InputOthers[0])   // アクションキー１つ目押下
            {
                m_State = new SlidingState(ref m_Rb2d, new Vector2(m_Param.sliding * Mathf.Clamp(transform.localScale.x, -1, 1), m_Rb2d.velocity.y));
                m_State.ExecAction = Sliding;
            }
        }

        // 横移動
        //============================================
        if (m_State < StatePriority.e_MOVE)
        {
            if (m_InputX != 0)  // left, rightキー押下
            {
                m_State = new MoveState();
                m_State.ExecAction = Move;
            }
        }
    }

    protected override void OnEvent(EventProcessor eventProcessor)
    {
        eventProcessor.Execute();
    }

    /// <summary>
    /// 待機時処理
    /// </summary>
    private void Idle()
    {
        m_Anim.SetFloat("Speed", Mathf.Abs(m_Param.dex * m_InputX));
        if (m_InputOthers[1])
        {
            OnAttack(0);
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // 左右移動
        //============================================
        m_Rb2d.velocity = new Vector2(m_InputX * m_Param.dex, m_Rb2d.velocity.y);
        if (0 < m_InputX)
        {
            // 右方向を向く
            transform.localScale = new Vector2(3, 3);
            m_Bullet.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (m_InputX < 0)
        {
            // 左方向を向く
            transform.localScale = new Vector2(-3, 3);
            m_Bullet.transform.eulerAngles = new Vector3(0, 0, 180);
        }

        // 移動アニメーション
        //============================================
        if (m_State < StatePriority.e_JUMP)     // ジャンプアニメーションを優先する
        {
            m_Anim.SetFloat("Speed", Mathf.Abs(m_Param.dex * m_InputX));
        }

        // ステート遷移（条件：待機）
        //============================================
        if (m_InputX == 0)
        {
            m_State = new IdleState();
            m_State.ExecAction = Idle;
        }
    }　

    /// <summary>
    /// スライディング
    /// </summary>
    private void Sliding()
    {
        // ステート遷移（条件：x速度 0.2以下）
        //============================================
        if (Mathf.Abs(m_Rb2d.velocity.x) < 0.2)
        {
            m_State = new IdleState();
            m_State.ExecAction = Idle;
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        // ステート遷移（条件：接地）
        //============================================
        if (m_Status.isGrounding)
        {
            m_State = new IdleState();
            m_State.ExecAction = Idle;
        }
    }

    protected void OnAttack(int atkCode)
    {
        // 攻撃の種類に合わせた前処理
        // TODO：ボム（攻撃アイテム）は攻撃ステートにて行う
        //============================================
        switch (atkCode)
        {
            case 0:
                m_ShotBullet = GameManager.GameProcessor.itemManager.GetItem(1);    // Player.BulletOnDebug
                m_ShotBullet.Execute();
                break;
            default:
                break;
        }

        // ステート遷移
        //============================================
        // m_State = new AttackState();
    }

    private void OnDamage()
    {
        // イベント作成
        //============================================
        DamageEvent damageEvent = new DamageEvent(ref m_State);
        damageEvent.ExecAction = () => m_Status.isDead = true;

        // イベント実行
        //============================================
        OnEvent(damageEvent);
    }
}
