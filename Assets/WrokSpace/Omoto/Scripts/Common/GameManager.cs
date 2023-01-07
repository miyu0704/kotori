using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;
using Item;

// �Ǘ��N���X
public class GameManager : MonoBehaviour
{
    // �Q�[���X�e�[�g���Ǘ�������AUI���Ăяo�����肷��i�s��
    //============================================
    public class GameProcessor : MonoBehaviour
    {
        public static ItemManager itemManager { get; private set; }

        public GameProcessor()
        {
            // �A�C�e���֘A�̏���������
            //============================================
            // ���\�[�X�Ǘ��𐶐�
            itemManager = new ItemManager();

            // TODO�F�Q�[���ɓo�ꂷ��A�C�e����o�^
            var playerBullet = itemManager.AddItem(new Player.Bullet());
            itemManager.AddItem(new Player.BulletOnDebug(playerBullet));    // Debug�@�\�i�f�R���[�g�j�t�����@�e
            itemManager.AddItem(new Player.Bomb());
        }

        ~GameProcessor()
        {
            // �A�C�e���֘A�̔j������
            //============================================
            itemManager.ClearItems();
            Destroy(itemManager);
        }

        // �ȉ� �Q�[�����s���\�b�h
        // TODO�F�K�v�Ȑi�s����
        //============================================

        // UI�\��
        public void CallUI(int id)
        {

        }
    }

    public GameProcessor gameProcessor { get; private set; }

    // �Q�[���Ɋւ���p�����[�^
    //============================================
    [CreateAssetMenu(fileName = "GameParameter", menuName = "ScriptableObjects/GameParameter")]
    public class GameParameter : ScriptableObject
    {
        // TODO�F�����ɃQ�[���Ɋւ���p�����[�^���L��
        public float volume;
    }

    [Header("GameParameter aseet path")]
    [SerializeField] string m_GPAssetPath;
    public GameParameter gameParameter { get; private set; }

    // ��������
    //============================================
    private void Awake()
    {
        // �V���O���g���A�^�b�`����
        if (SingletonAttacher<GameManager>.hasInstance)
        {
            // �d���I�u�W�F�N�g��j������
            Destroy(this.gameObject);
        }
        else
        {
            // �j������Ȃ��悤�ɂ���
            DontDestroyOnLoad(this.gameObject);

            // �Q�[���p�����[�^���
            // FIXME�F���̂��ǂݍ��܂�Ȃ�
            gameParameter = Resources.Load(m_GPAssetPath) as GameParameter;
        }
    }

    // �ȉ� �Q�[���Ǘ����\�b�h
    //============================================
    /// <summary>
    /// �Q�[�����s�ɂ����鏉������
    /// </summary>
    public void InitProcessor()
    {
        // ���s�Ǘ��N���X����
        gameProcessor = new GameProcessor();
    }

    /// <summary>
    /// �Q�[�����s�ɂ�����I������
    /// </summary>
    public void TerminateProcessor()
    {
        Destroy(gameProcessor);
    }
}
