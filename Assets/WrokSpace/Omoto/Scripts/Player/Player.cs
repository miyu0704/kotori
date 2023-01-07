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
    // �I�u�W�F�N�g�ϐ�
    //============================================
    [Header("���@�̒e")]
    [SerializeField]
    private GameObject m_Bullet;
    public GameObject bullet => m_Bullet;

    // �R���|�[�l���g�ϐ�
    //============================================
    private Rigidbody2D m_Rb2d;
    private Animator    m_Anim;

    // �l�ϐ�
    //============================================
    private float  m_InputX;
    private float  m_InputY;
    private bool[] m_InputOthers;

    [System.Serializable]
    class MyParameter : Parameter
    {
        public float jump;          // �W�����v��
        public float sliding;       // �X���C�f�B���O���x
    }

    class MyStatus
    {
        public bool  isGrounding;   // �ڒn���Ă��邩
        public bool  isJumping;     // �W�����v����
        public bool  isSticking;    // ����t������

        public bool  isDead;        // ���S��Ԃ�
    }

    [Header("�p�����[�^")]
    [SerializeField] MyParameter     m_Param;
                     MyStatus        m_Status;

    // ���̑��i�I�u�W�F�N�g�ŗL�ϐ��j
    //============================================
    [Header("Action Keys�iSliding, Attack, Bomb�j")]
    [SerializeField]
    private KeyCode[] m_ActionKeys;     // �A�N�V��������L�[

    // ���̑��i�X�e�[�g�j
    //============================================
    /// <summary>
    /// �v���C���[�X�e�[�g�񋓑�
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

    // ���̑��i�C�x���g�j
    //============================================
    class DamageEvent : EventProcessor
    {
        public DamageEvent(ref StateProcessor stateProcessor)
        {
            stateProcessor = new DamageState();

            // TODO�FGameOver��UI��\������i�ȉ��͈��j
            // SingletonAttacher<GameManager>.instance.gameProcessor.CallUI(e_GAMEOVER);
        }
    }

    /*
     * �_���[�W�C�x���g�̎d��
     * 
     * �P�D�v���C���[�����S��Ԃɂ���im_State��DamageState�ɑJ�ڂ���j
     * �Q�DGameOver��UI��\������
     * �i����UI��\�����������AUIManager�𓮍삳��������͎����Ȃ��B�C�x���g�͌����Ƃ��� "�o����" �ł���̂�
     * �@�C�x���g����K�؂ȃQ�[���v���Z�b�T�[�̏������Ăяo���A�Q�[���v���Z�b�T�[�����̏����ӔC�𕉂��B
     * �@�������A�Q�[���v���Z�b�T�[�͎w�����ł���̂ŁA���ړI�Ȏ��s�����͊e�N���X�iUIManager etc...�j���s���B�j
     */

    // ���̑��i�A�C�e���j
    // => GameProcessor�ɓo�^�������������ƁI
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
            // ���s�������`
            ExecAction = Attack;
        }

        private void Attack()
        {
            // TODO�F�G��S�Ėłڂ�����
        }
    }

    ItemProcessor m_ShotBullet;
    ItemProcessor m_UseBomb;

    // ��������
    //============================================
    void Start()
    {
        // TODO�F�Q�[���J�n���iTitleScene -> GameScene�j�ɂ�����s���B
        SingletonAttacher<GameManager>.instance.InitProcessor();

        // �R���|�[�l���g�擾
        //============================================
        m_Rb2d = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();

        // �X�e�[�g, �X�e�[�^�X������
        //============================================
        m_State = new IdleState();
        m_State.ExecAction = Idle;

        m_Status = new MyStatus();
        m_Status.isGrounding = true;
        m_Status.isJumping = false;
        m_Status.isSticking = false;
        m_Status.isDead = false;

        // �e����L�[�̒ǉ�
        //============================================
        m_InputOthers = new bool[m_ActionKeys.Length];

        // �A�C�e���֘A�̏���
        //============================================
    }

    // �X�V����
    //============================================
    void Update()
    {
        // ���쏈��
        //============================================
        // TODO�F�X�e�[�g���̕ύX
        if (m_State.GetStateName() != "state:Damage") 
            Ctrl();

        // �X�e�[�g����
        //============================================
        CheckState();           // �X�e�[�g�����ɉ������J�ڂȂ�
        m_State.Execute();      // �X�e�[�g����
    }

    // �Փ˂ɂ�鏈��
    //============================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ڒn����
        if (collision.gameObject.tag == "Ground")
        {
            m_Status.isGrounding = true;
            m_Status.isJumping   = false;
            return;
        }

        // ��e����
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamage();
        }
    }

    // �ȉ� �I�u�W�F�N�g���\�b�h
    //============================================
    protected override void Ctrl()
    {
        // �ړ�����
        //============================================
        m_InputX = Input.GetAxisRaw("Horizontal");
        m_InputY = Input.GetAxisRaw("Jump");

        // �A�N�V��������
        //============================================
        foreach (var keys in m_ActionKeys.Select((value, index) => new { value, index }))
        {
            m_InputOthers[keys.index] = Input.GetKeyDown(keys.value);
        }
    }

    protected override void CheckState()
    {
        // �W�����v or �X���C�f�B���O
        //============================================
        if (m_State < StatePriority.e_JUMP)
        {
            if (0 < m_InputY)            // Lshift�L�[����
            {
                m_State = new JumpState(ref m_Rb2d, new Vector2(m_Rb2d.velocity.x, m_Param.jump));
                m_State.ExecAction = Jump;
                m_State.ExecAction += Move;

                m_Status.isGrounding = false;
                m_Status.isJumping = true;
            }
            else if(m_InputOthers[0])   // �A�N�V�����L�[�P�ډ���
            {
                m_State = new SlidingState(ref m_Rb2d, new Vector2(m_Param.sliding * Mathf.Clamp(transform.localScale.x, -1, 1), m_Rb2d.velocity.y));
                m_State.ExecAction = Sliding;
            }
        }

        // ���ړ�
        //============================================
        if (m_State < StatePriority.e_MOVE)
        {
            if (m_InputX != 0)  // left, right�L�[����
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
    /// �ҋ@������
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
    /// �ړ�����
    /// </summary>
    private void Move()
    {
        // ���E�ړ�
        //============================================
        m_Rb2d.velocity = new Vector2(m_InputX * m_Param.dex, m_Rb2d.velocity.y);
        if (0 < m_InputX)
        {
            // �E����������
            transform.localScale = new Vector2(3, 3);
            m_Bullet.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (m_InputX < 0)
        {
            // ������������
            transform.localScale = new Vector2(-3, 3);
            m_Bullet.transform.eulerAngles = new Vector3(0, 0, 180);
        }

        // �ړ��A�j���[�V����
        //============================================
        if (m_State < StatePriority.e_JUMP)     // �W�����v�A�j���[�V������D�悷��
        {
            m_Anim.SetFloat("Speed", Mathf.Abs(m_Param.dex * m_InputX));
        }

        // �X�e�[�g�J�ځi�����F�ҋ@�j
        //============================================
        if (m_InputX == 0)
        {
            m_State = new IdleState();
            m_State.ExecAction = Idle;
        }
    }�@

    /// <summary>
    /// �X���C�f�B���O
    /// </summary>
    private void Sliding()
    {
        // �X�e�[�g�J�ځi�����Fx���x 0.2�ȉ��j
        //============================================
        if (Mathf.Abs(m_Rb2d.velocity.x) < 0.2)
        {
            m_State = new IdleState();
            m_State.ExecAction = Idle;
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump()
    {
        // �X�e�[�g�J�ځi�����F�ڒn�j
        //============================================
        if (m_Status.isGrounding)
        {
            m_State = new IdleState();
            m_State.ExecAction = Idle;
        }
    }

    protected void OnAttack(int atkCode)
    {
        // �U���̎�ނɍ��킹���O����
        // TODO�F�{���i�U���A�C�e���j�͍U���X�e�[�g�ɂčs��
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

        // �X�e�[�g�J��
        //============================================
        // m_State = new AttackState();
    }

    private void OnDamage()
    {
        // �C�x���g�쐬
        //============================================
        DamageEvent damageEvent = new DamageEvent(ref m_State);
        damageEvent.ExecAction = () => m_Status.isDead = true;

        // �C�x���g���s
        //============================================
        OnEvent(damageEvent);
    }
}
