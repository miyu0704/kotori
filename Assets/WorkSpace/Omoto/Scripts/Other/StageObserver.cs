using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Boss;

using Utility.Editor;
using Utility.Method;

/// <summary>
/// 動作別弾列挙体
/// </summary>
public enum StageType
{
    NORMAL = 0,
    BOSS,
}

// 弾列挙体をシリアライズ化
[Serializable]
public class StageTypeString : EnumString<StageType> { }

public class StageObserver : MonoBehaviour
{
    // オブジェクト変数
    //============================================
    [SerializeField] 
    Player m_Player;
    BossProcessor m_Boss;

    // 値変数
    //============================================
    [SerializeField] StageType m_Type;

    // プロパティ
    //============================================
    public Vector3 PlayerVec3 { get { return m_Player.gameObject.transform.position; } }
    public Vector3 BossVec3   { get { return m_Boss.gameObject.transform.position; } }

    // 初期処理
    //============================================
    void Start()
    {
        if(m_Type == StageType.BOSS)
        {
            StartCoroutine(MyMethod.DelayMethod(1, () => { m_Boss = FindObjectOfType<BossProcessor>(true); }));
        }
    }

    // 更新処理
    //============================================
    void Update()
    {
        // TODO：プレイヤーや敵の位置をデバッグ表示したりできる
    }
}
