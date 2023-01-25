using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility.Editor;
using Utility.Method;

/// <summary>
/// 動作別弾列挙体
/// </summary>
public enum BulletType
{
    STRAIGHT = 0,
    OTHER,
}

// 弾列挙体をシリアライズ化
[Serializable]
public class BulletTypeString : EnumString<BulletType> { }

public class BulletProcessor : MonoBehaviour
{
    // オブジェクト値変数
    //============================================
    // GameObject dcrObject;   // デコレート（エフェクトなど...）

    // 値変数
    //============================================
    [SerializeField] private float m_Speed;         // 速度
    [SerializeField] private float m_Damage;        // ダメージ
    [SerializeField] private BulletType m_Type;     // 弾の種類

    private bool m_IsActive;

    // プロパティ
    //============================================
    // ダメージ取得 -> 弾消去フラグが立つ
    public float Damage { get { m_IsActive = false; return m_Damage; } }

    // アクティブ状態の取得
    public bool IsActive { get { return m_IsActive; } }

    // 初期処理
    //============================================
    void Awake()
    {
        m_IsActive = true;
    }

    // 更新処理
    //============================================
    void Update()
    {
        // 弾種類別に動作する
        switch(m_Type)
        {
            case BulletType.STRAIGHT:   MoveStraight();     break;
            default:                                        break;
        }
    }

    // 衝突による処理
    //============================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Stage")
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(MyMethod.DelayMethod(1, () => { this.gameObject.SetActive(m_IsActive); }));
        }
    }

    // 以下 オブジェクトメソッド
    //============================================
    void MoveStraight()
    {
        // 自身の向きベクトル取得
        float angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
        Vector3 dir = new Vector3(Mathf.Cos(angleDir), Mathf.Sin(angleDir), 0.0f);

        // 自身の向きに移動
        transform.position += dir * m_Speed * Time.deltaTime;
    }
}
