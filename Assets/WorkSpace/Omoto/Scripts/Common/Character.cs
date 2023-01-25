using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

using CharacterState;
using EventState;

public abstract class Character : MonoBehaviour
{
    // コンポーネント変数
    //============================================
    protected Rigidbody2D   m_Rb2d;
    protected Animator      m_Anim;

    // 値変数
    //============================================
    /// <summary>
    /// ステート管理
    /// </summary>
    protected StateProcessor m_State;

    /// <summary>
    /// ステータス値
    /// </summary>
    protected class Parameter
    {
        public float dex;      // 移動速度
    }

    // 以下 オブジェクトメソッド
    //============================================
    /// <summary>
    /// キャラ共通の初期処理
    /// </summary>
    protected virtual void Init()
    {
        // コンポーネント取得
        //============================================
        m_Rb2d = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();
    }

    /// <summary>
    /// キャラ共通の更新処理
    /// </summary>
    protected virtual void Exec()
    {
        // ステート処理
        //============================================
        CheckState();           // ステート条件に沿った遷移など
        m_State.Execute();      // ステート処理
    }

    /// <summary>
    /// 操作処理
    /// </summary>
    protected virtual void Ctrl() { }

    /// <summary>
    /// ステートチェック
    /// </summary>
    protected abstract void CheckState();

    /// <summary>
    /// イベント処理（被ダメージ.攻撃時ンなど）
    /// </summary>
    /// <param name="eventProcessor"></param>
    protected abstract void OnEvent(EventProcessor eventProcessor);
}
