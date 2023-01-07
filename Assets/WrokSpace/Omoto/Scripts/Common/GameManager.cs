using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;
using Item;

// 管理クラス
public class GameManager : MonoBehaviour
{
    // ゲームステートを管理したり、UIを呼び出したりする進行役
    //============================================
    public class GameProcessor : MonoBehaviour
    {
        public static ItemManager itemManager { get; private set; }

        public GameProcessor()
        {
            // アイテム関連の初期化処理
            //============================================
            // リソース管理役生成
            itemManager = new ItemManager();

            // TODO：ゲームに登場するアイテムを登録
            var playerBullet = itemManager.AddItem(new Player.Bullet());
            itemManager.AddItem(new Player.BulletOnDebug(playerBullet));    // Debug機能（デコレート）付き自機弾
            itemManager.AddItem(new Player.Bomb());
        }

        ~GameProcessor()
        {
            // アイテム関連の破棄処理
            //============================================
            itemManager.ClearItems();
            Destroy(itemManager);
        }

        // 以下 ゲーム実行メソッド
        // TODO：必要な進行処理
        //============================================

        // UI表示
        public void CallUI(int id)
        {

        }
    }

    public GameProcessor gameProcessor { get; private set; }

    // ゲームに関するパラメータ
    //============================================
    [CreateAssetMenu(fileName = "GameParameter", menuName = "ScriptableObjects/GameParameter")]
    public class GameParameter : ScriptableObject
    {
        // TODO：ここにゲームに関するパラメータを記入
        public float volume;
    }

    [Header("GameParameter aseet path")]
    [SerializeField] string m_GPAssetPath;
    public GameParameter gameParameter { get; private set; }

    // 初期処理
    //============================================
    private void Awake()
    {
        // シングルトンアタッチ処理
        if (SingletonAttacher<GameManager>.hasInstance)
        {
            // 重複オブジェクトを破棄する
            Destroy(this.gameObject);
        }
        else
        {
            // 破棄されないようにする
            DontDestroyOnLoad(this.gameObject);

            // ゲームパラメータ代入
            // FIXME：何故か読み込まれない
            gameParameter = Resources.Load(m_GPAssetPath) as GameParameter;
        }
    }

    // 以下 ゲーム管理メソッド
    //============================================
    /// <summary>
    /// ゲーム実行における初期処理
    /// </summary>
    public void InitProcessor()
    {
        // 実行管理クラス生成
        gameProcessor = new GameProcessor();
    }

    /// <summary>
    /// ゲーム実行における終了処理
    /// </summary>
    public void TerminateProcessor()
    {
        Destroy(gameProcessor);
    }
}
